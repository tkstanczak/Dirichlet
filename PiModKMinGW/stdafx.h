// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>
#include <assert.h>

#define _STDINT_H
typedef long long intmax_t;
typedef unsigned long long uintmax_t;

#include "mpirxx.h"

#include <stack>

typedef long long Int64;
typedef unsigned long long UInt64;
typedef __int128_t Integer;

static inline Integer mpz2Integer(const mpz_t a)
{
    assert(mp_bits_per_limb == 64);
    union {
        UInt64 words[2];
        Integer res;
    } u;
    u.words[0] = u.words[1] = 0;
    mpz_export(u.words, 0, -1, sizeof(UInt64), 0, 0, a);
    return u.res;
}

static inline void Integer2mpz(Integer a, mpz_t b)
{
    assert(mp_bits_per_limb == 64);
    mpz_import(b, 2, -1, sizeof(UInt64), 0, 0, (UInt64*)&a);
}

static inline Integer mpz2Integer(mpz_class a)
{
    return mpz2Integer(a.get_mpz_t());
}

static inline mpz_class Integer2mpz(Integer a)
{
    mpz_class res;
    Integer2mpz(a, res.get_mpz_t());
    return res;
}

static inline Integer Square(Integer a)
{
    return a * a;
}

static inline Integer Min(Integer a, Integer b)
{
    return a < b ? a : b;
}

static inline Integer Max(Integer a, Integer b)
{
    return a > b ? a : b;
}

static inline Integer Power(Integer a, int b)
{
    Integer result = 1;
    for (int i = 1; i <= b; i++)
        result *= a;
    return result;
}

static inline UInt64 FloorSquareRoot(Integer a)
{
    mpz_t aRep, resultRep;
    mpz_init(aRep);
    mpz_init(resultRep);
    Integer2mpz(a, aRep);
    mpz_sqrt(resultRep, aRep);
    UInt64 result = mpz_get_ux(resultRep);
    mpz_clear(aRep);
    mpz_clear(resultRep);
    return result;
}

static inline UInt64 CeilingSquareRoot(Integer a)
{
    mpz_t aRep, resultRep, remRep;
    mpz_init(aRep);
    mpz_init(resultRep);
    mpz_init(remRep);
    Integer2mpz(a, aRep);
    mpz_sqrtrem(resultRep, remRep, aRep);
    UInt64 result = mpz_get_ux(resultRep);
    if (mpz_sgn(remRep) > 0)
        ++result;
    mpz_clear(aRep);
    mpz_clear(resultRep);
    mpz_clear(remRep);
    return result;
}

static inline Integer CeilingRoot(Integer a, int root)
{
    mpz_class result, rem;
    mpz_root(result.get_mpz_t(), Integer2mpz(a).get_mpz_t(), root);
    Integer result2 = mpz2Integer(result);
    return Power(result2, root) == a ? result2 : result2 + 1;
}

#ifdef _DEBUG
#define Assert(value) \
    do \
    { \
        if (!(value)) \
        { \
            ReportFailure(#value, __FILE__, __LINE__); \
            exit(1); \
        } \
    } while(0)  
static inline void ReportFailure(const char *value, const char *file, int line)
{
    printf("Assert: %s, %s:%d", value, file, line);
}
#else
#define Assert(value)
#endif

#include "hr_time.h"
#include "DivisorSummatoryFunctionOdd.h"
