using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Context.Models;
using Context.Relations;

namespace Context.Types
{
    public class CourseSubjectTypeConfiguration : IEntityTypeConfiguration<CourseSubject>
    {
        public void Configure(EntityTypeBuilder<CourseSubject> builder)
        {
            builder.HasKey(q => q.Id);

            builder.HasAlternateKey(q => q.IdCourse);
            builder.HasAlternateKey(q => q.IdSubject);
        }
    }

}
