using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CertificateAuthorityServer.API.Tools
{
    public static class CertificateUtils
    {
        private const string CertPemHeader = "-----BEGIN CERTIFICATE-----";
        private const string CertPemFooter = "-----END CERTIFICATE-----";

        private const string KeyPemHeader = "-----BEGIN RSA PRIVATE KEY-----";
        private const string KeyPemFooter = "-----END RSA PRIVATE KEY-----";

        public static string ToPemString(this X509Certificate2 certificate)
        {
            var certData = certificate.Export(X509ContentType.Cert);
            var certTextBuilder = new StringBuilder();

            certTextBuilder.AppendLine(CertPemHeader);
            certTextBuilder.AppendLine(Convert.ToBase64String(certData, Base64FormattingOptions.InsertLineBreaks));
            certTextBuilder.AppendLine(CertPemFooter);

            return certTextBuilder.ToString();
        }

        public static string KeyToPemString(this X509Certificate2 certificate)
        {
            var privateKeyData = certificate.GetRSAPrivateKey()!.ExportRSAPrivateKey();
            var privateKeyTextBuilder = new StringBuilder();

            privateKeyTextBuilder.AppendLine(KeyPemHeader);
            privateKeyTextBuilder.AppendLine(Convert.ToBase64String(privateKeyData, Base64FormattingOptions.InsertLineBreaks));
            privateKeyTextBuilder.AppendLine(KeyPemFooter);

            return privateKeyTextBuilder.ToString();
        }

        public static void SaveToFiles(this X509Certificate2 certificate, string pathToCertificateFile,
            string pathToPrivateKeyFile)
        {
            File.WriteAllText(pathToCertificateFile, certificate.ToPemString());

            if (certificate.HasPrivateKey)
            {
                File.WriteAllText(pathToPrivateKeyFile, certificate.KeyToPemString());
            }
        }
    }
}
