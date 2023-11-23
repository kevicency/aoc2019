module Day03

type internal Day =
  interface
  end

open System
open Xunit
open AoC.Toolbelt.Prelude

let parseLines =
  split ","
  >> Seq.map (splitAt 1)
  >> Seq.map (fun (dir, len) -> parseDirection dir, int len)

let follow (x, y) (dir: Direction) (len: int) =
  let (dx, dy) = dxy dir

  seq {
    for i in 1..len do
      yield (x + dx * i, y + dy * i, i)
  }

let createWire (lines: (Direction * int) seq) =
  let rec createWire' (lines: (Direction * int) seq) (x, y, l) (acc: Map<Point, int>) =
    match Seq.tryHead lines with
    | Some(dir, len) ->
      let points =
        follow (x, y) dir len
        |> Seq.map (fun (px, py, pl) -> (px, py, pl + l))
        |> Seq.toArray

      let acc = points |> Seq.fold (fun acc (px, py, pl) -> Map.add (px, py) pl acc) acc

      let next = points |> Array.last

      createWire' (lines |> Seq.skip 1) next acc
    | None -> acc

  createWire' lines (0, 0, 0) Map.empty

let p1 (input: String[]) =
  input
  |> Seq.map parseLines
  |> Seq.map createWire
  |> Seq.map (Map.keys >> Set.ofSeq)
  |> Set.intersectMany
  |> Seq.map (manhattan (0, 0))
  |> Seq.min

let p2 (input: String[]) =
  let wires = input |> Seq.map parseLines |> Seq.map createWire
  let intersections = wires |> Seq.map (Map.keys >> Set.ofSeq) |> Set.intersectMany

  intersections
  |> Seq.map (fun p -> wires |> Seq.map (Map.find p) |> Seq.sum)
  |> Seq.min

// Tests

let p1ex () : obj[] list =
  [ [| @"
        R8,U5,L5,D3
        U7,R6,D4,L4
       "
       6 |]
    [| @"
        R75,D30,R83,U83,L12,D49,R71,U7,L72
        U62,R66,U55,R34,D71,R55,D58,R83
       "
       159 |] ]

let p2ex () : obj[] list =
  [ [| @"
        R8,U5,L5,D3
        U7,R6,D4,L4
       "
       30 |]
    [| @"
        R75,D30,R83,U83,L12,D49,R71,U7,L72
        U62,R66,U55,R34,D71,R55,D58,R83
       "
       610 |] ]

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
