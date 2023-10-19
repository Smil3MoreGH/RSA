using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSA
{
    internal class Program
    {
        // Declare global variables for RSA algorithm parameters
        static BigInteger p, q, n, phi, e, d;
        static byte[] msgBytes; // Byte representation of the input message
        static List<BigInteger> en = new List<BigInteger>(); // List to store encrypted bytes
        static List<BigInteger> de = new List<BigInteger>(); // List to store decrypted bytes

        // Method to generate and select a prime number
        static BigInteger GenerateAndSelectPrime(string label)
        {
            List<BigInteger> primeOptions = new List<BigInteger>();

            // Generate 3 large prime numbers
            Console.WriteLine($"Generating 3 large prime numbers for {label}...");
            for (int i = 0; i < 3; i++)
            {
                BigInteger prime = BigPrimeGenerator.Generate1024BitPrime();
                primeOptions.Add(prime);
                Console.WriteLine($"{i + 1}. {prime}");
            }

            // Allow user to select one of the generated primes
            Console.WriteLine($"Choose a prime number for {label} (1-3):");
            int choice = int.Parse(Console.ReadLine());
            return primeOptions[choice - 1];
        }

        // Method to calculate 'e' and 'd' for RSA algorithm
        static void CalculateEd()
        {
            for (BigInteger i = 2; i < phi; i++)
            {
                if (phi % i == 0) continue; // Skip if 'i' divides 'phi'
                if (MillerRabin.IsPrime(i) && i != p && i != q) // Check if 'i' is prime and not equal to 'p' or 'q'
                {
                    e = i;
                    d = CalculateD(e);
                    if (d > 0)
                    {
                        break;
                    }
                }
            }
        }

        // Method to calculate 'd' for RSA algorithm
        static BigInteger CalculateD(BigInteger a)
        {
            for (BigInteger k = 1 + phi; ; k += phi)
                if (k % a == 0) return k / a;
        }

        // Method to encrypt the message
        static void Encrypt()
        {
            Console.WriteLine("\nStarting Encryption Process...");

            foreach (byte b in msgBytes)
            {
                BigInteger pt = new BigInteger(b); // Convert byte to BigInteger
                Console.WriteLine($"Original Byte: {pt}");

                // Encrypt byte using RSA formula
                BigInteger ct = BigInteger.ModPow(pt, e, n);
                Console.WriteLine($"Encrypted Byte: {ct}");

                en.Add(ct); // Add encrypted byte to list
            }

            // Display the encrypted bytes
            Console.WriteLine("\nThe Encrypted Bytes Are:");
            foreach (var c in en)
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine();
        }

        // Method to decrypt the encrypted message
        static void Decrypt()
        {
            Console.WriteLine("\nStarting Decryption Process...");

            foreach (BigInteger ct in en)
            {
                Console.WriteLine($"Encrypted Byte: {ct}");

                // Decrypt byte using RSA formula
                BigInteger pt = BigInteger.ModPow(ct, d, n);
                Console.WriteLine($"Decrypted Byte: {pt}");

                de.Add(pt); // Add decrypted byte to list
            }

            // Convert decrypted bytes back to string
            byte[] decryptedBytes = de.ConvertAll(b => (byte)b).ToArray();
            string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);

            Console.WriteLine($"\nThe Decrypted Message Is: {decryptedMessage}");
        }

        // Main method
        static void Main(string[] args)
        {
            p = GenerateAndSelectPrime("p");
            do
            {
                q = GenerateAndSelectPrime("q");
            } while (q == p); // Ensure 'p' and 'q' are distinct

            Console.WriteLine("Enter Message:");
            string msg = Console.ReadLine();
            msgBytes = Encoding.UTF8.GetBytes(msg); // Convert message to bytes

            n = p * q; // Calculate 'n'
            phi = (p - 1) * (q - 1); // Calculate 'phi'

            CalculateEd(); // Calculate 'e' and 'd'

            // Display the generated keys
            Console.WriteLine($"Public Key (e, n): ({e}, {n})");
            Console.WriteLine($"Private Key (d, n): ({d}, {n})");

            Encrypt(); // Encrypt the message
            Decrypt(); // Decrypt the message
        }
    }
}