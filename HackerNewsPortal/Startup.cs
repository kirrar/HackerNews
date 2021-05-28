using HackerNewsPortal.DataContext;
using HackerNewsPortal.Models;
using HackerNewsPortal.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HackerNewsPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private readonly char[] charsToTrim = { '[', ']' };

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Data>(opt => { opt.UseInMemoryDatabase(databaseName: "hacker_news"); opt.EnableSensitiveDataLogging(); });
            BuildProviders(services);

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            var scope = app.ApplicationServices.CreateScope();
            var service = scope.ServiceProvider.GetService<Data>();

            SetUpData(service);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        public void BuildProviders(IServiceCollection services)
        {
            services.AddScoped<IHackerNewsProvider, HackerNewsProvider>();
        }

        public void SetUpData(Data context)
        {
            List<int> storiesIds = new List<int>();

            WebRequest request = WebRequest.Create(Configuration.GetValue<Uri>("TopStories"));
            request.ContentType = "application/json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new(responseStream, Encoding.UTF8);
                    var jsonString = reader.ReadToEnd();

                    var trimmedJson = jsonString.Trim(charsToTrim);

                    foreach (var storyId in trimmedJson.Split(','))
                    {
                        storiesIds.Add(Convert.ToInt32(storyId));
                    }
                }
            }

            var sorted = storiesIds.OrderBy(x => x).ToList();

            foreach (var sortedStory in sorted)
            {
                var newStory = new Story();

                var Uri = string.Format("{0}{1}/{2}.json", Configuration.GetValue<string>("HackerNewsUrl"), Configuration.GetValue<string>("StoryPath"), sortedStory);

                WebRequest storyRequest = WebRequest.Create(Uri);
                storyRequest.ContentType = "application/json";

                HttpWebResponse storyResponse = (HttpWebResponse)storyRequest.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream responseStream = storyResponse.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        var jsonString = reader.ReadToEnd();

                        newStory = JsonConvert.DeserializeObject<Story>(jsonString);

                        if (newStory != null && newStory.url != null && !string.IsNullOrEmpty(newStory.url.ToString()))
                        {
                            var doesExist = context.Stories.Where(x => x.id == newStory.id).FirstOrDefault();

                            if (doesExist == null)
                            {
                                context.Stories.Add(newStory);
                            }
                        }
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
