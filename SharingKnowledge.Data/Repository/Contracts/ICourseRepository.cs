using SharingKnowledge.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Repository.Contracts
{
    public interface ICourseRepository
    {

        IQueryable<OpenCourse> GetAllOpenCourses();

        Task<IEnumerable<OpenCourse>> GetAllMaterializedOpenCourses();
    }
}
