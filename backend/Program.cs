using JournalAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using IAuthorizationService = JournalAPI.Services.Interfaces.IAuthorizationService;

namespace JournalAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = null;
            })
             .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                options.JsonSerializerOptions.MaxDepth = 64;
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.WithOrigins("https://localhost:7035", "http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
            });

            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAuthorizationService, AuthorizationService>();
            builder.Services.AddTransient<ISemesterService, SemesterService>();
            builder.Services.AddTransient<IGroupService, GroupService>();
            builder.Services.AddTransient<ISubjectService, SubjectService>();
            builder.Services.AddTransient<IRelationService, RelationService>();
            builder.Services.AddTransient<IMarkService, MarkService>();
            builder.Services.AddTransient<IStudentService, StudentService>();
            builder.Services.AddTransient<ILoggingService, LoggingService>();
            builder.Services.AddTransient<AppDbContext>();
        }

        private static void Configure(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
        }
    }
}