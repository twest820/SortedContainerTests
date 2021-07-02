library(dplyr)
library(ggplot2)
library(readr)

theme_set(theme_bw())

timings = read_csv("bin/x64/Release/net5.0/sortedSetsPerf.csv", col_types = cols(n = col_integer(), .default = col_double()))

# plot collection enumeration times
ggplot(timings) + 
  geom_line(aes(x = n, y = 1000 * listStructEnumerate, color = "List", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * listStructEnumerate, color = "List")) +
  geom_line(aes(x = n, y = 1000 * sortedSetStructEnumerate, color = "SortedSet", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * sortedSetStructEnumerate, color = "SortedSet")) +
  geom_line(aes(x = n, y = 1000 * sortedDictStructEnumerate, color = "SortedDictionary", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * sortedDictStructEnumerate, color = "SortedDictionary")) +
  geom_line(aes(x = n, y = 1000 * sortedListStructEnumerate, color = "SortedList", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * sortedListStructEnumerate, color = "SortedList")) +
  geom_line(aes(x = n, y = 1000 * listClassEnumerate, color = "List", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * listClassEnumerate, color = "List")) +
  geom_line(aes(x = n, y = 1000 * sortedSetClassEnumerate, color = "SortedSet", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * sortedSetClassEnumerate, color = "SortedSet")) +
  geom_line(aes(x = n, y = 1000 * sortedDictClassEnumerate, color = "SortedDictionary", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * sortedDictClassEnumerate, color = "SortedDictionary")) +
  geom_line(aes(x = n, y = 1000 * sortedListClassEnumerate, color = "SortedList", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * sortedListClassEnumerate, color = "SortedList")) +
  labs(x = "items in collection", y = "enumeration time, μs (i5-8256U single core turbo)", color = NULL, linetype = NULL) +
  scale_color_discrete(guide = guide_legend(ncol = 2)) +
  scale_linetype_manual(breaks = c("class", "struct"), values = c("solid", "longdash")) +
  scale_x_log10(breaks = c(1, 2, 5, 10, 20, 50, 100, 200, 500, 1000),
                minor_breaks = c(3, 4, 6, 7, 8, 9, 30, 40, 60, 70, 80, 90, 300, 400, 600, 700, 800, 900, 1500)) + 
  scale_y_log10(breaks = c(0.1, 0.2, 0.5, 1, 2, 5, 10, 50, 100, 200, 500, 1000),
                minor_breaks = c(0.15, 0.3, 0.4, 0.6, 0.7, 0.8, 0.9, 1.5, 3, 4, 6, 7, 8, 9, 15, 30, 40, 60, 70, 80, 90, 150, 300, 400, 600, 700, 800, 900)) +
  theme(legend.position = c(0.02, 0.98), legend.justification = c(0, 1))

# plot collection fill times
ggplot(timings) + 
  geom_line(aes(x = n, y = 1000 * listStructSearch, color = "List", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * listStructSearch, color = "List")) +
  geom_line(aes(x = n, y = 1000 * sortedSetStructAdd, color = "SortedSet", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * sortedSetStructAdd, color = "SortedSet")) +
  geom_line(aes(x = n, y = 1000 * sortedDictStructAdd, color = "SortedDictionary", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * sortedDictStructAdd, color = "SortedDictionary")) +
  geom_line(aes(x = n, y = 1000 * sortedListStructAdd, color = "SortedList", linetype = "struct")) +
  geom_point(aes(x = n, y = 1000 * sortedListStructAdd, color = "SortedList")) +
  geom_line(aes(x = n, y = 1000 * listClassSearch, color = "List", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * listClassSearch, color = "List")) +
  geom_line(aes(x = n, y = 1000 * sortedSetClassAdd, color = "SortedSet", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * sortedSetClassAdd, color = "SortedSet")) +
  geom_line(aes(x = n, y = 1000 * sortedDictClassAdd, color = "SortedDictionary", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * sortedDictClassAdd, color = "SortedDictionary")) +
  geom_line(aes(x = n, y = 1000 * sortedListClassAdd, color = "SortedList", linetype = "class")) +
  geom_point(aes(x = n, y = 1000 * sortedListClassAdd, color = "SortedList")) +
  labs(x = "items in collection", y = "creation time, μs (i5-8256U single core turbo)", color = NULL, linetype = NULL) +
  scale_color_discrete(guide = guide_legend(ncol = 2)) +
  scale_linetype_manual(breaks = c("class", "struct"), values = c("solid", "longdash")) +
  scale_x_log10(breaks = c(1, 2, 5, 10, 20, 50, 100, 200, 500, 1000),
                minor_breaks = c(3, 4, 6, 7, 8, 9, 30, 40, 60, 70, 80, 90, 300, 400, 600, 700, 800, 900, 1500)) + 
  scale_y_log10(breaks = c(0.1, 0.2, 0.5, 1, 2, 5, 10, 50, 100, 200, 500, 1000),
                minor_breaks = c(0.15, 0.3, 0.4, 0.6, 0.7, 0.8, 0.9, 1.5, 3, 4, 6, 7, 8, 9, 15, 30, 40, 60, 70, 80, 90, 150, 300, 400, 600, 700, 800, 900)) +
  theme(legend.position = c(0.02, 0.98), legend.justification = c(0, 1))
