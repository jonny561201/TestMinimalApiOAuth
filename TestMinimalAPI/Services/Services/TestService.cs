namespace TestMinimalAPI.Services.Services;

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