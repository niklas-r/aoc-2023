﻿module Program

open System.IO

let input = File.ReadLines("../../../input.txt")
let example = File.ReadLines("../../../example.txt")

let run part runner input expected =
    let result = runner input
    printfn "part %s: %d (should yield %d)" part result expected
    assert (result = expected)

run "example 1" Day7Impl.p1 example 6440
run "part 1" Day7Impl.p1 input 250232501

run "example 2" Day7Impl.p2 example 5905
run "part 2" Day7Impl.p2 input 249138943
