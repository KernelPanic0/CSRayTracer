
using Raylib_cs;

namespace CSRayTracer
{
    class Util
    {
        public static void writeImage(Raylib_cs.Color[] pixels, int width, int height)
        {
            WriteMetadata(width, height);

            StreamWriter writer = new StreamWriter("./out/render.ppm", true);
            for (int i = 0; i < pixels.Length; i++)
            {
                Color pixel = pixels[i];
                writer.WriteLine($"{pixel.R} {pixel.G} {pixel.B}");
            }

            writer.Close();
        }
        private static void WriteMetadata(int width, double height)
        {
            using (StreamWriter writer = new StreamWriter("./out/render.ppm"))
            {
                writer.Write($"P3\n{width.ToString()} {height.ToString()}\n255\n");
                /// Prefixes the file with the following necesarry metadata for the file format to understand the image 
                /// P3      <- Declares that colours are in ASCII
                /// 720 405 <- Declares resoltuoin
                /// 255     <- Declares colour range
            }
        }
    }
}