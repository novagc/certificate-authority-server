using System.Text;
using CertificateAuthorityServer.API.Models;
using CertificateAuthorityServer.API.Tools;
using Microsoft.AspNetCore.Mvc;

namespace CertificateAuthorityServer.API.Controllers
{
    [ApiController]
    [Route("api/v1/certificate/create")]
    [Produces("application/x-pem-file")]
    public class CreateController : ControllerBase
    {
        [HttpPost]
        public FileContentResult CreateCertificate([FromBody] CertificateParams certificateParams)
        {
            var certificate = CertificateBuilder.GenerateCertificate(Program.CaCertificate, certificateParams);
            Directory.CreateDirectory($"./certificates/{certificate.SerialNumber}");
            certificate.SaveToFiles($"./certificates/{certificate.SerialNumber}/certificate.pem", $"./certificates/{certificate.SerialNumber}/private-key.pem");

            return new FileContentResult(Encoding.UTF8.GetBytes(certificate.ToPemString()), "application/x-pem-file");
        }
    }
}
