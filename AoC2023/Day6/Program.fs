module Program

open System.IO

let input = File.ReadLines("../../../input.txt")
let example = File.ReadLines("../../../example.txt")

let run part runner input expected =
    printfn "part %s: %d (should yield %d)" part (runner input) expected
    assert (runner input = expected)

run "example 1" Day6Impl.p1 example 288
run "example 1" Day6Impl.p1 input 1083852
run "part 1" Day6Impl.p1 input 1083852
