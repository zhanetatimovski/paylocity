using Api.Configuration;
using Api.Extensions;
using Api.Repositories;
using Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PaylocityContext>(options => options.UseSqlServer(builder.Configuration["ConnectionString"]));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.EnableAnnotations();
	c.SwaggerDoc("v1", new OpenApiInfo
	{
		Version = "v1",
		Title = "Employee Benefit Cost Calculation Api",
		Description = "Api to support employee benefit cost calculations"
	});
});

var allowLocalhost = "allow localhost";
builder.Services.AddCors(options =>
{
	options.AddPolicy(allowLocalhost,
		policy => { policy.WithOrigins("http://localhost:3000", "http://localhost"); });
});

builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IDependentRepository, DependentRepository>();
builder.Services.AddTransient<IDependentService, DependentService>();
builder.Services.AddTransient<IPaymentCalculator, PaymentCalculator>();

builder.Services.AddOptions<BenefitsConfiguration>()
	.Bind(builder.Configuration.GetSection(BenefitsConfiguration.Section))
	.ValidateDataAnnotations();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.InitializeDatabase();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(allowLocalhost);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
