using System.Numerics;
using System.Security.Cryptography;

namespace RSA
{
    public class BigPrimeGenerator
    {
        // Verwendung von RNGCryptoServiceProvider für die Erzeugung kryptografisch sicherer Zufallszahlen
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        // Methode zur Generierung einer 1024-Bit-Primzahl
        public static BigInteger Generate1024BitPrime()
        {
            while (true)
            {
                // Generieren einer zufälligen 1024-Bit-Zahl
                BigInteger candidate = GenerateRandom1024BitNumber();

                // Überprüfen, ob die generierte Zahl mithilfe des Miller-Rabin-Primzahltests eine Primzahl ist
                if (MillerRabin.IsPrime(candidate))
                    return candidate; // Rückgabe der Primzahl
            }
        }

        // Methode zur Generierung einer zufälligen 1024-Bit-Zahl
        private static BigInteger GenerateRandom1024BitNumber()
        {
            byte[] bytes = new byte[128]; // 1024 Bits = 128 Bytes
            rng.GetBytes(bytes); // Füllen des Byte-Arrays mit zufälligen Werten

            bytes[0] |= 0x80; // Setzen des höchsten Bits, um sicherzustellen, dass die Zahl 1024 Bits lang ist
            bytes[bytes.Length - 1] |= 1; // Setzen des niedrigsten Bits, um sicherzustellen, dass die Zahl ungerade ist (erhöht die Chance, dass sie prim ist)

            // Konvertieren des Byte-Arrays in eine BigInteger und Rückgabe
            return new BigInteger(bytes);
        }
    }
}