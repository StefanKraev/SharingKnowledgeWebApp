using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository
{
    public class OpenCourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext dbContext;

        public OpenCourseRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<OpenCourse>> GetAllMaterializedOpenCourses()
        {
            return await dbContext
                .OpenCourses
                .AsNoTracking()
                .OrderBy(c => c.StartDate)
                .ToArrayAsync();
        }

        public IQueryable<OpenCourse> GetAllOpenCourses()
        {
            return dbContext
                .OpenCourses
                .AsNoTracking();
        }
    }
}
