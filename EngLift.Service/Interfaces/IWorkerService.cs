using EngLift.DTO.Word;

namespace EngLift.Service.Interfaces
{
    public interface IWorkerService
    {
        Task SaveWordsAsync(List<WordCreateExcelDTO> list);
    }
}
