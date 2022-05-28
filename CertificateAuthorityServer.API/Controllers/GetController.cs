using System.Security.Cryptography.X509Certificates;
using System.Text;
using CertificateAuthorityServer.API.Models;
using CertificateAuthorityServer.API.Tools;
using Microsoft.AspNetCore.Mvc;

namespace CertificateAuthorityServer.API.Controllers
{
    [ApiController]
    [Route("api/v1/certificate/get")]
    [Produces("application/x-pem-file")]
    public class GetController : ControllerBase
    {
        [HttpGet("ca")]
        [Produces("application/x-pem-file")]
        public FileContentResult GetCaCertificate()
        {
            return new FileContentResult (Encoding.UTF8.GetBytes(Program.CaCertificate.ToPemString()), "application/x-pem-file");
        }

        [HttpGet("{serialNumber}")]
        [Produces("application/x-pem-file")]
        public IActionResult GetCertificate(string serialNumber)
        {
            if (!Directory.Exists($"./certificates/{serialNumber}"))
            {
                return NotFound();
            }

            return new FileContentResult(System.IO.File.ReadAllBytes($"./certificates/{serialNumber}/certificate.pem"), "application/x-pem-file");
        }

        [HttpGet("{serialNumber}/key")]
        [Produces("application/x-pem-file")]
        public IActionResult GetPrivateKey(string serialNumber)
        {
            if (!Directory.Exists($"./certificates/{serialNumber}"))
            {
                return NotFound();
            }

            return new FileContentResult(System.IO.File.ReadAllBytes($"./certificates/{serialNumber}/private-key.pem"), "application/x-pem-file");
        }
    }
}