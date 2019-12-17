using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyMVCAssignment.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        [Required()]
        public string Name { get; set; }
        public ClassStatus Status { get; set; }
        public enum ClassStatus
        {
            Active = 1,
            DeActive = 0,
            Saved = 2,
            Deleted = 3
        }

        public virtual ICollection<Student> Students { get; set; }
    }
}