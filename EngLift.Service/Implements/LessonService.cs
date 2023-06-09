﻿using EngLift.Common;
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

        #region Admin
        public async Task<DataList<LessonItemDTO>> GetAllLessonByCourseId(Guid CourseId, BaseRequest request)
        {
            _logger.LogInformation($"LessonService -> GetAllLessonByCourseId with request {JsonConvert.SerializeObject(request)} and CourseId {CourseId}");
            var result = new DataList<LessonItemDTO>();
            IQueryable<Lesson> query = UnitOfWork.LessonsRepo.GetAll()
                .Where(x => x.CourseId == CourseId &&
                (String.IsNullOrEmpty(request.Search) ? true : x.Name.ToLower().Contains(request.Search)) &&
                (request.Active == null || x.Active == (bool)request.Active)
                ).Include(x => x.LessonWords);

            result.TotalRecord = query.Count();
            query = query.OrderByDescending(x => x.Prior);
            if (request.Sort != null)
            {
                switch (request.Sort)
                {
                    case 1: query = query.OrderBy(x => x.CreatedAt); break;
                    case 2: query = query.OrderByDescending(x => x.CreatedAt); break;
                    case 3: query = query.OrderBy(x => x.LessonWords.Count); break;
                    case 4: query = query.OrderByDescending(x => x.LessonWords.Count); break;
                    case 5: query = query.OrderBy(x => x.Prior); break;
                    case 6: query = query.OrderByDescending(x => x.Prior); break;
                    case 7: query = query.OrderBy(x => x.UpdatedAt); break;
                    case 8: query = query.OrderByDescending(x => x.UpdatedAt); break;
                }
            }
            var data = await query.Select(x => new LessonItemDTO
            {
                Id = x.Id,
                Image = x.Image,
                Prior = x.Prior,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                Description = x.Description,
                Name = x.Name,
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = x.UpdatedBy,
                Active = x.Active,
                CourseId = (Guid)x.CourseId,
                Author = x.Author,
                Viewed = x.Viewed,
                TotalWords = x.LessonWords.Count()
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
                Description = x.Description,
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
                Description = dto.Description,
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
            entity.Description = dto.Description;
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
        #endregion

        #region User
        public async Task<DataList<LessonItemUserDTO>> GetAllLessonUserByCourseId(Guid CourseId, BaseRequest request, Guid userId)
        {
            _logger.LogInformation($"LessonService -> GetAllLessonUserByCourseId with request {JsonConvert.SerializeObject(request)} and CourseId {CourseId}");
            var result = new DataList<LessonItemUserDTO>();
            if (!String.IsNullOrEmpty(request.Search)) request.Search = request.Search.ToLower();
            IQueryable<Lesson> query = UnitOfWork.LessonsRepo.GetAll()
                .Where(x => x.CourseId == CourseId &&
                (String.IsNullOrEmpty(request.Search) || x.Name.ToLower().Contains(request.Search)) &&
                x.Active == true && x.LessonWords.Count >= 4).Include(x => x.LessonWords);
            if (userId != Guid.Empty) query = query.Include(x => x.UserLessons);

            result.TotalRecord = query.Count();
            query = query.OrderBy(x => x.Prior);
            IQueryable<LessonItemUserDTO> data;
            if (userId == Guid.Empty)
            {
                data = query.Select(x => new LessonItemUserDTO
                {
                    Id = x.Id,
                    Image = x.Image,
                    Description = x.Description,
                    Name = x.Name,
                    CourseId = (Guid)x.CourseId,
                    Author = x.Author,
                    Viewed = x.Viewed,
                });
            }
            else
            {
                data = query.Select(x => new LessonItemUserDTO
                {
                    Id = x.Id,
                    Image = x.Image,
                    Description = x.Description,
                    Name = x.Name,
                    CourseId = (Guid)x.CourseId,
                    Author = x.Author,
                    Viewed = x.Viewed,
                    LastTimeStudy = x.UserLessons.Where(y => y.LessonId == x.Id).Select(y => y.UpdatedAt).FirstOrDefault(),
                    LevelLesson = x.UserLessons.Where(y => y.LessonId == x.Id).Select(y => y.Level).FirstOrDefault(),
                    NextTime = x.UserLessons.Where(y => y.LessonId == x.Id).Select(y => y.NextTime).FirstOrDefault(),
                });
            }
            var queryResult = await data.Skip((request.Page - 1) * request.Limit).Take(request.Limit).ToListAsync();

            result.Items = queryResult;
            _logger.LogInformation($"LessonService -> GetAllLessonUserByCourseId CourseId {CourseId} successfully");
            return result;
        }

        public async Task<LessonItemUserDTO> GetLessonUserById(Guid lessonId)
        {
            _logger.LogInformation($"LessonService -> GetLessonUserById with request lessonId {lessonId}");
            IQueryable<Lesson> query = UnitOfWork.LessonsRepo.GetAll()
                .Where(x => x.Active == true && x.Id == lessonId);
            var entity = await query.Select(x => new LessonItemUserDTO
            {
                Id = x.Id,
                Image = x.Image,
                Description = x.Description,
                Name = x.Name,
                CourseId = (Guid)x.CourseId,
                Author = x.Author,
                Viewed = x.Viewed,
            }).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.NOT_FOUND_LESSON);
            }
            _logger.LogInformation($"LessonService -> GetLessonUserById with request lessonId {lessonId} successfully");
            return entity;
        }

        public async Task<SingleId> SaveStudyHistoryUser(Guid userId, Guid lessonId, UserLessonDTO body)
        {
            _logger.LogInformation($"LessonService -> SaveStudyHistoryUser with request userId {userId} and body {JsonConvert.SerializeObject(body)}");
            var entity = await UnitOfWork.UserLessonRepo.GetAll().Where(x => x.UserId == userId && x.LessonId == lessonId).FirstOrDefaultAsync();
            var lessonEntity = await UnitOfWork.LessonsRepo.GetAll().Where(x => x.Id == lessonId).Include(x => x.LessonWords).Select(x => x.LessonWords.Count).FirstOrDefaultAsync();
            if (lessonEntity == null || lessonEntity < 4)
            {
                throw new ServiceExeption(HttpStatusCode.NotFound, ErrorMessage.LESSON_NOT_FOUND_OR_INVALID);
            }
            Console.WriteLine(DateTime.UtcNow.AddMinutes(+420));
            if (entity == null) //TH chưa có lịch sử
            {
                var userEntity = UnitOfWork.UsersRepo.GetById(userId);
                var userLesson = new UserLesson()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    LessonId = lessonId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Level = LevelLesson.LevelOne,
                    NextTime = DateTime.UtcNow.AddDays(1),
                    TotalWords = lessonEntity,
                };
                UnitOfWork.UserLessonRepo.Insert(userLesson);


                var oldTimeStudy = userEntity.LastTimeStudy;
                userEntity.LastTimeStudy = DateTime.UtcNow;
                userEntity.TotalWords += lessonEntity;
                if (oldTimeStudy == null || (DateTime.UtcNow - oldTimeStudy).Value.TotalHours > 48)
                {
                    userEntity.TotalDateStudied = 1;
                }
                else if ((DateTime.UtcNow - oldTimeStudy).Value.TotalHours > 24)
                {
                    userEntity.TotalDateStudied++;
                }
                UnitOfWork.UsersRepo.Update(userEntity);

                await UnitOfWork.SaveChangesAsync();
                _logger.LogInformation($"LessonService -> SaveStudyHistoryUser with request userId {userId} with add new UserLesson succssfully");
            }
            else //TH có lịch sử
            {
                entity.UpdatedAt = DateTime.UtcNow;
                if (entity.Level != LevelLesson.LevelFive && DateTime.UtcNow >= entity.NextTime)
                { //level up
                    entity.NextTime = NextTimeRemind(entity.Level);
                    entity.Level++;
                }
                UnitOfWork.UserLessonRepo.Update(entity);

                await UnitOfWork.SaveChangesAsync();
                _logger.LogInformation($"LessonService -> SaveStudyHistoryUser with request userId {userId} with update UserLesson succssfully");
            }

            return new SingleId() { Id = lessonId }; ;
        }
        #endregion

        private DateTime? NextTimeRemind(LevelLesson level)
        {
            switch (level)
            {
                case LevelLesson.LevelOne: return DateTime.UtcNow.AddDays(3);
                case LevelLesson.LevelTwo: return DateTime.UtcNow.AddDays(7);
                case LevelLesson.LevelThree: return DateTime.UtcNow.AddDays(15);
                case LevelLesson.LevelFour: return DateTime.UtcNow.AddDays(30);
                case LevelLesson.LevelFive: return null;
                default: return null;
            }
        }
    }


}
