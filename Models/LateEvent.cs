using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HappyMVCAssignment.Models
{
    public class LateEvent
    {
        public int Id { get; set; }

        [Required()]
        public DateTime LateDate { get; set; }

        [Required()]
        public Type LateType { get; set; }

        public enum Type
        {
            Push = 0,
            Money= 1
        }

        public double LateMoney { get; set; }
        public int PushCount { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

    }
}