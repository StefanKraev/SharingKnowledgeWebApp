namespace SharingKnowledge.Common
{
    public class ValidationConstrains
    {
        /*OpenCourse property validation*/
        public const int CourseTitleMinLength = 2;
        public const int CourseTitleMaxLength = 100;
        public const int CourseDescriptionMinLength = 10;
        public const int CourseDescriptionMaxLength = 2000;
        public const string CourseImageUrlRegularExpression = @"^(http|https):\/\/[^\s$.?#].[^\s]*$";
        public const int CourseImageUrlMaxLength = 2048;

        /*CourseCategory property validations*/
        public const int CategoryNameMinLength = 2;
        public const int CategoryNameMaxLength = 100;
    }
}
