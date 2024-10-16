using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpamWeb.PageServices
{
    public interface IInsightsPageService : IPageService
    {
        Task ClickFindButtonAsync();
        Task InputTextInSearchFieldAsync();
        Task<string> GetSearchResultTextAsync();
    }
}
