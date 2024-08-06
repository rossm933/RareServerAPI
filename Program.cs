using RareServerAPI.Models;
namespace RareServerAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            List<Users> users = new List<Users>
        {
            new Users
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password123",
                Bio = "Software developer with a passion for open-source projects.",
                Username = "johndoe",
                UserImage = null,
                CreatedOn = DateTime.Now.AddYears(-2),
                Active = true
            },
            new Users
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Password = "securepassword",
                Bio = "Graphic designer and illustrator.",
                Username = "janesmith",
                UserImage = null,
                CreatedOn = DateTime.Now.AddYears(-1),
                Active = true
            },
            new Users
            {
                Id = 3,
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                Password = "alicepassword",
                Bio = "Content writer and blogger.",
                Username = "alicejohnson",
                UserImage = null,
                CreatedOn = DateTime.Now.AddMonths(-6),
                Active = false
            }
        };

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/users", () =>
            {
                return users;
            });

            app.MapPost("/users", (Users newUsers) =>
            {
                newUsers.Id = users.Max(user => user.Id) + 1;
                users.Add(newUsers);
                return users;
            });

            app.Run();
        }
    }
}
