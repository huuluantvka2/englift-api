﻿using EngLift.DTO.Base;
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
        Task<DataList<WordSearchResultDTO>> SearchWordAsync(SearchWordDTO request);
        Task<DataList<WordSearchResultDTO>> GetAllWordUserByLessonId(Guid lessonId, BaseRequest request);
    }
}
