using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEnvFile.Tests
{
    static class Utils
    {
        public static IEnumerable<KeyValuePair<string, string>> ExpectedVariables = new[]
        {
            new KeyValuePair<string, string>("FirstKey", "FirstValue"),
            new KeyValuePair<string, string>("SecondKey", "SecondValue"),
            new KeyValuePair<string, string>("ThirdKey", "ThirdValue"),
            new KeyValuePair<string, string>("FourthKey", "FourthValue"),
            new KeyValuePair<string, string>("FifthKey", "FifthValue"),
            new KeyValuePair<string, string>("SixthKey", "SixthValue")
        };

        public static string PathToTestFile = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "./Test.env";
    }
}
