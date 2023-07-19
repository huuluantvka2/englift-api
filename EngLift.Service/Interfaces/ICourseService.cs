﻿using EngLift.DTO.Base;
using EngLift.DTO.Course;
using EngLift.DTO.Response;
using EngLift.Model.Entities;

namespace EngLift.Service.Interfaces
{
    public interface ICourseService
    {
        Task<DataList<CourseItemDTO>> GetAllCourse(BaseRequest request);
        Task<SingleId> CreateCourse(CourseCreateDTO dto);
        Task<SingleId> UpdateCourse(Guid Id, CourseUpdateDTO dto);
        Task<SingleId> DeleteCourse(Guid Id);
        Task<CourseItemDTO> GetCourseDetail(Guid Id);
        Task<DataList<CourseItemPublicDTO>> GetAllCorseUser(BaseRequest request);
        Task<CourseItemPublicDTO> GetCourseUserById(Guid courseId);
        Task<List<Word>> GetWordMigration();
        Task<List<Lesson>> GetLessonMigration();
        Task<List<Course>> GetCourseMigration();
    }
}
