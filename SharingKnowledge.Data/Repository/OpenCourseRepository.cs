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
            return resultCode == 1;
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

        public async Task<OpenCoursesDetailsViewModel?> GetOpenCourseByIdAsync(int openCourseId)
        {
            return await dbContext.OpenCourses
                .AsNoTracking()
                .Where(oc => oc.Id == openCourseId)
                .Select(oc => new OpenCoursesDetailsViewModel
                {
                    Title = oc.Title,
                    Description = oc.Description,
                    StartDate = oc.StartDate,
                    ImageUrl = oc.ImageUrl,
                    CategoryName = oc.Category.Name,
                    AuthorEmail = oc.Creator.Email ?? "Email not found!"
                })
                .SingleOrDefaultAsync();
        }
    }
}
