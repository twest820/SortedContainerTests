using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;

namespace SortedSetsPerf
{
    [Cmdlet(VerbsCommon.Get, "SortedSetsPerf")]
    public class GetSortedSetsPerf : Cmdlet
    {
        [Parameter]
        [ValidateRange(0, 1000)]
        public int Burnin;

        [Parameter]
        [ValidateNotNullOrEmpty]
        public int[] NArray { get; set; }

        [Parameter]
        [ValidateRange(1, 1000 * 1000)]
        public int Replicates;

        public GetSortedSetsPerf()
        {
            this.Burnin = 10;
            this.NArray = new int[] { 1, 2, 3, 4, 5, 7, 10, 20, 30, 40, 50, 70, 100, 200, 300, 500, 1000 };
            this.Replicates = 100;
        }

        protected override void ProcessRecord()
        {
            // save some random keys just to make 100% sure our test sets are the same; theoretically unnecessary
            Random pseudorandom = new(1);
            int[] randomValues = new int[this.NArray.Max()];
            for (int index = 0; index < randomValues.Length; ++index)
            {
                int randomKey = pseudorandom.Next() % 1000000;
                randomValues[index] = randomKey;
            }

            // burnin
            float completedIterations = 0.0F;
            float totalIterations = this.Burnin + this.Replicates;
            ProgressRecord progressRecord = new(0, "Get-SortedSetsPerf", "burnin...");
            for (int burninIteration = 0; burninIteration < Burnin; ++burninIteration)
            {
                this.CollectTimings(randomValues);

                ++completedIterations;
                progressRecord.PercentComplete = (int)(100.0F * completedIterations / totalIterations);
                this.WriteProgress(progressRecord);
            }

            // measurements
            progressRecord.StatusDescription = "collecting timings...";
            Timings averageTimings = new(this.NArray);
            for (int replicate = 0; replicate < Replicates; ++replicate)
            {
                averageTimings.Accumulate(this.CollectTimings(randomValues));

                ++completedIterations;
                progressRecord.PercentComplete = (int)(100.0F * completedIterations / totalIterations);
                this.WriteProgress(progressRecord);
            }

            averageTimings.Divide(Replicates);
            this.WriteObject(averageTimings);

            progressRecord.PercentComplete = 100;
            this.WriteProgress(progressRecord);
        }

        private Timings CollectTimings(int[] randomValues)
        {
            SortedDictionary<int, NonvalueClass> sortedDictClass = new();
            SortedDictionary<int, ValueStruct> sortedDictStruct = new();
            SortedList<int, NonvalueClass> sortedListClass = new();
            SortedList<int, ValueStruct> sortedListStruct = new();

            SortedSet<ValueStruct> sortedSetStruct = new(new StructComparer());
            SortedSet<NonvalueClass> sortedSetClass = new(new ClassComparer());

            List<ValueStruct> listStruct = new();
            List<NonvalueClass> listClass = new();

            StructComparer comparerStruct = new();
            ClassComparer comparerClass = new();

            Stopwatch stopwatch = new();
            Timings timings = new(this.NArray);
            for (int nIndex = 0; nIndex < this.NArray.Length; ++nIndex)
            {
                int n = this.NArray[nIndex];

                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    if (!sortedListStruct.ContainsKey(randomKey))
                    {
                        sortedListStruct.Add(randomKey, new ValueStruct() { int1 = iteration, int2 = randomKey });
                    }
                }
                stopwatch.Stop();
                timings.SortedListStructAdd[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                int accumulator = 0;
                int lastval = Int32.MinValue;
                IList<ValueStruct> sortedListValuesStruct = sortedListStruct.Values;
                for (int i = 0; i < sortedListValuesStruct.Count; i++)
                {
                    int nextVal = sortedListValuesStruct[i].int2;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;
                }
                stopwatch.Stop();
                timings.SortedListStructEnumerate[nIndex] = stopwatch.Elapsed;
                sortedListStruct.Clear();
                GC.Collect();


                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    if (!sortedListClass.ContainsKey(randomKey))
                    {
                        sortedListClass.Add(randomKey, new NonvalueClass() { int1 = iteration, int2 = randomKey });
                    }
                }
                stopwatch.Stop();
                timings.SortedListClassAdd[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                accumulator = 0;
                lastval = Int32.MinValue;
                IList<NonvalueClass> sortedListValuesClass = sortedListClass.Values;
                for (int index = 0; index < sortedListValuesClass.Count; index++)
                {
                    int nextVal = sortedListValuesClass[index].int2;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;
                }
                stopwatch.Stop();
                timings.SortedListClassEnumerate[nIndex] = stopwatch.Elapsed;
                sortedListClass.Clear();
                GC.Collect();

                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    if (!sortedDictStruct.ContainsKey(randomKey))
                    {
                        sortedDictStruct.Add(randomKey, new ValueStruct() { int1 = iteration, int2 = randomKey });
                    }
                }
                stopwatch.Stop();
                timings.SortedDictStructAdd[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                accumulator = 0;
                lastval = Int32.MinValue;
                foreach (KeyValuePair<int, ValueStruct> item in sortedDictStruct)
                {
                    int nextVal = item.Value.int2;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;
                }
                stopwatch.Stop();
                timings.SortedDictStructEnumerate[nIndex] = stopwatch.Elapsed;
                sortedDictStruct.Clear();
                GC.Collect();

                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    if (!sortedDictClass.ContainsKey(randomKey))
                    {
                        sortedDictClass.Add(randomKey, new NonvalueClass() { int1 = iteration, int2 = randomKey });
                    }
                }
                stopwatch.Stop();
                timings.SortedDictClassAdd[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                accumulator = 0;
                lastval = Int32.MinValue;
                foreach (KeyValuePair<int, NonvalueClass> item in sortedDictClass)
                {
                    int nextVal = item.Value.int2;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;
                }
                stopwatch.Stop();
                timings.SortedDictClassEnumerate[nIndex] = stopwatch.Elapsed;
                sortedDictClass.Clear();
                GC.Collect();


                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    ValueStruct item = new() { int1 = randomKey, int2 = iteration };
                    int idx = listStruct.BinarySearch(item, comparerStruct);
                    if (idx < 0)
                    {
                        listStruct.Insert(~idx, item);
                    }
                }
                stopwatch.Stop();
                timings.ListStructSearch[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                accumulator = 0;
                lastval = Int32.MinValue;
                for (int index = 0; index < listStruct.Count; ++index)
                {
                    int nextVal = listStruct[index].int1;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;
                }
                stopwatch.Stop();
                timings.ListStructEnumerate[nIndex] = stopwatch.Elapsed;
                listStruct.Clear();
                GC.Collect();


                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    var item = new NonvalueClass() { int1 = randomKey, int2 = iteration };
                    int idx = listClass.BinarySearch(item, comparerClass);
                    if (idx < 0)
                    {
                        listClass.Insert(~idx, item);
                    }
                }
                stopwatch.Stop();
                timings.ListClassSearch[nIndex] = stopwatch.Elapsed;
                stopwatch.Restart();


                accumulator = 0;
                lastval = Int32.MinValue;
                for (int index = 0; index < listClass.Count; index++)
                // foreach (var iter in listClass)
                {
                    int nextVal = listClass[index].int1;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;

                }
                stopwatch.Stop();
                timings.ListClassEnumerate[nIndex] = stopwatch.Elapsed;
                listClass.Clear();
                GC.Collect();


                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    ValueStruct newItem = new() { int1 = randomKey, int2 = iteration };
                    if (!sortedSetStruct.Contains(newItem))
                    {
                        sortedSetStruct.Add(newItem);
                    }
                }
                stopwatch.Stop();
                timings.SortedSetStructAdd[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                accumulator = 0;
                lastval = Int32.MinValue;
                foreach (ValueStruct item in sortedSetStruct)
                {
                    int nextVal = item.int1;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;

                }
                stopwatch.Stop();
                timings.SortedSetStructEnumerate[nIndex] = stopwatch.Elapsed;
                sortedSetStruct.Clear();
                GC.Collect();


                stopwatch.Restart();
                for (int iteration = 0; iteration < n; ++iteration)
                {
                    int randomKey = randomValues[iteration];
                    NonvalueClass newItem = new() { int1 = randomKey, int2 = iteration };
                    if (!sortedSetClass.Contains(newItem))
                    {
                        sortedSetClass.Add(newItem);
                    }
                }
                stopwatch.Stop();
                timings.SortedSetClassAdd[nIndex] = stopwatch.Elapsed;


                stopwatch.Restart();
                accumulator = 0;
                lastval = Int32.MinValue;
                foreach (NonvalueClass item in sortedSetClass)
                {
                    int nextVal = item.int1;
                    Debug.Assert(nextVal > lastval);
                    accumulator += nextVal;
                    lastval = nextVal;
                }
                stopwatch.Stop();
                timings.SortedSetClassEnumerate[nIndex] = stopwatch.Elapsed;
                sortedSetClass.Clear();
                GC.Collect();
            }

            return timings;
        }

        private class ClassComparer : IComparer<NonvalueClass>
        {
            public int Compare(NonvalueClass? x, NonvalueClass? y)
            {
                return x!.int1.CompareTo(y!.int1);
            }
        }

        private class NonvalueClass
        {
            public int int1;
            public int int2;
#pragma warning disable CS0649
            public int int3;
            public int int4;
#pragma warning restore CS0649
            //,int5,int6,int7,int8;
        }

        private class StructComparer : IComparer<ValueStruct>
        {
            public int Compare(ValueStruct x, ValueStruct y)
            {
                return x.int1.CompareTo(y.int1);
            }
        }

        private struct ValueStruct
        {
            public int int1;
            public int int2;
#pragma warning disable CS0649
            public int int3;
            public int int4;
#pragma warning restore CS0649
            //,int5,int6,int7,int8;
        }
    }
}
