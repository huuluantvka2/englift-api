using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.Word;
using EngLift.Model.Entities;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace EngLift.Service.Implements
{
    public class WordService : ServiceBase<WordService>, IWordService
    {
        private IDictionaryService _dictionaryService { get; set; }
        public WordService(ILogger<WordService> logger, IUnitOfWork unitOfWork, IDictionaryService dictionaryService) : base(logger, unitOfWork)
        {
            _dictionaryService = dictionaryService;
        }

        #region Admin
        public async Task<DataList<WordItemDTO>> GetAllWordByLessonId(WordRequest request)
        {
            _logger.LogInformation($"WordService -> GetAllWordByLessonId with request {JsonConvert.SerializeObject(request)} and LessonId {request.LessonId}");
            var result = new DataList<WordItemDTO>();
            IQueryable<Word> query = UnitOfWork.WordsRepo.GetAll()
                .Where(x =>
                (request.LessonId == null ? true : x.LessonWords.Any(y => y.LessonId == request.LessonId)) &&
                (String.IsNullOrEmpty(request.Search) ? true : (x.Content.ToLower().Contains(request.Search) || x.Trans.ToLower().Contains(request.Search))) &&
                (request.Active == null || x.Active == (bool)request.Active)
                );
            if (request.LessonId != null) query = query.Include(x => x.LessonWords);

            result.TotalRecord = query.Count();
            if (request.Sort != null)
            {
                switch (request.Sort)
                {
                    case 1: query = query.OrderBy(x => x.CreatedAt); break;
                    case 2: query = query.OrderByDescending(x => x.CreatedAt); break;
                    case 3: query = query.OrderBy(x => x.Content); break;
                    case 4: query = query.OrderByDescending(x => x.Content); break;
                    case 5: query = query.OrderBy(x => x.Trans); break;
                    case 6: query = query.OrderByDescending(x => x.Trans); break;
                    case 7: query = query.OrderBy(x => x.UpdatedAt); break;
                    case 8: query = query.OrderByDescending(x => x.UpdatedAt); break;
                }
            }
            else query = query.OrderByDescending(x => x.CreatedAt);

            var data = await query.Select(x => new WordItemDTO
            {
                Id = x.Id,
                Image = x.Image,
                Audio = x.Audio,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Content = x.Content,
                Example = x.Example,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,
                Active = x.Active,
                Phonetic = x.Phonetic,
                Trans = x.Trans,
                Position = x.Position
            }).Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            result.Items = data;
            _logger.LogInformation($"WordService -> GetAllWordByLessonId LessonId {request.LessonId} successfully");
            return result;
        }

        public async Task<WordItemDTO> GetWordDetail(Guid Id)
        {
            _logger.LogInformation($"WordService -> GetWordDetail with id {JsonConvert.SerializeObject(Id)}");
            var entity = await UnitOfWork.WordsRepo.GetAll().Where(x => x.Id == Id).Select(x => new WordItemDTO
            {
                Id = x.Id,
                Image = x.Image,
                Audio = x.Audio,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Content = x.Content,
                Example = x.Example,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,
                Active = x.Active,
                Phonetic = x.Phonetic,
                Trans = x.Trans,
                Position = x.Position
            }).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }

            _logger.LogInformation($"WordService -> GetWordDetail with request successfully");
            return entity;
        }


        public async Task<SingleId> CreateWord(WordCreateDTO dto)
        {
            _logger.LogInformation($"WordService -> CreateWord with dto {JsonConvert.SerializeObject(dto)}");

            var wordDB = UnitOfWork.WordsRepo.GetAll().Where(x => x.Content == dto.Content && x.Trans == dto.Trans).Include(x => x.LessonWords).FirstOrDefault();
            Word? word = null;
            if (wordDB == null)
            {
                var existLesson = UnitOfWork.LessonsRepo.GetAll().Any(x => x.Id == dto.LessonId);
                if (!existLesson)
                {
                    throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_COURSE);
                }

                word = new Word()
                {
                    Id = Guid.NewGuid(),
                    Image = dto.Image,
                    Content = dto.Content,
                    Example = dto.Example,
                    Active = dto.Active,
                    Phonetic = dto.Phonetic,
                    Trans = dto.Trans,
                    Position = dto.Position,
                };

                word.LessonWords = new List<LessonWord>() {
                    new LessonWord() { LessonId=dto.LessonId,WordId = word.Id}
                };

                var audio = await _dictionaryService.GetLinkAudio(dto.Content);
                if (!string.IsNullOrEmpty(audio)) word.Audio = audio;
                UnitOfWork.WordsRepo.Insert(word);

                await UnitOfWork.SaveChangesAsync();

                _logger.LogInformation($"WordService -> CreateWord successfully");
                return new SingleId() { Id = word.Id };
            }
            else
            {
                if (!wordDB.LessonWords.Any(x => x.LessonId == dto.LessonId))
                {
                    var newLessonWord = new LessonWord() { LessonId = dto.LessonId, WordId = wordDB.Id };
                    UnitOfWork.LessonWordRepo.Insert(newLessonWord);
                    await UnitOfWork.SaveChangesAsync();
                }
                return new SingleId() { Id = wordDB.Id };
            }


        }

        public async Task<SingleId> UpdateWord(Guid Id, WordUpdateDTO dto)
        {
            _logger.LogInformation($"WordService -> UpdateWord with dto {JsonConvert.SerializeObject(dto)}");
            var entity = UnitOfWork.WordsRepo.GetById(Id);
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }
            if (entity.Content != dto.Content)
            {
                var audio = await _dictionaryService.GetLinkAudio(dto.Content);
                if (!string.IsNullOrEmpty(audio)) entity.Audio = audio;
            }
            entity.Image = dto.Image;
            entity.Content = dto.Content;
            entity.Image = dto.Image;
            entity.Example = dto.Example;
            entity.Active = dto.Active;
            entity.Phonetic = dto.Phonetic;
            entity.Trans = dto.Trans;
            entity.Position = dto.Position;

            UnitOfWork.WordsRepo.Update(entity);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"WordService -> UpdateWord successfully");
            return new SingleId() { Id = Id };
        }

        public async Task<SingleId> DeleteWord(Guid Id, Guid LessonId)
        {
            _logger.LogInformation($"WordService -> DeleteWord with id {JsonConvert.SerializeObject(Id)}");
            var entity = UnitOfWork.WordsRepo.GetAll().Where(x => x.Id == Id).Include(x => x.LessonWords).FirstOrDefault();
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }

            if (entity.LessonWords.Count() > 1)
            {
                var lessonWord = entity.LessonWords.Where(x => x.LessonId == LessonId && x.WordId == Id).FirstOrDefault();
                if (lessonWord == null)
                {
                    throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
                }
                UnitOfWork.LessonWordRepo.Delete(lessonWord);
            }
            else
            {
                UnitOfWork.WordsRepo.Delete(entity);
            }
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"WordService -> AdminDeleteUser successfully");
            return new SingleId() { Id = Id };
        }

        public async Task<bool> CheckExistLesson(List<WordCreateExcelDTO> list)
        {
            _logger.LogInformation($"WordService -> CheckExistLesson with total rows: {list.Count()}");
            var groups = list.Select(x => x.LessonId).GroupBy(x => x).Select(x => x.Key);
            //check exist lesssonId
            foreach (var key in groups)
            {
                bool exist = await UnitOfWork.LessonsRepo.GetAll().AnyAsync(x => x.Id == Guid.Parse(key));
                if (!exist) throw new ServiceExeption(HttpStatusCode.BadRequest, $"Not found lessonId: {key}");
            }
            return true;
        }
        #endregion

        #region User
        public async Task<DataList<WordSearchResultDTO>> SearchWordAsync(SearchWordDTO request)
        {
            _logger.LogInformation($"WordService -> SearchWordAsync with content: {request.Content}");
            request.Content = request.Content.ToLower();
            var result = new DataList<WordSearchResultDTO>();
            var query = await UnitOfWork.WordsRepo.GetAll().Where(x => x.Content.Contains(request.Content)).Select(x => new WordSearchResultDTO
            {
                Id = x.Id,
                Image = x.Image,
                Audio = x.Audio,
                Content = x.Content,
                Example = x.Example,
                Phonetic = x.Phonetic,
                Trans = x.Trans,
                Position = x.Position
            }).Take(10).ToListAsync();
            result.Items = query;
            _logger.LogInformation($"WordService -> SearchWordAsync with content: {request.Content} successfully");
            return result;
        }

        public async Task<DataList<WordSearchResultDTO>> GetAllWordUserByLessonId(Guid lessonId, BaseRequest request)
        {
            _logger.LogInformation($"WordService -> GetAllWordUserByLessonId with request {JsonConvert.SerializeObject(request)} and LessonId {lessonId}");
            var result = new DataList<WordSearchResultDTO>();
            IQueryable<Word> query = UnitOfWork.WordsRepo.GetAll().Where(x => x.Active == true && x.LessonWords.Any(y => y.LessonId == lessonId)).OrderBy(x => x.Content);
            var data = await query.Select(x => new WordSearchResultDTO
            {
                Id = x.Id,
                Image = x.Image,
                Audio = x.Audio,
                Content = x.Content,
                Example = x.Example,
                Phonetic = x.Phonetic,
                Trans = x.Trans,
                Position = x.Position
            }).ToListAsync();
            result.Items = data;
            result.TotalRecord = data.Count;
            _logger.LogInformation($"WordService -> GetAllWordUserByLessonId with request {JsonConvert.SerializeObject(request)} and LessonId {lessonId} successfully");
            return result;
        }

        #endregion
    }
}
