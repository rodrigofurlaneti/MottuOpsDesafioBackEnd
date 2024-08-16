using MottuOpsDesafioBackEnd.Data.Interface;
using MottuOpsDesafioBackEnd.Data.Repository;

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
        services.AddControllersWithViews();

        // Adiciona cache distribuído para sessão
        services.AddDistributedMemoryCache();

        // Configura a sessão
        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Tempo de expiração da sessão
            options.Cookie.HttpOnly = true; // Segurança adicional
            options.Cookie.IsEssential = true; // Necessário para manter a sessão mesmo sem consentimento do usuário
        });

        // Register the repositories
        services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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