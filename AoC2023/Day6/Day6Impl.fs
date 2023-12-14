module Day6Impl

open System.Text.RegularExpressions

type RaceResult = { time: int64; distance: int64 }

type RaceAttempt =
    { resultToBeat: RaceResult
      myResults: RaceResult array }

let parseRaceResults (input: string array) =
    let re = Regex("\d+", RegexOptions.Compiled)
    let times = re.Matches input[0]
    let distances = re.Matches input[1]

    Seq.zip times distances
    |> Seq.map (fun (time, distance) ->
        { time = int64 time.Value
          distance = int64 distance.Value })
    |> Seq.toArray

let doRaceAttempts (raceResults: RaceResult array) =
    raceResults
    |> Array.map (fun raceResult ->
        let results =
            [| 0L .. raceResult.time |]
            |> Array.map (fun chargeTime ->
                let distance = chargeTime * (raceResult.time - chargeTime)

                { time = chargeTime
                  distance = distance })

        { resultToBeat = raceResult
          myResults = results })

let p1 (input: string seq) =
    let raceResults = parseRaceResults (input |> Seq.toArray)
    let raceAttempts = doRaceAttempts raceResults

    let numberOfNewTrackRecords =
        raceAttempts
        |> Array.map (fun raceAttempt ->
            raceAttempt.myResults
            |> Array.filter (fun myResult -> myResult.distance > raceAttempt.resultToBeat.distance))
        |> Array.fold (fun acc x -> acc * x.Length) 1

    numberOfNewTrackRecords

let p2 (input: string seq) =
    let raceResults =
        parseRaceResults (input |> Seq.toArray)
        |> Array.fold
            (fun acc x ->
                [| { time = int64 ((string acc[0].time) + (string x.time))
                     distance = int64 ((string acc[0].distance) + (string x.distance)) } |])
            [| { time = 0; distance = 0 } |]

    let raceAttempts = doRaceAttempts raceResults

    let numberOfNewTrackRecords =
        raceAttempts
        |> Array.map (fun raceAttempt ->
            raceAttempt.myResults
            |> Array.filter (fun myResult -> myResult.distance > raceAttempt.resultToBeat.distance))
        |> Array.fold (fun acc x -> acc * x.Length) 1

    numberOfNewTrackRecords
