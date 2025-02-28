using Devameet;
using Devameet.Models;
using Devameet.Repository;
using Devameet.Repository.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectstring = builder.Configuration.GetConnectionString("DefaultConnectString"); //Variavel de instancia da Connection String
builder.Services.AddDbContext<DevameetContext>(option => option.UseSqlServer(connectstring)); //Acesso ao banco de dados sempre que iniciado o programa.

builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>(); //Dependency injection
builder.Services.AddScoped<IMeetRepository, MeetRepositoryImpl>();
builder.Services.AddScoped<IMeetObjectsRepository, MeetObjectsRepositoryImpl>();
builder.Services.AddScoped<IRoomRepository, RoomRepositoryImpl>();


var jwtsettings = builder.Configuration.GetRequiredSection("JWT").Get<JWTKey>();

var secretKet = Encoding.ASCII.GetBytes(jwtsettings.SecretKey); // configuração do token
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(authentication =>
{
    authentication.RequireHttpsMetadata = false;
    authentication.SaveToken = true;
    authentication.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKet),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
