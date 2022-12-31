module Day04

open System
open Xunit
open AoC.Common.Prelude

let parseMinMax (input: String[]) =
  let minMax = input |> Seq.head |> split "-" |> Array.map int
  (minMax[0], minMax[1])

let isPassword (part: int) (n: int) =
  let s = string n

  s = asString (s |> Seq.sort)
  && Seq.exists (fun c -> let c' = string c in s.Contains(c' + c') && (part = 1 || not (s.Contains(c' + c' + c')))) s

let solve (part: int) (input: String[]) =
  input
  |> parseMinMax
  |> fun (min, max) -> [ min..max ] |> Seq.filter (isPassword part) |> Seq.length

let p1 (input: string[]) = solve 1 input
let p2 (input: string[]) = solve 2 input


// Tests

let p1ex () : obj[] list = [ [| @"111111-111111"; 1 |] ]
let p2ex () : obj[] list = [ [| @""; 0 |] ]

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
