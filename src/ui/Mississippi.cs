using System;
using System.Collections.Generic;

namespace Mississippi_DLL
{
    public class Mississippi
    {
        public Dictionary<string, ushort> main(string input, int k, int l, bool onlyMaximal)
        {
            string temp;
            int size_of_substring, max_size_of_substring;
            Dictionary<string, ushort> substrings;

            //Initialisierung
            substrings = new Dictionary<string, ushort>();

            input = input.ToUpper();
            input = System.Text.RegularExpressions.Regex.Replace(input, "[^A-ZßÄÖÜ0-9]+", string.Empty, System.Text.RegularExpressions.RegexOptions.Compiled);

            /*
                 Hauptprozedur
            */
            size_of_substring = l; //Länge des Substrings beginnt bei l (oder 1)
            max_size_of_substring = input.Length % 2 == 0 ? input.Length / 2 : input.Length / 2 + 1; //+ 1 für ungerade Strings ("abcde")

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
                size_of_substring = l;
            }
            /*
                 Hauptprozedur Ende
            */

            //löscht Substrings, die Teil anderer gleich häufiger Substrings sind
            if(onlyMaximal)
                filterInvalidSubstrings(ref substrings, ref k);

            return substrings;
        }

        //löscht Substrings, die Teil anderer gleich häufiger Substrings sind
        private void filterInvalidSubstrings(ref Dictionary<string, ushort> arr, ref int k)
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
    }
}
