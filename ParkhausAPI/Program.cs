using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var modelbuilder = new ODataConventionModelBuilder();
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
modelbuilder.EntitySet<ParkhausAPI.Models.Tickets>("Tickets");
modelbuilder.EntityType<ParkhausAPI.Models.Tickets>().HasKey(t => t.TicketID);
builder.Services.AddDbContext<ParkhausAPI.Context.ParkhausContext>(options =>
    options.UseSqlServer(connectionString));
var edmModel = modelbuilder.GetEdmModel();


builder.Services.AddControllers().AddOData(options => {
    options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents("", edmModel);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Parkhaus API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Parkhaus API");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.MapControllers();

app.Run();