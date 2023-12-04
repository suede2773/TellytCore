using TellytCore.services.interfaces;
using TellytCore.services;
using TellytCore.Data.interfaces;
using TellytCore.Data;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;
var dropFolder = configuration.GetValue<string>("DropFolder");
var logFileLocation = configuration.GetValue<string>("LogFileLocation");

builder.Services.AddCors(options => {
  options.AddPolicy("AllowAll",
      builder =>
      {
        builder.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader()
         .AllowAnyOrigin();
      });
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(dropFolder);
builder.Services.AddSingleton(logFileLocation);

builder.Services.AddScoped<ILogging, Logging>();
builder.Services.AddScoped<IHelper, Helper>();

builder.Services.AddScoped<IBaseDataAccess, BaseDataAccess>();
builder.Services.AddScoped<IAssetService, AssetService>();

builder.Services.AddScoped<IUser, User>();

builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
  options.ForwardedForHeaderName = "X-Coming-From";
});

builder.Services.Configure<IISServerOptions>(options =>
{
  options.MaxRequestBodySize = int.MaxValue;
  options.MaxRequestBodyBufferSize = int.MaxValue;
});

var app = builder.Build();

app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

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
app.UseForwardedHeaders();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
