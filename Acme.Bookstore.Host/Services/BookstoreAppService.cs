using Volo.Abp.Application.Services;
using Acme.Bookstore.Localization;

namespace Acme.Bookstore.Services;

/* Inherit your application services from this class. */
public abstract class BookstoreAppService : ApplicationService
{
    protected BookstoreAppService()
    {
        LocalizationResource = typeof(BookstoreResource);
    }
}