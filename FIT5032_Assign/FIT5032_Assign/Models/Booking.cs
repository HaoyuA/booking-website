namespace FIT5032_Assign.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        public int Id { get; set; }

        public DateTime CheckinDate { get; set; }

        public DateTime CheckoutDate { get; set; }

        [Required]
        public string ASPUserId { get; set; }

        public int? Rating { get; set; }

     
        public string Comment { get; set; }

        public int RoomId { get; set; }

        public double Price { get; set; }

        public int RoomNumber { get; set; }

        public virtual Room Room { get; set; }
    }
}
