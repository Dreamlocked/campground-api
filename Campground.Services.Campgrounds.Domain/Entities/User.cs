using System;
using System.Collections.Generic;

namespace Campground.Services.Campgrounds.Domain.Entities;

public partial class User
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Salt { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? UrlPhoto { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Campground> Campgrounds { get; set; } = new List<Campground>();
}
