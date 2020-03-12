using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using TimedTasksByDotNetCore.Models;
using TimedTasksByDotNetCore.Services;

namespace TimedTasksByDotNetCore
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<WallpaperService>();
            services.AddHangfire(x => x.UseStorage(new MemoryStorage()));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WallpaperService wallpaperService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireServer();
            app.UseHangfireDashboard();

            //RecurringJob.AddOrUpdate(()=>TestAsync(wallpaperService), Cron.Minutely());

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    RecurringJob.AddOrUpdate(()=>TestAsync(wallpaperService), Cron.Minutely());
                    //var data = await wallpaperService.GetWallparper(0, 6);
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }


        public async Task TestAsync(WallpaperService wallpaperService)
        {
            using (HttpClient _httpClient = new HttpClient())
            {
                // string url = "https://cn.bing.com/HPImageArchive.aspx?format=js&idx=8&n=25";
                string url = string.Format("https://cn.bing.com/HPImageArchive.aspx?format=js&idx={0}&n={1}&mkt=zh-cn", 0, 6);
                Uri uri = new Uri(url);
                //var httpClient = new HttpClient();
                string json = await _httpClient.GetStringAsync(uri);
                WallpapersData wallPapersData = JsonConvert.DeserializeObject<WallpapersData>(json);
            }

            //var data = await wallpaperService.GetWallparper(0, 6);
            // to do...
        }
    }
}
