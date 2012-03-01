﻿using System;
using System.Linq;
using System.Numerics;

namespace Decompose.Numerics
{
    public class PrimeCounting
    {
        public int Pi(int x)
        {
            return new SieveOfErostothones().TakeWhile(p => p <= x).Count();
        }

        public int PiWithPowers(int x)
        {
            var sum = Pi(x);
            for (int j = 2; true; j++)
            {
                var root = IntegerMath.FloorRoot(x, j);
                if (root == 1)
                    break;
                sum += Pi(root);
            }
            return sum;
        }

        public int ParityOfPi(long x)
        {
            if (x < 2)
                return 0;
            var parity = SumTwoToTheOmega(x) / 2 % 2;
            for (var j = 2; true; j++)
            {
                var root = IntegerMath.FloorRoot(x, j);
                if (root == 1)
                    break;
                parity ^= ParityOfPi(root);
            }
            return parity;
        }

        private int SumTwoToTheOmega(long x)
        {
            var limit = IntegerMath.FloorSquareRoot(x);
            var sum = 0;
            for (var d = (long)1; d <= limit; d++)
            {
                var mu = IntegerMath.Mobius(d);
                if (mu == 1)
                    sum += TauSum(x / (d * d));
                else if (mu == -1)
                    sum += 4 - TauSum(x / (d * d));
            }
            return sum;
        }

        private int TauSum(long y)
        {
            var sum = 0;
            var n = 1;
            while (true)
            {
                var term = y / n - n;
                if (term < 0)
                    break;
                sum ^= (int)(term & 1);
                ++n;
            }
            sum = 2 * sum + n - 1;
            return sum & 3;
        }

        public int ParityOfPi(BigInteger x)
        {
            if (x < 2)
                return 0;
            var parity = SumTwoToTheOmega(x) / 2 % 2;
            for (int j = 2; true; j++)
            {
                var root = IntegerMath.FloorRoot(x, j);
                if (root == 1)
                    break;
                parity ^= ParityOfPi(root);
            }
            return parity;
        }

        private int SumTwoToTheOmega(BigInteger x)
        {
            var limit = IntegerMath.FloorSquareRoot(x);
            var sum = 0;
            for (var d = (BigInteger)1; d <= limit; d++)
            {
                var mu = IntegerMath.Mobius(d);
                if (mu == 1)
                    sum += TauSum(x / (d * d));
                else if (mu == -1)
                    sum += 4 - TauSum(x / (d * d));
            }
            return sum;
        }

        private int TauSum(BigInteger y)
        {
            if (y <= long.MaxValue)
                return TauSum((long)y);
            var sum = 0;
            var  n = (BigInteger)1;
            while (true)
            {
                var term = y / n - n;
                if (term < 0)
                    break;
                sum ^= (int)(term & 1);
                ++n;
            }
            sum = 2 * sum + (int)((n - 1) & 3);
            return (int)(sum & 3);
        }
    }
}
