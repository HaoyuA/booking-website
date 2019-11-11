namespace FIT5032_Assign.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RoomStates
    {
        public int Id { get; set; }

        public int RoomId { get; set; }

        public DateTime Date { get; set; }

        public double PriceChange { get; set; }

        public int AvaibleRoom { get; set; }

        public virtual Room Room { get; set; }
    }
}
