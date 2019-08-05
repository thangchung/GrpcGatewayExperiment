using Gateway.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace Gateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot();//.AddGrpcHttpGateway(Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseMiddleware<ProxyMiddleware>();

            //app.UseRouting();

            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });*/

            app.UseOcelot(config =>
            {
                config.AddGrpcHttpGateway();
            }).Wait();
        }
    }

    /*public class ProxyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ProxyMiddleware> _logger;

        public ProxyMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ProxyMiddleware>();
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            using var stream = typeof(Greeter.GreeterClient).Assembly.GetManifestResourceStream("GrpcShared.greet.pb");

            var fileDescriptorSet = Serializer.Deserialize<FileDescriptorSet>(stream);

            var myServices = new List<MyServiceInfo>();

            foreach (var fileDescriptorProto in fileDescriptorSet.Files)
            {
                MyServiceInfo myService;

                foreach (var service in fileDescriptorProto.Services)
                {
                    myService = new MyServiceInfo
                    {
                        Name = service.Name
                    };

                    MyMethodInfo myMethod;

                    foreach (var method in service.Methods)
                    {
                        myMethod = new MyMethodInfo
                        {
                            Name = method.Name,
                            InputType = method.InputType[1..],
                            OutputType = method.OutputType[1..]
                        };

                        myService.Methods.Add(myMethod);
                    }

                    myServices.Add(myService);
                }
            }

            var aa = myServices;
        }
    }

    public class MyServiceInfo
    {
        public string Name { get; set; }
        public List<MyMethodInfo> Methods { get; set; } = new List<MyMethodInfo>();
    }

    public class MyMethodInfo
    {
        public string Name { get; set; }
        public string InputType { get; set; }
        public string OutputType { get; set; }
    }*/
}
