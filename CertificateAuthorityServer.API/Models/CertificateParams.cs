using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CertificateAuthorityServer.API.Models
{
    public class CertificateParams
    {
        [Required] 
        public string CommonName { get; set; } = null!;

        public string? Title { get; set; }
        public string? Country { get; set; }
        public string? Locality { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }
        public string? Organization { get; set; }

        public override string ToString()
        {
            var paramsBuilder = new StringBuilder();

            paramsBuilder.Append($"CN={CommonName}");
            paramsBuilder.Append(Title != null ? $", T={Title}" : "");
            paramsBuilder.Append(Country != null ? $", C={Country}" : "");
            paramsBuilder.Append(Locality != null ? $", L={Locality}" : "");
            paramsBuilder.Append(Street != null ? $", Street={Street}" : "");
            paramsBuilder.Append(PostalCode != null ? $", PostalCode={PostalCode}" : "");
            paramsBuilder.Append(Organization != null ? $", O={Organization}" : "");

            return paramsBuilder.ToString();
        }
    }
}
