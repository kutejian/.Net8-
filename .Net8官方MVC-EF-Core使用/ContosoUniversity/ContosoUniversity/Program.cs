using ContosoUniversity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<SchoolContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();


CreateDbIfNotExists(app.Services);




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//如果数据库不存在就创建数据库
 static void CreateDbIfNotExists(IServiceProvider host)
{
    using (var scope = host.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            //准备创建的数据库 里面设置了表的对应关系
            var context = services.GetRequiredService<SchoolContext>();
            //创建数据库
            context.Database.EnsureCreated();
            //在表中创建对应的内容
            DbInitializer.Initialize(context);
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError("创建DB成功");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "创建DB时出错.");
        }
    }
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
