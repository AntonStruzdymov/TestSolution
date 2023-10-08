using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TestDB.Entities
{
    public class User
    {
        public User() 
        {
            this.Roles = new List<Role>();
        }
        public int ID {get; set;}
        public string Name { get; set;}
        public int Age { get; set;}
        public string Email { get; set;}
        public virtual List<Role> Roles { get; set;}
        

    }
}
