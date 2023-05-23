using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Newtonsoft.Json.Serialization;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);
        
        // Add services to the container.

        //Enable CORS
        builder.Services.AddCors(c =>
        {
            c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });


        //Json Serializer
        builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
            = new DefaultContractResolver());
        
        //Otel config
        var serviceName = "StoreMongo";
        var serviceVersion = "0.1";

        builder.Services.AddOpenTelemetry()
                   .WithTracing(tracerProviderBuilder =>
                    tracerProviderBuilder.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri("http://localhost:4317/v1/traces");
                        opt.Headers = $"Authorization={Environment.GetEnvironmentVariable("0bf164a69482b4413540083180aca560ce84NRAL")}";
                    })
                      .AddSource(serviceName)
                     .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion))
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
               // .AddMongoDBInstrumentation()               
                                 );

        // Mongo Instrumentation config 
        //https:github.com/jbogard/MongoDB.Driver.Core.Extensions.DiagnosticSources

        var mongoUrl = MongoUrl.Create("mongodb+srv://<user>:<password>@cluster0.gt0eyjy.mongodb.net/");
        var clientSettings = MongoClientSettings.FromUrl(mongoUrl);
        var options = new InstrumentationOptions { CaptureCommandText = true };
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber(options));
        var mongoClient = new MongoClient(clientSettings);

        builder.Services.AddControllers();

        builder.Services.AddSingleton(mongoClient.GetDatabase("Store"));

        var app = builder.Build();


        // Configure the HTTP request pipeline.

        //Enable CORS
        app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

        app.UseAuthorization();

        app.MapControllers();

        app.Run();


        //To allow use photos

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
            RequestPath = "/Photos"
        });
    }
}
