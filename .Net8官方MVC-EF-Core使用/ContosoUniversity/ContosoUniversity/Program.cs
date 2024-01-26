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
//������ݿⲻ���ھʹ������ݿ�
 static void CreateDbIfNotExists(IServiceProvider host)
{
    using (var scope = host.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            //׼�����������ݿ� ���������˱�Ķ�Ӧ��ϵ
            var context = services.GetRequiredService<SchoolContext>();
            //�������ݿ�
            context.Database.EnsureCreated();
            //�ڱ��д�����Ӧ������
            DbInitializer.Initialize(context);
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError("����DB�ɹ�");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "����DBʱ����.");
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
