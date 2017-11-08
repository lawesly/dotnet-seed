﻿using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Modules.Site
{
    public interface ISiteInfo
    {
        string SiteName { get; set; }

        string BaseUrl { get; set; }

        string SuperUser { get; set; }

        //string SiteSalt { get; set; }

        //string Culture { get; set; }

        //string Calendar { get; set; }

        //string TimeZone { get; set; }

        //ResourceDebugMode ResourceDebugMode { get; set; }

        //bool UseCdn { get; set; }

        //int PageSize { get; set; }

        //int MaxPageSize { get; set; }

        //int MaxPagedCount { get; set; }

        //RouteValueDictionary HomeRoute { get; set; }
    }
}