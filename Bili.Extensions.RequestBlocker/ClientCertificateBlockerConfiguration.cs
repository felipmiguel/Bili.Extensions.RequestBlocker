﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bili.Extensions.RequestBlocker
{
    public class ClientCertificateBlockerConfiguration
    {
        public IEnumerable<string> AllowedThumbprints { get; set; }
    }
}
