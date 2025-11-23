using Amazon.S3;
using Amazon.Runtime;
using EDI837.src.Models;
using EDI837.src.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AppDataContext to services
builder.Services.AddDbContext<AppDataContext>( options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add dependency injected services
builder.Services.AddScoped<IEdi837FileService, Edi837FileService>();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddScoped<IEdiParserService, EdiParserService>();
builder.Services.AddScoped<IS3FileService, S3FileService>();

// Bind AWS options from configuration
var awsConfig = builder.Configuration.GetSection("AWS");
var credentials = new BasicAWSCredentials(
    awsConfig["AccessKey"],
    awsConfig["SecretKey"]
);

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = new AmazonS3Config
    {
        ServiceURL = awsConfig["ServiceURL"], // Points to Moto
        ForcePathStyle = true,                // REQUIRED for Moto
        AuthenticationRegion = awsConfig["Region"]
    };

    return new AmazonS3Client(credentials, config);
});

//Set EDI Key
EdiFabric.SerialKey.Set(builder.Configuration["EdiFabricSerialKey"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
