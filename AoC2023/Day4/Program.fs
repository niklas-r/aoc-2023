open System.IO

let input: string seq = File.ReadLines("../../../input.txt")

let double n = n * 2

let parseNumbers (index: int) (rows: string seq) =
    rows
    |> Seq.map (_.Split(": ").[1].Split("|").[index])
    |> Seq.map (fun line -> line.Trim().Replace("  ", " ").Split(" ") |> Array.map int |> Array.toList)
    |> Seq.toList

let parseCards (rows: string seq) =
    let winningNumbers = parseNumbers 0 rows
    let myNumbers = parseNumbers 1 rows

    List.zip winningNumbers myNumbers


let p1 input =
    let cards = parseCards input

    cards
        |> Seq.map (fun (winningNumbers, myNumbers) ->
            let matches = Set.intersect (Set.ofList winningNumbers) (Set.ofList myNumbers)

            match matches with
            | m when Seq.isEmpty m -> 0
            | _ ->
                matches
                |> Set.toList
                |> List.fold (fun acc _ -> if acc = 0 then 1 else double acc) 0)
        |> Seq.sum

printfn "p1: %A" (p1 input)
