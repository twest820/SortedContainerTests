using System;

namespace SortedSetsPerf
{
    public class Timings
    {
        public int[] NArray { get; private init; }

        public TimeSpan[] SortedListStructAdd { get; private init; }
        public TimeSpan[] SortedListStructEnumerate { get; private init; }
        public TimeSpan[] SortedListClassAdd { get; private init; }
        public TimeSpan[] SortedListClassEnumerate { get; private init; }
        public TimeSpan[] SortedDictStructAdd { get; private init; }
        public TimeSpan[] SortedDictStructEnumerate { get; private init; }
        public TimeSpan[] SortedDictClassAdd { get; private init; }
        public TimeSpan[] SortedDictClassEnumerate { get; private init; }

        public TimeSpan[] ListStructSearch { get; private init; }
        public TimeSpan[] ListStructEnumerate { get; private init; }
        public TimeSpan[] ListClassSearch { get; private init; }
        public TimeSpan[] ListClassEnumerate { get; private init; }
        public TimeSpan[] SortedSetStructAdd { get; private init; }
        public TimeSpan[] SortedSetStructEnumerate { get; private init; }
        public TimeSpan[] SortedSetClassAdd { get; private init; }
        public TimeSpan[] SortedSetClassEnumerate { get; private init; }

        public Timings(int[] nArray)
        {
            this.NArray = nArray;

            int capacity = nArray.Length;
            this.SortedListStructAdd = new TimeSpan[capacity];
            this.SortedListStructEnumerate = new TimeSpan[capacity];
            this.SortedListClassAdd = new TimeSpan[capacity];
            this.SortedListClassEnumerate = new TimeSpan[capacity];

            this.SortedDictStructAdd = new TimeSpan[capacity];
            this.SortedDictStructEnumerate = new TimeSpan[capacity];
            this.SortedDictClassAdd = new TimeSpan[capacity];
            this.SortedDictClassEnumerate = new TimeSpan[capacity];

            this.ListStructSearch = new TimeSpan[capacity];
            this.ListStructEnumerate = new TimeSpan[capacity];
            this.ListClassSearch = new TimeSpan[capacity];
            this.ListClassEnumerate = new TimeSpan[capacity];

            this.SortedSetStructAdd = new TimeSpan[capacity];
            this.SortedSetStructEnumerate = new TimeSpan[capacity];
            this.SortedSetClassAdd = new TimeSpan[capacity];
            this.SortedSetClassEnumerate = new TimeSpan[capacity];
        }

        public void Accumulate(Timings other)
        {
            for (int nIndex = 0; nIndex < this.SortedListStructAdd.Length; ++nIndex)
            {
                int thisN = this.NArray[nIndex];
                int otherN = other.NArray[nIndex];
                if (thisN != otherN)
                {
                    throw new ArgumentOutOfRangeException(nameof(other));
                }

                this.SortedListStructAdd[nIndex] += other.SortedListStructAdd[nIndex];
                this.SortedListStructEnumerate[nIndex] += other.SortedListStructEnumerate[nIndex];
                this.SortedListClassAdd[nIndex] += other.SortedListClassAdd[nIndex];
                this.SortedListClassEnumerate[nIndex] += other.SortedListClassEnumerate[nIndex];

                this.SortedDictStructAdd[nIndex] += other.SortedListStructAdd[nIndex];
                this.SortedDictStructEnumerate[nIndex] += other.SortedListStructAdd[nIndex];
                this.SortedDictClassAdd[nIndex] += other.SortedDictClassAdd[nIndex];
                this.SortedDictClassEnumerate[nIndex] += other.SortedDictClassEnumerate[nIndex];

                this.ListStructSearch[nIndex] += other.ListStructSearch[nIndex];
                this.ListStructEnumerate[nIndex] += other.ListStructEnumerate[nIndex];
                this.ListClassSearch[nIndex] += other.ListClassSearch[nIndex];
                this.ListClassEnumerate[nIndex] += other.ListClassEnumerate[nIndex];

                this.SortedSetStructAdd[nIndex] += other.SortedSetStructAdd[nIndex];
                this.SortedSetStructEnumerate[nIndex] += other.SortedSetStructEnumerate[nIndex];
                this.SortedSetClassAdd[nIndex] += other.SortedSetClassAdd[nIndex];
                this.SortedSetClassEnumerate[nIndex] += other.SortedSetClassEnumerate[nIndex];
            }
        }

        public void Divide(int replicates)
        {
            for (int nIndex = 0; nIndex < this.SortedListStructAdd.Length; ++nIndex)
            {
                this.SortedListStructAdd[nIndex] /= replicates;
                this.SortedListStructEnumerate[nIndex] /= replicates;
                this.SortedListClassAdd[nIndex] /= replicates;
                this.SortedListClassEnumerate[nIndex] /= replicates;

                this.SortedDictStructAdd[nIndex] /= replicates;
                this.SortedDictStructEnumerate[nIndex] /= replicates;
                this.SortedDictClassAdd[nIndex] /= replicates;
                this.SortedDictClassEnumerate[nIndex] /= replicates;

                this.ListStructSearch[nIndex] /= replicates;
                this.ListStructEnumerate[nIndex] /= replicates;
                this.ListClassSearch[nIndex] /= replicates;
                this.ListClassEnumerate[nIndex] /= replicates;

                this.SortedSetStructAdd[nIndex] /= replicates;
                this.SortedSetStructEnumerate[nIndex] /= replicates;
                this.SortedSetClassAdd[nIndex] /= replicates;
                this.SortedSetClassEnumerate[nIndex] /= replicates;
            }
        }
    }
}
