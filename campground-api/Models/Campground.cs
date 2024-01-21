using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class Campground
{
    public int Id { get; set; }

    public int HostId { get; set; }

    public int ProvinceId { get; set; }

    public string Title { get; set; } = null!;

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual User Host { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Province Province { get; set; } = null!;
}
