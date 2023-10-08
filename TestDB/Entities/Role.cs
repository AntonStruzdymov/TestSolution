using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestDB.Entities
{
    public class Role
    {
        public Role() 
        {
            this.Users = new List<User>();
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
        
    }
}
