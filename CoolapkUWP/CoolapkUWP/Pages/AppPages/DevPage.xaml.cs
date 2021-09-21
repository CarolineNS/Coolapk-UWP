﻿using CoolapkUWP.Core.Helpers;
using CoolapkUWP.Helpers;
using CoolapkUWP.Models;
using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace CoolapkUWP.Pages.AppPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DevPage : Page
    {
        private FeedDetailModel FeedDetail { get; set; }
        private Models.Pages.FeedListPageModels.UserDetail UserDetail { get; set; }

        public DevPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region 测试
            Uri uri = new Uri("https://qapi.ithome.com/api/content/getcontentdetail?id=5209");
            (bool _, string result) = await DataHelper.GetHtmlAsync(uri, "XMLHttpRequest");
            Models.Links.SourceFeedModel _ = new Models.Links.SourceFeedModel(JObject.Parse(result), Models.Links.LinkType.ITHome);
            //if (isSucceed)
            //{
            //    //main.Text = result;
            //    //var o = JObject.Parse(result);
            //    //webview.NavigateToString(o.TryGetValue("html", out JToken token) ? token.ToString() : "错误");
            //    //text.MessageText = o.TryGetValue("html", out JToken token) ? token.ToString() : "错误";
            //    //MarkdownText.Text = CSStoMarkDown(o.TryGetValue("html", out JToken token) ? token.ToString() : "错误");
            //    //title.Title = o.TryGetValue("title", out JToken Title) ? Title.ToString() : title.Title;
            //}
            //webview.NavigationCompleted += OnNavigationCompleted;
            //webview.NavigateToString("");
            //FeedDetail = await GetFeedDetailAsync("26933449");
            //content = new TileContent()
            //{
            //    Visual = new TileVisual()
            //    {
            //        Branding = TileBranding.NameAndLogo,
            //        DisplayName = FeedDetail.Info,
            //        TileMedium = new TileBinding()
            //        {
            //            Content = new TileBindingContentAdaptive()
            //            {
            //                Children =
            //                {
            //                    new AdaptiveText()
            //                    {
            //                        Text = FeedDetail.Username,
            //                        HintStyle = AdaptiveTextStyle.Base,
            //                    },

            //                    new AdaptiveText()
            //                    {
            //                        Text = FeedDetail.Message,
            //                        HintStyle = AdaptiveTextStyle.CaptionSubtle,
            //                        HintWrap = true
            //                    }
            //                }
            //            }
            //        },
            //    }
            //};
            #endregion
            //var notification = new TileNotification(content.GetXml());
            //TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        protected static async Task<FeedDetailModel> GetFeedDetailAsync(string id)
        {
            var (isSucceed, result) = await DataHelper.GetDataAsync(UriHelper.GetUri(UriType.GetFeedDetail, id), true);
            if (!isSucceed) { return null; }

            var detail = (JObject)result;
            return detail != null ? new FeedDetailModel(detail) : null;
        }

        private async void OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs e)
        {
            var userId = await webview.InvokeScriptAsync("eval", new[] { "return navigator.userAgent;" });
            main.Text = userId;
        }

        private static string CSStoMarkDown(string text)
        {
            Regex h1 = new Regex("<h1 style.*?>", RegexOptions.IgnoreCase);
            Regex h2 = new Regex("<h2 style.*?>", RegexOptions.IgnoreCase);
            Regex h3 = new Regex("<h3 style.*?>", RegexOptions.IgnoreCase);
            Regex div = new Regex("<div style.*?>", RegexOptions.IgnoreCase);
            Regex p = new Regex("<p style.*?>", RegexOptions.IgnoreCase);

            text = text.Replace("</h1>", "");
            text = text.Replace("</h2>", "");
            text = text.Replace("</h3>", "");
            text = text.Replace("</div>", "");
            text = text.Replace("</p>", "");

            text = h1.Replace(text, "#");
            text = h2.Replace(text, "##");
            text = h3.Replace(text, "###");
            text = text.Replace("<br>", "  \n");
            text = div.Replace(text, "");
            text = p.Replace(text, "");

            for (int i = 0; i < 20; i++) { text = text.Replace("(" + i.ToString() + ") ", " 1. "); }

            return text;
        }

        #region Task：任务

        private void GetDevMyList(string str)
        {
            //main.Text = str;
        }


        #endregion

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) { Frame.GoBack(); }
        }

        private void TitleBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private TileContent content;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri(Uri.Text);
            var (isSucceed, result) = await DataHelper.GetHtmlAsync(uri, "XMLHttpRequest");
        }
    }
}
