using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.Sqlite;

var builder = WebApplication.CreateBuilder(args);

var elsaSection = builder.Configuration.GetSection("Elsa");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder
  .Services
    .AddElsa(elsa => elsa
      .UseEntityFrameworkPersistence(ef => ef.UseSqlite())
      .AddConsoleActivities()
      .AddHttpActivities(elsaSection.GetSection("Server").Bind)
      .AddEmailActivities(elsaSection.GetSection("Smtp").Bind)
      .AddQuartzTemporalActivities()
      .AddWorkflowsFrom<Startup>()
    )
    .AddElsaApiEndpoints();

builder
  .Services
    .AddCors(optios => optios
      .AddDefaultPolicy(policy => policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseCors();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseHttpActivities();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
