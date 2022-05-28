using System.Security.Cryptography.X509Certificates;
using CertificateAuthorityServer.API.Tools;

namespace CertificateAuthorityServer.API
{
    public static class Program
    {
        public static X509Certificate2? CaCertificate;

        public static void Main(string[] args)
        {
            if (File.Exists("./ca.pem") && File.Exists("./ca-key.pem"))
            {
                CaCertificate = X509Certificate2.CreateFromPemFile("./ca.pem", "./ca-key.pem");
            }
            else
            {
                CaCertificate = CertificateBuilder.GenerateRootCertificate();
                CaCertificate.SaveToFiles("./ca.pem", "./ca-key.pem");
            }

            if (!Directory.Exists("./certificates"))
            {
                Directory.CreateDirectory("./certificates");
            }

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}