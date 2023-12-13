open System.IO

let input = File.ReadLines("../../../input.txt")
let example = File.ReadLines("../../../example.txt")

printfn "example p1: %d (should yield 35)" (Day5Impl.p1 example)
assert (Day5Impl.p1 example = (int64 35))
printfn "p1: %d" (Day5Impl.p1 input)