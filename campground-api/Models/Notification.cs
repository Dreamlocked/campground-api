using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreateAt { get; set; }

    public bool? Viewed { get; set; }

    public int? BookingId { get; set; }

    public virtual User User { get; set; } = null!;
}
