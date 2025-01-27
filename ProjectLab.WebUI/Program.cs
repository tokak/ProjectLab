using BundlerMinifier.TagHelpers; // CSS ve JavaScript dosyalarýný paketleme (bundling) ve küçültme (minification) iþlemleri için gerekli kütüphane.
using NToastNotify;
using WebMarkupMin.AspNetCore3; // HTML ve diðer çýktýlarý küçültmek (minify) için kullanýlan bir kütüphane.

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration.AddJsonFile("appsettings.json",optional:false).Build();


/// 
var services = builder.Services;
{
    services.AddLogging(options =>
    {
        options.AddDebug();
        options.AddConsole();
        options.AddSeq(configuration.GetSection("Seq"));
    }); // Uygulama günlüðü (log) ayarlarýný yapýlandýrýr.

    // MVC mimarisi için gerekli olan Controller ve View hizmetlerini ekler.
    services.AddControllersWithViews().AddNToastNotifyToastr(new NToastNotify.ToastrOptions
    {
        CloseButton= true,
        PositionClass = ToastPositions.BottomRight,
    }).AddRazorRuntimeCompilation();

    // CSS ve JavaScript dosyalarýný optimize etmek için paketleme ve küçültme ayarlarý yapýlandýrýlýyor.
    services.AddBundles(options =>
    {
        options.AppendVersion = true; // Her dosya için versiyon ekler (ör. "?v=1.0"), tarayýcý önbelleðini kontrol etmek için.
        options.UseBundles = true;   // Dosyalarýn paketlenmesini etkinleþtirir.
        options.UseMinifiedFiles = true; // Minify edilmiþ (küçültülmüþ) dosyalarýn kullanýlmasýný saðlar.
    });

    // HTML küçültme (minification) ve sýkýþtýrma (compression) ayarlarýný yapýlandýrýr.
    services.AddWebMarkupMin(options =>
    {
        options.AllowMinificationInDevelopmentEnvironment = false; // Geliþtirme ortamýnda HTML küçültme iþlemini devre dýþý býrakýr.
        options.AllowCompressionInDevelopmentEnvironment = false; // Geliþtirme ortamýnda HTML sýkýþtýrmayý devre dýþý býrakýr.
    }).AddHtmlMinification(); // HTML dosyalarýnýn küçültülmesini saðlar.
}

/// 
var app = builder.Build();
{
    // Uygulama geliþtirme ortamýnda deðilse, hata sayfasý ve güvenlik ayarlarý etkinleþtirilir.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error"); // Hatalar için özel bir hata sayfasý kullanýlýr.
        app.UseHsts(); // Tarayýcýya HSTS baþlýðý göndererek HTTPS baðlantýlarýný zorlar.
    }

    app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'e yönlendirir.
    app.UseStaticFiles(); // Statik dosyalarýn (ör. CSS, JS, görüntüler) sunulmasýný saðlar.

    app.UseRouting(); // URL yönlendirme iþlemlerini etkinleþtirir.
    app.UseWebMarkupMin(); // HTML sýkýþtýrma ve küçültme iþlemlerini uygular.
    app.UseAuthorization(); // Yetkilendirme mekanizmasýný etkinleþtirir.
    app.UseNToastNotify(); // Toastr bildirimlerini kullanmak için gerekli ayarlarý yapar.
    // Varsayýlan rota (URL þablonu) ayarlanýr.
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run(); // Uygulamayý çalýþtýrýr.
}
