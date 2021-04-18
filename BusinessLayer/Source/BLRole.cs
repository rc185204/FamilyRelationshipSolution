﻿using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRS.Common;

namespace FRS.BusinessLayer
{
    public class BLRole
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetAll()
        {
            List<Role> list = null;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                list = dbContext.Role.ToList();
            }
            return list;
        }

        public static Role Get(int RoleId)
        {
            Role Role = null;
            using (FamilyRelationshipContext dbContext = new FamilyRelationshipContext())
            {
                Role = dbContext.Role.Where<Role>(c => c.RoleId == RoleId).FirstOrDefault();
            }
            return Role;
        }
    }
}
