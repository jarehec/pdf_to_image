using System;
using System.IO;
using ImageMagick;

namespace PDFToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: ./pdf_to_image <pdf>");
                return;
            } else if (!File.Exists(args[0]))
            {
                Console.WriteLine("Could not find file: " + args[0]);
                return;
            }

            string b64Img;
            FileStream file;
            MagickReadSettings settings = new MagickReadSettings();

            settings.Density = new Density(300, 300);
            using (MagickImageCollection images = new MagickImageCollection())
            {
                int page_number = 1;
                images.Read(args[0], settings);
                foreach (MagickImage image in images)
                {
                    image.Format = MagickFormat.Jpg;
                    b64Img = image.ToBase64();
                    file = new FileStream(args[0] + "_" + page_number + ".txt", FileMode.Create);
                    for (int i = 0; i < b64Img.Length; i++)
                        file.WriteByte((byte)b64Img[i]);
            
                    ++page_number;
                }
            }
        }
    }
}