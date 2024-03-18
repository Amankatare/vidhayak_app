using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidhayakApp.Core.Entities
{
    public class Ward
    {
        [Key]
        [Column("WardId")]
        public int WardId { get; set; }
        public string WardName { get; set; }
    }
}
