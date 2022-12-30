namespace AoC.Common

open System.Net.Http
open System

open System.IO

module Automation =
  open System.Collections.Generic

  let private createClient () =
    let client = new HttpClient()

    if (Env.SessionToken = null) then
      failwith "AOC_SESSION_TOKEN not found in user secrets"

    client.DefaultRequestHeaders.Add("Cookie", $"session={Env.SessionToken}")
    client.DefaultRequestHeaders.UserAgent.ParseAdd(".NET (+via https://github.com/kmees/aoc.fs by kev.mees@gmail.com)")
    client.BaseAddress <- Uri "https://adventofcode.com"
    client

  let private fetchInput (year: int) (day: int) =
    async {
      try
        printfn "Fetching input for %d/%02d" year day
        let client = createClient ()
        let url = $"{year}/day/{day}/input"
        let! response = client.GetAsync url |> Async.AwaitTask
        let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask
        return content
      with :? HttpRequestException as ex ->
        printfn "Error fetching input: %s" ex.Message
        return ""
    }

  let ensureInput (year: int) (day: int) =
    let inputPath = Path.Join(Env.ProjectDir, $"input/Day%02d{day}.txt")

    if not (File.Exists inputPath) then
      let input = fetchInput year day |> Async.RunSynchronously

      if input <> "" then
        File.WriteAllText(inputPath, input)

    File.ReadAllLines inputPath

  let private submitAnswer (year: int) (day: int) (part: int) (answer: string) =
    async {
      try
        let client = createClient ()
        let url = $"{year}/day/{day}/answer"

        let content =
          new FormUrlEncodedContent([| KeyValuePair("level", part.ToString()); KeyValuePair("answer", answer) |])

        let! response = client.PostAsync(url, content) |> Async.AwaitTask

        if not response.IsSuccessStatusCode then
          printfn "Error submitting answer: %s" response.ReasonPhrase
          return false, response.ReasonPhrase
        else
          let! content = response.Content.ReadAsStringAsync() |> Async.AwaitTask

          if
            content.Contains("That's the right answer!")
            || content.Contains("You don't seem to be solving the right level.")
          then
            return true, content
          else
            return false, content
      with :? HttpRequestException as ex ->
        printfn "Error submitting answer: %s" ex.Message
        return false, ex.Message
    }

  let submit (day: int) (part: int) (answer, time) =
    let year = Env.Year

    if Submissions.isSolution day part answer then
      Submissions.setSolution day part (answer, time)
      printfn "✅ Answer is correct (already submitted)"
    else if Submissions.isAttempted day part answer then
      printfn "❌ Answer is incorrect (already attempted)"
    else
      let success, content = submitAnswer year day part answer |> Async.RunSynchronously

      if success then
        Submissions.setSolution day part (answer, time)
        printfn "✅ Answer is correct"
      else
        Submissions.addAttempt day part answer
        printfn "❌ Answer is incorrect (%s)" content
