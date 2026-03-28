using SharingKnowledge.Models;

namespace SharingKnowledge.Data.Repository.Contracts
{
    public interface IStudentRepository
    {
        Task<Student?> GetStudentByIdAsync(string studentId);
    }
}
