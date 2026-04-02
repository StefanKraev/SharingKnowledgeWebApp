using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using SharingKnowledge.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository
{
    public class OpenCourseRepository : ICourseRepository, IDisposable
    {
        private bool isDisposed = false;

        private readonly ApplicationDbContext dbContext;

        public OpenCourseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> AddOpenCourseAsync(OpenCourse openCourse)
        {
            await dbContext.AddAsync(openCourse);
            int resultCode = await dbContext.SaveChangesAsync();
            return resultCode > 0;
        }

        public async Task<IEnumerable<OpenCourse>> GetAllOpenCoursesAsync()
        {
            return await dbContext.OpenCourses
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.EnrolledStudents)
                .OrderByDescending(c => c.StartDate)
                .ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
                isDisposed = true;
            }
        }

        public async Task<OpenCourse?> GetOpenCourseByIdAsync(int openCourseId)
        {
            return await dbContext
                .OpenCourses
                .AsNoTracking()
                .Include(c => c.Category)
                .Include(c => c.Creator)
                .ThenInclude(s => s.User)
                .SingleOrDefaultAsync(c => c.Id == openCourseId);
        }

        public async Task<IEnumerable<CourseCategory>> GetAllCategories()
        {
            return await dbContext
                .CourseCategories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<bool> ExistsCategoryAsync(int categoryId)
        {
            return await dbContext
                .CourseCategories
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<OpenCourse?> GetCourseByIdForEditAsync(int openCourseId)
        {
            return await dbContext
                .OpenCourses
                .Include(c => c.Category)
                .Include(c => c.Creator)
                .SingleOrDefaultAsync(c => c.Id == openCourseId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<OpenCourse?> GetCourseForUpdateAsync(int courseId)
        {
            return await dbContext.OpenCourses
                .Include(c => c.Creator)
                .SingleOrDefaultAsync(oc => oc.Id == courseId);
        }

        public async Task<OpenCourse?> GetCourseForDeleteAsync(int courseId)
        {
            return await dbContext
                .OpenCourses
                .Include(c => c.Creator)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }

        public async Task<bool> DeleteCourseAsync(OpenCourse openCourse)
        {
            try
            {
                dbContext.OpenCourses.Remove(openCourse);
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<OpenCourse?> GetCourseByIdAsync(int id)
        {
            return await dbContext.OpenCourses.FindAsync(id);
        }
    }
}
