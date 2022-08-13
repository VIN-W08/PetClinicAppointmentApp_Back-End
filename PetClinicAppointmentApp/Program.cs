using PetClinicAppointmentApp;
using Microsoft.EntityFrameworkCore;
using PetClinicAppointmentApp.Interfaces;
using PetClinicAppointmentApp.Services;

var builder = WebApplication.CreateBuilder(args);
var dbConnectionString = builder.Configuration.GetConnectionString("PetClinicAppointmentDbConnectionString");
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PetClinicAppointmentDbContext>(options =>
    options.UseSqlServer(dbConnectionString)
);

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPetClinicService, PetClinicService>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceScheduleService, ServiceScheduleService>();
builder.Services.AddScoped<IClinicScheduleService, ClinicScheduleService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

/*app.UseHttpsRedirection();*/

app.UseAuthorization();

app.MapControllers();

app.Run();
