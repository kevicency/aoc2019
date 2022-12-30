module Day02

type internal Day =
  interface
  end

open System
open Xunit
open AoC.Common.Input

let split (sep: string) (str: string) =
  str.Split([| sep |], StringSplitOptions.RemoveEmptyEntries)

let parseRegister (input: string[]) =
  input |> Seq.collect (split ",") |> Seq.map int |> Seq.toArray

let rec intcode (i: int) (register: int array) =
  let op = register[i]

  match op with
  | 99 -> register[0]
  | 1
  | 2 ->
    let a = register[i + 1]
    let b = register[i + 2]
    let out = register[i + 3]

    register[out] <-
      if op = 1 then
        register[a] + register[b]
      else
        register[a] * register[b]

    intcode (i + 4) register
  | _ -> failwith "invalid opcode"

let init noun verb (register: int array) =
  if (register.Length > 12) then
    register[1] <- noun
    register[2] <- verb

  register

let p1 (input: String[]) =
  input |> parseRegister |> init 12 2 |> intcode 0

let p2 (input: String[]) =
  let register = input |> parseRegister

  let rec bruteforce noun verb =
    register
    |> Array.copy
    |> init noun verb
    |> intcode 0
    |> fun result ->
         if result = 19690720 then 100 * noun + verb
         else if noun = 99 then bruteforce 0 (verb + 1)
         else bruteforce (noun + 1) verb

  bruteforce 0 0


// Tests

let p1ex () : obj[] list =
  [ [| @"1,9,10,3,2,3,11,0,99,30,40,50"; 3500 |] ]

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
