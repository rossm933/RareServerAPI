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
            List<Posts> posts = new List<Posts>
        {
            new Posts
            {
                Id = 1,
                UserId = 101,
                CategoryId = 5,
                Title = "First Post",
                PublishedOn = DateTime.Now.AddDays(-10),
                ImageUrl = null,
                Content = "This is the content of the first post.",
                Approved = true
            },
            new Posts
            {
                Id = 2,
                UserId = 102,
                CategoryId = 3,
                Title = "Second Post",
                PublishedOn = DateTime.Now.AddDays(-5),
                ImageUrl = null,
                Content = "This is the content of the second post.",
                Approved = false
            },
            new Posts
            {
                Id = 3,
                UserId = 103,
                CategoryId = 1,
                Title = "Third Post",
                PublishedOn = DateTime.Now.AddDays(-2),
                ImageUrl = null,
                Content = "This is the content of the third post.",
                Approved = true
            }
        };
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });


            var app = builder.Build();

            // Use the CORS policy
            app.UseCors("AllowSpecificOrigin");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Get all posts
            app.MapGet("/posts", () =>
            {
                return posts;
            });
            
            // Get all posts by id
            app.MapGet("/posts/{id}", (int id) =>
            {
                Posts post = posts.FirstOrDefault(e => e.Id == id);
                if (post == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(post);
            });

            // Create a post
            app.MapPost("/posts", (Posts post) =>
            {

                post.Id = posts.Max(st => st.Id) + 1;
                posts.Add(post);
                return post;
            });


            // Delete a post
            app.MapDelete("/posts/{id}", (int id) =>
            {
                Posts post = posts.FirstOrDefault(st => st.Id == id);
                posts.Remove(post);
            });

            // Update a Post
            app.MapPut("/posts/{id}", (int id, Posts post) =>
            {
                Posts postToUpdate = posts.FirstOrDefault(st => st.Id == id);
                int postIndex = posts.IndexOf(postToUpdate);
                if (postToUpdate == null)
                {
                    return Results.NotFound();
                }
                if (id != post.Id)
                {
                    return Results.BadRequest();
                }
                posts[postIndex] = post;
                return Results.Ok();
            });

            // Get a users posts
            app.MapGet("/posts/user/{userId}", (int userId) =>
            {
                var userPosts = posts.Where(p => p.UserId == userId).ToList();
                if (userPosts == null || !userPosts.Any())
                {
                    return Results.NotFound($"No posts found for user.");
                }
                return Results.Ok(userPosts);
            });

            app.MapGet("/users", () =>
            {
                return users;
            });

            app.MapGet("/users/{id}", (int id) =>
            {
                Users user = users.FirstOrDefault(e => e.Id == id);
                if (id == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(user);

            });

            app.MapPost("/users", (Users newUsers) =>
            {
                newUsers.Id = users.Max(user => user.Id) + 1;
                users.Add(newUsers);
                return users;
            });

            // View User List (username, first/last name, email, ordered by username)
            app.MapGet("/users/userList", () =>
            {
                var userList = users.OrderBy(user => user.Username).ToList();
                return Results.Ok(userList);
            });

            //Filter by User
            app.MapGet("/posts/user/filterby/{userId}", (int id, int userId) =>
            {
                Users filteredUser = users.FirstOrDefault(user => user.Id == id);
                if (filteredUser == null)
                {
                    return Results.NotFound();
                }
                var filteredPosts = posts.Where(post => post.UserId == userId).ToList();
                return Results.Ok(filteredPosts);
            });

            app.Run();
        }
    }
}
