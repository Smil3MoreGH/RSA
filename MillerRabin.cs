using System;
using System.Numerics;

namespace RSA
{
    public class MillerRabin
    {
        // Use the Random class for generating random numbers
        private static Random random = new Random();

        // Method to check if a number is prime using the Miller-Rabin primality test
        public static bool IsPrime(BigInteger n, int k = 5)
        {
            // Base cases: 0, 1, and 4 are not prime; 2 and 3 are prime
            if (n <= 1 || n == 4) return false;
            if (n <= 3) return true;

            // Decompose (n - 1) into 2^r * d
            // Find r and d such that n = 2^r * d + 1 for some r >= 1
            BigInteger d = n - 1;
            int r = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                r++;
            }

            // Witness loop: Test the primality of 'n' using 'k' random witnesses
            for (int i = 0; i < k; i++)
            {
                // Generate a random number 'a' in the range [2, n-2]
                BigInteger a = 2 + (BigInteger)(random.NextDouble() * (double)(n - 4));

                // Compute x = a^d % n
                BigInteger x = BigInteger.ModPow(a, d, n);

                // If x is 1 or n-1, then move to the next iteration
                if (x == 1 || x == n - 1)
                    continue;

                // Square x and reduce modulo n
                for (int j = 0; j < r - 1; j++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == 1) return false; // 'n' is composite
                    if (x == n - 1) break; // Move to the next witness
                }

                // If x is not equal to n-1, then 'n' is composite
                if (x != n - 1)
                    return false;
            }

            // If none of the witnesses proved 'n' to be composite, then 'n' is probably prime
            return true;
        }
    }
}