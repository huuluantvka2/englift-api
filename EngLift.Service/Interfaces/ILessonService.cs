using EngLift.DTO.Base;
using EngLift.DTO.Lesson;
using EngLift.DTO.Response;

namespace EngLift.Service.Interfaces
{
    public interface ILessonService
    {
        Task<DataList<LessonItemDTO>> GetAllLessonByCourseId(Guid CourseId, BaseRequest request);
        Task<SingleId> CreateLesson(LessonCreateDTO dto);
        Task<SingleId> UpdateLesson(Guid Id, LessonUpdateDTO dto);
        Task<SingleId> DeleteLesson(Guid Id);
        Task<LessonItemDTO> GetLessonDetail(Guid Id);
        Task<DataList<LessonItemUserDTO>> GetAllLessonUserByCourseId(Guid CourseId, BaseRequest request);
    }
}
