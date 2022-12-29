namespace AoC.Common

open System
open System.IO

module Input =
  let splitlines (input: String) =
    input.Split("\n")
    |> Array.map (fun (x: String) -> x.Trim())
    |> Array.filter (fun (x: String) -> x <> "")

  let get (moduleMarker: Type) =
    let moduleName = moduleMarker.DeclaringType.Name
    File.ReadAllLines $"./input/{moduleName}.txt"
