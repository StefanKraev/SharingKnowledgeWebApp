using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharingKnowledge.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharingKnowledge.Data.Configurations
{
    public class ApplicatoinUserEntityConfig : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> entity)
        {
            var zeroStudentId = "d5812fbc-b5f3-46c1-8eb1-e6f817687dab";

            // 2. Dynamically created admin
            //var adminUser = new ApplicationUser
            //{
            //    Id = adminId,
            //    UserName = "admin@sharingknowledge.com",
            //    NormalizedUserName = "ADMIN@SHARINGKNOWLEDGE.COM",
            //    Email = "admin@sharingknowledge.com",
            //    NormalizedEmail = "ADMIN@SHARINGKNOWLEDGE.COM",
            //    EmailConfirmed = true,
            //    SecurityStamp = "788c37ed-27e5-43cb-a8b7-a3b14d054f45",
            //    ConcurrencyStamp = "899c37ed-27e5-43cb-a8b7-a3b14d054f45",
            //    PasswordHash = "AQAAAAIAAYagAAAAENK6YuTmQ7A7r+jy034pVbEJPS/LW5g4UXgEDumLOv2+Np/aof2+1PR3wMxUefv5AQ=="
            //};

            // 3. Statically created student
            var studentUser = new ApplicationUser
            {
                Id = zeroStudentId,
                UserName = "student0@sharingknowledge.com",
                NormalizedUserName = "STUDENT0@SHARINGKNOWLEDGE.COM",
                Email = "student0@sharingknowledge.com",
                NormalizedEmail = "STUDENT0@SHARINGKNOWLEDGE.COM",
                EmailConfirmed = true,
                SecurityStamp = "a22b37ed-11e5-43cb-a8b7-a3b14d054f45", // Unique stamp
                ConcurrencyStamp = "b33c37ed-22e5-43cb-a8b7-a3b14d054f45", // Unique stamp
                PasswordHash = "AQAAAAIAAYagAAAAENK6YuTmQ7A7r+jy034pVbEJPS/LW5g4UXgEDumLOv2+Np/aof2+1PR3wMxUefv5AQ=="
            };

            entity.HasData(studentUser);
        }
    }
}
