namespace Cryptography.SubstitutionCiphers
{
    public static class PermutationCryptor
    {
        public static string Encrypt(string openText, string encryptingKey)
        {
            var rowsCount = (int)System.Math.Ceiling(openText.Length / (double)encryptingKey.Length);
            int nullElementsCountInOpenTextMatrix = rowsCount * encryptingKey.Length - openText.Length;
            for (int i = 0; i < nullElementsCountInOpenTextMatrix; i++)
                openText += " ";

            var openTextMatrix = new string[rowsCount, encryptingKey.Length];
            for (int i = 0; i < openText.Length; i++)
                openTextMatrix[i % rowsCount, i / rowsCount] = openText[i].ToString().ToLower();

            ReplaceSymbolInTextMatrix(openTextMatrix, " ", "_");
            var permutation = GetPermutation(encryptingKey);
            var encryptedTextMatrix = new string[rowsCount, encryptingKey.Length];
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < permutation.Length; j++)
                    encryptedTextMatrix[i, j] = openTextMatrix[i, permutation[j]];
            }
            string encryptedText = string.Empty;
            foreach (var symbol in encryptedTextMatrix)
                encryptedText += symbol.ToString();
            return encryptedText;
        }

        public static string Decrypt(string encryptedText, string encryptingKey)
        {
            var stringsCount = (int)Math.Ceiling(encryptedText.Length / (double)encryptingKey.Length);
            var encryptedTextMatrix = new string[stringsCount, encryptingKey.Length];
            for (int i = 0; i < encryptedText.Length; i++)
                encryptedTextMatrix[i / encryptingKey.Length, i % encryptingKey.Length] = encryptedText[i].ToString().ToLower();

            var permutation = GetReversePermutation(encryptingKey);
            var openTextMatrix = new string[stringsCount, encryptingKey.Length];
            for (int i = 0; i < stringsCount; i++)
            {
                for (int j = 0; j < permutation.Length; j++)
                    openTextMatrix[i, j] = encryptedTextMatrix[i, permutation[j]];
            }
            ReplaceSymbolInTextMatrix(openTextMatrix, "_", " ");
            string openText = string.Empty;
            for (int j = 0; j < encryptingKey.Length; j++)
            {
                for (int i = 0; i < stringsCount; i++)
                    openText += openTextMatrix[i, j];
            }
            return openText;
        }

        private static void ReplaceSymbolInTextMatrix(string[,] TextMatrix, string oldSymbol, string newSymbol)
        {
            for (int i = 0; i < TextMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < TextMatrix.GetLength(1); j++)
                    TextMatrix[i, j] = TextMatrix[i, j].Replace(oldSymbol, newSymbol);
            }
        }

        private static int[] GetPermutation(string encryptingKey)
        {
            var encryptingKeyASCII = new int[encryptingKey.Length];
            for (int i = 0; i < encryptingKeyASCII.Length; i++)
            {
                encryptingKeyASCII[i] = encryptingKey[i];
            }
            var permutation = new int[encryptingKeyASCII.Length];
            for (int i = 0; i < permutation.Length; i++)
                permutation[i] = i;
            // Bubble sort.
            int temp;
            for (int i = 0; i < encryptingKeyASCII.Length; i++)
            {
                for (int j = i + 1; j < encryptingKeyASCII.Length; j++)
                {
                    if (encryptingKeyASCII[i] > encryptingKeyASCII[j])
                    {
                        temp = encryptingKeyASCII[i];
                        encryptingKeyASCII[i] = encryptingKeyASCII[j];
                        encryptingKeyASCII[j] = temp;
                        temp = permutation[i];
                        permutation[i] = permutation[j];
                        permutation[j] = temp;
                    }
                }
            }
            return permutation;
        }

        private static int[] GetReversePermutation(string encryptingKey)
        /** Working only with Ru and Eng alphabets.**/
        {
            var ALPHABET = "abcdefghijklmnopqrstuvwxyzабвгдеёжзийклмнопрстуфхцчшщъыьэюя";
            encryptingKey = encryptingKey.ToLower();
            var reversePermutation = new int[encryptingKey.Length];
            int currentNumber = 0;
            foreach (var symbol in ALPHABET)
            {
                for (int i = 0; i < encryptingKey.Length; i++)
                {
                    if (encryptingKey[i] == symbol)
                        reversePermutation[i] = currentNumber++;
                }
            }
            return reversePermutation;
        }
    }
}
