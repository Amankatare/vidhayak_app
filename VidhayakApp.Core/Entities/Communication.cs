using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace VidhayakApp.Core.Entities
{
    public class Communication
    {
        [Key]
        [Column("CommunicationId")]
        public int CommunicationId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        
        //Foreign key
        public int Id{ get; set; }
        public Item Item { get; set; }  
        public int UserID { get; set; }
        public User User{ get; set; }

        

    }
}
