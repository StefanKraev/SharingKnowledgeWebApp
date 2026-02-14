using SharingKnowledge.Services.Core.Interfaces;
using SharingKnowledge.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Services.Core
{
    public class OpenCoursesService : IOpenCoursesService
    {
        public Task<IEnumerable<OpenCoursesAllViewModel>> GetAllOpenCoursesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
