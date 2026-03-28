using Microsoft.EntityFrameworkCore;
using SharingKnowledge.Data.Repository.Contracts;
using SharingKnowledge.Models;

namespace SharingKnowledge.Data.Repository
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private bool isDisposed = false;

        private readonly ApplicationDbContext dbContext;

        public StudentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            return await dbContext
                .Users
                .OfType<Student>()
                .Include(s => s.EnrolledCourses)
                .ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(s => s.Id == studentId);
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
    }
}
