using FRS.BusinessLayer;
using FRS.DatabaseContext;
using FRS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FRS.DatabaseContextTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FamilyRelationshipContext FRSDbContext;

        private void btInit_Click(object sender, EventArgs e)
        {
            FRSDbContext = new FamilyRelationshipContext();

            bool flag = FRSDbContext.Database.CreateIfNotExists();

            if (flag == true)
            {
                

                CertificateType type = new CertificateType();
                type.CertificateTypeNmae = "二代身份证";
                type.Description = "二代身份证";
                FRSDbContext.CertificateType.Add(type);

                Role role = new Role();
                role.RoleName = "System admin";
                role.Description = "System admin";
                FRSDbContext.Role.Add(role);

                User user = new User();
                user.UserName = "admin";
                user.Password = "admin";
                user.UserTrueName = "admin";
                user.RoleId = 1;
                FRSDbContext.User.Add(user);

                FRSDbContext.SaveChanges();

                MessageBox.Show("数据库创建成功！");
            }
            else
            {
                //MessageBox.Show("数据库创建失败！");
            }

        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            //BLUser bluser = new BLUser();
            User user = BLUser.Valid("admin", "admin");
            if (user == null)
                MessageBox.Show("failed");
            else MessageBox.Show(user.UserTrueName);
        }

        private void btGetAllCertificateTypes_Click(object sender, EventArgs e)
        {
            List<CertificateType> list = BLCertificateType.GetAll();
            if (list == null)
                MessageBox.Show("failed");
            else 
                MessageBox.Show(list.Count.ToString());
        }

        private void btAddCertificateType_Click(object sender, EventArgs e)
        {
            CertificateType member = new CertificateType();
            member.CertificateTypeNmae = "驾驶证2";
            member.Description = "驾驶证2";
            //member.CertificateTypeId = 100;

            int rows = BLCertificateType.Add(member);
            MessageBox.Show(rows.ToString());
        }

        private void btRemoveCertificateType_Click(object sender, EventArgs e)
        {
            CertificateType member = BLCertificateType.GetAll().Last();

            int rows = BLCertificateType.Remove(member);
            MessageBox.Show(rows.ToString());
        }

        private void btUpdateCertificateType_Click(object sender, EventArgs e)
        {
            CertificateType member = BLCertificateType.GetAll().Last();
            member.CertificateTypeNmae = member.CertificateTypeNmae + "Update";
            int rows = BLCertificateType.Update(member);
            MessageBox.Show(rows.ToString());
        }

        private void btGetFamily_Click(object sender, EventArgs e)
        {
            Family member = BLFamily.Get(1);
            if (member != null)
                MessageBox.Show(member.FamilyId.ToString());
        }
    }
}
