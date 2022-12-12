using Cryptography.Mathematics;

namespace Cryptography.SubstitutionCiphers
{
    public class CaesarCryptor
    {
        private string alphabet;

        public CaesarCryptor(string alphabet)
        {
            this.alphabet = alphabet;
        }

        public string Encrypt(string openText, int key)
        {
            string cryptoText = string.Empty;
            foreach (var symbol in openText.ToLower())
            {
                if (alphabet.IndexOf(symbol) == -1)
                {
                    cryptoText += symbol;
                    continue;
                }
                char encryptedSymbol = alphabet[(alphabet.IndexOf(symbol) + key) % alphabet.Length];
                cryptoText += encryptedSymbol;
            }
            return cryptoText;
        }

        /// <summary>
        /// a, b is encrypting key. Encrypted alphabet element's index calculated as (a * t + b) % m, where
        /// m = alphabet length;
        /// t = encrypting symbol's index in alphabet.
        /// GCD(a,m) must be = 1.
        /// </summary>
        public string Encrypt(string openText, int a, int b)
        {
            ValidateKey(a, alphabet.Length);
            string cryptoText = string.Empty;
            foreach (var symbol in openText.ToLower())
            {
                if (alphabet.IndexOf(symbol) == -1)
                {
                    cryptoText += symbol;
                    continue;
                }
                int t = (a * alphabet.IndexOf(symbol) + b) % alphabet.Length;
                char encryptedSymbol = alphabet[t];
                cryptoText += encryptedSymbol;
            }
            return cryptoText;
        }

        public string Encrypt(string openText, int key, string keyWord)
        {
            var newAlphabet = GetNewAlphabet(key, keyWord);
            var cryptoText = string.Empty;
            foreach (var symbol in openText.ToLower())
            {
                if (alphabet.IndexOf(symbol) == -1)
                {
                    cryptoText += symbol;
                    continue;
                }
                cryptoText += newAlphabet[alphabet.IndexOf(symbol)];
            }
            return cryptoText;
        }

        public string Decrypt(string cryptoText, int key)
        {
            string openText = string.Empty;
            foreach (var symbol in cryptoText.ToLower())
            {
                if (alphabet.IndexOf(symbol) == -1)
                {
                    openText += symbol;
                    continue;
                }
                char encryptedSymbol = alphabet[(alphabet.IndexOf(symbol) - key + alphabet.Length) % alphabet.Length];
                openText += encryptedSymbol;
            }
            return openText;
        }

        /// <summary>
        /// /// a, b is encrypting k. Decrypted alphabet element's index calculated as a^(-1) * (t - b) % m, where
        /// m = alphabet length;
        /// t = encrypting symbol's index in alphabet.
        /// GCD(a,m) must be = 1. 
        /// </summary>
        public string Decrypt(string cryptoText, int a, int b)
        {
            int m = alphabet.Length;
            ValidateKey(a, m);
            string openText = string.Empty;
            foreach (var symbol in cryptoText.ToLower())
            {
                if (alphabet.IndexOf(symbol) == -1)
                {
                    openText += symbol;
                    continue;
                }
                int t = Evklid.GetReverseElement(a, m) * (alphabet.IndexOf(symbol) - b + m * b) % m;
                char encryptedSymbol = alphabet[t];
                openText += encryptedSymbol;
            }
            return openText;
        }

        public string Decrypt(string cryptoText, int key, string keyWord)
        {
            var newAlphabetArray = GetNewAlphabet(key, keyWord);
            var openText = string.Empty;
            string newAlphabet = string.Join("", newAlphabetArray);
            foreach (var symbol in cryptoText.ToLower())
            {
                if (alphabet.IndexOf(symbol) == -1)
                {
                    openText += symbol; ;
                    continue;
                }
                openText += alphabet[newAlphabet.IndexOf(symbol)];
            }
            return openText;
        }

        private void ValidateKey(int a, int m)
        {
            if (Evklid.GetGCD(a, m) != 1)
                throw new ArgumentException("GDC(a, m) must be 1!");
        }

        /// <summary>
        /// Returns the alphabet, that starting from the k-th position with keyword's symbols,
        /// and the remaining positions are filled with the remaining symbols of the original alphabet.
        /// </summary>
        /// <param name="k">Key</param>
        private char[] GetNewAlphabet(int k, string keyWord)
        {
            var keyWordSymbols = new HashSet<char>(keyWord.ToLower());
            var alphabetWithoutKeyWordSymbols = new HashSet<char>(alphabet);
            alphabetWithoutKeyWordSymbols.ExceptWith(keyWordSymbols);
            int keyWordSymbolsRightEdge = k + keyWordSymbols.Count;
            var tempIndex = keyWordSymbolsRightEdge;
            var newAlphabet = new char[alphabet.Length];
            foreach (var symbol in alphabetWithoutKeyWordSymbols)
            {
                newAlphabet[tempIndex % alphabet.Length] = symbol;
                tempIndex++;
            }
            foreach (var symbol in keyWordSymbols)
            {
                newAlphabet[tempIndex % alphabet.Length] = symbol;
                tempIndex++;
            }
            return newAlphabet;
        }
    }
}