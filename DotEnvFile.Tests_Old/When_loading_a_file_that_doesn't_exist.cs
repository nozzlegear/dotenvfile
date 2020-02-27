using System;
using Machine.Specifications;
using System.IO;

namespace DotEnvFile.Tests
{
    [Subject(typeof(DotEnvFile))]
    public class When_loading_a_file_that_doesnt_exist
    {
        Establish context = () =>
        {

        };

        Because of = () =>
        {
            try
            { 
                DotEnvFile.LoadFile("invalid/path.env");
            }
            catch (FileNotFoundException e)
            {
                Ex = e;
            }
        };

        It should_throw_an_exception = () =>
        {
            Ex.ShouldNotBeNull();
        };

        Cleanup after = () =>
        {

        };

        static FileNotFoundException Ex;
    }
}
