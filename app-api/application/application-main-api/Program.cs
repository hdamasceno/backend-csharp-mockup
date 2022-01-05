using application_infra_crossCutting.Filter;
using application_infra_crossCutting.InversionOfControl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMvc(config =>
    {
        config.EnableEndpointRouting = false;
        config.Filters.Add<NotificationFilter>();
    });
builder.Services.AddSwaggerGen();
builder.Services.AddMySqlDependency(builder.Configuration);
builder.Services.AddServiceDependency(builder.Configuration);
builder.Services.AddNotificationDependency();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();