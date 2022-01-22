using MySql.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Supply
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public int ProductId { get; set; }
        public string DCount { get; set; }
        public DateTime Date { get; set; }
       
    }
}
