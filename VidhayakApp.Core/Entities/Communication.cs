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
        [Column("DetailId")]
        public int CommunicationID { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        
        //Foreign key
        public int ItemID { get; set; }
        public Item Item { get; set; }  
        public int UserID { get; set; }
        public User User{ get; set; }

        

    }
}
