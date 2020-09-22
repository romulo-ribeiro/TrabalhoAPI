using Context.Models;
using Context.Relations;
using Microsoft.EntityFrameworkCore;

namespace Context
{
    public class EscolaContext : DbContext
    {
        public EscolaContext()
        {
        }
        public EscolaContext(DbContextOptions<EscolaContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=NT-04774\SQLEXPRESS01;Initial Catalog=BancoEscola;Integrated Security=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EscolaContext).Assembly);

            modelBuilder.Entity<CourseSubject>().HasOne(p => p.Course).WithMany(b => b.Enrollments).HasForeignKey(p => p.IdCourse);
            modelBuilder.Entity<CourseSubject>().HasOne(p => p.Subject).WithMany(b => b.Enrollment).HasForeignKey(p => p.IdSubject);
            
            modelBuilder.Entity<UserCourse>().HasOne(p => p.User).WithMany(b => b.InCourse).HasForeignKey(p => p.IdUser);
            modelBuilder.Entity<UserCourse>().HasOne(p => p.Course).WithMany(b => b.CourseContains).HasForeignKey(p => p.IdCourse);
            
            modelBuilder.Entity<UserSubject>().HasOne(p => p.User).WithMany(b => b.Registered).HasForeignKey(p => p.IdUser);
            modelBuilder.Entity<UserSubject>().HasOne(p => p.Subject).WithMany(b => b.Allocation).HasForeignKey(p => p.IdSubject);
        }

        public DbSet<Course> Curso { get; set; }
        public DbSet<Subject> Materia { get; set; }
        public DbSet<User> Usuario { get; set; }

        public DbSet<CourseSubject> Contem { get; set; }
        public DbSet<UserCourse> Lotacao { get; set; }
        public DbSet<UserSubject> Matricula { get; set; }
    }
}
