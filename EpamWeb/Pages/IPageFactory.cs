using EpamWeb.PageServices;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpamWeb.Pages
{
    public interface IPageFactory
    {
        IHomepageService CreateHomepageService(IPage page);
        IInsightsPageService CreateInsightsPageService(IPage page);

    }
}
