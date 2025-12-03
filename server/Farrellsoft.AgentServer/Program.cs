using A2A;
using A2A.AspnetCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var taskManager = new TaskManager();

app.UseHttpsRedirection();

app.Run();

