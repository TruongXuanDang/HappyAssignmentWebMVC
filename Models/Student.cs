using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyMVCAssignment.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required()]
        public string Name { get; set; }
        [Required()]
        public string Code { get; set; }
        [Required()]
        public string Phone { get; set; }
        [Required()]
        public string Email { get; set; }
        public StudentStatus Status { get; set; }
        public enum StudentStatus
        {
            Active = 1,
            DeActive = 0,
            Saved = 2,
            Deleted = 3
        }

        public int ClassroomId { get; set; }
        public virtual Classroom Classroom { get; set; }
        public virtual ICollection<LateEvent> LateEvents { get; set; }
    }
}