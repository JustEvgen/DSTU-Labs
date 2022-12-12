using Cryptography.Mathematics;
using System.Text;

namespace Cryptography.SubstitutionCiphers
{
    public static class TrisemusCryptor
    {
        public static string Encrypt(string openText, string keyWord, int rows, int columns, string alphabet)
        {
            var newAlphabetMatrix = AlphabetMatrix.GetNewAlphabetMatrix(keyWord, rows, columns, alphabet);
            var cryptoText = new StringBuilder();
            foreach (var symbol in openText.ToLower())
            {
                var index = AlphabetMatrix.IndexInMatrix(newAlphabetMatrix, symbol);
                if (index[0] != -1) cryptoText.Append(newAlphabetMatrix[(index[0] + 1) % rows, index[1]]);
                else cryptoText.Append(symbol);
            }
            return cryptoText.ToString();
        }

        public static string Decrypt(string cryptoText, string keyWord, int stringsCount, int columnsCount, string alphabet)
        {
            var newAlphabetMatrix = AlphabetMatrix.GetNewAlphabetMatrix(keyWord, stringsCount, columnsCount, alphabet);
            var openText = new StringBuilder();
            foreach (var symbol in cryptoText.ToLower())
            {
                var index = AlphabetMatrix.IndexInMatrix(newAlphabetMatrix, symbol);
                if (index[0] != -1) openText.Append(newAlphabetMatrix[(index[0] + stringsCount - 1) % stringsCount, index[1]]);
                else openText.Append(symbol);
            }
            return openText.ToString();
        }
    }
}
