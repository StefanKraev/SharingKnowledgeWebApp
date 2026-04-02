namespace SharingKnowledge.Common
{
    public static class ValidationConstrains
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

        /*Student property validations*/
        public const string StudentFNRegularExpression = @"^\d{1}MI\d{7}$";
        public const int StudentFNMaxLen = 15;

        /*Book property validations*/
        public const int BookTitleMinLength = 2;
        public const int BookTitleMaxLength = 60;
        public const int BookAuthorMinLength = 1;
        public const int BookAuthorMaxLength = 50;
        public const int BookDescriptionMinLength = 10;
        public const int BookDescriptionMaxLength = 200;
        public const int BookImageUrlMaxLength = 2048;
    }
}
