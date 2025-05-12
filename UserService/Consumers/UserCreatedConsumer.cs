using Events;
using MassTransit;
using UserService.Data;
using UserService.Models;

namespace UserService.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        private readonly UserDbContext _context;

        public UserCreatedConsumer(UserDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message = context.Message;

            var exists = _context.Users.Any(u => u.Id == message.Id);
            if (exists) return;

            var newUser = new User
            {
                Id = message.Id,
                FullName = message.FullName,
                Email = message.Email,
                GitHubUsername = message.GitHubUsername, // AuthService vermiyor
                Bio = "",
                Theme = "light"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
        }
    }
}
