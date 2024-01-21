using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class Province
{
    public int Id { get; set; }

    public int RegionId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Campground> Campgrounds { get; set; } = new List<Campground>();

    public virtual Region Region { get; set; } = null!;
}
