using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Chess.Databases;
using Chess.Services;
using dotenv.net;

DotEnv.Load(new DotEnvOptions(
    envFilePaths: new string[] { ".env.sample", ".env", }
));

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(typeof(AuthService), new AuthService());
builder.Services.AddSingleton(typeof(MainContext), new MainContext());

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            ValidAudience = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY")!)),
        };
    });

builder.Services.AddCors();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
        .WithOrigins(Environment.GetEnvironmentVariable("CORS_ORIGINS")!)
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
