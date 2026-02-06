using System.Numerics;
using Raylib_cs;

namespace CSRayTracer
{
    class UI
    {
        public int width;
        public int height;
        
        public Raylib_cs.Color[] pixels;
        private int rows = 1;
        public UI()
        {
			Raylib.InitWindow(800, 600, "CSRayTracer");
			Raylib.SetTargetFPS(60);
            this.width = 400;
			this.height = this.width / (16 / 9);

            pixels = new Raylib_cs.Color[width*(int)height];
        }

        public void AppendRow(Raylib_cs.Color[] pixels)
        {
            rows++;
            pixels.CopyTo(this.pixels, width * rows);
        }
        public void Draw()
        {
            // Fill it with something visible
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
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
                    Texture2D texture = Raylib.LoadTextureFromImage(img);

                    float scale = (float)Raylib.GetScreenWidth() / width;
                    Raylib.DrawTextureEx(texture, new Vector2(0, 0), 0f, scale, Color.White);

                    Raylib.UnloadTexture(texture);
                }
            }
            Raylib.EndDrawing();
        }
    }
}