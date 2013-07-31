using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recognition
{
    public class LGD
    {
        public string str1;
        public string str2;
        public int length = 0;
        public bool isEqual = false;

        public LGD() { 
        }

        public LGD(string str1, string str2)
        {
            Equal(str1, str2);
        }

        public void Equal(string str1, string str2) {
            this.str1 = str1;
            this.str2 = str2;
            if (str1 != null && str2 != null &&
                str1.Length > 0 && str2.Length > 0)
                Match();
        }

        /// <summary>
        /// Calculate the length of the most common substring of two strings
        /// </summary>
        public void Match()
        {
            // Example
            // "0003" and "00003"
            int[,] table = new int[str1.Length + 1, str2.Length + 1];

            //   |   0 0 0 3
            // -------------
            //   | 0 0 0 0 0
            // 0 | 0
            // 0 | 0
            // 0 | 0
            // 0 | 0
            // 3 | 0

            for (int i = 0; i < str1.Length + 1; i++)
                table[i, 0] = 0;

            for (int j = 0; j < str2.Length + 1; j++)
                table[0, j] = 0;

            //   |   0 0 0 3
            // -------------
            //   | 0 0 0 0 0
            // 0 | 0 1 1 1 1
            // 0 | 0 1 2 2 2
            // 0 | 0 1 2 3 3
            // 0 | 0 1 2 3 3
            // 3 | 0 1 2 3 [4]

            for (int i = 1; i < str1.Length + 1; i++)
            {
                for (int j = 1; j < str2.Length + 1; j++)
                {
                    if (str1[i - 1] == str2[j - 1])
                        table[i, j] = table[i - 1, j - 1] + 1;
                    else
                        table[i, j] = Math.Max(table[i, j - 1], table[i - 1, j]);
                }
            }

            this.length = table[str1.Length, str2.Length];
            isEqual = (this.length == str2.Length);
        }
    }
}
