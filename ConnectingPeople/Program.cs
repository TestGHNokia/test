using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConnectingPeople
{
    class Program
    {
        static Dictionary<int, Dictionary<string, char>> dict = new Dictionary<int, Dictionary<string, char>>
            {
                { '0', new Dictionary<string, char>
                    {
                        { "0", ' ' },
                        { "00", '0' }
                    }
                },
                { '1', new Dictionary<string, char>
                    {
                        { "1", '1' }
                    }
                },
                { '2', new Dictionary<string, char>
                    {
                        { "2", 'A' },
                        { "22", 'B' },
                        { "222", 'C' },
                        { "2222", '2' }
                    }
                },
                { '3', new Dictionary<string, char>
                    {
                        { "3", 'D' },
                        { "33", 'E' },
                        { "333", 'F' },
                        { "3333", '3' }
                    }
                },
                { '4', new Dictionary<string, char>
                    {
                        { "4", 'G' },
                        { "44", 'H' },
                        { "444", 'I' },
                        { "4444", '4' }
                    }
                },
                { '5', new Dictionary<string, char>
                    {
                        { "5", 'J' },
                        { "55", 'K' },
                        { "555", 'L' },
                        { "5555", '5' }
                    }
                },
                { '6', new Dictionary<string, char>
                    {
                        { "6", 'M' },
                        { "66", 'N' },
                        { "666", 'O' },
                        { "6666", '6' }
                    }
                },
                { '7', new Dictionary<string, char>
                    {
                        { "7", 'P' },
                        { "77", 'Q' },
                        { "777", 'R' },
                        { "7777", 'S' },
                        { "77777", '7' }
                    }
                },
                { '8', new Dictionary<string, char>
                    {
                        { "8", 'T' },
                        { "88", 'U' },
                        { "888", 'V' },
                        { "8888", '8' }
                    }
                },
                { '9', new Dictionary<string, char>
                    {
                        { "9", 'W' },
                        { "99", 'X' },
                        { "999", 'Y' },
                        { "9999", 'Z' },
                        { "99999", '9' }
                    }
                }
            };

        static void Main(string[] args)
        {
            const int groupDelay = 1;
            char? previousChar = null;

            var currentCode = new List<char>();
            var timer = new Stopwatch();
            timer.Start();


            StringBuilder row = new StringBuilder();

            var position = new Point(Console.CursorLeft, Console.CursorTop);
            while (true)
            {
                var pressedKey = Console.ReadKey(true);
                var pressedSymb = pressedKey.KeyChar;

                switch (pressedSymb)
                {
                    case '#':
                        {
                            timer.Restart();
                            currentCode = new List<char>();
                            Console.WriteLine();
                            row = new StringBuilder();
                        }
                        break;
                    case '*':
                        {
                            Console.SetCursorPosition(0, Console.CursorTop);
                            Console.Write(new string(' ', Console.WindowWidth));
                            Console.SetCursorPosition(0, (int)position.Y);

                            if (row.Length > 0)
                            {
                                row.Remove(row.Length - 1, 1);
                                if (row.Length > 0)
                                {
                                    var prevs = row[row.Length - 1];
                                    row[row.Length - 1] = dict.Where(w => w.Value.Any(ww => ww.Value == prevs)).Select(s => s.Value).FirstOrDefault().Reverse().Skip(1).FirstOrDefault().Value;

                                    Console.Write(row.ToString());
                                }
                            }
                        }
                        break;
                    default:
                        {
                            if (char.IsDigit(pressedSymb))
                            {
                                if ((!(previousChar == pressedSymb) || groupDelay < timer.Elapsed.TotalSeconds))
                                {
                                    currentCode = new List<char>();
                                    position = new Point(Console.CursorLeft, Console.CursorTop);
                                }
                                else
                                {
                                    row = new StringBuilder();
                                    Console.SetCursorPosition((int)position.X, (int)position.Y);
                                }

                                previousChar = pressedSymb;
                                currentCode.Add(pressedSymb);

                                var val = getValue(currentCode.ToArray());
                                row.Append(getValue(currentCode.ToArray()));
                                Console.Write(val);
                                timer.Restart();
                            }
                        }
                        break;
                }
            };
        }

        static char getValue(char[] code)
        {
            var sym = new string(code);

            var d = dict[sym[0]];
            if (d.ContainsKey(sym)) return d[sym];

            var entryMaxLength = d.Keys.Select(k => k.Length).Max();

            var newLenght = sym.Length % entryMaxLength;
            if (0 == newLenght) newLenght = entryMaxLength;

            return d[sym.Substring(0, newLenght)];
        }
    }
}
