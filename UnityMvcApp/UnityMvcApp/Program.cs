namespace UnityMvcApp
{
    using System.IO;
    using System.Net;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Unity.Microsoft.DependencyInjection;

    public class Program
    {
        public static void Main(string[] args)
        {
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(opt =>
                {
                    Program.ListenWithDevelopmentServer(opt, 5002, Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.AllowCertificate);
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUnityServiceProvider()
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        private static KestrelServerOptions ListenWithDevelopmentServer(
            KestrelServerOptions kestrelServerOptions,
            int port,
            Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode clientCertificateMode)
        {
            kestrelServerOptions.Listen(IPAddress.IPv6Any, port, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1;
                listenOptions.UseHttps(httpsOptions =>
                {
                    httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                    httpsOptions.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.NoCertificate;
                    httpsOptions.AllowAnyClientCertificate();
                });
            });

            return kestrelServerOptions;
        }
    }
}