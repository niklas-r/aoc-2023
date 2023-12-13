module Day5Impl

open System

let mapSeeds (line: string) =
    line
    |> _.Replace("seeds: ", "")
        .Split(" ", StringSplitOptions.RemoveEmptyEntries * StringSplitOptions.TrimEntries)
    |> Array.map int64

let partitionOn groupComplete list_ =
    let rec inner currentGroup remainingList =
        match remainingList with
        | [] -> [ currentGroup ]
        | head :: tail ->
            let complete = groupComplete currentGroup
            let additionalGroup = if complete then [ currentGroup ] else []
            let newCurrentGroup = if complete then [] else currentGroup @ [ head ]
            let newRemaining = if complete then remainingList else tail
            additionalGroup @ (inner newCurrentGroup newRemaining)

    inner [] list_

let parseMapping (group: string list) =
    group[1 .. (group.Length - 2)]
    |> List.map (fun str -> str.Split(" ") |> Array.map int64)
    |> List.map (fun x -> x[0], x[1], x[2])

let runMappings (seed: int64) (mappings: (int64 * int64 * int64) list list) =
    let rec inner (source: int64) (remainingMappings: (int64 * int64 * int64) list) =
        match remainingMappings with
        | [] -> source
        | head :: tail ->
            let d, s, l = head

            if source < s || source > (s + l - (int64 1)) then
                inner source tail
            else
                d + l - ((s + l) - source)

    let mutable result = seed

    for mapping in mappings do
        result <- inner result mapping

    result

let groupComplete (group: string list) =
    match group with
    | [] -> false // empty
    | [ _ ] -> false // one item
    | head :: tail -> // N items
        let last = tail |> List.last
        head.Contains(":") && String.IsNullOrEmpty last

let p1 (input: string seq) =
    let seeds = input |> Seq.head |> mapSeeds

    let mappings =
        partitionOn groupComplete (input |> Seq.skip 2 |> Seq.toList)
        |> List.map parseMapping

    let results = seeds |> Array.map (fun seed -> runMappings seed mappings)

    results |> Array.min

let p2 (input: string seq) =
    let seedRanges =
        input
        |> Seq.head
        |> mapSeeds
        |> Array.chunkBySize 2
        |> Array.map (fun x -> x.[0], x.[1])

    let mappings =
        partitionOn groupComplete (input |> Seq.skip 2 |> Seq.toList)
        |> List.map parseMapping

    let mutable minResult = Int64.MaxValue

    for index, seedRange in seedRanges |> Array.indexed do
        let a, b = seedRange
        let mutable i = a
        printfn "Starting range %d (%d to %d)" index a (a + b)

        // This takes a very long time to run
        while i < a + b do
            let result = runMappings i mappings

            if result < minResult then
                minResult <- result

            i <- i + (int64 1)

        printfn "Finished range %d" index

    minResult
