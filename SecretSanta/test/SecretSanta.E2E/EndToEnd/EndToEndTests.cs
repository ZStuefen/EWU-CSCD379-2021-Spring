using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlaywrightSharp;
using System.Linq;

namespace SecretSanta.Web.Tests
{
    [TestClass]
    public class EndToEndTests
    {
        private static WebHostServerFixture<Web.Startup, SecretSanta.Api.Startup> Server;

        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            Server = new();
        }

        [TestMethod]
        public async Task LaunchHomepage()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);
            //var response = await page.GoToAsync("https://localhost:5001");

            Assert.IsTrue(response.Ok);

            var headerContent = await page.GetTextContentAsync("body > header > div > a");
            Assert.AreEqual("Secret Santa", headerContent);
        }

        [TestMethod]
        public async Task GoToUsers()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Users");

            Assert.IsTrue(response.Ok);
        }

        [TestMethod]
        public async Task GoToGroups()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Groups");

            Assert.IsTrue(response.Ok);
        }

        [TestMethod]
        public async Task GoToGifts()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Gifts");

            Assert.IsTrue(response.Ok);
        }

        [TestMethod]
        public async Task CreateGift()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Gifts");

            await page.WaitForSelectorAsync("body > section > section > section");
            var gifts = await page.QuerySelectorAllAsync("body > section > section > section");
            int giftNum = gifts.Count();

            await page.ClickAsync("text=Create");

            await page.TypeAsync("input#Title", "Gift");
            await page.TypeAsync("input#Url", "google.com");
            await page.TypeAsync("input#Description", "A new gift");
            await page.TypeAsync("input#Priority", "1");
            await page.TypeAsync("input#UserId", "1");

            await page.ClickAsync("text=Create");

            await page.WaitForSelectorAsync("body > section > section > section");
            gifts = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(giftNum + 1, gifts.Count());
        }

        [TestMethod]
        public async Task ModifyLastGift()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Gifts");

            await page.WaitForSelectorAsync("body > section > section > section");
            await page.ClickAsync("body > section > section > section:last-child > a");

            await page.TypeAsync("input#Title", "UpdatedGift");

            await page.ClickAsync("text=Update");

            await page.WaitForSelectorAsync("body > section > section > section");
            var sectionText = await page.GetTextContentAsync("body > section > section > section:last-child > a > section > div");

            Assert.AreEqual("UpdatedGift", sectionText);
        }

        [TestMethod]
        public async Task DeleteLastGift()
        {
            var localhost = Server.WebRootUri.AbsoluteUri.Replace("127.0.0.1", "localhost");
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            var response = await page.GoToAsync(localhost);

            Assert.IsTrue(response.Ok);

            await page.ClickAsync("text=Gifts");

            await page.WaitForSelectorAsync("body > section > section > section");
            var gifts = await page.QuerySelectorAllAsync("body > section > section > section");
            int giftNum = gifts.Count();

            page.Dialog += (_, args) => args.Dialog.AcceptAsync();

            await page.ClickAsync("body > section > section > section:last-child > a > section > form > button");

            await page.WaitForSelectorAsync("body > section > section > section");
            gifts = await page.QuerySelectorAllAsync("body > section > section > section");
            Assert.AreEqual(giftNum - 1, gifts.Count());
        }
    }
}