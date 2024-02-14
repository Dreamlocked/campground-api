using System;
using System.Collections.Generic;

namespace Campground.Services.Campgrounds.Domain.Entities;

public partial class Campground
{
    public Guid Id { get; set; }

    public Guid HostId { get; set; }

    public string? Title { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public decimal PricePerNight { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool? Enable { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual User Host { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}
