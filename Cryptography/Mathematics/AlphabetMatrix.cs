using System.Text;

namespace Cryptography.Mathematics
{
    public class AlphabetMatrix
    {
        public static int[] IndexInMatrix(char[,] matrix, char symbol)
        {
            int[] elementsIndex = { -1, -1 };
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == symbol) return new int[] { i, j };
                }
            }
            return elementsIndex;
        }

        /// <summary>
        /// Returns a matrix where the keyword's symbols are written in lines (without repetitions), 
        /// and the remaining cells are filled with the remaining symbols of the original alphabet.
        /// </summary>
        public static char[,] GetNewAlphabetMatrix(string keyWord, int rows, int columns, string alphabet)
        {
            var keyWordSymbols = new HashSet<char>(keyWord.ToLower());
            var alphabetWithoutKeyWord = new HashSet<char>(alphabet);
            alphabetWithoutKeyWord.ExceptWith(keyWordSymbols);
            var newAlphabet = new StringBuilder();
            foreach (var symbol in keyWordSymbols) newAlphabet.Append(symbol);
            foreach (var symbol in alphabetWithoutKeyWord) newAlphabet.Append(symbol);
            var newAlphabetMatrix = new char[rows, columns];
            var tempIndex = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    newAlphabetMatrix[i, j] = newAlphabet[tempIndex++];
                }
            }
            return newAlphabetMatrix;
        }
    }
}
