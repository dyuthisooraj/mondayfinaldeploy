﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonApparelsDomain.Entities
{
    public class AdminLogin
    {
        public int Id { get; set; } = 0;


        //[Required(ErrorMessage = "Username is required")]
        [Column(TypeName = "varchar")]
        [StringLength(50, ErrorMessage = "{0} must contain atleast {2} characters.",MinimumLength = 3)]
        public string? UserName { get; set; }

        //[Required(ErrorMessage = "Please Enter Password")]
        [Column(TypeName = "varchar")]
        [StringLength(10, ErrorMessage = "{0} must contain atleast {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]


        public string? Password { get; set; }
    }
}
