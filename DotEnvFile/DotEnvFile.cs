using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotEnvFile
{
    /// <summary>
    /// Methods for interacting with a .env file.
    /// </summary>
    public static class DotEnvFile
    {
        /// <summary>
        /// Attempts to load a file at the given <see cref="path"/> and parse it into a <see cref="Dictionary{string, string}"/> object. Will throw a <see cref="FileNotFoundException"/> exception if the file can't be found.
        /// </summary>
        /// <param name="path">The path to load the file from.</param>
        public static Dictionary<string, string> LoadFile(string path)
        {
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException($"DotEnvFile: File not found at {path}");
            }

            var lines = File.ReadAllLines(path);
            var kvps = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                var kvp = ParseLine(line);

                kvps.Add(kvp.Key, kvp.Value);
            }

            return kvps;
        }

        /// <summary>
        /// Attempts to parse a line into a <see cref="KeyValuePair{string, string}"/>. Supports ':', '=' and spaces as the KVP delimiter.
        /// </summary>
        /// <example>DotEnvFile.ParseLine("MyKey=MyValue")</example>
        /// <param name="line">The line string to parse.</param>
        public static KeyValuePair<string, string> ParseLine(string line)
        {
            var regex = new Regex("( *: *)|( *= *)|( +)", RegexOptions.IgnoreCase);

            if (!regex.IsMatch(line))
            {
                throw new FormatException("Line does not contain valid separators. Valid separators include ':', '=' and spaces.");
            }

            // Split the line into 2 parts: the key and the value.
            var parts = regex.Split(line, 2);
            
            return new KeyValuePair<string, string>(parts.First().Trim(), parts.Last().Trim());
        }

        public static void InjectIntoEnvironment(Dictionary<string, string> variables, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            foreach (var kvp in variables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value, target);
            }
        }

        public static void RemoveFromEnvironment(Dictionary<string, string> variables, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            foreach (var kvp in variables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, null, target);
            }
        }
    }
}
