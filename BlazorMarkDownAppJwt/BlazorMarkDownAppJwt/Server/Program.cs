using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BlazorMarkDownAppJwt.Server.Services.Users;
using BlazorMarkDownAppJwt.Server.Services.MarkDowns;
using BlazorMarkDownAppJwt.Server.Entities;
using Microsoft.EntityFrameworkCore;
using BlazorMarkDownAppJwt.Server.Helpers;

var builder = WebApplication.CreateBuilder(args);



var dbPath = Path.Combine(builder.Environment.ContentRootPath, "Datas\\test_fullstack.db");

builder.Services.AddDbContext<DataBaseContext>(options => options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = "domain.com",
        ValidateIssuer = true,
        ValidIssuer = "domain.com",
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = JWTHelper.GetSecretKey()
    };
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();