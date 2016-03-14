﻿using Microsoft.AspNet.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace OfficeDevPnP.Core.Framework.Authentication
{
    public class SharePointAuthenticationOptions : AuthenticationOptions
    {
        public SharePointAuthenticationOptions() : base()
        {
            //Set automatic challenge to default
            AutomaticAuthenticate = true;
            AutomaticChallenge = true;
            AuthenticationScheme = SharePointAuthenticationDefaults.AuthenticationScheme;
        }

        /// <summary>
        /// Gets or sets if HTTPS is required for the metadata address or authority.
        /// The default is true. This should be disabled only in development environments.
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;
        public IConfiguration Configuration { get; set; }
    }
}
