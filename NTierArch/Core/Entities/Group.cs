using Core.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Group : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public bool IsDeleted { get; set; }

        public int Limit { get; set; }
        public Student Student { get; set; }
        public ICollection<Student> Students { get; set; }
    }
}
