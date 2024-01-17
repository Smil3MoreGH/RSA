using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace RSA
{
    internal class Program
    {
        // Globale Variablen für die RSA-Algorithmusparameter deklarieren
        static BigInteger p, q, n, phi, e, d;
        static byte[] msgBytes; // Byte-Darstellung der Eingabenachricht
        static List<BigInteger> en = new List<BigInteger>(); // Liste zur Speicherung verschlüsselter Bytes
        static List<BigInteger> de = new List<BigInteger>(); // Liste zur Speicherung entschlüsselter Bytes

        // Methode zur Generierung und Auswahl einer Primzahl
        static BigInteger GenerateAndSelectPrime(string label)
        {
            List<BigInteger> primeOptions = new List<BigInteger>();

            // 3 große Primzahlen generieren
            Console.WriteLine($"Generiere 2 große Primzahlen für {label}...");
            for (int i = 0; i < 2; i++)
            {
                BigInteger prime = BigPrimeGenerator.Generate1024BitPrime();
                primeOptions.Add(prime);
                Console.WriteLine($"{i + 1}. {prime}");
            }

            // Benutzer erlauben, eine der generierten Primzahlen auszuwählen
            Console.WriteLine($"Wähle eine Primzahl für {label} (1-2):");
            int choice = int.Parse(Console.ReadLine());
            return primeOptions[choice - 1];
        }

        // Methode zur Berechnung von 'e' und 'd' für den RSA-Algorithmus
        static void CalculateEd()
        {
            for (BigInteger i = 2; i < phi; i++)
            {
                if (phi % i == 0) continue; // Überspringen, wenn 'i' 'phi' teilt
                if (MillerRabin.IsPrime(i) && i != p && i != q) // Überprüfen, ob 'i' prim ist und nicht gleich 'p' oder 'q'
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

        // Methode zur Berechnung von 'd' für den RSA-Algorithmus
        static BigInteger CalculateD(BigInteger a)
        {
            for (BigInteger k = 1 + phi; ; k += phi)
                if (k % a == 0) return k / a;
        }

        // Methode zur Verschlüsselung der Nachricht
        static void Encrypt()
        {
            Console.WriteLine("\nBeginne Verschlüsselungsprozess...");

            foreach (byte b in msgBytes)
            {
                BigInteger pt = new BigInteger(b); // Byte in BigInteger konvertieren
                Console.WriteLine($"Original Byte: {pt}");

                // Byte mit RSA-Formel verschlüsseln
                BigInteger ct = BigInteger.ModPow(pt, e, n);
                Console.WriteLine($"Verschlüsseltes Byte: {ct}");

                en.Add(ct); // Verschlüsseltes Byte zur Liste hinzufügen
            }

            // Die verschlüsselten Bytes anzeigen
            Console.WriteLine("\nDie verschlüsselten Bytes sind:");
            foreach (var c in en)
            {
                Console.Write($"{c} ");
            }
            Console.WriteLine();
        }

        // Methode zur Entschlüsselung der verschlüsselten Nachricht
        static void Decrypt()
        {
            Console.WriteLine("\nBeginne Entschlüsselungsprozess...");

            foreach (BigInteger ct in en)
            {
                Console.WriteLine($"Verschlüsseltes Byte: {ct}");

                // Byte mit RSA-Formel entschlüsseln
                BigInteger pt = BigInteger.ModPow(ct, d, n);
                Console.WriteLine($"Entschlüsseltes Byte: {pt}");

                de.Add(pt); // Entschlüsseltes Byte zur Liste hinzufügen
            }

            // Die entschlüsselten Bytes zurück in einen String konvertieren
            byte[] decryptedBytes = de.ConvertAll(b => (byte)b).ToArray();
            string decryptedMessage = Encoding.UTF8.GetString(decryptedBytes);

            Console.WriteLine($"\nDie entschlüsselte Nachricht lautet: {decryptedMessage}");
        }

        // Hauptmethode
        static void Main(string[] args)
        {
            // Auswahl und Generierung der beiden Primzahlen p und q für RSA
            p = GenerateAndSelectPrime("p");
            do
            {
                q = GenerateAndSelectPrime("q");
            } while (q == p); // Sicherstellen, dass 'p' und 'q' unterschiedlich sind, um die Sicherheit zu gewährleisten

            // Eingabeaufforderung für die Nachricht, die verschlüsselt werden soll
            Console.WriteLine("Gib eine Nachricht ein:");
            string msg = Console.ReadLine();
            msgBytes = Encoding.UTF8.GetBytes(msg); // Konvertierung der Nachricht in ein Byte-Array

            // Berechnung des öffentlichen Schlüssels n (Produkt der beiden Primzahlen)
            n = p * q; // Berechnung von 'n' als Produkt von 'p' und 'q'
            phi = (p - 1) * (q - 1); // Berechnung der Eulerschen Phi-Funktion für 'n'

            // Aufruf der Methode zur Berechnung der RSA-Schlüssel 'e' und 'd'
            CalculateEd(); // Berechnung der Werte 'e' und 'd' für die Schlüsselpaare

            // Ausgabe der generierten Schlüssel
            Console.WriteLine($"Öffentlicher Schlüssel (e, n): ({e}, {n})");
            Console.WriteLine($"Privater Schlüssel (d, n): ({d}, {n})");

            // Verschlüsselung der eingegebenen Nachricht
            Encrypt(); // Start des Verschlüsselungsprozesses

            // Entschlüsselung der verschlüsselten Nachricht
            Decrypt(); // Start des Entschlüsselungsprozesses
        }

    }
}