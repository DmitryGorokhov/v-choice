using BLL.Interface;
using BLL.Service;
using BLL.Utils;
using DAL;
using DAL.Interface;
using DAL.Model;
using DAL.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace backend_v_choice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "backend_v_choice", Version = "v1" });
            });
            
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DBContext>(opt => opt.UseSqlServer(connection));
            services.AddScoped<ICommentsRepository, CommentsRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IPaginationRepository, PaginationRepository>();
            services.AddScoped<IRateRepository, RateRepository>();
            services.AddScoped<IStatisticRepository, StatisticRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAutorizationService, AutorizationService>();
            services.AddScoped<ICrudService, CrudService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<IPaginationService, PaginationService>();
            services.AddScoped<IStatisticService, StatisticService>();
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DBContext>();



            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../../frontend_v-choice/build";
            });

            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddCors();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "ViewerChoiceApp";
                options.LoginPath = "/";
                options.AccessDeniedPath = "/";
                options.LogoutPath = "/";
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            CreateUserRoles(services).Wait(); 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "backend_v_choice v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "../../frontend_v-choice";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            // Создание ролей администратора и пользователя
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            // Создание Администратора
            string adminEmail = "admin@mail.com";
            string adminPassword = "Admin21!";
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            // Создание Пользователя
            string userEmail = "user@mail.com";
            string userPassword = "User21!";
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                User user = new User { Email = userEmail, UserName = userEmail };
                IdentityResult result = await userManager.CreateAsync(user, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "user");
                }
            }
        }
    }
}
