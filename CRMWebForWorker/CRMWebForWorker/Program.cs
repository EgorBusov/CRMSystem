using CRMWebForWorker.ApiInteraction.ApiRequests;
using CRMWebForWorker.ApiInteraction.ApiResources;

var builder = WebApplication.CreateBuilder(args);
var baseUrl = builder.Configuration.GetValue<string>("BaseUrl:Url");
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped(provider => new AccountRequests(provider.GetRequiredService<HttpClient>(), baseUrl));
builder.Services.AddScoped(provider => new BlogRequests(provider.GetRequiredService<HttpClient>(), baseUrl));
builder.Services.AddScoped(provider => new OrderRequests(provider.GetRequiredService<HttpClient>(), baseUrl));
builder.Services.AddScoped(provider => new ProjectRequests(provider.GetRequiredService<HttpClient>(), baseUrl));
builder.Services.AddScoped(provider => new ResourceRequests(provider.GetRequiredService<HttpClient>(), baseUrl));
builder.Services.AddScoped(provider => new ServiceRequests(provider.GetRequiredService<HttpClient>(), baseUrl));
builder.Services.AddSingleton(provider => new ApiResources(baseUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
