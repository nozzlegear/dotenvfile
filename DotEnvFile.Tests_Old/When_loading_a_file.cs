using System;
using Machine.Specifications;
using System.Collections.Generic;

namespace DotEnvFile.Tests
{
    [Subject(typeof(DotEnvFile))]
    public class When_loading_a_file
    {
        Establish context = () =>
        {

        };

        Because of = () =>
        {
            Variables = DotEnvFile.LoadFile(Utils.PathToTestFile);

            try
            {
                DotEnvFile.LoadFile(Utils.PathToTestFile, true);
            }
            catch (FormatException e)
            {
                Ex = e;
            }
        };

        It should_load_a_dotenv_file = () =>
        {
            Ex.ShouldNotBeNull();
            Variables.ShouldNotBeNull();
            Variables.ShouldContain(Utils.ExpectedVariables);
        };

        Cleanup after = () =>
        {

        };

        static Dictionary<string, string> Variables;

        static FormatException Ex;
    }
}
