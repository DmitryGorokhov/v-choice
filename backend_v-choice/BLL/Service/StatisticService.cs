using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using DAL.Enum;
using DAL.Interface;
using DAL.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Service
{
    public class StatisticService : IStatisticService
    {
        private readonly ILogger _logger;
        private readonly IStatisticRepository _statisticRepository;
        private readonly IPaginationRepository _paginationRepository;
        private readonly IMapper _mapper;

        public StatisticService(ILogger<StatisticService> logger, IStatisticRepository sr, IPaginationRepository pr, IMapper mapper)
        {
            _statisticRepository = sr;
            _paginationRepository = pr;
            _logger = logger;
            _mapper = mapper;
        }

        public string ExportStatisticAsync(ExportStatisticQuery query, IWebHostEnvironment _appEnvironment)
        {
            _logger.LogInformation("Starting export statistic");

            _logger.LogInformation("Starting get film statistic");
            var filmStatistic = LoadFilmData(query.FilmSortingType).Select(e => _mapper.FilmModelToStatisticDTO(e));

            _logger.LogInformation("Starting get genres data");
            var genreStatistic = LoadGenreData(query.GenreSortingType).Select(e => _mapper.GenreModelToStatisticDTO(e));

            _logger.LogInformation("Starting get general statistic");
            _logger.LogInformation("Call GetGeneralStatisticAsync");
            GeneralStatistic generalStatistic = _statisticRepository.GetGeneralStatistic();

            _logger.LogInformation("Starting write statistic into pdf");

            string fileName = "Statistic_" + DateTime.Now.ToString("yymmssfff") + ".pdf";
            string path = Path.Combine("files", fileName);

            var doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(Path.Combine(_appEnvironment.WebRootPath, path), FileMode.Create));
            doc.Open();

            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);

            Phrase p = new Phrase($"Статистика ViewerChoice от {DateTime.Now.ToString("f")}", font);
            doc.Add(p);

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Общая статистика", font));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Общее количество фильмов", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.FilmsTotal.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Количество фильмов с рейтингом", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.FilmsRated.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Количество фильмов без рейтинга", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.FilmsNotRated.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Количество фильмов с комментариями", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.FilmsCommented.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Количество фильмов без комментариев", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.FilmsNotCommented.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Наименьший год создания фильма", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.MinYear.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Наибольший год создания фильма", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.MaxYear.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Общее количество комментариев", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.CommentsTotal.ToString(), font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("Наибольшее количество комментариев к фильму", font)) { Border = 0 };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(generalStatistic.CommentsMax.ToString(), font)) { Border = 0 };
            table.AddCell(cell);

            doc.Add(table);

            table = new PdfPTable(3);

            cell = new PdfPCell(new Phrase("Статистика по жанрам", font));
            cell.Colspan = 3;
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            table.AddCell(cell);

            table.AddCell(new Phrase("Название", font));
            table.AddCell(new Phrase("Количество запросов для фильтрации", font));
            table.AddCell(new Phrase("Количество фильмов", font));

            foreach (var g in genreStatistic)
            {
                table.AddCell(new Phrase(g.Value, font));
                table.AddCell(new Phrase(g.Requested.ToString(), font));
                table.AddCell(new Phrase(g.CountFilms.ToString(), font));
            }

            doc.Add(table);

            table = new PdfPTable(6);
            
            cell = new PdfPCell(new Phrase("Статистика по фильмам", font));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            cell.Border = 0;
            table.AddCell(cell);

            table.AddCell(new Phrase("Название", font));
            table.AddCell(new Phrase("Количество просмотров страницы", font));
            table.AddCell(new Phrase("Рейтинг", font));
            table.AddCell(new Phrase("Количество оценок", font));
            table.AddCell(new Phrase("Количество комментариев", font));
            table.AddCell(new Phrase("Количество в Избранном", font));

            foreach (var f in filmStatistic)
            {
                table.AddCell(new Phrase(f.Title, font));
                table.AddCell(new Phrase(f.Requested.ToString(), font));
                table.AddCell(new Phrase(f.AvRate.ToString(), font));
                table.AddCell(new Phrase(f.CountRate.ToString(), font));
                table.AddCell(new Phrase(f.CountComment.ToString(), font));
                table.AddCell(new Phrase(f.CountFavorite.ToString(), font));
            }

            doc.Add(table);

            doc.Close();

            return path;
        }

        public async Task<PaginationDTO<FilmStatisticDTO>> GetFilmStatisticAsync(FilmStaticticQuery query)
        {
            _logger.LogInformation("Starting get film statistic");
            try
            {
                IQueryable<Film> answer = LoadFilmData(query.SortingType);

                _logger.LogInformation("Call SplitByPagesAsync");
                (int total, IQueryable<Film> items) = await _paginationRepository.SplitByPagesAsync(answer, query.PageNumber, query.OnPageCount);

                _logger.LogInformation("Get film statistic has been done. Prepare DTO to return.");

                return new PaginationDTO<FilmStatisticDTO>(query)
                {
                    TotalCount = total,
                    Items = items.Select(e => _mapper.FilmModelToStatisticDTO(e)).ToList(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Get film statistic has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public GeneralStatistic GetGeneralStatistic()
        {
            _logger.LogInformation("Starting get general statistic");
            try
            {
                _logger.LogInformation("Call GetGeneralStatisticAsync");

                GeneralStatistic res = _statisticRepository.GetGeneralStatistic();

                _logger.LogInformation("Get general statistic has been done.");

                return res;
            }
            catch (Exception e)
            {
                _logger.LogError($"Get general statistic has thrown an exception: {e.Message}.");

                return null;
            }
        }

        public async Task<PaginationDTO<GenreStatisticDTO>> GetGenreStatisticAsync(GenreStaticticQuery query)
        {
            _logger.LogInformation("Starting get genre statistic");
            try
            {
                IQueryable<Genre> answer = LoadGenreData(query.SortingType);

                _logger.LogInformation("Call SplitByPagesAsync");
                (int total, IQueryable<Genre> items) = await _paginationRepository.SplitByPagesAsync(answer, query.PageNumber, query.OnPageCount);

                _logger.LogInformation("Get genre statistic has been done. Prepare DTO to return.");

                return new PaginationDTO<GenreStatisticDTO>(query)
                {
                    TotalCount = total,
                    Items = items.Select(e => _mapper.GenreModelToStatisticDTO(e)).ToList(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError($"Get genre statistic has thrown an exception: {e.Message}.");

                return null;
            }
        }

        private IQueryable<Film> LoadFilmData(FilmStatisticSortingType fst)
        {
            _logger.LogInformation("Call GetFilmStatistic method");

            return fst switch
            {
                FilmStatisticSortingType.Requested => _statisticRepository.GetFilmStatisticByRequested(),
                FilmStatisticSortingType.Rate => _statisticRepository.GetFilmStatisticByRate(),
                FilmStatisticSortingType.CountRate => _statisticRepository.GetFilmStatisticByCountRate(),
                FilmStatisticSortingType.Comments => _statisticRepository.GetFilmStatisticByComments(),
                FilmStatisticSortingType.Favorites => _statisticRepository.GetFilmStatisticByFavorites(),
                _ => _statisticRepository.GetFilmStatisticByRequested()
            };
        }

        private IQueryable<Genre> LoadGenreData(GenreStatisticSortingType gst)
        {
            _logger.LogInformation("Call GetGenreStatistic method");

            return gst switch
            {
                GenreStatisticSortingType.Films => _statisticRepository.GetGenreStatisticByFilms(),
                GenreStatisticSortingType.Requested => _statisticRepository.GetGenreStatisticByRequested(),
                _ => _statisticRepository.GetGenreStatisticByFilms()
            };
        }
    }
}
