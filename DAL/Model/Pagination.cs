using System.Collections.Generic;

namespace DAL.Model
{
    public class Pagination<T>
    {
        public Pagination()
        {
            Items = new HashSet<T>();
        }

        public IEnumerable<T> Items {get; set;}
        public int TotalCount { get; set; }
    }
}
