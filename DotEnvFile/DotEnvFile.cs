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
        /// <exception cref="FileNotFoundException">
        /// Thrown when the filepath cannot be resolved.
        /// </exception>
        /// <exception cref="FormatException">
        /// Thrown when <see cref="throwOnInvalidValues"/> is false and a line is empty or does not contain valid separators.
        /// </exception>
        public static Dictionary<string, string> LoadFile(string path, bool throwOnInvalidValues = false)
        {
            if (File.Exists(path) == false)
            {
                throw new FileNotFoundException($"File not found at {path}");
            }

            var lines = File.ReadAllLines(path);
            var kvps = new Dictionary<string, string>();

            foreach (var line in lines)
            {
                try
                {
                    var kvp = ParseLine(line);

                    kvps.Add(kvp.Key, kvp.Value);
                }
                catch (FormatException)
                {
                    if (throwOnInvalidValues)
                    {
                        throw;
                    }
                }
            }

            return kvps;
        }

        /// <summary>
        /// Attempts to parse a line into a <see cref="KeyValuePair{string, string}"/>. Supports ':', '=' and spaces as the KVP delimiter.
        /// </summary>
        /// <example>DotEnvFile.ParseLine("MyKey=MyValue")</example>
        /// <param name="line">The line string to parse.</param>
        /// <exception cref="FormatException">
        /// Thrown when a line is empty or does not contain valid separators.
        /// </exception>
        public static KeyValuePair<string, string> ParseLine(string line)
        {
            var regex = new Regex("( *: *)|( *= *)|( +)", RegexOptions.IgnoreCase);

            if (!regex.IsMatch(line))
            {
                throw new FormatException($"Line does not contain valid separators. Valid separators are ':', '=' and spaces. Line value was {line}");
            }

            // Split the line into 2 parts: the key and the value.
            var parts = regex.Split(line, 2);
            
            return new KeyValuePair<string, string>(parts.First().Trim(), parts.Last().Trim());
        }

        /// <summary>
        /// Takes a <see cref="Dictionary{string, string}"/> of values and injects them into the environment, making them accessible via <see cref="Environment.GetEnvironmentVariable(string)"/>.
        /// </summary>
        public static void InjectIntoEnvironment(Dictionary<string, string> variables, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            foreach (var kvp in variables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, kvp.Value, target);
            }
        }

        /// <summary>
        /// Removes environment variables injected by <see cref="InjectIntoEnvironment(Dictionary{string, string}, EnvironmentVariableTarget)"/>.
        /// </summary>
        public static void RemoveFromEnvironment(Dictionary<string, string> variables, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
        {
            foreach (var kvp in variables)
            {
                Environment.SetEnvironmentVariable(kvp.Key, null, target);
            }
        }
    }
}
