using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.Word;

namespace EngLift.Service.Interfaces
{
    public interface IWordService
    {
        Task<DataList<WordItemDTO>> GetAllWordByLessonId(Guid LessonId, BaseRequest request);
        Task<SingleId> CreateWord(WordCreateDTO dto);
        Task<SingleId> UpdateWord(Guid Id, WordUpdateDTO dto);
        Task<SingleId> DeleteWord(Guid Id);
        Task<WordItemDTO> GetWordDetail(Guid Id);
    }
}
