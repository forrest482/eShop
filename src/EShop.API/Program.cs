
var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// API Layer
builder.Services.AddWebApi(builder.Configuration);

// Add controllers with options
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

// Add Application Layer
builder.Services.AddApplication();

// Add Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);


// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<EShopDbContext>();

// Add Custom Exception Handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddProblemDetails();

// Add Response Compression
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
});

// Add HTTP Context Accessor
builder.Services.AddHttpContextAccessor();

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});



var app = builder.Build();

// Exception Handling
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "EShop API V1");
        options.RoutePrefix = string.Empty;
    });
}
else
{
    // Security headers
    app.UseHsts();
}

// Request Pipeline
app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseSerilogRequestLogging();

// Rate Limiting
app.UseRateLimiter();

// Map endpoints
app.MapControllers();

// Health Check endpoint
app.MapHealthChecks("/health");


if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
}


app.Run();


// Make the implicit Program class public so test projects can access it
public partial class Program { }