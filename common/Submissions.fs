namespace AoC.Common

open System
open System.IO
open FSharp.Json
open FSharp.Core

module Submissions =
  type Submission =
    { Solved: bool
      Solution: string
      Attempts: string array
      Time: float }

  let private submissionFile = Path.Join(Env.ProjectDir + "/.submissions.json")

  let private load () =
    if File.Exists(submissionFile) then
      Json.deserialize<Map<string, Submission>> (File.ReadAllText submissionFile)
    else
      Map.empty

  let private save (submissions: Map<string, Submission>) =
    File.WriteAllText(submissionFile, Json.serialize submissions)

  let private key (day: int) (part: int) = $"Day%02d{day}/{part}"

  let private get (day: int) (part: int) = load().TryFind(key day part)

  let isSolved (day: int) (part: int) =
    match get day part with
    | Some { Solved = true } -> true
    | _ -> false

  let isSolution (day: int) (part: int) (answer: string) =
    match get day part with
    | Some { Solution = s } -> s = answer && s <> ""
    | _ -> false

  let isAttempted (day: int) (part: int) (answer: string) =
    match get day part with
    | Some { Attempts = attempts } -> Array.contains answer attempts
    | _ -> false

  let addAttempt (day: int) (part: int) (answer: string) =
    let submissions = load ()

    let submission =
      match submissions.TryFind(key day part) with
      | Some s -> { s with Attempts = Array.append s.Attempts [| answer |] }
      | None ->
        { Solved = false
          Solution = ""
          Attempts = [| answer |]
          Time = 0.0 }

    save (submissions.Add(key day part, submission))

  let setSolution (day: int) (part: int) (answer, time) =
    let submissions = load ()

    let submission =
      match submissions.TryFind(key day part) with
      | Some s ->
        { s with
            Solved = true
            Solution = answer
            Time = time }
      | None ->
        { Solved = true
          Solution = answer
          Attempts = [||]
          Time = time }

    save (submissions.Add(key day part, submission))
