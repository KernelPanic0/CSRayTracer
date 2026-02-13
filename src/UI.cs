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
        private int windowWidth = 800;
        public UI(int width)
        {
            this.width = width;
            this.height = (int)(this.width / (16.0 / 9.0));

            Raylib.SetConfigFlags(Raylib_cs.ConfigFlags.ResizableWindow);
            // Raylib.SetTraceLogLevel(TraceLogLevel.Error);
            Raylib.InitWindow(windowWidth, (int)(windowWidth / (16.0 / 9.0)) + 40, "CSRayTracer");
            Raylib.SetTargetFPS(60);

            this.guiContainer = new RayGUI_cs.GuiContainer();

            RayGUI_cs.Button saveBtn = new RayGUI_cs.Button(0, 0, 120, 20, "Save to file");
            this.guiContainer.Add("saveButton", saveBtn);

            pixels = new Raylib_cs.Color[width * (int)height + width * 10];
        }

        public void AppendRow(Raylib_cs.Color[] pixels)
        {
            pixels.CopyTo(this.pixels, width * rows);
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
                            Height = rows,
                            Format = PixelFormat.UncompressedR8G8B8A8,
                            Mipmaps = 1
                        };
                        texture = Raylib.LoadTextureFromImage(img);

                        float scale = (float)Raylib.GetScreenWidth() / width;
                        Raylib.DrawTextureEx(texture, new Vector2(0, 0), 0f, scale, Color.White);

                        Raylib.DrawText("Options", 10, (int)((float)height * scale) + 2, 5, Color.Green);

                        guiContainer["saveButton"].Y = (int)((float)height * scale) + 20;
                    }
                }
            }

            this.guiContainer.Draw();

            Raylib.EndDrawing();
            Raylib.UnloadTexture(texture);
        }
    }
}