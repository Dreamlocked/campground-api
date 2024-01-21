using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class Review
{
    public int Id { get; set; }

    public int BookingsId { get; set; }

    public string Body { get; set; } = null!;

    public short Rating { get; set; }

    public string Review1 { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public virtual Booking Bookings { get; set; } = null!;
}
