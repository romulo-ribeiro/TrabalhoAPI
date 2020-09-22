using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Context.Models;


namespace Context.Types
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(q => q.IdUser);

            builder.Property(q => q.Name).HasMaxLength(100);
            builder.Property(q => q.Surname).HasMaxLength(100);
            builder.Property(q => q.Cpf).HasMaxLength(11);
            builder.Property(q => q.Role).IsRequired();
            builder.Property(q => q.Birthday);
        }
    }

}
