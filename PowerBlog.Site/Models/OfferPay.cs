﻿using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class OfferPay
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string? OfferWord { get; set; }
        public int? OfferPercentage { get; set; }
        public decimal? OfferAmount { get; set; }
        public string? Description { get; set; }
        public bool IsDisable { get; set; }
        public DateTime? CreatDate { get; set; }


        #region Relations
        public ICollection<Order>? Orders { get; set; }
        #endregion
    }
}
