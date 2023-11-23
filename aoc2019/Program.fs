module Program =
  open System.Reflection

  let (|Int|_|) str =
    match System.Int32.TryParse(str: string) with
    | (true, int) -> Some(int)
    | _ -> None

  [<EntryPoint>]
  let main args =
    printfn "Advent of Code 2019"
    printfn "----------------------------------------------------------------"

    match args[0] with
    | Int i -> AoC.Common.Runner.runDay i
    | "all"
    | "a" -> AoC.Common.Runner.runAll ()
    | "latest"
    | "l"
    | _ -> AoC.Common.Runner.runLatest ()

    printfn "\n----------------------------------------------------------------"
    0
