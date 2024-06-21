using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Domain.Entities
{
    public class User:IdentityUser
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Role {get;set;}
        public string? Code { get;set;}
        public ICollection<Admin> Admin { get; set; }
        public ICollection<Employee> Employee { get; set; }
    }
}
