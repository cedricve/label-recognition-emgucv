using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EgmuTesting
{
    public class LGD
    {
        public string str1;
        public string str2;
        public int length = 0;
        public bool isgelijk = false;

        public LGD() { 
        }

        public LGD(string str1, string str2)
        {
            Equal(str1, str2);
        }

        public void Equal(string str1, string str2) {
            this.str1 = str1;
            this.str2 = str2;
            // Bereken langst gemeenschappelijke deelstring
            if (str1 != null && str2 != null && str1.Length > 0 && str2.Length > 0)
                Match();
        }

        public void Match()
        {

            // Voorbeeld
            // Bereken langst gemeenschappelijke deelstring van "0003" en "00003"
            int[,] table = new int[str1.Length + 1, str2.Length + 1];
            // Initialiseer
            //
            //   | X 0 0 0 3
            // -------------
            // X | 0 0 0 0 0
            // 0 | 0
            // 0 | 0
            // 0 | 0
            // 0 | 0
            // 3 | 0

            for (int i = 0; i < str1.Length + 1; i++)
                table[i, 0] = 0;

            for (int j = 0; j < str2.Length + 1; j++)
                table[0, j] = 0;

            // Bereken lengte van langst gemeenschappelijke deelstring
            // lengte is [4] en komt steeds op het element (laatste kol, laatste rij) van
            // de matrix
            //
            //   | X 0 0 0 3
            // -------------
            // X | 0 0 0 0 0
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

            // Als de twee strings gelijk zijn hebben we
            // een match en verhogen we de teller.
            isgelijk = (this.length == str2.Length);
            
        }
    }
}
