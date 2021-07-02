### Overview
This repo is a fork of [Jamie Fristom's sorted container tests](https://github.com/JamieFristrom/SortedContainerTests) and extends the results
Jamie [published on his blog](https://www.gamedevblog.com/2019/07/sorted-sets-in-c-and-performance-a-mystery-remains.html) in July 2019. The main
changes here are

1. .NET 5.0 build and conversion from a command line untility to a PowerShell Core cmdlet.
2. Implementation of burnin iterations and replicated measurements rather than relying on singleton measurements.