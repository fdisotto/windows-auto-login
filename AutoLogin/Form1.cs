using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AutoLogin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var key = root.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon", true))
                {
                    try
                    {
                        var username = key.GetValue("DefaultUserName");
                        txtUsername.Text = username.ToString();
                    } catch (Exception ex)
                    {
                        txtUsername.Text = "";
                    }

                    try
                    {
                        var password = key.GetValue("DefaultPassword");
                        txtPassword.Text = password.ToString();
                    }
                    catch (Exception ex)
                    {
                        txtPassword.Text = "";
                    }

                    try
                    {
                        var auto = key.GetValue("AutoAdminLogon");
                        ckAutoLogin.Checked = auto.ToString() == "1";
                    }
                    catch (Exception ex)
                    {
                        ckAutoLogin.Checked = false;
                    }
                };
            };
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                using (var key = root.OpenSubKey(@"Software\Microsoft\Windows NT\CurrentVersion\Winlogon", true))
                {
                    var username = txtUsername.Text;
                    var password = txtPassword.Text;
                    var auto = ckAutoLogin.Checked;

                    try
                    {
                        key.SetValue("DefaultUserName", username, RegistryValueKind.String);
                        key.SetValue("DefaultPassword", password, RegistryValueKind.String);
                        key.SetValue("AutoAdminLogon", auto ? "1" : "0", RegistryValueKind.String);

                        MessageBox.Show("Successfully saved!", "Nice!", MessageBoxButtons.OK, MessageBoxIcon.None);
                    } catch (Exception ex)
                    {
                        MessageBox.Show("Error during save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
