using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CampgroundId { get; set; }

    public DateOnly ArrivingDate { get; set; }

    public DateOnly LeavingDate { get; set; }

    public decimal? PricePerNight { get; set; }

    public int NumNights { get; set; }

    public virtual Campground Campground { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual User User { get; set; } = null!;
}
