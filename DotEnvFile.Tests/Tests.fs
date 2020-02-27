module Tests

open System
open System.Collections.Generic
open System.IO
open Xunit

let expectedVariables =
    [ KeyValuePair("FirstKey", "FirstValue")
      KeyValuePair("SecondKey", "SecondValue")
      KeyValuePair("ThirdKey", "ThirdValue")
      KeyValuePair("FourthKey", "FourthValue")
      KeyValuePair("FifthKey", "FifthValue")
      KeyValuePair("SixthKey", "SixthValue") ]

let pathToTestFile = "./Test.env"

[<Fact>]
let ``Should load a dotenv file`` () =
    let variables = DotEnvFile.LoadFile pathToTestFile true
    
    for variable in variables do
        Assert.Contains(expectedVariables, (=) variable)
    
[<Fact>]
let ``Should throw an exception when loading a file that does not exist`` () =
    Assert.Throws<FileNotFoundException>(fun _ ->
        DotEnvFile.LoadFile "fake.env" true
        |> ignore)
    |> ignore 

[<Fact>]
let ``Should parse a line into a KeyValuePair`` () =
    let kvp key value =
        KeyValuePair(key, value)
    let lines =
        [ "FirstKey: FirstValue", kvp "FirstKey" "FirstValue"
          "SecondKey= SecondValue", kvp "SecondKey" "SecondValue"
          "ThirdKey ThirdValue", kvp "ThirdKey" "ThirdValue"
          "FourthKey : FourthValue", kvp "FourthKey" "FourthValue"
          "FifthKey   =   FifthValue", kvp "FifthKey" "FifthValue"
          "SixthKey        SixthValue", kvp "SixthKey" "SixthValue" ]
        
    for (line, expectedValue) in lines do
        let parsed = DotEnvFile.ParseLine line
        Assert.Equal(expectedValue, parsed)
        
[<Fact>]
let ``Should parse multiple lines into a dictionary`` () =
    let lines =
        [ "FirstKey: FirstValue"
          "SecondKey= SecondValue"
          "ThirdKey ThirdValue"
          String.Empty
          "   "
          ""
          "FourthKey : FourthValue"
          "FifthKey   =   FifthValue"
          "SixthKey        SixthValue" ]
    let parsed = DotEnvFile.ParseLines lines true
    
    Assert.Equal(6, parsed.Count)
    
[<Fact>]
let ``Should return an empty dictionary if lines are empty`` () =
    let lines = []
    let parsed = DotEnvFile.ParseLines lines true
    
    Assert.Equal(0, parsed.Count)
    
    let lines =
        [ String.Empty
          ""
          "    " ]
    let parsed = DotEnvFile.ParseLines lines true
    
    Assert.Equal(0, parsed.Count)
    
[<Fact>]
let ``Should throw a FormatException when parsing a bad line`` () =
    Assert.Throws<FormatException>(fun _ ->
        DotEnvFile.ParseLine "foo"
        |> ignore)
    |> ignore
    
    Assert.Throws<FormatException>(fun _ ->
        DotEnvFile.ParseLine String.Empty
        |> ignore)
    |> ignore
    
[<Fact>]
let ``Should inject variables into the environment`` () =
    let variables = DotEnvFile.LoadFile pathToTestFile true
    
    DotEnvFile.InjectIntoEnvironment EnvironmentVariableTarget.Process variables
    
    for variable in variables do
        let existing = Environment.GetEnvironmentVariable variable.Key
        Assert.NotNull existing 
        Assert.Equal(variable.Value, existing)
    
[<Fact>]
let ``Should remove variables from the environment`` () =
    let variables = DotEnvFile.LoadFile pathToTestFile true
    
    DotEnvFile.InjectIntoEnvironment EnvironmentVariableTarget.Process variables
    
    for variable in variables do
        let existing = Environment.GetEnvironmentVariable variable.Key
        Assert.NotNull existing 
        Assert.Equal(variable.Value, existing)
        
    DotEnvFile.RemoveFromEnvironment EnvironmentVariableTarget.Process variables
    
    for variable in variables do
        let existing = Environment.GetEnvironmentVariable variable.Key
        Assert.Null existing 
