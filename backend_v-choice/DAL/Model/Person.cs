using System.Collections.Generic;

namespace DAL.Model
{
    public class Person
    {
        public Person()
        {
            Participations = new HashSet<Participation>();
        }
        
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhotoPath { get; set; }
        public virtual ICollection<Participation> Participations { get; set; }
    }
}
