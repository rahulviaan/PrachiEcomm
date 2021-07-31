using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Text;


namespace DAL.Models.Entities
{
    public partial class UserContext : DbContext
    {

        public UserContext()
        {
        }

        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<UserBookIds> UserBookIds { get; set; }
        public virtual DbSet<ClassSubjects> ClassSubjects { get; set; }
        
        public virtual DbSet<ReadEdgeLogins> ReadEdgeLogins { get; set; }
        public virtual DbSet<ReadEdgeUserLoginInfo> ReadEdgeUserLoginInfo { get; set; }
        public virtual DbSet<ReadEdgeTrialUsers> ReadEdgeTrialUsers { get; set; }
        public virtual DbSet<TeacherSubjectClass> TeacherSubjectClass { get; set; }
        public virtual DbSet<SubscriptionPayment> SubscriptionPayment { get; set; }
        public virtual DbSet<SubscriptionMaster> SubscriptionMaster { get; set; }
        public virtual DbSet<UserSubscription> UserSubscription { get; set; }
        public virtual DbSet<ReadEdgeSubscriptionPayment> ReadEdgeSubscriptionPayment { get; set; }
        public virtual DbSet<AspNetUsersModel> AspNetUsersModel { get; set; }

        
    }
}
