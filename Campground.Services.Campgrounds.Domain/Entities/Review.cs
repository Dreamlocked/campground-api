using System;
using System.Collections.Generic;

namespace Campground.Services.Campgrounds.Domain.Entities;

public partial class Review
{
    public Guid Id { get; set; }

    public Guid BookingId { get; set; }

    public string? Comment { get; set; }

    public int? Rating { get; set; }

    public DateTime? CreateAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
