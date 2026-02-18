using System.Numerics;
using Raylib_cs;
using RayGUI_cs;
using System.Threading;
using System.Threading.Tasks;


namespace CSRayTracer
{
    class UI
    {
        public int width;
        public int height;
        private Raylib_cs.Color[] pixels;
        private RayGUI_cs.GuiContainer guiContainer;
        private int rows = 0;
        private int x = 0;
        private int windowWidth = 800;
        private Font font;
        public UI(int width)
        {
            this.width = width;
            this.height = (int)(this.width / (16.0 / 9.0));

            Raylib.SetConfigFlags(Raylib_cs.ConfigFlags.ResizableWindow);
            Raylib.SetTraceLogLevel(TraceLogLevel.Error);
            Raylib.InitWindow(windowWidth, (int)(windowWidth / (16.0 / 9.0)) + 40, "CSRayTracer");
            Raylib.SetTargetFPS(60);
            font = Raylib.LoadFont("./Inter.ttf");

            RayGUI.LoadGUI(font);
            this.guiContainer = new RayGUI_cs.GuiContainer();

            RayGUI_cs.Button saveBtn = new RayGUI_cs.Button(5, 0, 120, 20, "Save to file");
            saveBtn.Event = () =>
            {
                Util.writeImage(pixels, width, height);
            };
            this.guiContainer.Add("saveButton", saveBtn);

            pixels = new Raylib_cs.Color[width * (int)height + width * 10];
        }

        public void AppendPixel(Raylib_cs.Color pixel, int index)
        {
            this.pixels[index] = pixel;
        }

        public void AppendRow(Raylib_cs.Color[] pixels, int index)
        {
            pixels.CopyTo(this.pixels, index);
            rows++;
        }
        public void Draw(Lock renderLock)
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);

            Texture2D texture;
            lock (renderLock)
            {
                unsafe
                {
                    fixed (Raylib_cs.Color* ptr = pixels)
                    {
                        Raylib_cs.Image img = new Raylib_cs.Image
                        {
                            Data = ptr,
                            Width = width,
                            Height = height,
                            Format = PixelFormat.UncompressedR8G8B8A8,
                            Mipmaps = 1
                        };
                        texture = Raylib.LoadTextureFromImage(img);

                        int menuHeight = 70;
                        float scaleW = (float)Raylib.GetScreenWidth() / width;
                        float scaleH = (float)(Raylib.GetScreenHeight() - menuHeight) / height;
                        float scale = Math.Min(scaleW, scaleH);
                        float imageX = (Raylib.GetScreenWidth() - width * scale) / 2f;

                        Component saveButton = guiContainer["saveButton"];

                        Raylib.DrawTextureEx(texture, new Vector2(imageX, 0), 0f, scale, Color.White);

                        int renderHeight = (int)(height * scale);
                        Raylib.DrawRectangleLines(1, renderHeight + 1, Raylib.GetScreenWidth() - 1, menuHeight - 1, Color.White);

                        Raylib.DrawTextEx(font, "Pixels Per Second: ...", new Vector2(130, renderHeight + 20), 15, 1, Color.White);
                        Raylib.DrawTextEx(font, "Rays Per Second: ...", new Vector2(130, renderHeight + 30), 15, 1, Color.White);

                        Raylib.DrawRectangleLines(280, renderHeight + menuHeight / 2, Raylib.GetScreenWidth() - 285, 11, Color.White);
                        Raylib.DrawRectangle(280, renderHeight + menuHeight / 2, Raylib.GetScreenWidth() - 300, 10, Color.DarkGreen);

                        string remainingTimeString = "Estimated Remaining Time: 20s";
                        Raylib.DrawTextEx(font, remainingTimeString, new Vector2(Raylib.GetScreenWidth() / 2, renderHeight + menuHeight / 4), 15, 1, Color.White);

                        saveButton.Y = renderHeight + menuHeight / 2 - saveButton.Height / 2;

                    }
                }
            }

            this.guiContainer.Draw();

            Raylib.EndDrawing();
            Raylib.UnloadTexture(texture);
        }
    }
}