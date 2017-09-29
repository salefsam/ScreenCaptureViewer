using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ScreenCaptureViewer
{
    public partial class Form1 : Form
    {
        FolderBrowserDialog fbd = new FolderBrowserDialog();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.Description = "Select Path For All Employees";
            DialogResult result = fbd.ShowDialog();
            if(result==DialogResult.OK || result==DialogResult.Yes)
            {
                txtPath.Text = fbd.SelectedPath;
            }
            else
            {
                MessageBox.Show("Must Select A Valid Path To Continue");
            }
            fbd.Dispose();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            treeView1.Visible = true;
            trackBar2.Visible = true;
            
            trackBar2.SetRange(08, 18);
            lblTimeValue.Text = "8:00 AM";
            
            //Add Directories
            loadDirectories("07_30_00", "08_30_59");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            treeView1.Visible = false;
            trackBar2.Visible = false;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Text.Contains(".png"))
            {
                if (e.Node.Checked)
                {
                    UncheckAllNodes(e.Node.TreeView.Nodes, e.Node);
                    pictureBox1.Image = Image.FromFile(fbd.SelectedPath + "\\" + e.Node.Parent.Text + "\\" + e.Node.Text);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    pictureBox1.Image = null;
                }
            }
            else
            {
                //Checking if it's already checked allows us to break out of the endless loop this creates by setting the node to unchecked (casues the treeview1_aftercheck to kick off again)
                if (e.Node.Checked == true)
                {
                    e.Node.Checked = false;
                }
                else
                {
                    //dont do anything because it's already unchecked.
                }
            }
        }

        private void loadDirectories()
        {
            treeView1.Nodes.Clear();
            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(txtPath.Text);
            var node = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            stack.Push(node);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }
                foreach (var file in directoryInfo.GetFiles())
                    currentNode.Nodes.Add(new TreeNode(file.Name));
            }
            treeView1.Nodes.Add(node);
        }

        private void loadDirectories(string filterStart, string filterEnd)
        {
            treeView1.Nodes.Clear();
            var stack = new Stack<TreeNode>();
            var rootDirectory = new DirectoryInfo(txtPath.Text);
            var node = new TreeNode(rootDirectory.Name) { Tag = rootDirectory };
            stack.Push(node);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                var directoryInfo = (DirectoryInfo)currentNode.Tag;
                foreach (var directory in directoryInfo.GetDirectories())
                {
                    var childDirectoryNode = new TreeNode(directory.Name) { Tag = directory };
                    currentNode.Nodes.Add(childDirectoryNode);
                    stack.Push(childDirectoryNode);
                }
                foreach (var file in directoryInfo.GetFiles())
                //if filename is within +- 30 minutes of filter than add it
                {
                    var tempFilterStart = filterStart.Replace('_', ':');
                    var tempFilterEnd = filterEnd.Replace('_', ':');
                    var tempFile = file.ToString().Replace('_', ':').Remove(file.ToString().IndexOf('.'),4);

                    if((Convert.ToDateTime(tempFile)>Convert.ToDateTime(tempFilterStart))&&(Convert.ToDateTime(tempFile)<Convert.ToDateTime(tempFilterEnd)))
                    {
                        currentNode.Nodes.Add(new TreeNode(file.Name));
                    }
                }
            }
            treeView1.Nodes.Add(node);
            treeView1.ExpandAll();
        }

        public void UncheckAllNodes(TreeNodeCollection nodes, TreeNode currentTreeNode)
        {
            foreach (TreeNode node in nodes)
            {
                if(node != currentTreeNode)
                {
                    node.Checked = false;
                    CheckChildren(node, false, currentTreeNode);
                }
            }
        }

        private void CheckChildren(TreeNode rootNode, bool isChecked, TreeNode currentTreeNode)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                if (node != currentTreeNode)
                {
                    CheckChildren(node, isChecked, currentTreeNode);
                    node.Checked = isChecked;
                }
            }
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            //don't allow folders to be selected....
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if(trackBar2.Value.ToString() == "8")
            {
                lblTimeValue.Text = "8:00 AM";
                loadDirectories("07_30_00", "08_30_59");
            }
            if (trackBar2.Value.ToString() == "9")
            {
                lblTimeValue.Text = "9:00 AM";
                loadDirectories("08_30_00", "09_30_59");
            }
            if (trackBar2.Value.ToString() == "10")
            {
                lblTimeValue.Text = "10:00 AM";
                loadDirectories("09_30_00", "10_30_59");
            }
            if (trackBar2.Value.ToString() == "11")
            {
                lblTimeValue.Text = "11:00 AM";
                loadDirectories("10_30_00", "11_30_59");
            }
            if (trackBar2.Value.ToString() == "12")
            {
                lblTimeValue.Text = "12:00 PM";
                loadDirectories("11_30_00", "12_30_59");
            }
            if (trackBar2.Value.ToString() == "13")
            {
                lblTimeValue.Text = "1:00 PM";
                loadDirectories("12_30_00", "13_30_59");
            }
            if (trackBar2.Value.ToString() == "14")
            {
                lblTimeValue.Text = "2:00 PM";
                loadDirectories("13_30_00", "14_30_59");
            }
            if (trackBar2.Value.ToString() == "15")
            {
                lblTimeValue.Text = "3:00 PM";
                loadDirectories("14_30_00", "15_30_59");
            }
            if (trackBar2.Value.ToString() == "16")
            {
                lblTimeValue.Text = "4:00 PM";
                loadDirectories("15_30_00", "16_30_59");
            }
            if (trackBar2.Value.ToString() == "17")
            {
                lblTimeValue.Text = "5:00 PM";
                loadDirectories("16_30_00", "17_30_59");
            }
            if (trackBar2.Value.ToString() == "18")
            {
                lblTimeValue.Text = "6:00 PM";
                loadDirectories("17_30_00", "18_30_59");
            }
        }
    }
}