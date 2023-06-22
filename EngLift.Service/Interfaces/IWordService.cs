using EngLift.DTO.Response;
using EngLift.DTO.Word;

namespace EngLift.Service.Interfaces
{
    public interface IWordService
    {
        Task<DataList<WordItemDTO>> GetAllWordByLessonId(WordRequest request);
        Task<SingleId> CreateWord(WordCreateDTO dto);
        Task<SingleId> UpdateWord(Guid id, WordUpdateDTO dto);
        Task<SingleId> DeleteWord(Guid id, Guid lessonId);
        Task<WordItemDTO> GetWordDetail(Guid id);
        Task<bool> CheckExistLesson(List<WordCreateExcelDTO> list);
    }
}
