using System;
using System.Collections.Generic;

namespace Campground.Services.Campgrounds.Domain.Entities;
public partial class Booking
{
    public Guid Id { get; set; }

    public Guid CampgroundId { get; set; }

    public Guid UserId { get; set; }

    public DateTime ArrivingDate { get; set; }

    public DateTime LeavingDate { get; set; }

    public DateTime? CreateAt { get; set; }

    public bool? Paid { get; set; }

    public bool? Attended { get; set; }

    public string? ReviewBody { get; set; }

    public DateTime? ReviewCreateAt { get; set; }

    public int? ReviewRating { get; set; }

    public virtual Campground Campground { get; set; } = null!;

    public virtual Review? Review { get; set; }

    public virtual User User { get; set; } = null!;
}
