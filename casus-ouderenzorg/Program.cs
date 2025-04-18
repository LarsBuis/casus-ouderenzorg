using casus_ouderenzorg.DAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register Razor Pages
builder.Services.AddRazorPages();

// Register TaskDal so it can be injected into your Razor Page Model
// IMPORTANT: Do this before calling builder.Build(), using builder.Services (not app.Services).
builder.Services.AddScoped<TaskDal>(sp =>
    new TaskDal(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PatientDal>(sp =>
    new PatientDal(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<OrderDal>(sp =>
    new OrderDal(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<TransportDal>(sp =>
    new TransportDal(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline...
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();