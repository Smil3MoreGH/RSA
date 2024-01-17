using System;
using System.Numerics;

namespace RSA
{
    public class MillerRabin
    {
        // Nutzung der Klasse 'Random' für die Erzeugung zufälliger Zahlen
        private static Random random = new Random();

        // Methode zur Überprüfung, ob eine Zahl eine Primzahl ist, mit dem Miller-Rabin-Primzahltest
        public static bool IsPrime(BigInteger n, int k = 5)
        {
            // Basisfälle: 0, 1 und 4 sind keine Primzahlen; 2 und 3 sind Primzahlen
            if (n <= 1 || n == 4) return false;
            if (n <= 3) return true;

            // Zerlegung von (n - 1) in 2^r * d
            // Suche nach r und d, sodass n = 2^r * d + 1 ist, wobei r >= 1
            BigInteger d = n - 1;
            int r = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                r++;
            }

            // Schleife zur Überprüfung der Primzahleigenschaft von 'n' mit 'k' zufälligen Zeugen
            for (int i = 0; i < k; i++)
            {
                // Generierung einer zufälligen Zahl 'a' im Bereich [2, n-2]
                BigInteger a = 2 + (BigInteger)(random.NextDouble() * (double)(n - 4));

                // Berechnung von x = a^d % n
                BigInteger x = BigInteger.ModPow(a, d, n);

                // Wenn x gleich 1 oder n-1 ist, gehe zur nächsten Iteration
                if (x == 1 || x == n - 1)
                    continue;

                // Quadrieren von x und Reduzierung modulo n
                for (int j = 0; j < r - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1) return false; // 'n' ist zusammengesetzt
                    if (x == n - 1) break; // Wechsel zum nächsten Zeugen
                }

                // Wenn x nicht gleich n-1 ist, dann ist 'n' zusammengesetzt
                if (x != n - 1)
                    return false;
            }

            // Wenn keiner der Zeugen 'n' als zusammengesetzt beweist, dann ist 'n' wahrscheinlich eine Primzahl
            return true;
        }
    }
}
