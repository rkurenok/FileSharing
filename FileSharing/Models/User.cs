using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FileSharing.Models
{
    public class User
    {
        //[ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        //public int Age { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}