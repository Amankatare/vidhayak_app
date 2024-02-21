using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhayakApp.Core.Entities
{
    public class Role { 

        [Key]
        [Column("RoleId")]
        public  int RoleId {get; set;}

        public string RoleName { get; set;}


    }
}
