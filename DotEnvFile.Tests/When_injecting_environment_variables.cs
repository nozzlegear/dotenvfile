using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEnvFile.Tests
{
    [Subject(typeof(DotEnvFile))]
    public class When_injecting_environment_variables
    {
        Establish context = () =>
        {

        };

        Because of = () =>
        {
            Variables = DotEnvFile.LoadFile(Utils.PathToTestFile);

            DotEnvFile.InjectIntoEnvironment(Variables);
        };

        It should_inject_variables = () =>
        {
            Utils.ExpectedVariables.ShouldEachConformTo(kvp => Environment.GetEnvironmentVariable(kvp.Key) == kvp.Value);
        };

        Cleanup after = () =>
        {
            DotEnvFile.RemoveFromEnvironment(Variables);
        };

        static Dictionary<string, string> Variables;
    }
}
