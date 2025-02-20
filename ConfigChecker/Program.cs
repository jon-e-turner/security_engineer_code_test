using ConfigChecker.Abstractions;
using ConfigChecker.Dtos;
using ConfigChecker.Endpoints;
using ConfigChecker.Persistance;
using ConfigChecker.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
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

      // Configure JSON Serializer options.
      builder.Services.Configure<JsonSerializerOptions>(options => 
      {
        options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower));
        options.WriteIndented = true;
      });

      // Use a channel for async communication between app sections.
      builder.Services.AddSingleton(Channel.CreateUnbounded<ProcessingRequestDto>());
      builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<ProcessingRequestDto>>().Reader);
      builder.Services.AddSingleton(svc => svc.GetRequiredService<Channel<ProcessingRequestDto>>().Writer);

      // Configure services.
      builder.Services.AddOptions<FileUploadServiceOptions>()
        .BindConfiguration(nameof(FileUploadServiceOptions));

      // Add worker services.
      builder.Services.AddHostedService<ConfigurationAnalyzer>();
      builder.Services.AddScoped<IFileUploadService, FileUploadService>();
      builder.Services.AddScoped<IReportStore, ReportStore>();

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
