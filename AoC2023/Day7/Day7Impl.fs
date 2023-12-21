module Day7Impl

type CardRank =
    | Joker = 1
    | Two = 2
    | Three = 3
    | Four = 4
    | Five = 5
    | Six = 6
    | Seven = 7
    | Eight = 8
    | Nine = 9
    | Ten = 10
    | Jack = 11
    | Queen = 12
    | King = 13
    | Ace = 14

let createRank rank =
    match rank with
    | '2' -> CardRank.Two
    | '3' -> CardRank.Three
    | '4' -> CardRank.Four
    | '5' -> CardRank.Five
    | '6' -> CardRank.Six
    | '7' -> CardRank.Seven
    | '8' -> CardRank.Eight
    | '9' -> CardRank.Nine
    | 'T' -> CardRank.Ten
    | 'J' -> CardRank.Jack
    | 'Q' -> CardRank.Queen
    | 'K' -> CardRank.King
    | 'A' -> CardRank.Ace
    | _ -> failwith "Invalid card rank"

let createRankWithJoker rank =
    if rank = 'J' then CardRank.Joker else createRank rank

type HandType =
    | HighCard of CardRank
    | OnePair
    | TwoPair
    | ThreeCard
    | FullHouse
    | FourCard
    | FiveCard

type Card = { Rank: CardRank }

let getHandType (cards: Card array) =
    let rankCounts =
        cards
        |> Array.groupBy (_.Rank)
        |> Array.map (fun (rank, cards) -> (rank, cards.Length))
        |> Array.sortByDescending fst
        |> Array.sortByDescending snd

    // Here be dragons
    match rankCounts with
    | [| (_, 5) |] -> FiveCard
    | [| (_, 4); (CardRank.Joker, 1) |]
    | [| (CardRank.Joker, 4); (_, 1) |]
    | [| (_, 3); (CardRank.Joker, 2) |]
    | [| (CardRank.Joker, 3); (_, 2) |] -> FiveCard
    | [| (_, 4); (x, 1) |] when x <> CardRank.Joker -> FourCard
    | [| (_, 3); (_, 1); (CardRank.Joker, 1) |]
    | [| (CardRank.Joker, 3); (_, 1); (_, 1) |]
    | [| (_, 2); (CardRank.Joker, 2); (_, 1) |] -> FourCard
    | [| (x, 3); (y, 2) |] when x <> CardRank.Joker && y <> CardRank.Joker -> FullHouse
    | [| (_, 2); (_, 2); (CardRank.Joker, 1) |] -> FullHouse
    | [| (x, 3); (_, 1); (_, 1) |] when x <> CardRank.Joker -> ThreeCard
    | [| (CardRank.Joker, 2); (_, 1); (_, 1); (_, 1) |] -> ThreeCard
    | [| (_, 2); (_, 1); (_, 1); (CardRank.Joker, 1) |] -> ThreeCard
    | [| (x, 2); (y, 2); (_, 1) |] when x <> CardRank.Joker && y <> CardRank.Joker -> TwoPair
    | [| (x, 2); (_, 1); (_, 1); (_, 1) |] when x <> CardRank.Joker -> OnePair
    | [| (_, 1); (_, 1); (_, 1); (_, 1); (CardRank.Joker, 1) |] -> OnePair
    | _ -> cards |> Array.maxBy (_.Rank) |> (fun x -> HighCard x.Rank)

type Hand(cards: string, bid: int, hasJoker: bool) =
    let cards' =
        match cards.Length with
        | 5 ->
            cards
            |> Array.ofSeq
            |> Array.map (fun x ->
                if not hasJoker then
                    { Rank = createRank x }
                else
                    { Rank = createRankWithJoker x })
        | _ -> failwith "Invalid number of cards"

    let handType = getHandType cards'

    member this.Cards = cards'
    member this._Cards = cards
    member this.Bid = bid
    member this.HandType = handType

    override this.ToString() =
        $"{this.HandType} ({this._Cards}, {this.Bid})"

let (|LeftIsGreater|RightIsGreater|Equal|) (left: HandType, right: HandType) =
    match left, right with
    | FiveCard, FiveCard
    | FourCard, FourCard
    | FullHouse, FullHouse
    | ThreeCard, ThreeCard
    | TwoPair, TwoPair
    | OnePair, OnePair
    | HighCard _, HighCard _ -> Equal
    | _ ->
        if left > right then LeftIsGreater
        elif left < right then RightIsGreater
        else Equal

let compareHands (left: Hand) (right: Hand) =
    let compareHandTypes (leftType: HandType) (rightType: HandType) =
        match leftType, rightType with
        | LeftIsGreater -> 1
        | RightIsGreater -> -1
        | Equal -> 0

    let firstRuling = compareHandTypes left.HandType right.HandType

    if firstRuling <> 0 then
        firstRuling
    else
        Array.zip left.Cards right.Cards
        |> Array.fold (fun acc (l, r) -> if acc <> 0 then acc else compare l.Rank r.Rank) 0

let sortHands (hands: Hand array) = hands |> Array.sortWith compareHands

let p1 (input: string seq) =
    let hands =
        input
        |> Seq.map (fun x ->
            let split = x.Split(" ")
            Hand(split[0], int split[1], false))
        |> Seq.toArray

    let sortedHands = hands |> sortHands

    let result =
        sortedHands
        |> Array.indexed
        |> Array.sumBy (fun (i, hand) -> int64 (hand.Bid * (i + 1)))

    result

let p2 (input: string seq) =
    let hands =
        input
        |> Seq.map (fun x ->
            let split = x.Split(" ")
            Hand(split[0], int split[1], true))
        |> Seq.toArray

    let sortedHands = hands |> sortHands

    let result =
        sortedHands
        |> Array.indexed
        |> Array.sumBy (fun (i, hand) -> int64 (hand.Bid * (i + 1)))

    result
