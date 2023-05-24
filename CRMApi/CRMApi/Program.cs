using CRMApi.Context;
using CRMApi.Interfaces;
using CRMApi.Services;
using CRMApi.Services.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Строка подключения не найдена");
        builder.Services.AddDbContext<CRMSystemContext>(options => options.UseSqlServer(connection));

        builder.Services.AddScoped<IJWT, JWT>();
        builder.Services.AddScoped<IAccountData, AccountData>();
        builder.Services.AddScoped<IBlogData, BlogData>();
        builder.Services.AddScoped<IOrderData, OrderData>();
        builder.Services.AddScoped<IProjectData, ProjectData>();
        builder.Services.AddScoped<IServiceData, ServiceData>();
        builder.Services.AddScoped<IResourceData, ResourceData>();
        builder.Services.AddSingleton<ISenderMail, SenderMail>();
        builder.Services.AddSingleton<IPictureManager, PictureManager>();


        builder.Services.AddAuthentication(options => //добаление аутентификации в DI
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => //добавляет поддержку аутентификации с помощью JWT Bearer token
        {
            options.RequireHttpsMetadata = false; //только HTTPS-соединения при передаче токена, если true(HTTPS зашифрован и более безопасен чем HTTP)
            options.SaveToken = true; //сохраняет токен в контексте, позволяет использовать в дальнейшем
            options.TokenValidationParameters = new TokenValidationParameters //условия валидации токена
            {
                ValidateIssuerSigningKey = true, //проверка подписи токена при аутентификации
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SecretKey"))),//секретный ключ для подписи
                ValidateIssuer = false, //проверка издателя токена
                ValidateAudience = false //проверка аудитории токена
            };
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
    }
}