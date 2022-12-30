module Day00

type internal Day =
  interface
  end

open System
open Xunit
open AoC.Common.Input

let p1 (input: String[]) = ""
let p2 (input: String[]) = ""


// Tests

let p1ex () : obj[] list = [ [| @"" 0 |] ]
let p2ex () : obj[] list = [ [| @"" 0 |] ]

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
module Day00

type internal Day =
  interface
  end

open System
open Xunit
open AoC.Common.Input

let p1 (input: String[]) = ''
let p2 (input: String[]) = ''


// Tests

let p1ex () : obj[] list = [ [| @""; 0 |] ]
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
