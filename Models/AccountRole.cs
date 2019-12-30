using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HappyMVCAssignment.Models
{
    public class AccountRole:IdentityRole
    {
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}