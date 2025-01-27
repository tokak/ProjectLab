using BundlerMinifier.TagHelpers; // CSS ve JavaScript dosyalar�n� paketleme (bundling) ve k���ltme (minification) i�lemleri i�in gerekli k�t�phane.
using NToastNotify;
using WebMarkupMin.AspNetCore3; // HTML ve di�er ��kt�lar� k���ltmek (minify) i�in kullan�lan bir k�t�phane.

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
    }); // Uygulama g�nl��� (log) ayarlar�n� yap�land�r�r.

    // MVC mimarisi i�in gerekli olan Controller ve View hizmetlerini ekler.
    services.AddControllersWithViews().AddNToastNotifyToastr(new NToastNotify.ToastrOptions
    {
        CloseButton= true,
        PositionClass = ToastPositions.BottomRight,
    }).AddRazorRuntimeCompilation();

    // CSS ve JavaScript dosyalar�n� optimize etmek i�in paketleme ve k���ltme ayarlar� yap�land�r�l�yor.
    services.AddBundles(options =>
    {
        options.AppendVersion = true; // Her dosya i�in versiyon ekler (�r. "?v=1.0"), taray�c� �nbelle�ini kontrol etmek i�in.
        options.UseBundles = true;   // Dosyalar�n paketlenmesini etkinle�tirir.
        options.UseMinifiedFiles = true; // Minify edilmi� (k���lt�lm��) dosyalar�n kullan�lmas�n� sa�lar.
    });

    // HTML k���ltme (minification) ve s�k��t�rma (compression) ayarlar�n� yap�land�r�r.
    services.AddWebMarkupMin(options =>
    {
        options.AllowMinificationInDevelopmentEnvironment = false; // Geli�tirme ortam�nda HTML k���ltme i�lemini devre d��� b�rak�r.
        options.AllowCompressionInDevelopmentEnvironment = false; // Geli�tirme ortam�nda HTML s�k��t�rmay� devre d��� b�rak�r.
    }).AddHtmlMinification(); // HTML dosyalar�n�n k���lt�lmesini sa�lar.
}

/// 
var app = builder.Build();
{
    // Uygulama geli�tirme ortam�nda de�ilse, hata sayfas� ve g�venlik ayarlar� etkinle�tirilir.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error"); // Hatalar i�in �zel bir hata sayfas� kullan�l�r.
        app.UseHsts(); // Taray�c�ya HSTS ba�l��� g�ndererek HTTPS ba�lant�lar�n� zorlar.
    }

    app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'e y�nlendirir.
    app.UseStaticFiles(); // Statik dosyalar�n (�r. CSS, JS, g�r�nt�ler) sunulmas�n� sa�lar.

    app.UseRouting(); // URL y�nlendirme i�lemlerini etkinle�tirir.
    app.UseWebMarkupMin(); // HTML s�k��t�rma ve k���ltme i�lemlerini uygular.
    app.UseAuthorization(); // Yetkilendirme mekanizmas�n� etkinle�tirir.
    app.UseNToastNotify(); // Toastr bildirimlerini kullanmak i�in gerekli ayarlar� yapar.
    // Varsay�lan rota (URL �ablonu) ayarlan�r.
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run(); // Uygulamay� �al��t�r�r.
}
