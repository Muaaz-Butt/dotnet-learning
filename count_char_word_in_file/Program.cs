using System;
using System.IO;

namespace count_char_word_in_file
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "dummyData.txt";

            WriteDummyDataToFile(filePath);

            ReadAndDisplayFileData(filePath);
        }

        static void WriteDummyDataToFile(string filePath)
        {
            string[] lines = { "Muaaz Butt" };

            File.WriteAllLines(filePath, lines);

            Console.WriteLine("Dummy data has been written successfully");
        }

        static void ReadAndDisplayFileData(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);

            int numberOfCharacters = fileContent.Length;
            int numberOfWords = fileContent.Split(new[] { ' ', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;

            
            Console.WriteLine($"Character count: {numberOfCharacters}");
            Console.WriteLine($"Word count: {numberOfWords}");
        }
    }

}