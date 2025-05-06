using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Store_app.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";
        [MaxLength(100)]
        public string Brand { get; set; } = "";
        [MaxLength(100)]
        public string categgory { get; set; } = "";
        [Precision(16, 2)]
        public string price { get; set; } = "";

        public string Descrption { get; set; } = "";
     
        public string Imgaqfillname { get; set; } = "";
        public DateTime createdAt { get; set; }
    }
}
