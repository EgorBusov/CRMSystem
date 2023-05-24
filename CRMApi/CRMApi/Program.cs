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
        string connection = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("������ ����������� �� �������");
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


        builder.Services.AddAuthentication(options => //��������� �������������� � DI
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => //��������� ��������� �������������� � ������� JWT Bearer token
        {
            options.RequireHttpsMetadata = false; //������ HTTPS-���������� ��� �������� ������, ���� true(HTTPS ���������� � ����� ��������� ��� HTTP)
            options.SaveToken = true; //��������� ����� � ���������, ��������� ������������ � ����������
            options.TokenValidationParameters = new TokenValidationParameters //������� ��������� ������
            {
                ValidateIssuerSigningKey = true, //�������� ������� ������ ��� ��������������
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SecretKey"))),//��������� ���� ��� �������
                ValidateIssuer = false, //�������� �������� ������
                ValidateAudience = false //�������� ��������� ������
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