module Day7.Tests

open Day7Impl
open NUnit.Framework
open FsUnit

// Can't find the bug so here's a bunch of test cases

let compareCases =
    [
      // Five of a kind
      ("AAAAA", 200), ("AAAAA", 100), 0
      ("AAAAA", 200), ("KKKKK", 100), 1
      ("KKKKK", 200), ("AAAAA", 100), -1
      ("AAAAA", 200), ("JJJJJ", 100), 1
      // Four of a kind
      ("44434", 200), ("44434", 100), 0
      ("AAAAT", 200), ("TAAAA", 100), 1
      ("34333", 200), ("33334", 100), 1
      ("KKKJK", 200), ("KKJKK", 100), 1
      // Full house
      ("KKKQQ", 200), ("KKKQQ", 100), 0
      ("KKKQQ", 200), ("QQKKK", 100), 1
      ("QQKKK", 200), ("KKKQQ", 100), -1
      ("JTJTJ", 200), ("KKKQQ", 100), -1
      // Threes
      ("KKK34", 200), ("KKK34", 100), 0
      ("KKK34", 200), ("KKK35", 100), -1
      ("KKK35", 200), ("KKK34", 100), 1
      ("KKK34", 200), ("KKK56", 100), -1
      // Two pairs
      ("KKQQJ", 200), ("KKQQJ", 100), 0
      ("KKQQJ", 200), ("QQKKJ", 100), 1
      ("QQKKJ", 200), ("KKQQJ", 100), -1
      ("KKQQJ", 200), ("KKQTT", 100), 1
      // One pair
      ("KKQJT", 200), ("KKQJT", 100), 0
      ("KKQJT", 200), ("KKTJT", 100), -1
      ("KKTJT", 200), ("KKQJT", 100), 1
      ("45674", 200), ("45684", 100), -1 ]
    |> List.map TestCaseData

[<TestCaseSource(nameof compareCases)>]
let ShouldCompareHands (left: string * int, right: string * int, expected: int) =
    let l = Hand((fst left), (snd left))
    let r = Hand((fst right), (snd right))
    Assert.AreEqual(expected, compareHands l r)

let sortCases =
    [|
       // Example from the problem
       ([| Hand("32T3K", 765)
           Hand("T55J5", 684)
           Hand("KK677", 28)
           Hand("KTJJT", 220)
           Hand("QQQJA", 483) |],
        [| Hand("32T3K", 765)
           Hand("KTJJT", 220)
           Hand("KK677", 28)
           Hand("T55J5", 684)
           Hand("QQQJA", 483) |])
       // Second case
       ([|
           // High cards
           Hand("62345", 1)
           Hand("24356", 3)
           Hand("23456", 2)
           // One pair
           Hand("66234", 4)
           Hand("22456", 44)
           Hand("26634", 5)
           Hand("66234", 6)
           Hand("82366", 66)
           // Threes
           Hand("34442", 77)
           Hand("34353", 7)
           Hand("33435", 8)
           Hand("33345", 99)
           // Two pairs
           Hand("22334", 10)
           Hand("33224", 11)
           Hand("23234", 12)
           Hand("43322", 13)
           Hand("23366", 133)
           // Full house
           Hand("56565", 17)
           Hand("33232", 14)
           Hand("33322", 15)
           Hand("22233", 16)
           // Four of a kind
           Hand("56666", 21)
           Hand("55554", 20)
           Hand("55455", 18)
           Hand("45555", 19)
           // Five of a kind
           Hand("44444", 22)
           Hand("22222", 23) |],

        [|
           // High cards
           Hand("23456", 2)
           Hand("24356", 3)
           Hand("62345", 1)
           // One pair
           Hand("22456", 44)
           Hand("26634", 5)
           Hand("66234", 4)
           Hand("66234", 6)
           Hand("82366", 66)
           // Two pairs
           Hand("22334", 10)
           Hand("23234", 12)
           Hand("23366", 133)
           Hand("33224", 11)
           Hand("43322", 13)
           // Threes
           Hand("33345", 99)
           Hand("33435", 8)
           Hand("34353", 7)
           Hand("34442", 77)
           // Full house
           Hand("22233", 16)
           Hand("33232", 14)
           Hand("33322", 15)
           Hand("56565", 17)
           // Four of a kind
           Hand("45555", 19)
           Hand("55455", 18)
           Hand("55554", 20)
           Hand("56666", 21)
           // Five of a kind
           Hand("22222", 23)
           Hand("44444", 22) |]) |]

    |> Array.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource(nameof sortCases)>]
let ShouldSortHands (hands: Hand array, expected: Hand array) =
    let sortedHands = sortHands hands

    sortedHands
    |> Array.zip expected
    // |> Array.map (fun (a, b) -> (a._Cards, a.Bid), (a._Cards, b.Bid))
    |> Array.iter (fun (a, b) -> a.ToString() |> should equal (b.ToString()))
