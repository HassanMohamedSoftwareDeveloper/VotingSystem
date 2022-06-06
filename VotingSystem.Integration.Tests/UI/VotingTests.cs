using System.Net;
using System.Net.Http.Headers;
using VotingSystem.Integration.Tests.Fixtures;
using VotingSystem.Integration.Tests.Infrastructure;
using BaseVotingTests = VotingSystem.Integration.Tests.VotingTests;

namespace VotingSystem.Integration.Tests.UI;

public class VotingTests : IClassFixture<VotingFixture>
{
    private readonly VotingFixture _factory;

    public VotingTests(VotingFixture factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task OnGet()
    {
        var client = _factory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
        
        var pollPage = await client.GetAsync("/Poll/1");
        var pollHtml = await pollPage.Content.ReadAsStringAsync();


        var cookieToken = AntiForgeryUtils.ExtractCookieToken(pollPage.Headers);
        var formToken = AntiForgeryUtils.ExtractFormToken(pollHtml, "test_csrf_field");

        var request = new HttpRequestMessage(HttpMethod.Post, "/Poll/1");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string,string>("counterId","1"),
            new KeyValuePair<string,string>("test_csrf_field",formToken)
        });

        request.Headers.Add("Cookie", $"test_csrf_cookie={cookieToken}");

        var response = await client.SendAsync(request);

        var content = await response.Content.ReadAsStringAsync();

        Equal(HttpStatusCode.OK, response.StatusCode);

        DbContextUtils.ActionDatabase(_factory.Services,ctx =>
        {
            BaseVotingTests.AssertVotedForCounter(ctx, "test@test.com", 1);
        });
    }
}
