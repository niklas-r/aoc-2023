module Day6Impl

open System.Text.RegularExpressions

type RaceResult = { time: int; distance: int }

type RaceAttempt =
    { resultToBeat: RaceResult
      myResults: RaceResult array }

let parseRaceResults (input: string array) =
    let re = Regex("\d+", RegexOptions.Compiled)
    let times = re.Matches input[0]
    let distances = re.Matches input[1]

    Seq.zip times distances
    |> Seq.map (fun (time, distance) ->
        { time = int time.Value
          distance = int distance.Value })
    |> Seq.toArray

let doRaceAttempts (raceResults: RaceResult array) =
    raceResults
    |> Array.map (fun raceResult ->
        let results =
            [| 0 .. raceResult.time |]
            |> Array.map (fun chargeTime ->
                let distance = chargeTime * (raceResult.time - chargeTime)
                { time = chargeTime; distance = distance })

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
