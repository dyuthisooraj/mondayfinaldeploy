using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HalcyonApparelsMVC.Models
{
    public class TempData
    {
        [DisplayName("Id")]
        
        [Column(TypeName = "INT")]
        public int AccessoryId { get; set; }

        [DisplayName("Title")]
       
        [Column(TypeName = "VARCHAR")]
        [StringLength(50, MinimumLength = 3)]
        public string AccessoryName { get; set; } = null!;

        [DisplayName("Type")]
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(50, MinimumLength = 3)]
        public string AccessoryType { get; set; } = null!;

        [DisplayName("Brand")]
        
        [Column(TypeName = "VARCHAR")]
        [StringLength(50, MinimumLength = 3)]
        public string AccessoryBrand { get; set; } = null!;

        [DisplayName("Price")]
        
        [Column(TypeName = "INT")]
        public int AccessoryPrice { get; set; }

        [DisplayName("Discount")]
        [Column(TypeName = "INT")]
        public int AccessoryDiscount { get; set; }

        public string ImageUrl { get; set; }


        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile ImageFile { get; set; }
    }
}
