using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ot_Sims_Givebox.helper
{
    public static class Levenshtein
    {

       public static int ComputeDistance(string a, string b, bool caseSensitive)
        {
            if (!caseSensitive)
            {
                a = a.ToLower();
                b = b.ToLower();
            }

            int m = a.Length;
            int n = b.Length;
            int[,] d = new int[m + 1, n + 1];

            for (int i = 0; i <= m; i++)
                d[i, 0] = i;
            for (int i = 0; i <= n; i++)
                d[0, i] = i;

            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    int cost;
                    if (a[i - 1] == b[j - 1])
                        cost = 0;
                    else
                        cost = 1;
                    d[i, j] = Min(d[i - 1, j] + 1,
                                    d[i, j - 1] + 1,
                                    d[i - 1, j - 1] + cost);
                }
            }

            return d[m, n];
        }

      public static double ComputeCorrelation(string a, string b, bool caseSensitive)
        {
            int distance = ComputeDistance(a, b, caseSensitive);
            int longest = Max(a.Length, b.Length);
            return 1 - (double)distance / longest;
        }

        private static T Min<T>(T arg0, params T[] args) where T : IComparable
        {
            T min = arg0;
            for (int i = 0; i < args.Length; i++)
            {
                T x = args[i];
                if (x.CompareTo(min) < 0)
                    min = x;
            }
            return min;
        }

        private static T Max<T>(T arg0, params T[] args) where T : IComparable
        {
            T max = arg0;
            for (int i = 0; i < args.Length; i++)
            {
                T x = args[i];
                if (x.CompareTo(max) > 0)
                    max = x;
            }
            return max;
        }
    }
}