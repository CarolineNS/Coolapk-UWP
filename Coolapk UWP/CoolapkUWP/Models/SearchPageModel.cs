﻿using Newtonsoft.Json.Linq;
using Windows.UI.Xaml.Controls;

namespace CoolapkUWP.Models
{
    internal class SearchWord
    {
        public Symbol Symbol { get; set; }
        public string Title { get; set; }

        public SearchWord(JObject keys)
        {
            if (keys.Value<string>("logo").Contains("cube", System.StringComparison.Ordinal))
            {
                Symbol = Symbol.Shop;
            }
            else if (keys.Value<string>("logo").Contains("xitongguanli", System.StringComparison.Ordinal))
            {
                Symbol = Symbol.Contact;
            }
            else
            {
                Symbol = Symbol.Find;
            }
            Title = keys.Value<string>("title");
        }

        public string GetTitle()
        {
            switch (Symbol)
            {
                case Symbol.Shop:
                case Symbol.Contact:
                    return Title.Substring(5);

                default:
                    return Title;
            }
        }
    }
}