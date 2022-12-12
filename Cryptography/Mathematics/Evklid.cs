namespace Cryptography.Mathematics
{
    public static class Evklid
    {
        public static int GetGCD(int a, int m)
        {
            return m == 0 ? a : GetGCD(m, a % m);
        }

        public static int GetReverseElement(int a, int m)
        {
            {
                (int gcd, int x, int y) = ExtendedEvklid(a, m);
                if (x < 0) x += m;
                return x;
            }
        }

        private static (int, int, int) ExtendedEvklid(int a, int m)
        {
            if (m == 0) return (a, 1, 0);
            (int d, int x, int y) = ExtendedEvklid(m, a % m);
            return (d, y, x - y * (a / m));
        }
    }
}