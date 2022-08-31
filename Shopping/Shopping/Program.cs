using Microsoft.EntityFrameworkCore;
using Shopping.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Crear un servicio datacontext, con una  expresion lambda inciada por "o" para especificar que es de opcion, se pone lo que se considera que representa
builder.Services.AddDbContext<DataContext>(o =>
{
    //aca llamo a mi string de conexión
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// a traves de la consola de paquetes  nugget mediante el comando,add-migration, cada vez que se haga un cambio en las tablas se debe correr