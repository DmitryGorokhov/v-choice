using System.Linq;
using DAL.Model;

namespace DAL
{
    public class DbInitializer
    {
        public static void Init(DBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Genre.Any()) return;

            var genres = new Genre[]
            {
                new Genre{ Value="Комедия" },
                new Genre{ Value="Боевик" },
                new Genre{ Value="Триллер" },
                new Genre{ Value="Драма" },
                new Genre{ Value="Ужасы" },
                new Genre{ Value="Документальный" },
                new Genre{ Value="Мультфильм" },
                new Genre{ Value="Сериал" }
            };

            foreach (Genre g in genres)
                context.Genre.Add(g);

            context.SaveChanges();
        }
    }
}
