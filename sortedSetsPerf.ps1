# cd ".\Documents\tools\SortedSetsPerf\bin\x64\Release\net5.0"
Import-Module ".\SortedSetsPerf.dll"

$timings = Get-SortedSetsPerf

$location = Get-Location
Write-Timings -Timings $timings -File "$location\sortedSetsPerf.csv"