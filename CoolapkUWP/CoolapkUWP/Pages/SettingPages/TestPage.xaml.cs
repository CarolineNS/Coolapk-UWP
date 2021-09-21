﻿using CoolapkUWP.Helpers;
using CoolapkUWP.Pages.FeedPages;
using CoolapkUWP.ViewModels.FeedListPage;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CoolapkUWP.Pages.SettingPages
{
    public sealed partial class TestPage : Page
    {
        private string Url = "/feed/";
        private int i;

        public TestPage()
        {
            InitializeComponent();
            //var loader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView("EmojiId");
            //System.Diagnostics.Debug.WriteLine(
            //loader.GetString("?")
            //    );
            comboBoxVersion.SelectedValue = ApplicationData.Current.LocalSettings.Values["Version"];
        }

        void IndexPage_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            //分享一个链接
            Uri shareLinkString = ValidateAndGetUri(url.Text);
            if (shareLinkString != null)
            {
                //创建一个数据包
                DataPackage dataPackage = new DataPackage();

                //把要分享的链接放到数据包里
                dataPackage.SetWebLink(shareLinkString);

                //数据包的标题（内容和标题必须提供）
                dataPackage.Properties.Title = "链接分享测试";
                //数据包的描述
                dataPackage.Properties.Description = url.Text;

                //给dataRequest对象赋值
                DataRequest request = args.Request;
                request.Data = dataPackage;
            }
            else
            {
                DataPackage dataPackage = new DataPackage();
                dataPackage.SetText(url.Text);
                dataPackage.Properties.Title = "内容分享测试";
                dataPackage.Properties.Description = "内含文本";
                DataRequest request = args.Request;
                request.Data = dataPackage;
            }
        }

        private Uri ValidateAndGetUri(string uriString)
        {
            Uri uri = null;
            try
            {
                uri = new Uri(uriString);
            }
            catch (FormatException)
            {
                UIHelper.ShowMessage(url.Text + "并不是一个链接");
            }
            return uri;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var f = FeedListPageViewModelBase.GetProvider(FeedListType.UserPageList, await Core.Helpers.NetworkHelper.GetUserIDByNameAsync(uid.Text));
            if (f != null) { UIHelper.NavigateInSplitPane(typeof(FeedListPage), f); }
        }

        private void IDComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Add "using Windows.UI;" for Color and Colors.
            string Type = e.AddedItems[0].ToString();
            switch (Type)
            {
                case "动态":
                    Url = "/feed/";
                    i = 0;
                    break;
                case "酷图":
                    Url = "/picture/";
                    i = 0;
                    break;
                case "问答":
                    Url = "/question/";
                    i = 0;
                    break;
                case "用户":
                    Url = "/u/";
                    i = 0;
                    break;
                case "话题":
                    Url = "/t/";
                    i = 0;
                    break;
                case "应用":
                    i = 1;
                    break;
                case "数码":
                    Url = "/product/";
                    i = 0;
                    break;
                case "页面":
                    Url = "/page?url=";
                    i = 2;
                    break;
                case "设备":
                    Url = "";
                    i = 3;
                    break;
                case "看看号":
                    Url = "/dyh/";
                    i = 0;
                    break;
                case "收藏集":
                    Url = "/collection/";
                    i = 0;
                    break;
                default:
                    Url = "/feed/";
                    i = 0;
                    break;
            }
        }

        // In a real app, these would be initialized with actual data
        private const string from = "动态磁贴测试";
        private const string subject = "这是一个通知";
        private const string body = "这个通知不会消失，除非你手动清除它";


        // Construct the tile content
        private readonly TileContent content = new TileContent()
        {
            Visual = new TileVisual()
            {
                TileMedium = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = from
                            },

                            new AdaptiveText()
                            {
                                Text = subject,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            },

                            new AdaptiveText()
                            {
                                Text = body,
                                HintStyle = AdaptiveTextStyle.CaptionSubtle
                            }
                        }
                    }
                },

                TileWide = new TileBinding()
                {
                    Content = new TileBindingContentAdaptive()
                    {
                        Children =
                {
                    new AdaptiveText()
                    {
                        Text = from,
                        HintStyle = AdaptiveTextStyle.Subtitle
                    },

                    new AdaptiveText()
                    {
                        Text = subject,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    },

                    new AdaptiveText()
                    {
                        Text = body,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
                    }
                }
            }
        };

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack) { Frame.GoBack(); }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            UIHelper.ShowMessage(message.Text);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(BrowserPage), new object[] { false, "http://www.all-tool.cn/Tools/ua/" });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(BrowserPage), new object[] { false, "https://m.coolapk.com/mp/do?c=userDevice&m=myDevice" });
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenLinkAsync(url.Text);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (i == 1)
            {
                var f = FeedListPageViewModelBase.GetProvider(FeedListType.AppPageList, ID.Text);
                if (f != null)
                {
                    UIHelper.NavigateInSplitPane(typeof(FeedListPage), f);
                }
            }
            else if (i == 2)
            {
                if (ID.Text.StartsWith("V", StringComparison.Ordinal)) { UIHelper.Navigate(typeof(IndexPage), new ViewModels.IndexPage.ViewModel(Url + ID.Text, true)); }
                else { UIHelper.Navigate(typeof(IndexPage), new ViewModels.IndexPage.ViewModel(ID.Text, true)); }
            }
            else if (i == 3)
            {
                var f = FeedListPageViewModelBase.GetProvider(FeedListType.DevicePageList, ID.Text);
                if (f != null)
                {
                    UIHelper.NavigateInSplitPane(typeof(FeedListPage), f);
                }
            }
            else { UIHelper.OpenLinkAsync(Url + ID.Text); }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            // Create the tile notification
            var notification = new TileNotification(content.GetXml());
            // Send the notification to the primary tile
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(BrowserPage), new object[] { false, url.Text });
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(AppPages.DevPage));
        }

        protected void ShowUIButton_Click(object sender, RoutedEventArgs e)
        {
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();

            dataTransferManager.DataRequested += IndexPage_DataRequested;

            DataTransferManager.ShowShareUI();
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (JumpList.IsSupported())
            {
                var list = await JumpList.LoadCurrentAsync();
                list.Items.Clear();//建议每次在添加之前清除掉原先已经存在的数据
                list.Items.Add(JumpListItem.CreateSeparator());

                new List<JumpListItem>()
                  {
                      //CreateJumpListItem("feed","动态","页面",new Uri("ms-appx:///Assets/facebook.png")),
                      //CreateJumpListItem("notification","通知","页面",new Uri("ms-appx:///Assets/github.png")),
                      CreateJumpListItem("test","打开测试页面","测试",new Uri("ms-appx:///Assets/Icons/ic_settings_white_24dp.png")),
                      //CreateJumpListItem("settings","设置","页面",new Uri("ms-appx:///Assets/Icons/ic_settings_white_24dp.png"))
                  }.ForEach((item) =>
                  {
                      list.Items.Add(item);
                  });
                await list.SaveAsync();
            }
        }

        private void MainPageV7_loaded(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(MainPageV7));
        }

        private void MainPageV11_loaded(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(MainPage));
        }

        private async void MainPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (JumpList.IsSupported())
            {
                var list = await JumpList.LoadCurrentAsync();
                list.Items.Clear();//建议每次在添加之前清除掉原先已经存在的数据
                await list.SaveAsync();
            }
        }

        private async void Restart(object sender, RoutedEventArgs e)
        {
            await CoreApplication.RequestRestartAsync(string.Empty);
        }

        private static JumpListItem CreateJumpListItem(string arguments, string displayName, string groupName, Uri uri)
        {
            JumpListItem item = JumpListItem.CreateWithArguments(arguments, displayName);
            item.GroupName = groupName;
            item.Logo = uri;
            return item;
        }

        private void comboBoxVersion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string temp = comboBoxVersion.SelectedItem.ToString();
            ApplicationData.Current.LocalSettings.Values["Version"] = temp;
        }

        private void TitleBar_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            UIHelper.NavigateInSplitPane(typeof(IndexPage), new ViewModels.IndexPage.ViewModel("/main/init", true));
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            UIHelper.SetBadgeNumber(message.Text);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UIHelper.StatusBar_ShowMessage(message.Text);
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            UIHelper.ShowProgressBar();
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            UIHelper.PausedProgressBar();
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            UIHelper.ErrorProgressBar();
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            ApplicationView.GetForCurrentView().TryEnterViewModeAsync(ApplicationViewMode.Default);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            UIHelper.Navigate(typeof(IndexPage), new ViewModels.IndexPage.ViewModel(url.Text, true));
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            UIHelper.NavigateInSplitPane(typeof(AdaptivePage), new ViewModels.AdaptivePage.ViewModel("536381", ViewModels.AdaptivePage.ListType.UserFeed, "htmlFeed"));
        }
    }
}