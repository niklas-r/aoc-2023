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
    let l = Hand((fst left), (snd left), false)
    let r = Hand((fst right), (snd right), false)
    Assert.AreEqual(expected, compareHands l r)

let sortCases =
    [|
       // Example from the problem
       ([| Hand("32T3K", 765, false)
           Hand("T55J5", 684, false)
           Hand("KK677", 28, false)
           Hand("KTJJT", 220, false)
           Hand("QQQJA", 483, false) |],
        [| Hand("32T3K", 765, false)
           Hand("KTJJT", 220, false)
           Hand("KK677", 28, false)
           Hand("T55J5", 684, false)
           Hand("QQQJA", 483, false) |])
       // Second case
       ([|
           // High cards
           Hand("62345", 1, false)
           Hand("24356", 3, false)
           Hand("23456", 2, false)
           // One pair
           Hand("66234", 4, false)
           Hand("22456", 44, false)
           Hand("26634", 5, false)
           Hand("66234", 6, false)
           Hand("82366", 66, false)
           // Threes
           Hand("34442", 77, false)
           Hand("34353", 7, false)
           Hand("33435", 8, false)
           Hand("33345", 99, false)
           // Two pairs
           Hand("22334", 10, false)
           Hand("33224", 11, false)
           Hand("23234", 12, false)
           Hand("43322", 13, false)
           Hand("23366", 133, false)
           // Full house
           Hand("56565", 17, false)
           Hand("33232", 14, false)
           Hand("33322", 15, false)
           Hand("22233", 16, false)
           // Four of a kind
           Hand("56666", 21, false)
           Hand("55554", 20, false)
           Hand("55455", 18, false)
           Hand("45555", 19, false)
           // Five of a kind
           Hand("44444", 22, false)
           Hand("22222", 23, false) |],

        [|
           // High cards
           Hand("23456", 2, false)
           Hand("24356", 3, false)
           Hand("62345", 1, false)
           // One pair
           Hand("22456", 44, false)
           Hand("26634", 5, false)
           Hand("66234", 4, false)
           Hand("66234", 6, false)
           Hand("82366", 66, false)
           // Two pairs
           Hand("22334", 10, false)
           Hand("23234", 12, false)
           Hand("23366", 133, false)
           Hand("33224", 11, false)
           Hand("43322", 13, false)
           // Threes
           Hand("33345", 99, false)
           Hand("33435", 8, false)
           Hand("34353", 7, false)
           Hand("34442", 77, false)
           // Full house
           Hand("22233", 16, false)
           Hand("33232", 14, false)
           Hand("33322", 15, false)
           Hand("56565", 17, false)
           // Four of a kind
           Hand("45555", 19, false)
           Hand("55455", 18, false)
           Hand("55554", 20, false)
           Hand("56666", 21, false)
           // Five of a kind
           Hand("22222", 23, false)
           Hand("44444", 22, false) |])
       ([| Hand("32T3K", 765, true)
           Hand("T55J5", 684, true)
           Hand("KK677", 28, true)
           Hand("KTJJT", 220, true)
           Hand("QQQJA", 483, true) |],
        [| Hand("32T3K", 765, true)
           Hand("KK677", 28, true)
           Hand("T55J5", 684, true)
           Hand("QQQJA", 483, true)
           Hand("KTJJT", 220, true) |]) |]

    |> Array.map (fun (a, b) -> TestCaseData(a, b))

[<TestCaseSource(nameof sortCases)>]
let ShouldSortHands (hands: Hand array, expected: Hand array) =
    let sortedHands = sortHands hands

    sortedHands
    |> Array.zip expected
    |> Array.iter (fun (a, b) -> a.ToString() |> should equal (b.ToString()))


let handTypeCases =
    [|
        "AJJJJ", 1, true, HandType.FiveCard
        "AAJJJ", 2, true, HandType.FiveCard
        "AAAJJ", 3, true, HandType.FiveCard
        "AAAAJ", 4, true, HandType.FiveCard
        "AAAAA", 5, true, HandType.FiveCard
        "JJJJJ", 5, true, HandType.FiveCard
        "ATJJJ", 6, true, HandType.FourCard
        "AATJJ", 7, true, HandType.FourCard
        "AAATJ", 8, true, HandType.FourCard
        "AAAAT", 9, true, HandType.FourCard
        "ATTJJ", 10, true, HandType.FourCard
        "AATTJ", 11, true, HandType.FullHouse
        "AAATT", 12, true, HandType.FullHouse
        "AT4JJ", 13, true, HandType.ThreeCard
        "AA4TJ", 14, true, HandType.ThreeCard
        "AAA4T", 15, true, HandType.ThreeCard
        "A234J", 16, true, HandType.OnePair
        "A2345", 17, true, HandType.HighCard CardRank.Ace
    |]
    |> Array.map (fun (a, b, c, d) -> TestCaseData(a, b, c, d))

[<TestCaseSource(nameof handTypeCases)>]
let ShouldGetHandType (hand: string, bid: int, hasJoker: bool, expected: HandType) =
    let h = Hand(hand, bid, hasJoker)
    h.HandType |> should equal expected
