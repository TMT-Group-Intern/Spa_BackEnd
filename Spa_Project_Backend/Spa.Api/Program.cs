using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Spa.Application.Commands;
using Spa.Domain.IRepository;
using Spa.Domain.IService;
using Spa.Domain.Service;
using Spa.Infrastructure;
using Spa.Infrastructures;





var builder = WebApplication.CreateBuilder(args); // cấu hình service và midleware cần thiết

// Add services to the container.


builder.Services.AddControllers();  //xử lí request http và phản hồi dựa trên controller
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //add swagger để test api 

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly));
//register Configuration
ConfigurationManager configuration = builder.Configuration;


//Add Database Service
builder.Services.AddDbContext<SpaDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("Spa.Infrastructure")));

//Register services
//Customer
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
//Apppointment
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
//Service
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
//Register services



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();  //sử dụng swagger
}
//allow accept api to font-end

app.UseCors(op => op.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHttpsRedirection();  //thêm middleware để chuyển http sang https để thêm bảo mật

app.UseAuthorization();  //middleware xử lí ủy uyền 

app.MapControllers();  //định tuyến controller 

app.Run();  // xử lí yêu cầu http đến server
