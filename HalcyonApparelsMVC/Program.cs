using HalcyonApparelsMVC.Interfaces;
using HalcyonApparelsMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>

{

    options.LoginPath = "/LoginMVC/Login";

    options.AccessDeniedPath = "/denied";

    options.Events = new CookieAuthenticationEvents()

    {

        OnSigningIn = async context =>

        {

            /*var principal = context.Principal;

            if (principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))

            {

                if (principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value == model.password)

                {

                    var claimsIdentity = principal.Identity as ClaimsIdentity;

                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

                }

            }*/


            await Task.CompletedTask;

        },

        OnSignedIn = async content =>

        {

            await Task.CompletedTask;

        },

        OnValidatePrincipal = async content =>

        {

            await Task.CompletedTask;

        }

    };
} );

builder.Services.AddSingleton<IAuthenticate, SalesforceAuthenticate>();
builder.Services.AddSingleton<ISalesforceData, SalesforceData>();
builder.Services.AddSingleton<IMailSender, MailSender>();

builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromMinutes(10);
});
;

builder.Services.AddSingleton<FluentEmail.Core.Interfaces.ISender>(x =>
{
    return new FluentEmail.Smtp.SmtpSender(new Func<SmtpClient>(() =>
    {
        var client = new SmtpClient("smtp-relay.sendinblue.com", 587);
        client.SendCompleted += delegate (object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
                System.Diagnostics.Trace.TraceError($"Error sending email: {e.Error.Message}");
            if (sender is SmtpClient)
                (sender as SmtpClient).Dispose();
        };
        return client;
    }));
});

var from = builder.Configuration.GetSection("Mail")["From"];

var mailSender = builder.Configuration.GetSection("Send")["FromEmail"];
var mailPassword = builder.Configuration.GetSection("Send")["APIKey"];
var mailPort = Convert.ToInt32(builder.Configuration.GetSection("Send")["Port"]);


builder.Services
    .AddFluentEmail(mailSender, from)
    .AddRazorRenderer()
    .AddSmtpSender(new SmtpClient("smtp-relay.sendinblue.com")
    {
        UseDefaultCredentials = false,
        Port = mailPort,
        Credentials = new NetworkCredential(mailSender, mailPassword),
        EnableSsl = true,

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.Use(async (context, next) =>
{
    await next.Invoke();

    //After going down the pipeline check if we 404'd. 
    if (context.Response.StatusCode == StatusCodes.Status404NotFound)
    {
         context.Response.Redirect("/Home/Error");
    }
});

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSalesforceMiddleware();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
   pattern: "{controller=LoginMVC}/{action=Login}/{id?}");
//pattern: "{controller=Home}/{action=AccessoryView}/{id?}");


app.Run();
