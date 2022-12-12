using System.Text;

namespace Cryptography.SubstitutionCiphers
{
    public static class VigenereCryptor
    {

        public static string Encrypt(string openText, string keyWord, string alphabet)
        {
            var cryptoText = new StringBuilder();
            keyWord = keyWord.ToLower();
            openText = RemoveSymbols(openText.ToLower(), alphabet);
            for (int i = 0; i < openText.Length; i++)
            {
                var keySymbol = keyWord[i % keyWord.Length];
                var newSymbolIndex = (alphabet.IndexOf(openText[i]) + alphabet.IndexOf(keySymbol)) % alphabet.Length;
                cryptoText.Append(alphabet[newSymbolIndex]);
            }
            return cryptoText.ToString();
        }

        public static string Decrypt(string cryptoText, string keyWord, string alphabet)
        {
            var openText = new StringBuilder();
            keyWord = keyWord.ToLower();
            cryptoText = RemoveSymbols(cryptoText.ToLower(), alphabet);
            for (int i = 0; i < cryptoText.Length; i++)
            {
                var keySymbol = keyWord[i % keyWord.Length];
                var newSymbolIndex = (alphabet.IndexOf(cryptoText[i]) - alphabet.IndexOf(keySymbol) + alphabet.Length) % alphabet.Length;
                openText.Append(alphabet[newSymbolIndex]);
            }
            return openText.ToString();
        }

        private static string RemoveSymbols(string text, string alphabet)
        {
            var newText = new StringBuilder();
            foreach (var symbol in text)
            {
                if (alphabet.IndexOf(symbol) != -1) newText.Append(symbol);
            }
            return newText.ToString();
        }
    }
}
