using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Context.Models;


namespace Context.Types
{
    public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(q => q.IdCourse);

            builder.Property(q => q.Name).HasMaxLength(100);
            builder.Property(q => q.Status).HasMaxLength(100);
        }
    }

}
