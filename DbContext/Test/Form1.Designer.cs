
namespace FRS.DatabaseContextTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btInit = new System.Windows.Forms.Button();
            this.btLogin = new System.Windows.Forms.Button();
            this.btGetAllCertificateTypes = new System.Windows.Forms.Button();
            this.btAddCertificateType = new System.Windows.Forms.Button();
            this.btRemoveCertificateType = new System.Windows.Forms.Button();
            this.btUpdateCertificateType = new System.Windows.Forms.Button();
            this.btGetFamily = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btInit
            // 
            this.btInit.Location = new System.Drawing.Point(2, 12);
            this.btInit.Name = "btInit";
            this.btInit.Size = new System.Drawing.Size(166, 48);
            this.btInit.TabIndex = 0;
            this.btInit.Text = "init";
            this.btInit.UseVisualStyleBackColor = true;
            this.btInit.Click += new System.EventHandler(this.btInit_Click);
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(171, 12);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(166, 48);
            this.btLogin.TabIndex = 1;
            this.btLogin.Text = "Login";
            this.btLogin.UseVisualStyleBackColor = true;
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // btGetAllCertificateTypes
            // 
            this.btGetAllCertificateTypes.Location = new System.Drawing.Point(2, 66);
            this.btGetAllCertificateTypes.Name = "btGetAllCertificateTypes";
            this.btGetAllCertificateTypes.Size = new System.Drawing.Size(166, 48);
            this.btGetAllCertificateTypes.TabIndex = 2;
            this.btGetAllCertificateTypes.Text = "GetAllCertificateTypes";
            this.btGetAllCertificateTypes.UseVisualStyleBackColor = true;
            this.btGetAllCertificateTypes.Click += new System.EventHandler(this.btGetAllCertificateTypes_Click);
            // 
            // btAddCertificateType
            // 
            this.btAddCertificateType.Location = new System.Drawing.Point(174, 66);
            this.btAddCertificateType.Name = "btAddCertificateType";
            this.btAddCertificateType.Size = new System.Drawing.Size(166, 48);
            this.btAddCertificateType.TabIndex = 3;
            this.btAddCertificateType.Text = "AddCertificateType";
            this.btAddCertificateType.UseVisualStyleBackColor = true;
            this.btAddCertificateType.Click += new System.EventHandler(this.btAddCertificateType_Click);
            // 
            // btRemoveCertificateType
            // 
            this.btRemoveCertificateType.Location = new System.Drawing.Point(346, 66);
            this.btRemoveCertificateType.Name = "btRemoveCertificateType";
            this.btRemoveCertificateType.Size = new System.Drawing.Size(166, 48);
            this.btRemoveCertificateType.TabIndex = 4;
            this.btRemoveCertificateType.Text = "RemoveCertificateType";
            this.btRemoveCertificateType.UseVisualStyleBackColor = true;
            this.btRemoveCertificateType.Click += new System.EventHandler(this.btRemoveCertificateType_Click);
            // 
            // btUpdateCertificateType
            // 
            this.btUpdateCertificateType.Location = new System.Drawing.Point(528, 66);
            this.btUpdateCertificateType.Name = "btUpdateCertificateType";
            this.btUpdateCertificateType.Size = new System.Drawing.Size(166, 48);
            this.btUpdateCertificateType.TabIndex = 5;
            this.btUpdateCertificateType.Text = "UpdateCertificateType";
            this.btUpdateCertificateType.UseVisualStyleBackColor = true;
            this.btUpdateCertificateType.Click += new System.EventHandler(this.btUpdateCertificateType_Click);
            // 
            // btGetFamily
            // 
            this.btGetFamily.Location = new System.Drawing.Point(12, 188);
            this.btGetFamily.Name = "btGetFamily";
            this.btGetFamily.Size = new System.Drawing.Size(156, 41);
            this.btGetFamily.TabIndex = 6;
            this.btGetFamily.Text = "GetFamily";
            this.btGetFamily.UseVisualStyleBackColor = true;
            this.btGetFamily.Click += new System.EventHandler(this.btGetFamily_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btGetFamily);
            this.Controls.Add(this.btUpdateCertificateType);
            this.Controls.Add(this.btRemoveCertificateType);
            this.Controls.Add(this.btAddCertificateType);
            this.Controls.Add(this.btGetAllCertificateTypes);
            this.Controls.Add(this.btLogin);
            this.Controls.Add(this.btInit);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btInit;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.Button btGetAllCertificateTypes;
        private System.Windows.Forms.Button btAddCertificateType;
        private System.Windows.Forms.Button btRemoveCertificateType;
        private System.Windows.Forms.Button btUpdateCertificateType;
        private System.Windows.Forms.Button btGetFamily;
    }
}

