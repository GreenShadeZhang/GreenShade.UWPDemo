using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TimedTasksByDotNetCore.Models;

namespace TimedTasksByDotNetCore.Services
{
    public class WallpaperService
    {
        private readonly HttpClient _httpClient;
        public WallpaperService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<WallpapersData> GetWallparper(int index, int number)
        {
            // string url = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=8&n=25";
            string url = string.Format("https://cn.bing.com/HPImageArchive.aspx?format=js&idx={0}&n={1}&mkt=zh-cn", index, number);
            Uri uri = new Uri(url);
            //var httpClient = new HttpClient();
            string json = await _httpClient.GetStringAsync(uri);
            WallpapersData wallPapersData = JsonConvert.DeserializeObject<WallpapersData>(json);
            return wallPapersData;
        }
    }
}
