using Api.Configuration;
using Api.Configuration.Jobs;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddHangfireConfiguration(builder.Configuration);
builder.Services.AddAutoMapper();
builder.Services.AddDependenciesInjection();


var app = builder.Build();
app.InitializeDbTestDataAsync();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.UseHangfireDashboard();
app.MapHangfireDashboard("/hangfire");
//RecurringJob.AddOrUpdate(() => Console.WriteLine("Hello from hangfire"), "* * * * *");

app.MapRazorPages();
app.RunNotExecutedJobs();

app.Run();
