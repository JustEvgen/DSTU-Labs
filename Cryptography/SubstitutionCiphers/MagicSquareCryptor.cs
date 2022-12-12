namespace Cryptography.SubstitutionCiphers
{
    public static class MagicSquareCryptor
    {
        public static string Encrypt(string openText, int[,] magicSquare)
        {
            var n = magicSquare.GetLength(0);
            var textInMagicSquare = new string[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    textInMagicSquare[i, j] = openText[magicSquare[i, j] - 1].ToString().ToLower();
            }
            ReplaceSymbolInTextMatrix(textInMagicSquare, " ", "_");
            string cryptoText = string.Empty;
            foreach (string symbol in textInMagicSquare)
                cryptoText += symbol;
            return cryptoText;
        }

        public static string Decrypt(string cryptoText, int[,] magicSquare)
        {
            var n = magicSquare.GetLength(0);
            var textInMagicSquare = new string[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    textInMagicSquare[i, j] = cryptoText[i * n + j].ToString().ToLower();
            }
            var openTextArray = new string[(int)System.Math.Pow(n, 2)];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    openTextArray[magicSquare[i, j] - 1] = textInMagicSquare[i, j];
            }
            return string.Join(string.Empty, openTextArray);
        }

        private static void ReplaceSymbolInTextMatrix(string[,] TextMatrix, string oldSymbol, string newSymbol)
        {
            var n = TextMatrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    TextMatrix[i, j] = TextMatrix[i, j].Replace(oldSymbol, newSymbol);
            }
        }

    }
}
