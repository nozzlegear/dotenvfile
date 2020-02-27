using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEnvFile.Tests
{
    [Subject(typeof(DotEnvFile))]
    public class When_removing_environment_variables
    {
        Establish context = () =>
        {

        };

        Because of = () =>
        {
            var variables = DotEnvFile.LoadFile(Utils.PathToTestFile);
            DotEnvFile.InjectIntoEnvironment(variables);
            DotEnvFile.RemoveFromEnvironment(variables);
        };

        It should_remove_variables = () =>
        {
            Utils.ExpectedVariables.ShouldEachConformTo(kvp => Environment.GetEnvironmentVariable(kvp.Key) == null);
        };

        Cleanup after = () =>
        {

        };
    }
}
