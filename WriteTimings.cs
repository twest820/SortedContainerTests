using System;
using System.Globalization;
using System.IO;
using System.Management.Automation;

namespace SortedSetsPerf
{
    [Cmdlet(VerbsCommunications.Write, "Timings")]
    public class WriteTimings : Cmdlet
    {
        [Parameter(Mandatory = true)]
        [ValidateNotNullOrEmpty]
        public string? File { get; set; }

        [Parameter(Mandatory = true)]
        [ValidateNotNull]
        public Timings? Timings { get; set; }

        protected override void ProcessRecord()
        {
            if (String.IsNullOrWhiteSpace(this.File))
            {
                throw new InvalidOperationException("Output file path parameter is null, empty, or whitespace.");
            }
            if (this.Timings == null)
            {
                throw new InvalidOperationException("Timings parameter is null.");
            }

            using FileStream stream = new(this.File, FileMode.Create, FileAccess.Write, FileShare.Read);
            using StreamWriter writer = new(stream);

            writer.WriteLine("n,sortedListStructAdd,sortedListStructEnumerate,sortedListClassAdd,sortedListClassEnumerate," +
                               "sortedDictStructAdd,sortedDictStructEnumerate,sortedDictClassAdd,sortedDictClassEnumerate," +
                               "listStructSearch,listStructEnumerate,listClassSearch,listClassEnumerate," +
                               "sortedSetStructAdd,sortedSetStructEnumerate,sortedSetClassAdd,sortedSetClassEnumerate");

            for (int nIndex = 0; nIndex < this.Timings.NArray.Length; ++nIndex)
            {
                writer.WriteLine(this.Timings.NArray[nIndex].ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedListStructAdd[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedListStructEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedListClassAdd[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedListClassEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedDictStructAdd[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedDictStructEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedDictClassAdd[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedDictClassEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.ListStructSearch[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.ListStructEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.ListClassSearch[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.ListClassEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedSetStructAdd[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedSetStructEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedSetClassAdd[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "," +
                                 this.Timings.SortedSetClassEnumerate[nIndex].TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
