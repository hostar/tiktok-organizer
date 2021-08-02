using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using tiktok_organizer.ViewModels;

namespace tiktok_organizer.Views
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel ctx;
        private static readonly HttpClient httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public void AfterInit()
        {
            ctx = (MainWindowViewModel)DataContext;
        }

        public string GetTemporaryDirectory()
        {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        private async void Run(object sender, RoutedEventArgs e)
        {
            // create new instance of chrome browser
            var tmpDir = GetTemporaryDirectory();
            Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", $@"--remote-debugging-port=9222 --user-data-dir=""{tmpDir}""");

            using var document = JsonDocument.Parse(await httpClient.GetStringAsync("http://127.0.0.1:9222/json/version"));
            var webSocketDebuggerUrl = document.RootElement.EnumerateObject().FirstOrDefault(p => p.Name == "webSocketDebuggerUrl").Value.GetString();

            await using var browser = await Puppeteer.ConnectAsync(new ConnectOptions()
            {
                BrowserWSEndpoint = webSocketDebuggerUrl
            });
            using var page = await browser.NewPageAsync();

            await page.GoToAsync($"https://www.tiktok.com/@{ctx.ProfileName}");
            var sel = await page.WaitForSelectorAsync(".like");
            await sel.ClickAsync();

            await page.WaitForSelectorAsync(".video-feed-item-wrapper");
            var allVideosElement = await page.QuerySelectorAllAsync(".video-feed-item-wrapper");

            int i = 0;
            foreach (var element in allVideosElement)
            {
                var href = await element.GetPropertyAsync("href");
                var linkVideo = await href.JsonValueAsync();

                var thumb = await element.QuerySelectorAllAsync(".image-card");
                if (thumb.Length == 0)
                { // scroll down
                    await page.Mouse.WheelAsync(0, 500);

                    /*
                    //await page.EvaluateExpressionAsync("window.scrollBy(0, 100)");
                    */

                    while (thumb.Length == 0)
                    {
                        thumb = await element.QuerySelectorAllAsync(".image-card");
                        Thread.Sleep(500);
                    }
                }

                System.Drawing.Image thumbnail = null;
                if (thumb.Length > 0)
                {
                    thumbnail = System.Drawing.Image.FromStream(await thumb[0].ScreenshotStreamAsync());
                    //thumbnail.Save(tmpDir + $"\\img-{i}.jpg");
                    i++;
                    /*
                    var style = await page.EvaluateFunctionAsync<Dictionary<string, string>>(
                        "e => Object.entries(e.style).filter(i => isNaN(i[0]) && i[1]).map(i => { return { [i[0]] : i[1]}}).reduce((acc, cur) => { return {...acc, ...cur}}, {})", thumb[0]);
                    

                    try
                    {
                        linkThumb = style["backgroundImage"].Substring(5, style["backgroundImage"].LastIndexOf('"') - 5);
                    }
                    catch
                    {

                    }
                    */
                }                

                ctx.VideoList.Add(new Models.Video { VideoLink = linkVideo.ToString(), VideoThumb = thumbnail });
            }
            
            int a = 0;
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
