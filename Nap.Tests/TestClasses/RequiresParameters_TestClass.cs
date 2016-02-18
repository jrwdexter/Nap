namespace Nap.Tests.TestClasses
{
    public class RequiresParameters_TestClass : TestClass
    {
        public RequiresParameters_TestClass(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
