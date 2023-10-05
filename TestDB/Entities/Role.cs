using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.Abstractions;

namespace TestDB.Entities
{
    public class Role : IBaseEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
