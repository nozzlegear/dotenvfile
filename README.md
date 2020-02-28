# DotEnvFile

[![Build Status](https://travis-ci.org/nozzlegear/dotenvfile.svg?branch=master)](https://travis-ci.org/nozzlegear/dotenvfile)
[![NuGet](https://img.shields.io/nuget/v/dotenvfile.svg?maxAge=3600)](https://www.nuget.org/packages/dotenvfile/)
[![license](https://img.shields.io/github/license/nozzlegear/dotenvfile.svg?maxAge=3600)](https://github.com/nozzlegear/dotenvfile/blob/master/LICENSE)

DotEnvFile is a small .NET utility for parsing environment variables from `.env` files and optionally injecting them into the current environment. It supports .NET Core and .NET Standard. While this package is written in F#, it's completely usable in C# without knowing _anything_ about the F# language. 

## Installation

You can download DotEnvFile from Nuget. Use the dotnet CLI to install it:

```sh
dotnet add package dotenvfile
```

Or via Nuget package manager in Visual Studio:

```bash
Install-Package DotEnvFile
```

Or via [Paket](https://github.com/fsprojects/paket) for F# projects:

```bash
paket add nuget DotEnvFile
```

## File formatting

DotEnvFile expects your file to contain one variable per line, with each variable formatted in the scheme `Key:Value`, `Key=Value`, or `Key Value`. It will try to be forgiving by ignoring extra whitespace and empty lines.

Example `.env` file:

```
FirstKey: FirstValue
SecondKey= SecondValue
ThirdKey ThirdValue
FourthKey : FourthValue
``` 

## Usage

### Load environment variables from your file:

This function accepts a boolean that tells the tool whether it should throw an exception when it encounters a value it can't parse, or if it should skip the line and continue silently. 

```cs
string pathToFile = "/path/to/file.env";
bool throwOnFormatException = true;
IDictionary<string, string> variables = DotEnvFile.LoadFile(pathToFile, throwOnFormatException);
```

### Parse a single line:

```cs
KeyValuePair<string, string> variable = DotEnvFile.ParseLine("MyKey=MyValue");
```

### Inject the variables into your environment:

```cs
DotEnvFile.InjectIntoEnvironment(System.EnvironmentVariableTarget.Process, variables);

Console.WriteLine(Environment.GetEnvironmentVariable("MyKey")); // "MyValue"
```

### Remove the variables from your environment:

```cs
DotEnvFile.RemoveFromEnvironment(System.EnvironmentVariableTarget.Process, variables);

Console.WriteLine(Environment.GetEnvironmentVariable("MyKey") == null); // True
```
