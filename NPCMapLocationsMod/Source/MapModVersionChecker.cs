﻿/*
Version checker for the mod.
Gives notifications for updates.
*/
using System;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using StardewModdingAPI;

namespace NPCMapLocations
{
    class MapModVersionChecker
    { 
        private static string uri = "https://api.github.com/repos/Bouhm/stardew-valley-mods/releases";
        public const string VERSION = "1.42";
        public static string notification = "";

        // Notification message
        public static async void getNotification()
        {
            double latestVer = await getLatestVersion();
            double currentVer = Convert.ToDouble(VERSION);
            if (latestVer == -1)
            {
                notification = "";
            }
            else if (latestVer > currentVer)
            {
                notification = " - Update is Available.";
            }
            else if (latestVer == currentVer)
            {
                notification = " - Latest Version.";
            }
            else
            {
                notification = "";
            }
        }

        public static async Task<JArray> GetJsonAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Map Mod Version Checker");
                try
                {
                    var jsonString = await client.GetStringAsync(uri).ConfigureAwait(false);
                    return JArray.Parse(jsonString);
                }
                catch
                {
                    return null;
                }
            }
        }

        private static async Task<double> getLatestVersion()
        {
            JArray json = await GetJsonAsync(uri);
            if (json == null)
            {
                return -1.0;
            }
            string tag = (string)json[0]["tag_name"];
            return Convert.ToDouble(tag.Split('v')[1]);
        }
    }
}
