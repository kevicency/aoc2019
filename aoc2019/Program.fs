module Program =
  open System.Reflection

  [<EntryPoint>]
  let main _ =
    printfn "Advent of Code 2019"
    printfn "----------------------------------------------------------------"
    AoC.Common.Runner.runLatest ()
    printfn "\n----------------------------------------------------------------"
    0
