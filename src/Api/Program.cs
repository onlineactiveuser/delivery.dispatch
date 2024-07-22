using IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddHandler()
.AddRepository()
.AddSwaggerConfiguration()
.AddSettings(builder.Configuration)
.AddService()
.AddDatabaseConfiguration(builder.Configuration);


var app = builder
    .LogBuilder()
    .Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCorsConfiguration();
app.UseSwaggerConfiguration(app.Environment);
app.MapControllers();

app.Run();
