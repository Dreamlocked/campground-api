using campground_api.Models;
using campground_api.Models.Dto;
using campground_api.Utils;
using Microsoft.EntityFrameworkCore;

namespace campground_api.Services
{
    public class UserService
    {
        private readonly CampgroundContext _context;
        public UserService(CampgroundContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll() =>
            await _context.Users.ToListAsync();

        public async Task<User?> Get(int id) =>
            await _context.Users
                .Include(u => u.Campgrounds)
                    .ThenInclude(c => c.Images)
                .Include(u => u.Bookings)
                .ThenInclude(b => b.Reviews)
                .FirstOrDefaultAsync(u => u.Id == id);

        public async Task<User> Create(SignInDto signInDto)
        {
            if(await _context.Users.AnyAsync(u => u.Username == signInDto.Username))
            {
                throw new Exception("Username already exists");
            }

            var user = new User()
            {
                Username = signInDto.Username,
                Email = signInDto.Email,
                FirstName = signInDto.FirstName,
                LastName = signInDto.LastName,
                Salt = Encript.GenerateSalt(),
            };

            user.Hash = Encript.GetSHA256Hash(signInDto.Password + user.Salt);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
            {
                return null;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<User?> Update(int id, SignInDto signInDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if(user is null) return null;

                user.Username = signInDto.Username;
                user.Email = signInDto.Email;
                user.Hash = Encript.GetSHA256Hash(signInDto.Password + user.Salt);

                await _context.SaveChangesAsync();

                return user;
            }
            catch(DbUpdateConcurrencyException) when(!UserExists(id))
            {
                return null;
            }
        }

        public async Task<User?> GetUserLogin(LoginDto userDto)
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == userDto.Username);

            if(user != null && user.Hash == Encript.GetSHA256Hash(userDto.Password + user.Salt)) return user;

            return null;
        }

        private bool UserExists(int id) =>
            _context.Users.Any(e => e.Id == id);
    }
}
