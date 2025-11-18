using Azure.AI.TextAnalytics;
using Azure.Identity;
using ChatInRealTime.Api;
using ChatInRealTime.Core;
using ChatInRealTime.Core.Mapper;
using ChatInRealTime.Infrastructure;
using ChatInRealTime.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Api ServiceExtensions
builder.Services.LowerCaseRoutes();
builder.Services.AddAutoFluentValidation();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithJwt();

//Core Services
builder.Services.AddCoreServices();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<AutoMapperUserProfile>();
    cfg.AddProfile<AutoMapperMessageProfile>();
});

var signalRConnection = builder.Configuration["Azure:SignalR:ConnectionString"];
var dbConnection = builder.Configuration["Azure:AzureDb:ConnectionString"];

//add SignalR
builder.Services.AddSignalR()
    .AddAzureSignalR(signalRConnection);


//add Text Analytics Client
builder.Services.AddAnalysTextAzureService(builder.Configuration);

//add database
builder.Services.AddAppDbContext(dbConnection!);

//add add repository 
builder.Services.AddRepository();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(
            "http://127.0.0.1:5500",
            "http://localhost:5500",  
            "https://localhost:7007"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedData.Initialize(serviceProvider);
}

app.UseCors("AllowAll");
app.MapHub<ChatHub>("/chatHub");
app.Run();
