using ConfigChecker.Abstractions;
using ConfigChecker.DTOs;
using ConfigChecker.Endpoints;
using ConfigChecker.Persistance;
using ConfigChecker.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace ConfigChecker
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddAuthorization();
      builder.Services.AddAntiforgery();

      // Add persistence via DbContext.
      builder.Services.AddDbContext<FindingsDbContext>(options =>
      {
        var connectionString = builder.Configuration.GetConnectionString(nameof(FindingsDbContext))
          ?? throw new ArgumentException("Connection string not set in appSettings.json.");

        options.UseSqlite(connectionString);
      });

      // Configure services.
      builder.Services.AddOptions<FileUploadServiceOptions>()
        .BindConfiguration(nameof(FileUploadServiceOptions));

      // Add worker services.
      builder.Services.AddHostedService<ConfigurationAnalyzer>();
      builder.Services.AddScoped<IFileUploadService, FileUploadService>();
      builder.Services.AddScoped<IReportStore, ReportStore>();

      // Use a channel for async communication between app sections.
      builder.Services.AddSingleton(Channel.CreateUnbounded<ProcessingRequestDto>());

      var app = builder.Build();

      app.UseAntiforgery();
      app.UseAuthorization();

      if (app.Environment.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHttpsRedirection();
      }

      // Routes
      app.MapConfigCheckerEndpoints();

      app.Run();
    }
  }
}
