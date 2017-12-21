using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SwitchPythonVersion
{
    public partial class Form1 : Form
    {
        private bool loading = true;
        public Form1()
        {
            InitializeComponent();
            lblMsg.Text = string.Empty;
            if (File.Exists("SwitchPythonVersion.config"))
            {
                using (var fs = File.Open("SwitchPythonVersion.config", FileMode.Open))
                {
                    var sr = new StreamReader(fs);
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (line.StartsWith("Py2x"))
                        {
                            this.textBox1.Text = line.Replace("Py2x=", string.Empty);
                        }
                        if (line.StartsWith("Py3x"))
                        {
                            this.textBox2.Text = line.Replace("Py3x=", string.Empty);
                        }
                    }
                    sr.Close();
                }
            }
            loading = false;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox3.Text = LoadPath();
        }
        private void button1_Click(object sender, EventArgs e)
        {

            var TargetPath = textBox1.Text.Trim();
            var OldPath = textBox2.Text.Trim();

            SwitchPy(TargetPath, OldPath);

        }

        private void SwitchPy(string TargetPath, string OldPath)
        {
            if (!string.IsNullOrEmpty(TargetPath) && !string.IsNullOrEmpty(OldPath))
            {
                textBox3.Text = LoadPath();
                var pathList = textBox3.Text.Split(';');

                for (int i = 0; i < pathList.Length; i++)
                {
                    var item = pathList[i];
                    if (item.ToLower().Trim().StartsWith(OldPath.ToLower()))
                    {
                        item = item.Replace(OldPath, TargetPath);
                    }
                    pathList[i] = item;
                }
                var newPathValue = string.Join(";", pathList);
                SysEnvironment.SetSysEnvironment("PATH", newPathValue);
                lblMsg.Text = "PATH Saved! - - " + DateTime.Now.ToString();
                textBox3.Text = LoadPath();
            }
            else
            {
                BB();
            }
        }

        private void BB()
        {
            MessageBox.Show("路径都不写，你想上天啊？放心吧，你写好了，哥会提你保存的。");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var TargetPath = textBox2.Text.Trim();
            var OldPath = textBox1.Text.Trim();

            SwitchPy(TargetPath, OldPath);
        }


        private string LoadPath()
        {
            return SysEnvironment.GetSysEnvironmentByName("PATH");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!loading)
            {
                using (var fs = File.Open("SwitchPythonVersion.config", FileMode.Create))
                {
                    var sw = new StreamWriter(fs);
                    sw.WriteLine("Py2x=" + this.textBox1.Text);
                    sw.WriteLine("Py3x=" + this.textBox2.Text);
                    sw.Close();
                }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SysEnvironment.SetSysEnvironment("PATH",textBox3.Text);
            lblMsg.Text = "PATH Saved! - - " + DateTime.Now.ToString();
        }
    }
}
