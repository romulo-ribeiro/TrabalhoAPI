using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Context.Models;
using Context.Relations;

namespace Context.Types
{
    public class UserSubjectTypeConfiguration : IEntityTypeConfiguration<UserSubject>
    {
        public void Configure(EntityTypeBuilder<UserSubject> builder)
        {
            builder.HasKey(q => q.IdUser);
            builder.HasKey(q => q.IdSubject);
            builder.Property(q => q.Grade);
        }
    }

}
