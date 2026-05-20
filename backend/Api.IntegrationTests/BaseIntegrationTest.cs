namespace Api.IntegrationTests;

using static Testing;

public class BaseIntegrationTest
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}
