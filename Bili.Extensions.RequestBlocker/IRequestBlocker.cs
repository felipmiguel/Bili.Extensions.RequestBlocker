using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bili.Extensions.RequestBlocker
{
    public interface IRequestBlocker
    {
        Task<bool> AllowRequestAsync(HttpContext context);
    }
}
