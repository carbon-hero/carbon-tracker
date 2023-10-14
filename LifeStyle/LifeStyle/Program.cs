using ElmahCore.Mvc;
using ElmahCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using StackExchange.Profiling.Storage;
using Core;
using LifeStyle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder
        .WithOrigins("*")
        .AllowAnyMethod()
        .SetIsOriginAllowed(origin => true)
        .AllowAnyHeader();
}));

_ = new Common(builder.Configuration);
// Register the Swagger generator, defining 1 or more Swagger documents
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
});

var apiVersion = "1.0.0";

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LifeStyle API", Version = "v1", Description = "Version: " + apiVersion });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
    var xmlFile = Path.ChangeExtension(typeof(Program).Assembly.Location, ".xml");
    c.IncludeXmlComments(xmlFile);

});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string contentRoot = builder.Services.BuildServiceProvider().GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>().ContentRootPath;
builder.Services.AddElmah<XmlFileErrorLog>(options =>
{
    options.LogPath = contentRoot + "log";
    options.ApplicationName = "LifeStyle";
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddMemoryCache();
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

    // (Optional) Control which SQL formatter to use, InlineFormatter is the default
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    options.TrackConnectionOpenClose = true;
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Auto;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LifeStyle API V1");
    c.DefaultModelsExpandDepth(-1);
});
app.UseHttpsRedirection();
app.UseMiniProfiler();
app.UseDefaultFiles();
if (!Directory.Exists(Constants.UploadFolder)) { Directory.CreateDirectory(Constants.UploadFolder); }
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    OnPrepareResponse = (ctx) =>
    {
        ctx.Context.Response.Headers["Access-Control-Allow-Origin"] = "*";
        ctx.Context.Response.Headers["Access-Control-Allow-Headers"] = "Origin, X-Requested-With, Content-Type, Accept";
    },
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), Constants.UploadFolder)),
    RequestPath = $"/{Constants.UploadFolder}"
});
app.UseElmah();
app.UseHttpContext();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("CorsPolicy"); // allow credentials
app.UseHttpsRedirection();
app.MapControllers();
app.Run();