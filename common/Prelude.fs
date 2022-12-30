namespace AoC.Common

open System

module Prelude =

  let split (sep: string) (str: string) =
    str.Split([| sep |], StringSplitOptions.RemoveEmptyEntries)

  let splitlines (input: String) =
    input.Split("\n")
    |> Array.map (fun (x: String) -> x.Trim())
    |> Array.filter (fun (x: String) -> x <> "")
