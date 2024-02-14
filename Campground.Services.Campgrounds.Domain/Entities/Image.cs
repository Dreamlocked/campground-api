using System;
using System.Collections.Generic;

namespace Campground.Services.Campgrounds.Domain.Entities;

public partial class Image
{
    public Guid Id { get; set; }

    public Guid CampgroundsId { get; set; }

    public string? Filename { get; set; }

    public string? Url { get; set; }

    public string? Alt { get; set; }

    public virtual Campground Campgrounds { get; set; } = null!;
}
