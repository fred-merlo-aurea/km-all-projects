using System;
using System.Collections.Generic;
using System.Linq;

namespace Core_AMS.Utilities
{
    public class FuzzySearch
    {
        /// <summary>
        /// In the search process, for each word in the wordlist, the Levenshtein-distance is computed, and with this distance, a sCore_AMS. 
        /// This sCore_AMS represents how good the strings match. The input argument fuzzyness determines how much the strings can differ.
        /// </summary>
        /// <param name="searchForWord"></param>
        /// <param name="wordList"></param>
        /// <param name="fuzzyness"></param>
        /// <returns></returns>
        public List<string> Search(string searchForWord, List<string> searchList, double fuzzyness)
        {
            List<string> foundWords = new List<string>();

            foreach (string s in searchList)
            {
                // Calculate the Levenshtein-distance:
                int levenshteinDistance =
                    LevenshteinDistance(searchForWord, s);

                // Length of the longer string:
                int length = Math.Max(searchForWord.Length, s.Length);

                // Calculate the sCore_AMS:
                double sCore_AMS = 1.0 - (double)levenshteinDistance / length;

                // Match?
                if (sCore_AMS > fuzzyness)
                    foundWords.Add(s);
            }
            return foundWords;
        }
        public int Search(string src, string dest)
        {
            int levenshteinDistance = LevenshteinDistance(src, dest);
            // Length of the longer string:
            int length = Math.Max(src.Length, dest.Length);
            // Calculate the sCore_AMS:
            decimal dist = (decimal)levenshteinDistance / (decimal)length;
            int convert = (int)(dist * 100);
            int sCore_AMS = 100 - convert;
            return sCore_AMS;
        }
        private int LevenshteinDistance(string src, string dest)
        {
            int[,] d = new int[src.Length + 1, dest.Length + 1];
            int i, j, cost;
            char[] str1 = src.ToCharArray();
            char[] str2 = dest.ToCharArray();

            for (i = 0; i <= str1.Length; i++)
            {
                d[i, 0] = i;
            }
            for (j = 0; j <= str2.Length; j++)
            {
                d[0, j] = j;
            }
            for (i = 1; i <= str1.Length; i++)
            {
                for (j = 1; j <= str2.Length; j++)
                {

                    if (str1[i - 1] == str2[j - 1])
                        cost = 0;
                    else
                        cost = 1;

                    d[i, j] =
                        Math.Min(
                            d[i - 1, j] + 1,              // Deletion
                            Math.Min(
                                d[i, j - 1] + 1,          // Insertion
                                d[i - 1, j - 1] + cost)); // Substitution

                    if ((i > 1) && (j > 1) && (str1[i - 1] ==
                        str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
                    {
                        d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
                    }
                }
            }

            return d[str1.Length, str2.Length];
        }
    }
}
