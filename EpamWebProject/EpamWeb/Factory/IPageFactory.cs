﻿using EpamWeb.Pages;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public interface IPageFactory
    {
        Task<IPage> GetOrCreatePageAsync(string testName);
        Task RemovePageAsync(string testName);
        IHomepage CreateHomepage(IPage page);
        IInsightsPage CreateInsightsPage(IPage page);
    }
}
