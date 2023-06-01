using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Base;
using EngLift.DTO.Lesson;
using EngLift.DTO.Response;
using EngLift.Model.Entities;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace EngLift.Service.Implements
{
    public class LessonService : ServiceBase<LessonService>, ILessonService
    {
        public LessonService(ILogger<LessonService> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {

        }
        public async Task<DataList<LessonItemDTO>> GetAllLessonByCourseId(Guid CourseId, BaseRequest request)
        {
            _logger.LogInformation($"LessonService -> GetAllLessonByCourseId with request {JsonConvert.SerializeObject(request)} and CourseId {CourseId}");
            var result = new DataList<LessonItemDTO>();
            IQueryable<Lesson> query = UnitOfWork.LessonsRepo.GetAll()
                .Where(x => x.CourseId == CourseId && String.IsNullOrEmpty(request.Search) ? true : x.Name.ToLower().Contains(request.Search))
                .Include(x => x.Course);

            result.TotalRecord = query.Count();
            query = query.OrderByDescending(x => x.Prior);
            if (request.Sort != null)
            {
                switch (request.Sort)
                {
                    case 1: query = query.OrderBy(x => x.CreatedAt); break;
                    case 2: query = query.OrderByDescending(x => x.CreatedAt); break;
                }
            }
            var data = await query.Select(x => new LessonItemDTO
            {
                Id = x.Id,
                Image = x.Image,
                Prior = x.Prior,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Desciption = x.Desciption,
                Name = x.Name,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,
                Active = x.Active,
                CourseId = (Guid)x.CourseId,
                Author = x.Author,
                Viewed = x.Viewed,
            }).Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            result.Items = data;
            _logger.LogInformation($"LessonService -> GetAllLessonByCourseId CourseId {CourseId} successfully");
            return result;
        }

        public async Task<LessonItemDTO> GetLessonDetail(Guid Id)
        {
            _logger.LogInformation($"LessonService -> GetLessonDetail with id {JsonConvert.SerializeObject(Id)}");
            var entity = await UnitOfWork.LessonsRepo.GetAll().Where(x => x.Id == Id).Include(x => x.Course).Select(x => new LessonItemDTO
            {
                Id = x.Id,
                Image = x.Image,
                Prior = x.Prior,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Desciption = x.Desciption,
                Name = x.Name,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,
                Active = x.Active,
                CourseId = (Guid)x.CourseId,
                Author = x.Author,
                Viewed = x.Viewed,
            }).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }

            _logger.LogInformation($"LessonService -> GetLessonDetail with request successfully");
            return entity;
        }


        public async Task<SingleId> CreateLesson(LessonCreateDTO dto)
        {
            _logger.LogInformation($"LessonService -> CreateLesson with dto {JsonConvert.SerializeObject(dto)}");

            var existCourse = UnitOfWork.CoursesRepo.GetAll().Any(x => x.Id == dto.CourseId);
            if (!existCourse)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_COURSE);
            }
            Lesson lesson = new Lesson()
            {
                Id = Guid.NewGuid(),
                Active = true,
                Desciption = dto.Desciption,
                Image = dto.Image,
                Name = dto.Name,
                Prior = dto.Prior ?? 1,
                Author = dto.Author,
                CourseId = dto.CourseId,
                Viewed = 0,
            };

            UnitOfWork.LessonsRepo.Insert(lesson);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"LessonService -> CreateLesson successfully");
            return new SingleId() { Id = lesson.Id };
        }

        public async Task<SingleId> UpdateLesson(Guid Id, LessonUpdateDTO dto)
        {
            _logger.LogInformation($"LessonService -> UpdateLesson with dto {JsonConvert.SerializeObject(dto)}");
            var entity = UnitOfWork.LessonsRepo.GetById(Id);
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }
            entity.Prior = dto.Prior;
            entity.Desciption = dto.Desciption;
            entity.Image = dto.Image;
            entity.Name = dto.Name;
            entity.Active = dto.Active;
            entity.Author = dto.Author;


            UnitOfWork.LessonsRepo.Update(entity);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"LessonService -> UpdateLesson successfully");
            return new SingleId() { Id = Id };
        }

        public async Task<SingleId> DeleteLesson(Guid Id)
        {
            _logger.LogInformation($"LessonService -> DeleteLesson with id {JsonConvert.SerializeObject(Id)}");
            var entity = UnitOfWork.LessonsRepo.GetById(Id);
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }

            UnitOfWork.LessonsRepo.Delete(entity);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"LessonService -> AdminDeleteUser successfully");
            return new SingleId() { Id = Id };
        }
    }
}
