using System.Runtime.CompilerServices;

namespace DissertationThemes.ImporterApp
{
    internal class Program
    {
        static int Main(string[] args)
        {
            bool clearDb = false;
            string? path = string.Empty;

            foreach (string arg in args)
            {
                if (arg == "-r" || arg == "--remove-previous-data")
                {
                    clearDb = true;
                }
                else if(File.Exists(arg))
                {
                    path = arg;
                }
            }
            if (path == string.Empty)
            {
                Console.WriteLine("Enter path for csv file:");
                path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("Path does not exists!");
                    return 1;
                }
            }

            var parseResult = CsvParser.ParseFileAndSaveToDb(path, clearDb);
            if (!parseResult)
            {
                Console.WriteLine("There was some problem during importing to the DB. Not all data were imported correctly!");
                return 1;
            }
            return 0;
        }
    }
}
