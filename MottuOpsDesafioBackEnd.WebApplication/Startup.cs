using MottuOpsDesafioBackEnd.Business.Interface;
using MottuOpsDesafioBackEnd.Business.Service;
using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Data.Repository;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); 
            options.Cookie.HttpOnly = true; 
            options.Cookie.IsEssential = true; 
        });

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICourierService, CourierService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IMotorcycleService, MotorcycleService>();
        services.AddScoped<IMotorcycleTypeService, MotorcycleTypeService>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<ICourierRepository, CourierRepository>();
        services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
        services.AddScoped<IMotorcycleTypeRepository, MotorcycleTypeRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IUserRepository, UserRepository>(); 
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseSession();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}