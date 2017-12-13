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
            const string Nte = "n-t-e-3-";
            Dictionary<string, List<string>> commands = new Dictionary<string, List<string>>();
        
            if (isValid)
            {
                if (options.Count > 0)
                {
                    try
                    {
                        string[] files = options.FileName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string file in files)
                        {
                            var fileName = string.Format($"{Directory.GetCurrentDirectory()}\\{ExampleFileFolder}\\{file.Trim()}");

                            using (StreamReader r = new StreamReader(fileName))
                            {
                                string Hl7String = r.ReadToEnd();
                                commands[file] = Hl7String.Split(new string[] { "\r\n\r\n" },
                                    StringSplitOptions.RemoveEmptyEntries).ToList();
                            }
                        }
                        bool exists = Directory.Exists(ResultFileFolder);
                        if (!exists)
                        {
                            Directory.CreateDirectory(ResultFileFolder);
                        }

                        string d = DateTime.Now.ToString("yyyyMMdd_HH.mm.ss.fff", CultureInfo.InvariantCulture);
                        string path = $"{ResultFileFolder}\\{files.Count()}_{files[0]}_{options.Count}_{d}.txt";
                        string sqlPath = $"{ResultFileFolder}\\{files.Count()}_{files[0]}_{options.Count}_{d}.sql";

                        StringBuilder sbControlId = new StringBuilder();
                        for (int i = 0; i < options.Count; i++)
                        {
                            foreach (string file in files)
                            {
                                List<string> command = commands[file];
                                List<string> obr3 = Enumerable.Repeat(string.Empty, command.Count).ToList();

                                foreach (string c in command)
                                {
                                    string s = c.Trim();

                                    StringBuilder sb = new StringBuilder(s.Substring(0, FindStringNumberNIndexPosition(s, '|', 9) + 1));
                                    string guid = Guid.NewGuid().ToString("N");
                                    sbControlId.Append($", {guid}");
                                    sb.Append(guid);

                                    if (!s.Contains($"{Environment.NewLine}OBR|") && !s.Contains($"{Environment.NewLine}NTE|"))
                                    {
                                        sb.Append(s.Substring(FindStringNumberNIndexPosition(s, '|', 10)));
                                    }

                                    if (s.Contains($"{Environment.NewLine}OBR|") && !s.Contains($"{Environment.NewLine}NTE|"))
                                    {
                                        sb.Append(s.Substring(FindStringNumberNIndexPosition(s, '|', 10), FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 3) - FindStringNumberNIndexPosition(s, '|', 10) + 1));
                                        sb.Append(Obr);

                                        int index = command.FindIndex(a => a == c);
                                        if (options.Mode == 1)
                                        {
                                            if (index % 2 == 0)
                                            {
                                                obr3[index] = Guid.NewGuid().ToString("N");
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - 1]);
                                            }
                                        }
                                        else
                                        {
                                            if (index < command.Count / 2)
                                            {
                                                obr3[index] = Guid.NewGuid().ToString("N");
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - command.Count / 2]);
                                            }
                                        }
                                        sb.Append(s.Substring(FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 4)));
                                    }

                                    if (!s.Contains($"{Environment.NewLine}OBR|") && s.Contains($"{Environment.NewLine}NTE|"))
                                    {
                                        sb.Append(s.Substring(FindStringNumberNIndexPosition(s, '|', 10), FindStringNumberNIndexPositionInCatalog(s, "NTE", '|', 3) - FindStringNumberNIndexPosition(s, '|', 10) + 1));
                                        sb.Append(Nte);

                                        int index = command.FindIndex(a => a == c);
                                        if (options.Mode == 1)
                                        {
                                            if (index % 2 == 0)
                                            {
                                                obr3[index] = Guid.NewGuid().ToString("N");
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - 1]);
                                            }
                                        }
                                        else
                                        {
                                            if (index < command.Count / 2)
                                            {
                                                obr3[index] = Guid.NewGuid().ToString("N");
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - command.Count / 2]);
                                            }
                                        }
                                        sb.Append(s.Substring(FindStringNumberNIndexPositionInCatalog(s, "NTE", '|', 4)));
                                    }

                                    if (s.Contains($"{Environment.NewLine}OBR|") && s.Contains($"{Environment.NewLine}NTE|"))
                                    {
                                        sb.Append(s.Substring(FindStringNumberNIndexPosition(s, '|', 10), FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 3) - FindStringNumberNIndexPosition(s, '|', 10) + 1));
                                        sb.Append(Obr);

                                        int index = command.FindIndex(a => a == c);
                                        if (options.Mode == 1)
                                        {
                                            if (index % 2 == 0)
                                            {
                                                obr3[index] = Guid.NewGuid().ToString("N");
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - 1]);
                                            }
                                        }
                                        else
                                        {
                                            if (index < command.Count / 2)
                                            {
                                                obr3[index] = Guid.NewGuid().ToString("N");
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - command.Count / 2]);
                                            }
                                        }

                                        sb.Append(s.Substring(FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 4), FindStringNumberNIndexPositionInCatalog(s, "NTE", '|', 3) - FindStringNumberNIndexPositionInCatalog(s, "OBR", '|', 4) + 1));
                                        sb.Append(Nte);

                                        if (options.Mode == 1)
                                        {
                                            if (index % 2 == 0)
                                            {
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - 1]);
                                            }
                                        }
                                        else
                                        {
                                            if (index < command.Count / 2)
                                            {
                                                sb.Append(obr3[index]);
                                            }
                                            else
                                            {
                                                sb.Append(obr3[index - command.Count / 2]);
                                            }
                                        }

                                        sb.Append(s.Substring(FindStringNumberNIndexPositionInCatalog(s, "NTE", '|', 4)));
                                    }

                                    sb.AppendLine();
                                    sb.AppendLine();
                                    File.AppendAllText(path, sb.ToString());

                                }
                            }
                        }
                        File.WriteAllText(sqlPath, sbControlId.ToString().Substring(2));
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
