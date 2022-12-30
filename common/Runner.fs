namespace AoC.Common

open Microsoft.FSharp.Core

module Runner =
  open System
  open System.Diagnostics
  open System.Reflection
  open System.IO
  open Automation

  module private Internals =
    let year = AppDomain.CurrentDomain.FriendlyName.Replace("aoc", "") |> int
    let maxDay = 25
    let maxPart = 2

    type DRun = delegate of string[] -> string
    type DEx = delegate of unit -> obj[] list

    type Part =
      { Day: int
        Part: int
        Run: DRun
        Ex: DEx }

    type Day = { Day: int; Parts: Part list }

    let tryGetPart (t: Type) (day: int) (part: int) : Part option =
      opt {
        let! run =
          opt {
            let! methodInfo =
              t.GetMethod($"p{part}", BindingFlags.Public ||| BindingFlags.Static)
              |> Option.ofObj

            let invoke (args: obj) =
              methodInfo.Invoke(null, [| args |]).ToString()

            return invoke
          }

        let! ex =
          opt {
            let! methodInfo =
              t.GetMethod($"p{part}ex", BindingFlags.Public ||| BindingFlags.Static)
              |> Option.ofObj

            return methodInfo.CreateDelegate<DEx>()
          }

        return
          { Day = day
            Part = part
            Run = run
            Ex = ex }
      }

    let tryGetDay (assembly: Assembly) (day: int) : Day option =
      assembly.GetTypes()
      |> Seq.tryFind (fun t -> t.Name = $"Day%02d{day}" || t.Name = $"Day%d{day}")
      |> Option.map (fun t ->
        { Day = day
          Parts = [ 1..maxPart ] |> List.choose (tryGetPart t day) })

    let getDays (assembly: Assembly) =
      [ 1..maxDay ] |> List.choose (tryGetDay assembly)

    let runExamples (run: DRun, ex: DEx) =
      printf $"Ex: \t"
      let examples = ex.Invoke()
      let stopwatch = Stopwatch.StartNew()

      examples
      |> List.iter (fun (args) ->
        let input = args[ 0 ].ToString()

        if input <> "" then
          let expected = args[ 1 ].ToString()
          let lines = Input.splitlines input
          let result = run.Invoke(lines)

          if result <> expected then
            printfn $"FAILED"
            failwith $"Example failed.\n> Expected: {expected}\n> Result:   {result}")

      printfn $"OK\t({stopwatch.Elapsed.TotalMilliseconds}ms)"

    let runPart (part: Part) =
      printfn $"\nDay {part.Day}, Part {part.Part}"

      let input = ensureInput year part.Day

      runExamples (part.Run, part.Ex)

      let stopwatch = Stopwatch.StartNew()
      let answer = part.Run.Invoke(input)
      let time = stopwatch.Elapsed.TotalMilliseconds
      stopwatch.Stop()
      printfn $"Answer:\t{answer}\t({time}ms)"

      if answer <> "" then
        submit part.Day part.Part (answer, time)


    let runDay (day: Day) =
      // day.Prewarm |> Option.iter (runPrewarm day.Day)
      day.Parts |> List.iter runPart

  let runDay n =
    Assembly.GetCallingAssembly()
    |> Internals.getDays
    |> List.tryFind (fun d -> d.Day = n)
    |> Option.iter Internals.runDay

  let runDayPart n m =
    Assembly.GetCallingAssembly()
    |> Internals.getDays
    |> List.tryFind (fun d -> d.Day = n)
    |> Option.bind (fun day -> day.Parts |> List.tryFind (fun p -> p.Part = m))
    |> Option.iter Internals.runPart

  let runAll () =
    Assembly.GetCallingAssembly()
    |> Internals.getDays
    |> List.iter (fun day ->
      day |> Internals.runDay
      printfn "\n----------------------------------------------------------------")

  let runLatest () =
    Assembly.GetCallingAssembly()
    |> Internals.getDays
    |> List.last
    |> Internals.runDay
