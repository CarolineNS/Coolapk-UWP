﻿using System;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace CoolapkUWP.Core.Helpers
{
    public class TokenHelper
    {
        private static readonly string Guid = System.Guid.NewGuid().ToString();
        private static readonly string aid = RandHexString(16);
        private static readonly string mac = RandMacAdress();
        private static string SystemManufacturer;
        private static string SystemProductName;

        public static string DeviceCode;

        private TokenVersion TokenVersion;

        static TokenHelper()
        {
            EasClientDeviceInformation deviceInfo = new EasClientDeviceInformation();
            SystemManufacturer = deviceInfo.SystemManufacturer;
            SystemProductName = deviceInfo.SystemProductName;
            DeviceCode = CreateDeviceCode(aid, mac, SystemManufacturer, SystemManufacturer, SystemProductName, $"CoolapkUWP {Package.Current.Id.Version.ToFormattedString(3)}");
        }

        public TokenHelper(TokenVersion version = TokenVersion.TokenV2)
        {
            TokenVersion = version;
        }

        /// <summary>
        /// GetToken Generate a token with random device info
        /// </summary>
        public string GetToken()
        {
            switch (TokenVersion)
            {
                case TokenVersion.TokenV1:
                    return GetCoolapkAppToken();
                default:
                case TokenVersion.TokenV2:
                    return GetTokenWithDeviceCode(DeviceCode);
            }
        }

        /// <summary>
        /// GetTokenWithDeviceCode Generate a token with your device code
        /// </summary>
        private string GetTokenWithDeviceCode(string deviceCode)
        {
            string timeStamp = DateTime.Now.ConvertDateTimeToUnixTimeStamp().ToString();

            string base64TimeStamp = timeStamp.GetBase64(true);
            string md5TimeStamp = timeStamp.GetMD5();
            string md5DeviceCode = deviceCode.GetMD5();

            string token = $"token://com.coolapk.market/dcf01e569c1e3db93a3d0fcf191a622c?{md5TimeStamp}${md5DeviceCode}&com.coolapk.market";
            string base64Token = token.GetBase64(true);
            string md5Base64Token = base64Token.GetMD5();
            string md5Token = token.GetMD5();

            string bcryptSalt = $"{$"$2y$10${base64TimeStamp}/{md5Token}".Substring(0, 31)}u";
            string bcryptresult = BCrypt.Net.BCrypt.HashPassword(md5Base64Token, bcryptSalt);

            string appToken = $"v2{bcryptresult.GetBase64(true)}";

            return appToken;
        }

        private static string GetCoolapkAppToken()
        {
            double timeStamp = DateTime.Now.ConvertDateTimeToUnixTimeStamp();
            string hex_timeStamp = $"0x{Convert.ToString((int)timeStamp, 16)}";
            // 时间戳加密
            string md5_timeStamp = $"{timeStamp}".GetMD5();
            string token = $"token://com.coolapk.market/c67ef5943784d09750dcfbb31020f0ab?{md5_timeStamp}${Guid}&com.coolapk.market";
            string md5_token = token.GetBase64().GetMD5();
            string appToken = $"{md5_token}{Guid}{hex_timeStamp}";
            return appToken;
        }

        /// <summary>
        /// CreateDeviceCode Generace your custom device code
        /// </summary>
        private static string CreateDeviceCode(string aid, string mac, string manufactor, string brand, string model, string buildNumber)
        {
            return $"{aid}; ; ; {mac}; {manufactor}; {brand}; {model}; {buildNumber}".GetBase64(true).Reverse();
        }

        private static string RandMacAdress()
        {
            Random rand = new Random();
            string macAdress = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                macAdress += rand.Next(256).ToString("x2");
                if (i != 5)
                {
                    macAdress += ":";
                }
            }
            return macAdress;
        }

        private static string RandHexString(int n)
        {
            Random rand = new Random();
            byte[] bytes = new byte[n];
            rand.NextBytes(bytes);
            return BitConverter.ToString(bytes).ToUpperInvariant().Replace("-", "");
        }
    }

    public enum TokenVersion
    {
        TokenV1,
        TokenV2
    }
}
