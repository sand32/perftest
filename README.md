# perftest
This project is meant to compare Node.js and ASP.NET Core in a more real-world scenario where they must implement a modern REST API (C# must use reflection to do this, in my experience) and handle JSON serialization.

# Test System
Intel(R) Core(TM) i5-5250U CPU @ 1.60GHz
16GB DDR3 @ 1600MHz
Ubuntu 16.04 LTS 64-bit

# Results
These results were gathered using [wrk](https://github.com/wg/wrk).

The following results seem to corroborate the general benchmarks available via various sources. ASP.NET Core remains faster despite the overhead of reflection.

## ASP.NET Core List Route, No Parameters
```
Running 10s test @ http://localhost:5000
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.28ms    1.72ms  46.52ms   95.32%
    Req/Sec     4.91k   302.07     5.49k    70.50%
  97818 requests in 10.00s, 104.86MB read
Requests/sec:   9781.35
Transfer/sec:     10.49MB
```

## Node.js List Route, No Parameters
```
Running 10s test @ http://localhost:5000
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.39ms  321.33us   8.61ms   95.45%
    Req/Sec     3.61k   294.50     3.92k    95.00%
  71948 requests in 10.00s, 78.70MB read
Requests/sec:   7193.77
Transfer/sec:      7.87MB
```

## ASP.NET Core List Route, With Search Parameter
```
Running 10s test @ http://localhost:5000?job=Friend
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.44ms    2.21ms  36.67ms   94.21%
    Req/Sec     4.85k   377.60     5.75k    70.00%
  96426 requests in 10.00s, 103.37MB read
Requests/sec:   9641.43
Transfer/sec:     10.34MB
```

## Node.js List Route, With Search Parameter
```
Running 10s test @ http://localhost:5000?job=Friend
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.36ms  345.03us  11.11ms   96.22%
    Req/Sec     3.71k   202.99     3.98k    86.50%
  73773 requests in 10.00s, 80.70MB read
Requests/sec:   7377.05
Transfer/sec:      8.07MB
```

## ASP.NET Core List Route, With Selection Parameter
```
Running 10s test @ http://localhost:5000?fields=name,job
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.37ms    2.06ms  48.58ms   94.88%
    Req/Sec     4.90k   490.20     5.83k    85.50%
  97574 requests in 10.00s, 104.60MB read
Requests/sec:   9754.91
Transfer/sec:     10.46MB
```

## Node.js List Route, With Selection Parameter
```
Running 10s test @ http://localhost:5000?fields=name,job
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.34ms  248.66us   7.12ms   96.48%
    Req/Sec     3.76k   137.07     4.03k    68.50%
  74841 requests in 10.00s, 81.87MB read
Requests/sec:   7483.74
Transfer/sec:      8.19MB
```
