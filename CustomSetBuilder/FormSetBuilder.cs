using CustomSetBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomSetBuilder
{
    public partial class FormSetBuilder : Form
    {
        List<UCPicturePage> ucPicturePage = new List<UCPicturePage>();
        UCPicturePage selectedPage;

        public FormSetBuilder()
        {
            InitializeComponent();
        }

        private void FormSetBuilder_Load(object sender, EventArgs e)
        {

        }

        private void btnDirectorySelector_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                ChooseFolder(folderBrowserDialog1.SelectedPath);
            }
        }

        public void ChooseFolder(string selectedPath)
        {

            ListDirectory(treeViewFolders, selectedPath);
        }

        private void ListDirectory(TreeView treeView, string path)
        {
            treeView.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(path);
            treeView.Nodes.Add(CreateDirectoryNode(rootDirectoryInfo));
        }

        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);

            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));
            }

            int imageCount = 0;
            foreach (var file in directoryInfo.GetFiles())
            {
                if (file.Extension == ".jpg" || file.Extension == ".png")
                {
                    var imgNode = new TreeNode(file.Name);
                    imgNode.Tag = file.FullName;
                    imgNode.ImageIndex = 2;

                    directoryNode.Nodes.Add(imgNode);
                    imageCount++;
                }
            }
            if (imageCount > 0)
                directoryNode.ExpandAll();

            return directoryNode;
        }
    }
}
