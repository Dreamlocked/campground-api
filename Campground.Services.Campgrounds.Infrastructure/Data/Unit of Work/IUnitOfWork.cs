using Campground.Services.Campgrounds.Infrastructure.Data.Repository;
using Campground.Services.Campgrounds.Infrastructure.Data.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Campground.Services.Campgrounds.Infrastructure.Data.Unit_of_Work
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        CampgroundRepository CampgroundRepository { get; }
        UserRepository UserRepository { get; }
        Task CompleteAsync();
    }
}
