using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using v_choice.Models;

namespace v_choice.Data
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
                new Genre{ Value="Драмма" },
                new Genre{ Value="Ужасы" },
                new Genre{ Value="Документальный" },
                new Genre{ Value="Мультфильм" },
                new Genre{ Value="Сериал" }
            };

            foreach (Genre g in genres)
                context.Genre.Add(g);

            context.SaveChanges();

            Film f = new Film { Title = "Default Film", Year = 2021, Description = "Some description" };
            context.Film.Add(f);

            context.SaveChanges();
        }
    }
}
