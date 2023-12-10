open System.Collections.Generic
open System.IO

type Card =
    { Number: int
      WinningNumbers: int list
      MyNumbers: int list }

type CardMatchNumber = int

type Tree<'a> =
    | Leaf of value: 'a
    | Node of value: 'a * children: Tree<'a> list

let input: string seq = File.ReadLines("../../../input.txt")

let double n = n * 2

let parseNumbers (index: int) (rows: string seq) =
    rows
    |> Seq.map (_.Split(": ").[1].Split("|").[index])
    |> Seq.map (fun line -> line.Trim().Replace("  ", " ").Split(" ") |> Array.map int |> Array.toList)
    |> Seq.toList

let parseCards (rows: string seq) : Card list =
    let winningNumbers = parseNumbers 0 rows
    let myNumbers = parseNumbers 1 rows

    List.fold2
        (fun acc winningNumbers myNumbers ->
            let card =
                { Number = acc.Length + 1
                  WinningNumbers = winningNumbers
                  MyNumbers = myNumbers }

            acc @ [card])
        []
        winningNumbers
        myNumbers

let buildTrees (cards: Card list) =
    let rec buildTree (cardIndex: int) =
        if cardIndex >= cards.Length then
            []
        else
            let card = cards.[cardIndex]
            let winningNumbersSet = HashSet<_>(card.WinningNumbers)
            let matches = card.MyNumbers |> List.filter winningNumbersSet.Contains
            let matchCount = matches.Length
            if matchCount = 0 then
                [Leaf card]
            else
                let wonCards = List.collect (fun i -> buildTree (cardIndex + i)) [1 .. matchCount]
                [Node(card, wonCards)]

    List.collect buildTree [0 .. cards.Length - 1]

let rec sumTree (tree: Tree<Card>) =
    match tree with
    | Leaf _ -> 1
    | Node (_, children) -> 1 + (children |> List.map sumTree |> List.sum)

let p1 input =
    let cards = parseCards input

    cards
    |> Seq.map (fun card ->
        let matches =
            Set.intersect (Set.ofList card.WinningNumbers) (Set.ofList card.MyNumbers)

        match matches with
        | m when Seq.isEmpty m -> 0
        | _ ->
            matches
            |> Set.toList
            |> List.fold (fun acc _ -> if acc = 0 then 1 else double acc) 0)
    |> Seq.sum

let p2 input =
    let cards = parseCards input
    let trees = buildTrees cards
    let sum = trees |> List.map sumTree |> List.sum

    sum

printfn "p1: %A" (p1 input)
printfn "p2: %A" (p2 input)
