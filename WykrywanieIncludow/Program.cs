using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WykrywanieIncludow
{
    
    class Program
    {
        public static List<List<(Char, int, bool)>> STATES = new List<List<(char, int, bool)>>
        {
            new List<(char, int, bool)> { ('#', 1, true) },
            new List<(char, int, bool)> { ('i', 2, true) },
            new List<(char, int, bool)> { ('n', 3, true) },
            new List<(char, int, bool)> { ('c', 4, true) },
            new List<(char, int, bool)> { ('l', 5, true) },
            new List<(char, int, bool)> { ('u', 6, true) },
            new List<(char, int, bool)> { ('d', 7, true) },
            new List<(char, int, bool)> { ('e', 8, true) },
            new List<(char, int, bool)> {
                (' ', 8, true), ('"', 9, true), ('_', 10, true), ('<', 11, true) }, //8
            new List<(char, int, bool)> { ('"', 12, false) }, // 9
            new List<(char, int, bool)> { ('_', 13, false) }, // 10
            new List<(char, int, bool)> { ('>', 14, false) }, // 11
            new List<(char, int, bool)> {
                ('>', 12, false), ('_', 12, false), ('"', 15, true)}, // 12
            new List<(char, int, bool)> {
                ('"', 13, false), ('>', 13, false), ('_', 15, true)}, // 13
            new List<(char, int, bool)> {
                ('_', 14, false), ('"', 14, false), ('>', 15, true)}, // 14
            new List<(char, int, bool)> { } // 15
        };


        private static string _filePath = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);

        static void Main(string[] args)
        {
            _filePath = Directory.GetParent(_filePath).FullName;
            String path = Directory.GetParent(Directory.GetParent(Directory.GetParent(_filePath).FullName).FullName).FullName;

            if (args.Length > 0)
            {
                path += "\\" + args[0];
            }

            using (StreamReader sr = File.OpenText(path))
            {
                int counter = 0;
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    counter++;
                    string m = GetMatch(line);
                    if (m == null)
                    {
                        Console.WriteLine("Dla lini nr {1} wystąpił błąd: \n\"{0}\"\n ", line, counter);
                    }
                    else
                    {
                        int posA = m.IndexOf('.');
                        if (posA != -1)
                        {
                            m = m.Substring(0, posA);
                        }
                        Console.WriteLine("{0}\n", m);
                    }
                }
            }

            Console.ReadKey();
        }

        private static string GetMatch(string line)
        {
            int state = 0;
            int from = 0;

            for (int i = 0; i < line.Length; i++)
            {
                int new_state = -1;
                if ((new_state = NextState(line[i], state)) != -1)
                {
                    state = new_state;
                }
                else
                {
                    return null;
                }

                if (state == 9 || state == 10 || state == 11)
                {
                    from = i + 1;
                }
                else if (state == 15)
                {
                    return line.Substring(from, i - from);
                }
            }
            return null;
        }

        private static int NextState(char c, int state)
        {
            var Transitions = STATES[state];
            foreach (var item in Transitions.Where(testc => testc.Item3).AsEnumerable())
            {
                if (item.Item1 == c)
                {
                    return item.Item2;
                }
            }
            foreach (var item in Transitions.Where(testc => !testc.Item3).AsEnumerable())
            {
                if (item.Item1 != c)
                {
                    return item.Item2;
                }
            }
            return -1;
        }
    }    
}
