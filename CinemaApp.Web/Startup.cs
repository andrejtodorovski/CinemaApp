using CinemaApp.Domain.DTO;
using CinemaApp.Domain.Identity;
using CinemaApp.Repository;
using CinemaApp.Repository.Implementation;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Implementation;
using CinemaApp.Service.Interface;
using CinemaApp.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaApp.Web
{
    public class Startup
    {
        private EmailSettings emailSettings;

        public Startup(IConfiguration configuration)
        {
            emailSettings = new EmailSettings();
            Configuration = configuration;
            Configuration.GetSection("EmailSettings").Bind(emailSettings);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<CinemaAppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddRoles<IdentityRole>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));


            services.AddScoped<EmailSettings>(es => emailSettings);
            services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailSettings));

            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IProjectionService, ProjectionService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IEmailService, EmailService>();


            services.AddControllersWithViews();
            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/account/login";
                options.LogoutPath = $"/account/logout";
                options.AccessDeniedPath = $"/account/login";
            });

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            SeedRoles(services.GetRequiredService<RoleManager<IdentityRole>>());
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);

        }
        // (најавата да вклучува улоги на корисниците). Можни улоги се: администратор, стандарден корисник
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "User"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Admin"
                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
