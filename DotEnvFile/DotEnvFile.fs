module DotEnvFile

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions

/// <summary>
/// Attempts to parse a line into a KeyValuePair. Supports ':', '=' and spaces as the KVP delimiter.
/// </summary>
/// <param name="line">The line string to parse.</param>
/// <exception cref="System.FormatException">
/// Thrown when a line is empty or does not contain valid separators.
/// </exception>
let ParseLine (line : string) : KeyValuePair<string, string> =
    let regex = Regex("( *: *)|( *= *)|( +)", RegexOptions.IgnoreCase)

    if not (regex.IsMatch line) then
        line
        |> sprintf "Line does not contain valid separators. Valid separators are ':', '=' and spaces. Line value was %s"
        |> FormatException
        |> raise

    // Split the line into 2 parts, the key and the value, and trim each
    let parts = regex.Split(line, 2)
    let trim (fn : string seq -> string) (parts : string seq) =
        let part = fn parts 
        part.Trim()
    
    KeyValuePair(trim Seq.head parts, trim Seq.last parts)


/// <summary>
/// Parses the given lines into a dictionary. Skips empty lines. 
/// </summary>
/// <param name="lines">The lines to parse.</param>
/// <exception cref="FormatException">
/// Thrown when <see cref="throwOnInvalidValues"/> is false and a line does not contain valid separators.
/// </exception>
let ParseLines (lines : string seq) (throwOnInvalidValues : bool) : IDictionary<string, string> =
    let rec readLines (output : Map<string, string>) (rest : string list) =
        match rest with
        | [] ->
            output
        | line :: rest when System.String.IsNullOrWhiteSpace line ->
            // The line is completely empty
            readLines output rest 
        | line :: rest ->
            let output = 
                try
                    let kvp = ParseLine line 
                    Map.add kvp.Key kvp.Value output
                with
                | :? FormatException ->
                    if throwOnInvalidValues then
                        reraise ()
                    else
                        output 
            readLines output rest
    
    List.ofSeq lines 
    |> readLines Map.empty
    |> Map.toSeq
    |> dict
    

/// <summary>
/// Attempts to load a file at the given <see cref="path"/> and parse it into a dictionary.
/// </summary>
/// <param name="path">The path to load the file from.</param>
/// <exception cref="FileNotFoundException">
/// Thrown when the filepath cannot be resolved.
/// </exception>
/// <exception cref="FormatException">
/// Thrown when <see cref="throwOnInvalidValues"/> is false and a line does not contain valid separators.
/// </exception>
let LoadFile (path: string) (throwOnInvalidValues : bool) : IDictionary<string, string> =
    if not (File.Exists path) then
        sprintf "File not found at %s" path
        |> FileNotFoundException
        |> raise
            
    ParseLines (File.ReadAllLines path) throwOnInvalidValues
    
    
/// <summary>
/// Takes a dictionary of values and injects them into the environment.
/// </summary>
let InjectIntoEnvironment (environment : EnvironmentVariableTarget) (variables : IDictionary<string, string>) : unit =
    for kvp in variables do
        Environment.SetEnvironmentVariable(kvp.Key, kvp.Value, environment)
    
    
/// <summary>
/// Takes a dictionary of values and removes them from the environment.
/// </summary>
let RemoveFromEnvironment (environment : EnvironmentVariableTarget) (variables : IDictionary<string, string>) : unit =
    for kvp in variables do
        Environment.SetEnvironmentVariable(kvp.Key, null, environment)
