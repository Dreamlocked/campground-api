using System;
using System.Collections.Generic;

namespace campground_api.Models;

public partial class Region
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Province> Provinces { get; set; } = new List<Province>();
}
