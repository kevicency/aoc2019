module Day01

open System
open Xunit
open AoC.Toolbelt.Prelude

let parseInts = Array.map (fun (x: string) -> int x)

let calcFuel x = x / 3 - 2

let p1 (input: String[]) =
  input |> parseInts |> Seq.map calcFuel |> Seq.sum

let p2 (input: String[]) =
  let rec calcFuelRec acc x =
    let f = calcFuel x
    if f <= 0 then acc else calcFuelRec (acc + f) f

  input |> parseInts |> Seq.map (calcFuelRec 0) |> Seq.sum

// #Tests

let p1ex () : obj[] list =
  [ [| @"
          12
          14
          1969
          100756
         "
       2 + 2 + 654 + 33583 |] ]

let p2ex () : obj[] list =
  [ [| @"
        14
        1969
        100756
        "
       2 + 966 + 50346 |] ]

[<Theory>]
[<MemberData(nameof p1ex)>]
let ``p1 examples`` (input: String) expected =
  if input <> "" then
    let result = p1 (splitlines input)
    Assert.Equal(expected, result)

[<Theory>]
[<MemberData(nameof p2ex)>]
let ``p2 examples`` (input: String) expected =
  if input <> "" then
    let result = p2 (splitlines input)
    Assert.Equal(expected, result)
