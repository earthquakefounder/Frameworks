using Entities.Contexts;
using Entities.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configurations.Identity
{
    internal class AppUserConfiguration : EntityTypeConfiguration<_AppUser>
    {
        public AppUserConfiguration()
        {
            ToTable("AppUser");

            HasKey(x => x.ID);
           
            Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(
                        new IndexAttribute("IX_AppUser_Email", 1)
                        {
                            IsUnique = true
                        })
                );

            Property(x => x.UserName)
                .HasMaxLength(255)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, 
                    new IndexAnnotation(
                        new IndexAttribute("IX_AppUser_UserName", 1)
                        {
                            IsUnique = true
                        })
                );

            Property(x => x.Name).HasMaxLength(255);

            Property(x => x.PasswordHash).IsOptional();
            Property(x => x.Salt).IsOptional();
        }
    }
}
