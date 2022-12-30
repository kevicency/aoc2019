namespace AoC.Common

open System

module Prelude =

  let split (sep: string) (str: string) =
    str.Split([| sep |], StringSplitOptions.RemoveEmptyEntries)

  let splitAt (index: int) (str: string) =
    str.Substring(0, index), str.Substring(index)

  let splitlines (input: String) =
    input.Split("\n")
    |> Array.map (fun (x: String) -> x.Trim())
    |> Array.filter (fun (x: String) -> x <> "")

  let manhattan (x1, y1) (x2, y2) = abs (x1 - x2) + abs (y1 - y2)

  type Direction =
    | N
    | E
    | S
    | W

  type Point = int * int

  let parseDirection (dir: string) =
    match dir.ToUpper() with
    | "N"
    | "U" -> N
    | "E"
    | "R" -> E
    | "S"
    | "D" -> S
    | "W"
    | "L" -> W
    | _ -> failwith "invalid direction"

  let dxy (dir: Direction) =
    match dir with
    | N -> (0, 1)
    | E -> (1, 0)
    | S -> (0, -1)
    | W -> (-1, 0)
