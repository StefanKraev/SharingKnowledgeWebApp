using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Configurations
{
    public class UserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            string ghostAdminUserId = "007e37ed-27e5-43cb-a8b7-a3b14d054f45";

            var adminUser = new ApplicationUser
            {
                Id = ghostAdminUserId,
                UserName = "GhostAdmin",
                NormalizedUserName = "GHOSTADMIN",
                Email = "admin@sharingknowledge.com",
                NormalizedEmail = "ADMIN@SHARINGKNOWLEDGE.COM",
                EmailConfirmed = true,
                SecurityStamp = "3235650d-6e47-49f3-9d0a-04664879201a",
                ConcurrencyStamp = "86778f79-246e-4861-8271-6c589679199c",
                PasswordHash = "AQAAAAIAAYagAAAAEGVx7nHYW18d6vEjqAitwoxLyms6pEPIbLVGX7rivzEBJMpHtztkFthMsfbEaXgWvQ=="
            };

            builder.HasData(adminUser);
        }
    }
}
