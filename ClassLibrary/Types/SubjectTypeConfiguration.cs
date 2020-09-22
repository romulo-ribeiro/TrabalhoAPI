using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Context.Models;


namespace Context.Types
{
    public class SubjectTypeConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(q => q.IdSubject);

            builder.Property(q => q.Name).HasMaxLength(100);
            builder.Property(q => q.Status).IsRequired();

        }
    }

}
