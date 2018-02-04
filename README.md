# perftest
This project is meant to compare Node.js and ASP.NET Core in a more real-world scenario where they must implement a modern REST API (C# must use reflection to do this, in my experience) and handle JSON serialization.

# Results
These results were gathered using [wrk](https://github.com/wg/wrk).

The following results seems to corroborate the general benchmarks available via various sources. ASP.NET Core remains faster despite the overhead of reflection.

## ASP.NET Core List Route, No Parameters
```
Running 10s test @ http://localhost:5000/
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.43ms    2.08ms  43.59ms   94.09%
    Req/Sec     4.79k   374.67     5.51k    72.50%
  95279 requests in 10.00s, 102.14MB read
Requests/sec:   9526.74
Transfer/sec:     10.21MB
```

## Node.js List Route, No Parameters
```
Running 10s test @ http://localhost:5000/
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.50ms  749.21us  20.97ms   93.55%
    Req/Sec     3.44k   636.86     3.94k    89.11%
  69243 requests in 10.10s, 75.74MB read
Requests/sec:   6855.92
Transfer/sec:      7.50MB
```

## ASP.NET Core List Route, With Search Parameter
```
Running 10s test @ http://localhost:5000?job=Friend
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     7.92ms   40.79ms 390.74ms   97.01%
    Req/Sec     4.80k   622.02     8.50k    94.87%
  93263 requests in 10.10s, 99.98MB read
Requests/sec:   9234.06
Transfer/sec:      9.90MB
```

## Node.js List Route, With Search Parameter
```
Running 10s test @ http://localhost:5000?job=Friend
  2 threads and 10 connections
  Thread Stats   Avg      Stdev     Max   +/- Stdev
    Latency     1.34ms  235.93us   5.90ms   93.15%
    Req/Sec     3.75k   184.64     4.05k    75.00%
  74658 requests in 10.00s, 81.67MB read
Requests/sec:   7462.35
Transfer/sec:      8.16MB
```
