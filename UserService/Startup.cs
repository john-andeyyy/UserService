using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using System;
using Model.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

//using TODO_API_net.Models.basicAuth;/
//using static TODO_API_net.Models.basicAuth.BasicAuthServices;

namespace UserService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddScoped<IBasicAuthService, BasicAuthService>();

            var db_host = Environment.GetEnvironmentVariable("DB_HOST");
            var db_port = Environment.GetEnvironmentVariable("DB_PORT");
            var db_name = Environment.GetEnvironmentVariable("DB_NAME");
            var db_user = Environment.GetEnvironmentVariable("DB_USER");
            var db_password = Environment.GetEnvironmentVariable("DB_PASS");

            var conn = $"Server={db_host};Port={db_port};Database={db_name};Uid={db_user};" +
                $"Pwd={db_password};Convert Zero Datetime=True";

            BaseManager.ConnectionString = conn;

            services.AddTransient<MySqlConnection>(_ => new MySqlConnection(conn));

            //BasicAuthServices.conn = conn;
            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                .AddMySql5()
                .WithGlobalConnectionString(conn)
                .ScanIn(typeof(Startup).Assembly).For.Migrations());

            services.AddHttpContextAccessor();
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SIGNING_KEY"))),
                        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
                        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AuthPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion((authHandlerContext) =>
                    {
                        var claims = authHandlerContext.User.Claims;

                        try
                        {
                            string Username = claims.First(x => x.Type == "Username").Value;
                            var manager = new UserManager();
                            var isvalid = manager.GetUserByUsername(Username);



                            if (!String.IsNullOrEmpty(isvalid?.Username)) { return true; } else { return false; }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message.ToString());
                            return false;
                        }
                    });
                });
            });

            var jwtSettings = new JwtSettings();
            jwtSettings.IssuerSigningKey = Environment.GetEnvironmentVariable("SIGNING_KEY");
            jwtSettings.ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
            jwtSettings.ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            jwtSettings.ExpirationSeconds = int.Parse(Environment.GetEnvironmentVariable("EXPIRATION_SECONDS"));

            services.AddSingleton(jwtSettings);


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            using (var scope = app.ApplicationServices.CreateScope())
            {
                Console.WriteLine("\n");
                var dbConnection = scope.ServiceProvider.GetRequiredService<MySqlConnection>();
                try
                {
                    dbConnection.Open();
                    Console.WriteLine("Database connection successful!!!");
                    dbConnection.Close();
                }
                catch (Exception error)
                {
                    Console.WriteLine($"X Database connection failed: {error.Message}");
                }

                //try
                //{
                //    var migrator = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                //    //migrator.MigrateDown(0);// remove all
                //    migrator.MigrateUp();
                //    Console.WriteLine("Migrations applied successfully!");
                //}
                //catch (Exception error)
                //{
                //    Console.WriteLine($"X Migration failed: {error.Message}");
                //}
                //Console.WriteLine("\n");


            }


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to User Service!");
                });
            });
        }
    }
}
