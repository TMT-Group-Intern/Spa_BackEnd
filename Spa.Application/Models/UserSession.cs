using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Models
{
    public record class UserSession(string? Code, string? Email, string? Name, string? Role);
}
