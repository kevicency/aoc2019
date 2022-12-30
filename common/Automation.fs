namespace AoC.Common

open System.Net.Http
open Microsoft.Extensions.Configuration
open System

open System.IO

module Automation =
  let private config =
    (new ConfigurationBuilder())
      .AddUserSecrets("5e2624a9-8850-4f3c-807c-14c4fd6b532b")
      .Build()

  let private baseDir = AppDomain.CurrentDomain.BaseDirectory
  let private projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent.FullName

  let private createClient () =
    let client = new HttpClient()
    let token = config["AOC_SESSION_TOKEN"]

    if (token = null) then
      failwith "AOC_SESSION_TOKEN not found in user secrets"

    client.DefaultRequestHeaders.Add("Cookie", $"session={token}")
    client.DefaultRequestHeaders.UserAgent.ParseAdd(".NET (+via https://github.com/kmees/aoc.fs by kev.mees@gmail.com)")
    client.BaseAddress <- Uri "https://adventofcode.com"
    client

  let fetchInput (year: int) (day: int) =
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
    let inputPath = Path.Join(projectDir, $"input/Day%02d{day}.txt")

    if not (File.Exists inputPath) then
      let input = fetchInput year day |> Async.RunSynchronously

      if input <> "" then
        File.WriteAllText(inputPath, input)

    File.ReadAllLines inputPath
