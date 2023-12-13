open System.IO

let input = File.ReadLines("../../../input.txt")
let example = File.ReadLines("../../../example.txt")

let run part runner input expected =
    printfn "part %s: %d (should yield %d)" part (runner input) expected
    assert (runner input = expected)

run "example 1" Day5Impl.p1 example (int64 35)
run "example 2" Day5Impl.p2 example (int64 46)

run "1" Day5Impl.p1 input (int64 226172555)
run "2" Day5Impl.p2 input (int64 47909639)
