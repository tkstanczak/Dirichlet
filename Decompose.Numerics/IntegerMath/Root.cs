﻿using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Decompose.Numerics
{
    public static partial class IntegerMath
    {
        private static double log2 = Math.Log(2);

        public static T FloorRoot<T>(T a, T b)
        {
            var degree = (Number<T>)b;
            var absA = Number<T>.Abs(a);
            if (degree.IsEven && a != absA)
                throw new InvalidOperationException("negative radicand");
            Number<T> power;
            var c = FloorRootCore(absA, Number<T>.Log(absA).Real, degree, out power);
            return a == absA ? c : -c;
        }

        private static Number<T> FloorRootCore<T>(Number<T> a, double logA, Number<T> degree, out Number<T> power)
        {
            var log = logA / (double)degree;
            var shift = Math.Max((int)Math.Floor(log / log2) - 64, 0);
            log -= shift * log2;
            var c = (Number<T>)Math.Floor(Math.Exp(log)) << shift;
            power = Number<T>.Power(c, degree);
            if (power <= a && Number<T>.Power(c + 1, degree) > a)
                return c;
            var cPrev = Number<T>.Zero;
            var degreeMinusOne = degree - 1;
            while (true)
            {
                var cNext = (a / Number<T>.Power(c, degreeMinusOne) + degreeMinusOne * c) / degree;
                if (cNext == cPrev)
                {
                    if (cNext < c)
                        c = cNext;
                    break;
                }
                cPrev = c;
                c = cNext;
            }
            power = Number<T>.Power(c, degree);
            Debug.Assert(power <= a && Number<T>.Power(c + 1, degree) > a);
            return c;
        }

        public static T Root<T>(T a, T b)
        {
            var degree = (Number<T>)b;
            var absA = Number<T>.Abs(a);
            if (degree.IsEven && a != absA)
                throw new InvalidOperationException("negative radicand");
            Number<T> power;
            var c = FloorRootCore(absA, Number<T>.Log(absA).Real, degree, out power);
            if (power != absA)
                throw new InvalidOperationException("not a perfect power");
            return a == absA ? c : -c;
        }

        private static IEnumerable<int> primes = new SieveOfErostothones();

        public static T PerfectPower<T>(T a)
        {
            var absA = Number<T>.Abs(a);
            var bits = (Number<T>)Math.Floor(Number<T>.Log(absA, 2).Real);
            var logA = Number<T>.Log(absA).Real;
            foreach (var p in primes)
            {
                if (absA != a && p == 2)
                    continue;
                var b = (Number<T>)p;
                if (b > bits)
                    break;
                Number<T> power;
                var c = FloorRootCore<T>(absA, logA, b, out power);
                if (power == absA)
                    return b * PerfectPower<T>(absA == a ? c : -c);
            }
            return Number<T>.One;
        }
    }
}
