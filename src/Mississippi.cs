using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mississippi_Console
{
    class Mississippi
    {
        static void Main(string[] args)
        {
            //Deklarationen
            string input, temp;
            int k, l, size_of_substring, max_size_of_substring;
            Dictionary<string, short> substrings;
            Stopwatch stopwatch;

            Console.Write("Mississippi\n---------------------------------\nFindet einzigartige Teilzeichenketten eines Textes\n\n - Filtert Teilzeichenketten anderer gleich häufiger Teilzeichenketten\n - Minimale Häufigkeit und minimale Länge sind anzugeben");
            while (true)
            {
                //Initialisierung
                substrings = new Dictionary<string, short>();
                k = 0;
                l = 0;

                //Texteingabe
                Console.Write("\n---------------------------------\n\nSchritt 1:\nDen zu verarbeitenden Text eingeben:\n");
                Console.SetIn(new StreamReader(Console.OpenStandardInput(8192), Console.InputEncoding, false, 8192));
                input = Console.ReadLine();
                input = System.Text.RegularExpressions.Regex.Replace(input, "[^a-zA-Z0-9]+", string.Empty, System.Text.RegularExpressions.RegexOptions.Compiled);
                input.ToUpper();
                Console.WriteLine(input.Length);
                //Parametereingabe
                Console.Write("\nSchritt 2:\nDie Häufigkeit k und die minimale Länge l eingeben (durch Leerzeichen getrennt):\n");
                string[] tmp = Console.ReadLine().Split();
                try
                {
                    if (!int.TryParse(tmp[0], out k)) Console.WriteLine("Nur ganze Zahlen!");
                    if (!int.TryParse(tmp[1], out l)) Console.WriteLine("Nur ganze Zahlen!");
                }
                catch (Exception)
                {
                    Console.WriteLine("Falsch formatierte Eingaben!");
                    continue;
                }

                size_of_substring = l == 0 ? 1 : l; //Länge des Substrings beginnt bei l (oder 1)

                Console.WriteLine("\nRechne mit k = " + k + " und l = " + l + "...");

                //Stoppuhr
                stopwatch = Stopwatch.StartNew();

                /*
                    Hauptprozedur
                */

                max_size_of_substring = input.Length / 2 + 1; //+ 1 für ungerade Strings ("abcde")

                for (int i = 0; i < input.Length; i++)
                {
                    while (size_of_substring <= max_size_of_substring) //der Substring wird bei jeder Iteration um 1 verlängert
                    {
                        temp = input.Substring(i, size_of_substring); //Substring von der Position i der Rahmenschleife aus

                        //Hinzufügen des Substrings zum Dictionary
                        if (substrings.ContainsKey(temp))
                            substrings[temp]++;
                        else
                            substrings.Add(temp, 1);

                        size_of_substring++; //Inkrement der Substringlänge
                    }

                    //gegen Ende des Eingabetextes muss die Substringlänge gestutzt werden (sonst: out of range)
                    if (i + max_size_of_substring + 1 > input.Length)
                        max_size_of_substring--;

                    //Reset der Minimiallänge
                    size_of_substring = l == 0 ? 1 : l;
                }
                //Hauptprozedur Ende

                //löscht Substrings, die Teil anderer gleich häufiger Substrings sind
                filterInvalidSubstrings(ref substrings, ref k);

                stopwatch.Stop();

                //Ausgabe
                output(ref substrings, ref k);
                Console.WriteLine("Verstrichene Zeit: " + stopwatch.ElapsedMilliseconds / 1000 + " Sekunde(n)");
            }
        }

        //löscht Substrings, die Teil anderer gleich häufiger Substrings sind
        static void filterInvalidSubstrings(ref Dictionary<string, short> arr, ref int k)
        {
            List<string> keysToDelete = new List<string>();

            foreach (var item in arr)
            {
                if (item.Value < k)
                {
                    keysToDelete.Add(item.Key);
                    continue;
                }
                foreach (var stack in arr)
                {
                    if (item.Key != stack.Key && stack.Key.Contains(item.Key) && item.Value == stack.Value)
                    {
                        keysToDelete.Add(item.Key);
                        break;
                    }
                }
            }
            foreach (string s in keysToDelete)
            {
                arr.Remove(s);
            }
        }

        //Ausgabe
        static void output(ref Dictionary<string, short> arr, ref int k)
        {
            if (arr.Count == 0)
                Console.WriteLine("Keine Ergebnisse!");
            else
            {
                Console.WriteLine("\n\nErgebnisse:");
                foreach (var v in arr)
                {
                    if (v.Value >= k) //Kontrolle der Häufigkeit
                        Console.WriteLine(v);
                }
            }
        }
    }
}
