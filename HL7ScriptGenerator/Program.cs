using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7ScriptGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new CmdOptions();
            var isValid = CommandLine.Parser.Default.ParseArgumentsStrict(args, options);

            const string ExampleFileFolder = "ExampleFiles";
            const string ResultFileFolder = "ResultFiles";
            const string Obr = "o-b-r-3-";
            List<string> command;
        
            if (isValid)
            {
                if (options.Count > 0)
                {
                    try
                    {
                        var fileName = string.Format($"{Directory.GetCurrentDirectory()}\\{ExampleFileFolder}\\{options.FileName}");

                        using (StreamReader r = new StreamReader(fileName))
                        {
                            string Hl7String = r.ReadToEnd();
                            command = Hl7String.Split(new string[] { "\r\n\r\n" },
                                StringSplitOptions.RemoveEmptyEntries).ToList();
                        }

                        bool exists = Directory.Exists(ResultFileFolder);
                        if (!exists)
                        {
                            Directory.CreateDirectory(ResultFileFolder);
                        }
                        string path = $"{ResultFileFolder}\\{options.FileName}_{options.Count}_{DateTime.Now.ToString("yyyyMMdd_HH.mm.ss.fff", CultureInfo.InvariantCulture)}.txt";
                        for (int i = 0; i < options.Count; i++)
                        {
                            foreach (string c in command)
                            {
                                string s = c.Trim();

                                StringBuilder sb = new StringBuilder(s.Substring(0, FindStringNumberNIndexPosition(s, '|', 9) + 1));
                                sb.Append(Guid.NewGuid().ToString("N"));
                                sb.Append(s.Substring(FindStringNumberNIndexPosition(s, '|', 10), FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 3) - FindStringNumberNIndexPosition(s, '|', 10) + 1));
                                sb.Append(Obr);
                                sb.Append(Guid.NewGuid().ToString("N"));
                                sb.Append(s.Substring(FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 4)));
                                sb.AppendLine();
                                sb.AppendLine();
                                File.AppendAllText(path, sb.ToString());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Count must greater than 0.");
                }
            }
            else
            {
                // Display the default usage information
                Console.WriteLine(options.GetUsage());
            }
        }

        private static int FindStringNumberNIndexPosition(string s, char c, int number)
        {
            int startIndex = -1;
            int hitCount = 0;

            // Search for all occurrences of the target.
            while (hitCount < number)
            {
                startIndex = s.IndexOf(
                    c, startIndex + 1,
                    s.Length - startIndex - 1);

                // Exit the loop if the target is not found.
                if (startIndex < 0)
                    break;

                hitCount++;
            }
            return startIndex;
        }

        private static int FindStringNumberNIndexPositionInCatalog(string s, string catalog, char c, int number)
        {
            int startIndex = s.IndexOf(catalog);
            return startIndex + FindStringNumberNIndexPosition(s.Substring(startIndex), '|', number);
        }
    }
}
