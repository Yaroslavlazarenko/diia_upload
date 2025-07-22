using DiiaDocsUploader.Contexts;
using DiiaDocsUploader.DIExtensions;
using DiiaDocsUploader.Models.FileSystem;
using DiiaDocsUploader.Services;
using DiiaDocsUploader.Services.Auth;
using DiiaDocsUploader.Storage;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace DiiaDocsUploader;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.AddDiiaCredentials();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<ISessionTokenService, SandboxSessionTokenService>();

        builder.Services.AddTransient<BranchService>();
        builder.Services.AddTransient<OfferService>();
        builder.Services.AddTransient<DeepLinkService>();
        
        builder.Services.Configure<FileSystemStorageOptions>(
            builder.Configuration.GetSection("FileSystemStorageOptions"));
        
        builder.Services.AddDbContext<DiiaDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        
        builder.Services.AddScoped<IStorageService, FileSystemStorage>();

        builder.Services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy => policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        app.UseCors();

        app.UseAuthorization();

        app.MapGet("/", () => "ok");

        app.MapControllers();

        app.Run();
    }
}