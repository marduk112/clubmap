﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClubMap.Startup))]
namespace ClubMap
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
