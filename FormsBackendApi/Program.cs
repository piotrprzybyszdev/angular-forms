using FluentValidation;
using FormsBackendApi;
using FormsBackendBusiness;
using FormsBackendInfrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite("FileName=forms.db", b => b.MigrationsAssembly("FormsBackendApi")));

builder.Services.AddAutoMapper(typeof(DtoEntityMapperProfile));
IdentityConfiguration.Configure(builder.Services);
ServiceConfiguration.Configure(builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UsePathBase("/api");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

ValidatorOptions.Global.LanguageManager.Enabled = false;
app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
