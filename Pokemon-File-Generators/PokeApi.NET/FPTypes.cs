﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PokeAPI
{
    public enum Unit : byte { Value }

    public static class Maybe
    {
        internal const MethodImplOptions AggressiveInlining = (MethodImplOptions)0x100; // AggressiveInlining, will only happen when .NET 4.5+ is installed.

        [MethodImpl(AggressiveInlining)]
        public static Maybe<T> Just<T>(T value) => new Maybe<T>(value);
    }
    public struct Maybe<T> // Nullable is only for structs, need support for any T
    {
        public readonly static Maybe<T> Nothing = new Maybe<T>();

        readonly T v;

        public bool HasValue
        {
            get;
        }
        public T Value
        {
            get
            {
                if (!HasValue)
                    throw new InvalidOperationException();

                return v;
            }
        }

        public Maybe(T value)
        {
            HasValue = true;
            v = value;
        }
    }
}
