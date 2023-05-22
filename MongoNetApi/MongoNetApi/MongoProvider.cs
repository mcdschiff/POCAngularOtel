using OpenTelemetry.Trace;

namespace MongoNetApi
{
    public static class MongoProvider
    {
        public static TracerProviderBuilder AddMongoDBInstrumentation(this TracerProviderBuilder builder)
        {
            return builder.AddSource("MongoDB.Driver.Core.Extensions.DiagnosticSources");
        }
    }
}
