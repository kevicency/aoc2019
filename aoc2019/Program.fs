module Program =
  [<EntryPoint>]
  let main _ =
    printfn "Advent of Code 2019"
    printfn "----------------------------------------------------------------"
    AoC.Common.Runner.runLatest ()
    0
