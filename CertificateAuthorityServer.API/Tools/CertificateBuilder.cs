using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CertificateAuthorityServer.API.Models;

namespace CertificateAuthorityServer.API.Tools;

public static class CertificateBuilder
{
    private static readonly Random Rnd = new ();
    private static readonly SHA256 Sha = SHA256.Create();

    public static X509Certificate2 GenerateRootCertificate()
    {
        var rsaKey = RSA.Create(4096);
        var subject = "CN=nova-ca-server";
        var offset = DateTimeOffset.Now.AddYears(5);
        var certReq = new CertificateRequest(subject, rsaKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        
        certReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, true));
        certReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(certReq.PublicKey, false));
        
        return certReq.CreateSelfSigned(DateTimeOffset.Now, offset);
    }

    public static X509Certificate2 GenerateCertificate(X509Certificate2 caCertificate, CertificateParams certificateParams)
    {
        var rsaKey = RSA.Create(4096);
        var subject = certificateParams.ToString();
        var offset = DateTimeOffset.Now.AddYears(1);
        var serialNumber = GetRandomSerialNumber();
        var clientReq = new CertificateRequest(subject, rsaKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        clientReq.CertificateExtensions.Add(new X509BasicConstraintsExtension(false, false, 0, false));
        clientReq.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, false));
        clientReq.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(clientReq.PublicKey, false));

        return clientReq.Create(caCertificate, DateTimeOffset.Now, offset, serialNumber).CopyWithPrivateKey(rsaKey);
    }

    private static byte[] GetRandomSerialNumber()
    {
        var array = new byte[16];
        var temp = BitConverter.GetBytes(DateTime.Now.ToBinary());

        for (int i = 0; i < 8; i++)
        {
            array[i] = temp[i];
        }

        temp = BitConverter.GetBytes(Rnd.NextInt64());

        for (int i = 8; i < 16; i++)
        {
            array[i] = temp[i - 8];
        }

        return Sha.ComputeHash(array);
    }
}