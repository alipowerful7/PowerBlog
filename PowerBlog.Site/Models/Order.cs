using System.ComponentModel.DataAnnotations;
namespace PowerBlog.Site.Models
{
    public class Order
    {
        [Key]
        public long Id { get; set; }
        public DateTime? OrderDate { get; set; } = DateTime.Now;
        public DateTime? PayDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public string? OfferWord { get; set; }

        #region Relations
        public User? User { get; set; }
        public long? UserId { get; set; }
        public Blog? Blog { get; set; }
        public long? BlogId { get; set; }
        #endregion
    }
}
