using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpamWeb.PageServices
{
    public interface IHomepageService : IPageService
    {
        Task ClickHamburgerMenuAsync();
        Task<List<string>> GetHamburgerMenuListItemsAsync();
    }
}
