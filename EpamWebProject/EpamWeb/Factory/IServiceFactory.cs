﻿using EpamWeb.Services;
using Microsoft.Playwright;

namespace EpamWeb.Factory
{
    public interface IServiceFactory
    {
        IHomepageService CreateHomepageService(IPage page);
        IInsightsPageService CreateInsightsPageService(IPage page);
    }
}