using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Acme.Bookstore.Menus;
using OpenIddict.Abstractions;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Components.Web.LeptonXLiteTheme.Themes.LeptonXLite;
using Volo.Abp.AspNetCore.Components.WebAssembly.LeptonXLiteTheme;
using Volo.Abp.AspNetCore.Components.Web.Theming.Routing;
using Volo.Abp.OpenIddict;
using Volo.Abp.Identity.Blazor.WebAssembly;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.Blazor.WebAssembly;
using Volo.Abp.Autofac.WebAssembly;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.Blazor.WebAssembly;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Blazor.WebAssembly;
using Volo.Abp.SettingManagement.Blazor.WebAssembly;
using Volo.Abp.SettingManagement;
using Volo.Abp.UI.Navigation;

namespace Acme.Bookstore;

[DependsOn(
    typeof(BookstoreContractsModule),

    // ABP Framework packages
    typeof(AbpAutofacWebAssemblyModule),

    // Theme
    typeof(AbpAspNetCoreComponentsWebAssemblyLeptonXLiteThemeModule),

    // Account module packages
    typeof(AbpAccountHttpApiClientModule),

    // OpenIddict module packages
    typeof(AbpOpenIddictDomainSharedModule),

    // Identity module packages
    typeof(AbpIdentityBlazorWebAssemblyModule),
    typeof(AbpIdentityHttpApiClientModule),

    // Language Management module packages
    typeof(AbpTenantManagementBlazorWebAssemblyModule),
    typeof(AbpTenantManagementHttpApiClientModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementBlazorWebAssemblyModule),
    typeof(AbpPermissionManagementHttpApiClientModule),

    // Feature Management module packages
    typeof(AbpFeatureManagementBlazorWebAssemblyModule),
    typeof(AbpFeatureManagementHttpApiClientModule),

    // Setting Management module packages
    typeof(AbpSettingManagementHttpApiClientModule),
    typeof(AbpSettingManagementBlazorWebAssemblyModule)
)]
public class BookstoreBlazorModule : AbpModule
{
    public const string RemoteServiceName = "Default";

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var environment = context.Services.GetSingletonInstance<IWebAssemblyHostEnvironment>();
        var builder = context.Services.GetSingletonInstance<WebAssemblyHostBuilder>();

        ConfigureAuthentication(builder);
        ConfigureHttpClient(context, environment);
        ConfigureHttpClientProxies(context);
        ConfigureBlazorise(context);
        ConfigureRouter(context);
        ConfigureMenu(context);
        ConfigureAutoMapper(context);
    }

    private void ConfigureRouter(ServiceConfigurationContext context)
    {
        Configure<AbpRouterOptions>(options =>
        {
            options.AppAssembly = typeof(BookstoreBlazorModule).Assembly;
        });
    }

    private void ConfigureMenu(ServiceConfigurationContext context)
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new BookstoreMenuContributor(context.Services.GetConfiguration()));
        });
    }

    private void ConfigureBlazorise(ServiceConfigurationContext context)
    {
        context.Services
            .AddBootstrap5Providers()
            .AddFontAwesomeIcons();
    }

    private static void ConfigureAuthentication(WebAssemblyHostBuilder builder)
    {
        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("AuthServer", options.ProviderOptions);
            options.UserOptions.NameClaim = OpenIddictConstants.Claims.Name;
            options.UserOptions.RoleClaim = OpenIddictConstants.Claims.Role;

            options.ProviderOptions.DefaultScopes.Add("Bookstore");
            options.ProviderOptions.DefaultScopes.Add("roles");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.DefaultScopes.Add("phone");
        });
    }

    private static void ConfigureHttpClient(ServiceConfigurationContext context, IWebAssemblyHostEnvironment environment)
    {
        context.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(environment.BaseAddress)
        });
    }

    private void ConfigureHttpClientProxies(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(BookstoreContractsModule).Assembly,
            RemoteServiceName
        );
    }

    private void ConfigureAutoMapper(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<BookstoreBlazorModule>();
        });
    }
}
