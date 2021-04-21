using FRS.Common;
using FRS.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FRS.DatabaseContext
{
    /// <summary>
    /// 
    /// </summary>
    public class FamilyRelationshipContext: DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Role> Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Family> Family { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<FamilyMember> FamilyMember { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<CertificateType> CertificateType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FamilyRelationshipContext() : base()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionStr"></param>
        public FamilyRelationshipContext(string connectionStr) : base(connectionStr)
        {

        }

        //public FamilyRelationshipContext(DbContextOptions<FamilyRelationshipContext> options)
        //    : base(options)
        //{
        //}


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //// 
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //// 
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //// 
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FamilyRelationshipContext GetFamilyRelationshipContext()
        {
            FamilyRelationshipContext dbContext = new (AppSettings.SqlConn);
            return dbContext;
        }

    }
}
