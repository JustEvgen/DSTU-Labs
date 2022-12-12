using Cryptography.Mathematics;
using System.Text;

namespace Cryptography.SubstitutionCiphers
{
    public class TwoSquareCryptor
    {
        private string additionalSymbol;
        private string alphabet;
        private char[,] matrix1, matrix2;
        private int rows;
        private int columns;

        public TwoSquareCryptor(char[,] matrix1, char[,] matrix2, string additionalSymbol, string alphabet)
        {
            this.matrix1 = matrix1;
            this.matrix2 = matrix2;
            this.additionalSymbol = additionalSymbol;
            this.alphabet = alphabet;
            rows = matrix1.GetLength(0);
            columns = matrix1.GetLength(1);
        }

        public string Encrypt(string openText)
        {
            var textInBigrams = ParseToBigrams(RemoveSymbols(openText.ToLower(), alphabet));
            var cryptoText = new StringBuilder();
            for (int i = 0; i < textInBigrams.Length; i += 2)
            {
                cryptoText.Append(EncryptBigram(textInBigrams[i], textInBigrams[i + 1]));
            }
            return cryptoText.ToString();
        }

        public string Decrypt(string cryptoText)
        {
            var openText = new StringBuilder();
            for (int i = 0; i < cryptoText.Length; i += 2)
            {
                openText.Append(DecryptBigram(cryptoText[i], cryptoText[i + 1]));
            }
            openText.Replace(additionalSymbol, "");
            return openText.ToString();
        }

        private string ParseToBigrams(string text) // TO DO: Check for 3 or more similar symbols
        {
            var newText = new StringBuilder();
            var textLength = text.Length;
            var textLengthParity = textLength % 2;
            if (textLengthParity != 0) textLength -= 1;
            for (int i = 0; i < textLength; i += 2)
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

        private string EncryptBigram(char leftSymbol, char rightSymbol)
        {
            var leftSymbolIndex = AlphabetMatrix.IndexInMatrix(matrix1, leftSymbol);
            var rightSymbolIndex = AlphabetMatrix.IndexInMatrix(matrix2, rightSymbol);
            if (leftSymbolIndex[0] == rightSymbolIndex[0])
            {
                leftSymbol = matrix2[leftSymbolIndex[0], leftSymbolIndex[1]];
                rightSymbol = matrix1[rightSymbolIndex[0], rightSymbolIndex[1]];
            }
            else
            {
                leftSymbol = matrix2[leftSymbolIndex[0], rightSymbolIndex[1]];
                rightSymbol = matrix1[rightSymbolIndex[0], leftSymbolIndex[1]];
            }
            return string.Concat(leftSymbol, rightSymbol);
        }

        private string DecryptBigram(char leftSymbol, char rightSymbol)
        {
            var leftSymbolIndex = AlphabetMatrix.IndexInMatrix(matrix2, leftSymbol);
            var rightSymbolIndex = AlphabetMatrix.IndexInMatrix(matrix1, rightSymbol);
            if (leftSymbolIndex[0] == rightSymbolIndex[0])
            {
                leftSymbol = matrix1[leftSymbolIndex[0], leftSymbolIndex[1]];
                rightSymbol = matrix2[rightSymbolIndex[0], rightSymbolIndex[1]];
            }
            else
            {
                leftSymbol = matrix1[leftSymbolIndex[0], rightSymbolIndex[1]];
                rightSymbol = matrix2[rightSymbolIndex[0], leftSymbolIndex[1]];
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
