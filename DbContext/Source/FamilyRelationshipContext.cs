using FRS.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace FRS.DatabaseContext
{
    public class FamilyRelationshipContext: DbContext
    {

        public DbSet<Role> Role { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<Family> Family { get; set; }

        public DbSet<FamilyMember> FamilyMember { get; set; }

        public DbSet<CertificateType> CertificateType { get; set; }


        public FamilyRelationshipContext() : base()
        {

        }

        /// <summary>
        /// 继承父类，
        /// "name=FamilyRelationship" base中使用数据库连接字符串
        /// </summary>
        //public FamilyRelationshipContext() : base("Data Source = Localhost;Initial Catalog = FamilyRelationshipSolution;User Id = sa;Password = sa;")
        //{

        //}

        //public FamilyRelationshipContext(string connectionStr) : base(connectionStr)
        //{

        //}

        //public FamilyRelationshipContext(DbContextOptions<FamilyRelationshipContext> options)
        //    : base(options)
        //{
        //}


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //// 禁用默认表名复数形式
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //// 禁用一对多级联删除
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //// 禁用多对多级联删除
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);


        }

    }
}
