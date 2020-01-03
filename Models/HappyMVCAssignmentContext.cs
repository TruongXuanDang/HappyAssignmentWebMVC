﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HappyMVCAssignment.Models
{
    public class HappyMVCAssignmentContext : IdentityDbContext<Account>
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public HappyMVCAssignmentContext() : base("name=HappyMVCAssignmentContext")
        {
        }

        public static HappyMVCAssignmentContext Create()
        {
            return new HappyMVCAssignmentContext();
        }

        public System.Data.Entity.DbSet<HappyMVCAssignment.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<HappyMVCAssignment.Models.Classroom> Classrooms { get; set; }

        public System.Data.Entity.DbSet<HappyMVCAssignment.Models.LateEvent> LateEvents { get; set; }

        public System.Data.Entity.DbSet<HappyMVCAssignment.Models.LateSetting> LateSettings { get; set; }
    }
}
