using HomeBanking.Models;
using HomeBanking.Repositories.Implementations;
using HomeBanking.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


//agregamos el contexto ******

builder.Services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("HomeBankingConexion")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{

    //Aqui obtenemos todos los services registrados en la App
    var services = scope.ServiceProvider;
    try
    {

        // En este paso buscamos un service que este con la clase HomeBankingContext
        var context = services.GetRequiredService<HomeBankingContext>();
        DBInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ha ocurrido un error al enviar la información a la base de datos!");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
