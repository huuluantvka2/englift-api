using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Word;
using EngLift.Model.Entities;
using EngLift.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EngLift.Service.Implements
{
    public class WorkerService : ServiceBase<WorkerService>, IWorkerService
    {
        private IDictionaryService _dictionaryService { get; set; }
        public WorkerService(ILogger<WorkerService> logger, IUnitOfWork unitOfWork, IDictionaryService dictionaryService) : base(logger, unitOfWork)
        {
            _dictionaryService = dictionaryService;
        }
        public async Task SaveWordsAsync(List<WordCreateExcelDTO> list)
        {
            _logger.LogInformation($"WorkerService -> SaveWords with total rows: {list.Count()}");
            foreach (var wordItem in list)
            {
                _logger.LogInformation($"WorkerService -> SaveWords ------------------->WORD: {wordItem.Content}");
                Guid? wordId = await UnitOfWork.WordsRepo.GetAll().Where(x => x.Content == wordItem.Content && x.Trans == wordItem.Trans).Select(x => x.Id).FirstOrDefaultAsync();
                if (wordId == null || Guid.Empty == wordId)
                {
                    Word word = new Word()
                    {
                        Id = Guid.NewGuid(),
                        Image = wordItem.Image,
                        Content = wordItem.Content,
                        Example = wordItem.Example,
                        Active = wordItem.Active,
                        Phonetic = wordItem.Phonetic,
                        China = wordItem.China,
                        Trans = wordItem.Trans,
                        Position = wordItem.Position,
                    };
                    word.LessonWords = new List<LessonWord>() {
                    new LessonWord() { LessonId=Guid.Parse(wordItem.LessonId),WordId = word.Id}
                };
/*                    var audio = await _dictionaryService.GetLinkAudio(wordItem.Content);
                    if (!string.IsNullOrEmpty(audio)) word.Audio = audio;*/
                    UnitOfWork.WordsRepo.Insert(word);
                    _logger.LogInformation($"WorkerService -> SaveWords successfully");
                }
                else
                {
                    var exist = await UnitOfWork.LessonWordRepo.GetAll().AnyAsync(x => x.LessonId == Guid.Parse(wordItem.LessonId));
                    if (!exist)
                    {
                        var newLessonWord = new LessonWord() { LessonId = Guid.Parse(wordItem.LessonId), WordId = (Guid)wordId };
                        UnitOfWork.LessonWordRepo.Insert(newLessonWord);
                    }
                }
            }
            await UnitOfWork.SaveChangesAsync();
            _logger.LogInformation($"WorkerService -> SaveWords with total rows: {list.Count()} successfully");
        }
    }
}
