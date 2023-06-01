
using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Base;
using EngLift.DTO.Course;
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
    public class CourseService : ServiceBase<CourseService>, ICourseService
    {
        public CourseService(ILogger<CourseService> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {

        }
        public async Task<DataList<CourseItemDTO>> GetAllCourse(BaseRequest request)
        {
            _logger.LogInformation($"CourseService -> GetAllCourse with request {JsonConvert.SerializeObject(request)}");
            var result = new DataList<CourseItemDTO>();
            IQueryable<Course> query = UnitOfWork.CoursesRepo.GetAll()
                .Where(x => String.IsNullOrEmpty(request.Search) ? true :
                        (
                        x.Name.ToLower().Contains(request.Search)
                        ))
                .Include(x => x.Lessons);

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
            var data = await query.Select(x => new CourseItemDTO
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
                TotalLesson = x.Lessons.Count(),
            }).Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            result.Items = data;
            _logger.LogInformation($"CourseService -> GetAllCourse with request successfully");
            return result;
        }

        public async Task<CourseItemDTO> GetCourseDetail(Guid Id)
        {
            _logger.LogInformation($"CourseService -> GetCourseDetail with id {JsonConvert.SerializeObject(Id)}");
            var entity = await UnitOfWork.CoursesRepo.GetAll().Where(x => x.Id == Id).Include(x => x.Lessons).Select(x => new CourseItemDTO
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
                TotalLesson = x.Lessons.Count(),
            }).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }

            _logger.LogInformation($"CourseService -> GetCourseDetail with request successfully");
            return entity;
        }


        public async Task<SingleId> CreateCourse(CourseCreateDTO dto)
        {
            _logger.LogInformation($"CourseService -> CreateCourse with dto {JsonConvert.SerializeObject(dto)}");
            Course course = new Course()
            {
                Id = Guid.NewGuid(),
                Active = true,
                Desciption = dto.Desciption,
                Image = dto.Image,
                Name = dto.Name,
                Prior = dto.Prior ?? 1
            };

            UnitOfWork.CoursesRepo.Insert(course);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"CourseService -> CreateCourse successfully");
            return new SingleId() { Id = course.Id };
        }

        public async Task<SingleId> UpdateCourse(Guid Id, CourseUpdateDTO dto)
        {
            _logger.LogInformation($"CourseService -> UpdateCourse with dto {JsonConvert.SerializeObject(dto)}");
            var entity = UnitOfWork.CoursesRepo.GetById(Id);
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }
            entity.Prior = dto.Prior;
            entity.Desciption = dto.Desciption;
            entity.Image = dto.Image;
            entity.Name = dto.Name;
            entity.Active = dto.Active;

            UnitOfWork.CoursesRepo.Update(entity);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"CourseService -> UpdateCourse successfully");
            return new SingleId() { Id = Id };
        }

        public async Task<SingleId> DeleteCourse(Guid Id)
        {
            _logger.LogInformation($"CourseService -> DeleteCourse with id {JsonConvert.SerializeObject(Id)}");
            var entity = UnitOfWork.CoursesRepo.GetById(Id);
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND);
            }

            UnitOfWork.CoursesRepo.Delete(entity);
            await UnitOfWork.SaveChangesAsync();

            _logger.LogInformation($"CourseService -> AdminDeleteUser successfully");
            return new SingleId() { Id = Id };
        }
    }
}
