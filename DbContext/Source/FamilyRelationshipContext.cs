using FRS.Models;
using System;
using System.Data.Entity;

namespace FRS.DatabaseContext
{
    public class FamilyRelationshipContext: DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public FamilyRelationshipContext() : base()
        { 
        }


        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
