﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CentersAPI.Models.EFModels
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__EFMigrationsHistory> C__EFMigrationsHistory { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationVersoin> ApplicationVersoins { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CenterBrancheUser> CenterBrancheUsers { get; set; }
        public virtual DbSet<CenterBranchImage> CenterBranchImages { get; set; }
        public virtual DbSet<CenterBranch> CenterBranchs { get; set; }
        public virtual DbSet<CenterCertificatePath> CenterCertificatePaths { get; set; }
        public virtual DbSet<CenterFollower> CenterFollowers { get; set; }
        public virtual DbSet<CenterImage> CenterImages { get; set; }
        public virtual DbSet<CenterNotification> CenterNotifications { get; set; }
        public virtual DbSet<CenterRate> CenterRates { get; set; }
        public virtual DbSet<Center> Centers { get; set; }
        public virtual DbSet<CenterUser> CenterUsers { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<ContactUsMessage> ContactUsMessages { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseStudent> CourseStudents { get; set; }
        public virtual DbSet<CourseType> CourseTypes { get; set; }
        public virtual DbSet<CoursLove> CoursLoves { get; set; }
        public virtual DbSet<EndUser> EndUsers { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<Mailbox> Mailboxes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<TrainningCenterCategory> TrainningCenterCategories { get; set; }
        public virtual DbSet<UserCategory> UserCategories { get; set; }
        public virtual DbSet<UserFavourite> UserFavourites { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSearchHistory> UserSearchHistories { get; set; }
    }
}
