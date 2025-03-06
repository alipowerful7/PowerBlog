using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class OfferPay
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string? OfferWord { get; set; }
        public double? OfferPercentage { get; set; }
        public double? OfferAmount { get; set; }
        public string? Description { get; set; }
    }
}
