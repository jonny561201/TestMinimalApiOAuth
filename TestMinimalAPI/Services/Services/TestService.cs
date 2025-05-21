namespace TestMinimalAPI.Test.Services;

interface ITestService
{
    string GetResponseText();
}

public class TestService : ITestService
{
    public string GetResponseText()
    {
        return "Success Test";
    }
}