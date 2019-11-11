namespace FIT5032_Assign.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class HotelModel : DbContext
    {
        public HotelModel()
            : base("name=HotelModel")
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<Image> Image { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<RoomStates> RoomStates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .Property(e => e.Latitude)
                .HasPrecision(10, 8);

            modelBuilder.Entity<Hotel>()
                .Property(e => e.Longitutde)
                .HasPrecision(11, 8);
        }
    }
}
