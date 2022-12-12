using Cryptography.Mathematics;
using System.Text;

namespace Cryptography.SubstitutionCiphers
{
    public class PlayFairCryptor
    {
        private string alphabet;
        private string additionalSymbol;
        private int rows;
        private int columns;

        public PlayFairCryptor(string alphabet, string additionalSymbol, int keyMatrixRows, int keyMatrixColumns)
        {
            this.alphabet = alphabet;
            this.additionalSymbol = additionalSymbol;
            rows = keyMatrixRows;
            columns = keyMatrixColumns;
        }

        public string Encrypt(string openText, string keyWord)
        {
            var alphabetMatrix = AlphabetMatrix.GetNewAlphabetMatrix(keyWord, rows, columns, alphabet);
            var textInBigrams = ParseToBigrams(RemoveSymbols(openText.ToLower(), alphabet));
            var cryptoText = new StringBuilder();
            for (int i = 0; i < textInBigrams.Length; i += 2)
            {
                cryptoText.Append(EncryptBigram(textInBigrams[i], textInBigrams[i + 1], alphabetMatrix));
            }
            return cryptoText.ToString();
        }

        public string Decrypt(string cryptoText, string keyWord)
        {
            var alphabetMatrix = AlphabetMatrix.GetNewAlphabetMatrix(keyWord, rows, columns, alphabet);
            var openText = new StringBuilder();
            for (int i = 0; i < cryptoText.Length; i += 2)
            {
                openText.Append(DecryptBigram(cryptoText[i], cryptoText[i + 1], alphabetMatrix));
            }
            openText.Replace(additionalSymbol, "");
            return openText.ToString();
        }

        private string ParseToBigrams(string text)
        {
            var newText = new StringBuilder();
            var textLengthParity = text.Length % 2;
            for (int i = 0; i < text.Length; i += 2)
            {
                char leftSymbol = text[i];
                char rightSymbol = text[i + 1];
                if (leftSymbol == rightSymbol)
                {
                    newText.Append(leftSymbol + additionalSymbol);
                    textLengthParity = (textLengthParity + 1) % 2;
                    i--;
                }
                else newText.Append(string.Concat(leftSymbol, rightSymbol));
            }
            if (textLengthParity != 0) newText.Append(string.Concat(text[text.Length - 1], additionalSymbol));
            return newText.ToString();
        }

        private string EncryptBigram(char leftSymbol, char rightSymbol, char[,] alphabetMatrix)
        {
            var leftSymbolIndex = AlphabetMatrix.IndexInMatrix(alphabetMatrix, leftSymbol);
            var rightSymbolIndex = AlphabetMatrix.IndexInMatrix(alphabetMatrix, rightSymbol);
            if (leftSymbolIndex[0] == rightSymbolIndex[0])
            {
                leftSymbol = alphabetMatrix[leftSymbolIndex[0], (leftSymbolIndex[1] + 1) % columns];
                rightSymbol = alphabetMatrix[rightSymbolIndex[0], (rightSymbolIndex[1] + 1) % columns];
            }
            else if (leftSymbolIndex[1] == rightSymbolIndex[1])
            {
                leftSymbol = alphabetMatrix[(leftSymbolIndex[0] + 1) % rows, leftSymbolIndex[1]];
                rightSymbol = alphabetMatrix[(rightSymbolIndex[0] + 1) % rows, leftSymbolIndex[1]];
            }
            else
            {
                leftSymbol = alphabetMatrix[leftSymbolIndex[0], rightSymbolIndex[1]];
                rightSymbol = alphabetMatrix[rightSymbolIndex[0], leftSymbolIndex[1]];
            }
            return string.Concat(leftSymbol, rightSymbol);
        }

        private string DecryptBigram(char leftSymbol, char rightSymbol, char[,] alphabetMatrix)
        {
            var leftSymbolIndex = AlphabetMatrix.IndexInMatrix(alphabetMatrix, leftSymbol);
            var rightSymbolIndex = AlphabetMatrix.IndexInMatrix(alphabetMatrix, rightSymbol);
            if (leftSymbolIndex[0] == rightSymbolIndex[0])
            {
                leftSymbol = alphabetMatrix[leftSymbolIndex[0], (leftSymbolIndex[1] - 1 + columns) % columns];
                rightSymbol = alphabetMatrix[rightSymbolIndex[0], (rightSymbolIndex[1] - 1 + columns) % columns];
            }
            else if (leftSymbolIndex[1] == rightSymbolIndex[1])
            {
                leftSymbol = alphabetMatrix[(leftSymbolIndex[0] - 1 + columns) % rows, leftSymbolIndex[1]];
                rightSymbol = alphabetMatrix[(rightSymbolIndex[0] - 1 + columns) % rows, leftSymbolIndex[1]];
            }
            else
            {
                leftSymbol = alphabetMatrix[leftSymbolIndex[0], rightSymbolIndex[1]];
                rightSymbol = alphabetMatrix[rightSymbolIndex[0], leftSymbolIndex[1]];
            }
            return string.Concat(leftSymbol, rightSymbol);
        }

        private string RemoveSymbols(string text, string alphabet)
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
