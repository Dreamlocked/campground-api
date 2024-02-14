using System;
using System.Collections.Generic;

namespace Campground.Services.Campgrounds.Domain.Entities;

public partial class Notification
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime CreateAt { get; set; }

    public bool? Viewed { get; set; }
}
