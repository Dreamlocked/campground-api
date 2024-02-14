using Campground.Services.Campgrounds.Infrastructure.Data.Repository.Base;
using Campground.Services.Campgrounds.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Services.Campgrounds.Infrastructure.Data.Repository
{
    public class CampgroundRepository(CampgroundContext dbContext) : BaseRepository<Domain.Entities.Campground>(dbContext) 
    {
        public async Task<Domain.Entities.Campground?> GetByIdWithDetails(Guid id)
        {
            return await _dbContext.Campgrounds
                .Include(c => c.Host)
                .Include(c => c.Bookings)
                .ThenInclude(b => b.Review)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}
