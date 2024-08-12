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
                UserImage = "https://images.playground.com/7344181b53c14f018042ea2a7ec1cc3e.jpeg",
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
                UserImage = "-.-",
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
                UserImage = ":D",
                CreatedOn = DateTime.Now.AddMonths(-6),
                Active = false
            }
        };
            List<Posts> posts = new List<Posts>
        {
            new Posts
            {
                Id = 1,
                UserId = 1,
                CategoryId = 5,
                Title = "First Post",
                PublishedOn = DateTime.Now.AddDays(-10),
                ImageUrl = null,
                Content = "I'm a lil dough monster... Please don't bake me.",
                Approved = true,
                TagIds = new List<int> { 1 }

            },

            new Posts
            {
                Id = 4,
                UserId = 1,
                CategoryId = 5,
                Title = "First Post",
                PublishedOn = DateTime.Now.AddDays(-10),
                ImageUrl = null,
                Content = "This is the content of the first post.",
                Approved = true,
                TagIds = new List<int> { 1, 2 }
            },
            new Posts
            {
                Id = 2,
                UserId = 2,
                CategoryId = 3,
                Title = "Second Post",
                PublishedOn = DateTime.Now.AddDays(-5),
                ImageUrl = null,
                Content = "This is the content of the second post.",
                Approved = false,
                TagIds = new List<int> { 3, 2 }
            },
            new Posts
            {
                Id = 3,
                UserId = 3,
                CategoryId = 1,
                Title = "Third Post",
                PublishedOn = DateTime.Now.AddDays(-2),
                ImageUrl = null,
                Content = "This is the content of the third post.",
                Approved = true,
                TagIds = new List<int> { 2 }
            }
        };
            List<Categories> categories = new List<Categories>
            {
            new Categories
            {
                Id = 1,
                Label = "Technology"
            },
            new Categories
            {
                Id = 2,
                Label = "Science"
            },
            new Categories
            {
                Id = 3,
                Label = "Art"
            },
            new Categories
            {
                Id = 4,
                Label = "Sports"
            }
            };
            List<Tags> tags = new List<Tags>
            {
                new Tags
                {
                TagId = 1,
                Label = "Travel Goals"
                },
                new Tags
                {
                TagId = 2,
                Label = "Foodie Finds"
                },
                new Tags
                {
                TagId = 3,
                Label = "Tech Trends"
                },
            };
            List<Subscriptions> subs = new List<Subscriptions>
                {
                new Subscriptions
                {
                    Id = 1,
                    AuthorId = 2,
                    FollowerId = 3,
                    CreatedDate = DateTime.Now.AddMonths(-6)
                },
                new Subscriptions
                {
                    Id = 2,
                    AuthorId = 3,
                    FollowerId = 1,
                    CreatedDate = DateTime.Now.AddMonths(-3)
                },
                 new Subscriptions
                {
                    Id = 3,
                    AuthorId = 3,
                    FollowerId = 2,
                    CreatedDate = DateTime.Now.AddMonths(-4)
                },
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
                var orderedPosts = posts.OrderByDescending(c => c.PublishedOn).ToList();
                return Results.Ok(orderedPosts);
            });
            
            // Get all posts by id
            app.MapGet("/posts/{id}", (int id) =>
            {
                Posts? post = posts.FirstOrDefault(e => e.Id == id);
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
                Posts? postToUpdate = posts.FirstOrDefault(st => st.Id == id);
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

            // Get Categories
            app.MapGet("/categories", () =>
            {
                var orderedCategories = categories.OrderBy(c => c.Label).ToList();
                return Results.Ok(orderedCategories);
            });

            // Create a Category
            app.MapPost("/categories", (Categories category) =>
            {

                category.Id = categories.Max(st => st.Id) + 1;
                categories.Add(category);
                return category;
            });

            //Get all users sorted alphabetically
            app.MapGet("/users", () =>
            {
                return users.OrderBy(u => u.Username);
            });

            //Get a user by Id
            app.MapGet("/users/{id}", (int? id) =>
            {
                Users? user = users.FirstOrDefault(e => e.Id == id);
                if (id == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(user);

            });

            //Post a user
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

            // Filter post list by category
            app.MapGet("/posts/category/{id}", (int id) =>
            {
                var postByCategory = posts.Where(p => p.CategoryId == id).ToList();
                if (id == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(postByCategory);

            });

            //Create Tag
            app.MapPost("/tags", (Tags newTags) =>
            {
                newTags.TagId = tags.Max(tag => tag.TagId) + 1;
                tags.Add(newTags);
                return tags;
            });

            //View Tag List
            app.MapGet("/tags", () =>
            {
                var sortedTags = tags.OrderBy(tag => tag.Label).ToList();
                return sortedTags;
            });

            //all subscriptions
            app.MapGet("/subscriptions", () =>
            {
                return subs;
            });

            //Get a users subscibers
            app.MapGet("/subscriptions/subscribers/{id}", (int? id) =>
            {
                var sub = subs.Where(s => s.AuthorId == id);
                if (id == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(sub);
            });

            //Get a users subscriptions
            app.MapGet("/subscriptions/following/{id}", (int? id) =>
            {
                var sub = subs.Where(s => s.FollowerId == id);
                if (id == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(sub);
            });

            //Create a new subscription date, FollowerId and Id included in creation
            app.MapPost("/subscriptions/{id}", (int? id, Subscriptions newSub) =>
             {
                 newSub.Id = subs.Max(sub => sub.Id) +1;
                 newSub.AuthorId = id;
                 newSub.CreatedDate = DateTime.Now;
                 subs.Add(newSub);
                 return subs;

             });

            // Delete a subscription
            app.MapDelete("/subscriptions/{id}", (int? id) =>
            {
                Subscriptions? sub = subs.FirstOrDefault(s => s.Id == id);
                subs.Remove(sub);
                return Results.Ok(subs);
            });

            //gets a single subscription 1 would be a uid if a more complex app was being made
            app.MapGet("/subscriptions/1/{id}", (int? id) =>
            {
                var sub = subs.FirstOrDefault(s => s.FollowerId == 1 && s.AuthorId == id);
                if (sub == null)
                {
                    return Results.Ok(new { }); // Return an empty object if no subscription is found
                }
                    return Results.Ok(sub);
            });


            // Get all Posts with Tags
            app.MapGet("/posts_and_tags", () =>
            {
                // Join posts with their tags based on TagIds
                var postsWithTags = posts.Select(post => new
                {
                    post.Id,
                    post.UserId,
                    post.CategoryId,
                    post.Title,
                    post.PublishedOn,
                    post.ImageUrl,
                    post.Content,
                    post.Approved,
                    Tags = tags.Where(tag => post.TagIds.Contains(tag.TagId)).ToList()
                });

                return Results.Ok(postsWithTags);
            });

            // Get a specific Post with Tags by its ID
            app.MapGet("/posts_and_tags/{id}", (int id) =>
            {
                var post = posts.SingleOrDefault(p => p.Id == id);

                // Get the tags associated with the post
                var tagsForPost = tags.Where(tag => post.TagIds.Contains(tag.TagId)).ToList();

                // Create an anonymous object to represent the post with its tags
                var postWithTags = new
                {
                    post.Id,
                    post.UserId,
                    post.CategoryId,
                    post.Title,
                    post.PublishedOn,
                    post.ImageUrl,
                    post.Content,
                    post.Approved,
                    Tags = tagsForPost
                };

                return Results.Ok(postWithTags);
            });


            app.Run();
        }
    }
}
