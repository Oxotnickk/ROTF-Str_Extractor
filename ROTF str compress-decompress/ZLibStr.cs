//This code is OUTDATED

using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

public class ZlibStr
{
    private const int HEADER_SIZE = 20;

    public static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: ZlibStr -c | -d <input_file>");
            return;
        }

        string mode = args[0].ToLower();
        string inputFile = args[1];
        string outputFile = "";

        if (mode == "-c")
        {
            outputFile = Path.GetFileNameWithoutExtension(inputFile) + "_c" + Path.GetExtension(inputFile);
            CompressFile(inputFile, outputFile);
        }
        else if (mode == "-d")
        {
            outputFile = Path.GetFileNameWithoutExtension(inputFile) + "_d" + Path.GetExtension(inputFile);
            DecompressFile(inputFile, outputFile);
        }
        else
        {
            Console.WriteLine("Invalid mode. Please specify '-c' or '-d'.");
        }
    }

    static void CompressFile(string inputFile, string outputFile)
    {
        try
        {
            using (FileStream inputStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
            {
                byte[] headerBytes = new byte[HEADER_SIZE];
                inputStream.Read(headerBytes, 0, HEADER_SIZE);

                outputStream.Write(headerBytes, 0, HEADER_SIZE);

                using (DeflaterOutputStream compressionStream = new DeflaterOutputStream(outputStream))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        compressionStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"Compression successful! File saved as: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during compression: {ex.Message}");
        }
    }

    static void DecompressFile(string inputFile, string outputFile)
    {
        try
        {
            using (FileStream compressedStream = new FileStream(inputFile, FileMode.Open))
            using (FileStream outputStream = new FileStream(outputFile, FileMode.Create))
            {
                byte[] headerBytes = new byte[HEADER_SIZE];
                compressedStream.Read(headerBytes, 0, HEADER_SIZE);

                outputStream.Write(headerBytes, 0, HEADER_SIZE);

                using (InflaterInputStream decompressionStream = new InflaterInputStream(compressedStream))
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = decompressionStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }

                Console.WriteLine($"Decompression successful! File saved as: {outputFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during decompression: {ex.Message}");
        }
    }
}