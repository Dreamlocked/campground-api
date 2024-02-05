using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Salt { get; set; }

    public string? Hash { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Campground> Campgrounds { get; set; } = new List<Campground>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
