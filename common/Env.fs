namespace AoC.Common

open System
open System.IO
open Microsoft.Extensions.Configuration

module Env =
  let private config = (new ConfigurationBuilder()).AddEnvironmentVariables().Build()

  let BaseDir = AppDomain.CurrentDomain.BaseDirectory
  let ProjectDir = Directory.GetParent(BaseDir).Parent.Parent.Parent.FullName
  let Year = AppDomain.CurrentDomain.FriendlyName.Replace("aoc", "") |> int
  let SessionToken = config["AOC_SESSION_TOKEN"]
