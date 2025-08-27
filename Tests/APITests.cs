using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using NUnitTestProject1.Framework.Helpers;
using NUnitTestProject1.Pages;


namespace NUnitTestProject1
{

    [TestFixture]
    public class APITests
    {
        [Test]
        public async Task Members_me_boards_should_have_first_item_with_string_id_and_status_200()
        {

            // GET {{baseUrl}}/1/members/me/boards?key={{trelloKey}}&token={{trelloToken}}&fields=id,name,url
            var url = ApiHelper.Q("members/me/boards", new Dictionary<string, string?>
            {
                ["fields"] = "id,name,url"
            });

            var fullUrl = ApiHelper.Client.BaseAddress + url;

            TestContext.Progress.WriteLine("Calling: " + fullUrl);
            TestContext.Progress.WriteLine($"Key (first 8): {ApiHelper.Key?.Substring(0, 8)}...");
            TestContext.Progress.WriteLine($"Token (first 8): {ApiHelper.Token?.Substring(0, 8)}...");

            var resp = await ApiHelper.Client.GetAsync(url);

            // Status 200
            resp.StatusCode.Should().Be(HttpStatusCode.OK);

            // JSON body checks
            var boards = await resp.Content.ReadFromJsonAsync<List<TrelloBoard>>();
            boards.Should().NotBeNull();
            boards!.Should().NotBeEmpty();

            var first = boards[0];
            first.id.Should().NotBeNullOrWhiteSpace();

            TestContext.WriteLine($"boardId={first.id}");
        }
    }
}