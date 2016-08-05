# DotEnvFile

DotEnvFile is a small .NET utility for parsing environment variables from `.env` files and optionally injecting them into the current environment.

### Installation

You can download DotEnvFile from NuGet with NuGet package manager:

```bash
Install-Package DotEnvFile
```

Or via [Paket](https://github.com/fsprojects/paket):

```bash
paket add nuget DotEnvFile
```

### File formatting

DotEnvFile expects your file to contain one variable per line, with each variable formatted in the scheme `Key:Value`, `Key=Value`, or `Key Value`. It will try to be forgiving by ignoring extra whitespace and empty lines.

Example `.env` file:

```
FirstKey: FirstValue
SecondKey= SecondValue
ThirdKey ThirdValue
FourthKey : FourthValue
``` 

### Usage

Load environment variables from your file:

```cs
string pathToFile = "/path/to/file.env";
Dictionary<string, string> variables = DotEnvFile.LoadFile(pathToFile);
```

Parse a single line:

```cs
KeyValuePair<string, string> variable = DotEnvFile.ParseLine("MyKey=MyValue");
```

Inject the variables into your environment:

```cs
DotEnvFile.InjectIntoEnvironment(variables);

Console.WriteLine(Environment.GetEnvironmentVariable("MyKey")); // "MyValue"
```

Remove the variables from your environment:

```cs
DotEnvFile.RemoveFromEnvironment(variables);

Console.WriteLine(Environment.GetEnvironmentVariable("MyKey") == null); // True
```