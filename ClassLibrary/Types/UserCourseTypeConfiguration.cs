using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Context.Models;
using Context.Relations;

namespace Context.Types
{
    public class UserCourseTypeConfiguration : IEntityTypeConfiguration<UserCourse>
    {
        public void Configure(EntityTypeBuilder<UserCourse> builder)
        {
            builder.HasKey(q => q.Id);

            builder.HasAlternateKey(q => q.IdUser);
            builder.HasAlternateKey(q => q.IdCourse);
        }
    }

}
