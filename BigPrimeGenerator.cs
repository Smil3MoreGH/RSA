using System.Numerics;
using System.Security.Cryptography;

namespace RSA
{
    public class BigPrimeGenerator
    {
        // Use RNGCryptoServiceProvider for generating cryptographically secure random numbers
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        // Method to generate a 1024-bit prime number
        public static BigInteger Generate1024BitPrime()
        {
            while (true)
            {
                // Generate a random 1024-bit number
                BigInteger candidate = GenerateRandom1024BitNumber();

                // Check if the generated number is prime using the Miller-Rabin primality test
                if (MillerRabin.IsPrime(candidate))
                    return candidate; // Return the prime number
            }
        }

        // Method to generate a random 1024-bit number
        private static BigInteger GenerateRandom1024BitNumber()
        {
            byte[] bytes = new byte[128]; // 1024 bits = 128 bytes
            rng.GetBytes(bytes); // Fill the byte array with random values

            bytes[0] |= 0x80; // Set the highest bit to ensure the number is 1024 bits long
            bytes[bytes.Length - 1] |= 1; // Set the lowest bit to ensure the number is odd (increases the chance of it being prime)

            // Convert the byte array to a BigInteger and return
            return new BigInteger(bytes);
        }
    }
}