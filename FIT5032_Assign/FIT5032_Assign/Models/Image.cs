namespace FIT5032_Assign.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        public int Id { get; set; }

        public int HotelId { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        public string ImagePath { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
