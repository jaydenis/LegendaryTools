using ComponentFactory.Krypton.Navigator;
using ComponentFactory.Krypton.Toolkit;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using LegendaryData.Models;
using LegendaryData.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegendaryData
{
    public partial class DataBrowser : Form
    {
        private readonly IRepositoryAuthor repositoryAuthors;
        private readonly IRepositoryHeroes repositoryHeroes;
        private readonly IRepositoryMasterminds repositoryMasterminds;
        private readonly IRepositoryTeams repositoryTeams;
        private readonly IRepositoryHenchmen repositoryHenchmen;
        private readonly IRepositoryVillains repositoryVillains;
        // Cache the full expanded size of the outlook navigator
        private int _widthLeftRight;

        AuthorViewModel selectedAuthorViewModel = new AuthorViewModel();
        List<AuthorViewModel> authorViewModels = new List<AuthorViewModel>();
        List<Stat_Author> authorsList = new List<Stat_Author>();
        List<Heroes> heroesList = new List<Heroes>();
        List<Stat_Affiliation> teamList = new List<Stat_Affiliation>();
        List<Masterminds> mastermindsList = new List<Masterminds>();
        List<VillainGroups> villainsList = new List<VillainGroups>();
        List<Henchmen> henchmenList = new List<Henchmen>();

        // Set of fake notes entries
        private object[] _notes = new object[]{ new string[] { "Bug Reports v1.2", "Featuer Requests v1.3", "Wish List" },
                                                new string[] { "Xmas List", "Birthday Dates" },
                                                new string[] { "Season Schedule", "Party Invites", "Jokes", "Diary" } };

        public DataBrowser(IRepositoryAuthor repositoryAuthors, IRepositoryHeroes repositoryHeroes, IRepositoryMasterminds repositoryMasterminds, IRepositoryTeams repositoryTeams, IRepositoryVillains repositoryVillains, IRepositoryHenchmen repositoryHenchmen)
        {
            this.repositoryAuthors = repositoryAuthors;
            this.repositoryHeroes = repositoryHeroes;
            this.repositoryMasterminds = repositoryMasterminds;
            this.repositoryTeams = repositoryTeams;
            this.repositoryVillains = repositoryVillains;
            this.repositoryHenchmen = repositoryHenchmen;
            InitializeComponent();
        }
		
		private void DataBrowser_Load(object sender, EventArgs e)
        {


            GetAllData();
            // Set the initial main and detail pages
            kryptonNavigatorMain.SelectedIndex = 0;
            kryptonNavigatorDetails.SelectedIndex = 0;

            // Start with all folders expanded
            treeViewMailFolders.ExpandAll();

            // Set initial focus to the tree view for mail
            treeViewMailFolders.Focus();
            //treeViewMailFolders.SelectedNode = treeViewMailFolders.Nodes[2].Nodes[0];

            // Set the initial set of notes entries
            radioNotes_CheckedChanged(radioProject, EventArgs.Empty);

            kryptonManager.GlobalPaletteMode = PaletteModeManager.Office2010Black;
        }

        #region GetData
        private void GetAllData()
        {
            this.Cursor = Cursors.WaitCursor;
            GetHeroes();
            GetHenchmen();
            GetAuthors();
            GetTeams();
            GetMasterminds();
            GetVillains();
            this.Cursor = Cursors.Default;
        }
        private List<Heroes> GetHeroes()
        {
            if (heroesList.Count == 0)
            {
                heroesList = this.repositoryHeroes.GetAll().Result.ToList();
            }

            foreach (var item in heroesList)
            {
                
                dtHeroes.Rows.Add(item.HeroName,item.BGGLink,item.Author,item.ExpansionName,item.Universe,item.TeamName, item.IsOfficial.Value,item.Year);
            }

            //heroesBindingSource.DataSource = heroesList;

            return heroesList;
        }

        private List<AuthorViewModel> GetAuthors()
        {
            if (authorsList.Count == 0)
            {
                authorsList = this.repositoryAuthors.GetAll().Result.ToList();
                
            }

            foreach (var item in authorsList)
            {
                dtAuthors.Rows.Add(item.AuthorName, item.BGGLink);
            }

            //authorViewModelBindingSource.DataSource = authorViewModels;
            //dataGridViewAuthors.DataSource = authorsList;
            return authorViewModels;
        }

        private List<Stat_Affiliation> GetTeams()
        {
            if (teamList.Count == 0)
            {
                teamList = this.repositoryTeams.GetAll().Result.ToList();
            }

            foreach(var item in teamList)
            {
                dtTeams.Rows.Add(item.TeamName);
            }
            return teamList;
        }

        private List<Masterminds> GetMasterminds()
        {
            if (mastermindsList.Count == 0)
            {
                mastermindsList = this.repositoryMasterminds.GetAll().Result.ToList();
            }
            foreach (var item in mastermindsList)
            {
                dtMasterminds.Rows.Add(item.MastermindName, item.BGGLink, item.Author, item.ExpansionName, item.UniverseName, item.Year);
            }
            return mastermindsList;
        }

        private List<VillainGroups> GetVillains()
        {
            if (villainsList.Count == 0)
            {
                villainsList = this.repositoryVillains.GetAll().Result.ToList();

                //if (villainsList.Count == 0)
                //    SyncVillains();
            }

            foreach (var item in villainsList)
            {
                dtVillainGroups.Rows.Add(item.VillainGroup, item.BGGLink, item.Author, item.ExpansionName, item.UniverseName, item.Year);
            }


            //villainGroupsBindingSource.DataSource = villainsList;
            return villainsList;
        }

        private List<Henchmen> GetHenchmen()
        {
            if (henchmenList.Count == 0)
            {
                henchmenList = this.repositoryHenchmen.GetAll().Result.ToList();

                //if (henchmenList.Count == 0)
                //    SyncHenchmen();
            }

            foreach (var item in henchmenList)
            {
                dtHenchmen.Rows.Add(item.HenchmenName, item.BGGLink, item.Author, item.ExpansionName, item.UniverseName, item.Year);
            }
            return henchmenList;

        }

        #endregion

        private void buttonSpecExpandCollapse_Click(object sender, EventArgs e)
        {
            kryptonSplitContainerMain.SuspendLayout();
            kryptonNavigatorMain.SuspendLayout();

            // Is the navigator currently in full mode?
            if (kryptonNavigatorMain.NavigatorMode == NavigatorMode.OutlookFull)
            {
                // Make the left panel of the splitter fixed in size
                kryptonSplitContainerMain.FixedPanel = FixedPanel.Panel1;
                kryptonSplitContainerMain.IsSplitterFixed = true;

                // Remember the current height of the header group
                _widthLeftRight = kryptonNavigatorMain.Width;

                // Switch to the mini mode
                kryptonNavigatorMain.NavigatorMode = NavigatorMode.OutlookMini;

                // Discover the new width required to display the mini mode
                int newWidth = kryptonNavigatorMain.PreferredSize.Width;

                // Make the header group fixed just as the new height
                kryptonSplitContainerMain.Panel1MinSize = newWidth;
                kryptonSplitContainerMain.SplitterDistance = newWidth;

                // Switch the arrow to point the opposite way
                buttonSpecExpandCollapse.TypeRestricted = PaletteNavButtonSpecStyle.ArrowRight;
            }
            else
            {
                // Switch to the full mode
                kryptonNavigatorMain.NavigatorMode = NavigatorMode.OutlookFull;

                // Make the bottom panel not-fixed in size anymore
                kryptonSplitContainerMain.FixedPanel = FixedPanel.None;
                kryptonSplitContainerMain.IsSplitterFixed = false;

                // Put back the minimum size to the original
                kryptonSplitContainerMain.Panel1MinSize = 100;

                // Calculate the correct splitter we want to put back
                kryptonSplitContainerMain.SplitterDistance = _widthLeftRight;

                // Switch the arrow to point the opposite way
                buttonSpecExpandCollapse.TypeRestricted = PaletteNavButtonSpecStyle.ArrowLeft;
            }

            kryptonSplitContainerMain.ResumeLayout();
            kryptonNavigatorMain.ResumeLayout();
        }

        private void kryptonNavigatorMain_SelectedPageChanged(object sender, EventArgs e)
        {
            // Update the details page to match the main pages
            kryptonNavigatorDetails.SelectedIndex = kryptonNavigatorMain.SelectedIndex;
        }

        private void treeViewMail_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Remove selection from the other tree
            if (sender == treeViewMailFavs)
                treeViewMailFolders.SelectedNode = null;
            else
                treeViewMailFavs.SelectedNode = null;

            // Cast event source to the correct type
            TreeView tv = (TreeView)sender;

            // Update the mail heading entries
            if (tv.SelectedNode != null)
            {
                // Update the title to match the folder
                kryptonPageMailDetails.TextTitle = tv.SelectedNode.Text;

                // Set the data grid to show details from this table
                kryptonDataGridView1.DataSource = dataSet.Tables[tv.SelectedNode.Text];
            }
            else
            {
                // Update the title to a generic title
                kryptonPageMailDetails.TextTitle = "Data";

                // Nothing selected so remove any source from the data grid
                kryptonDataGridView1.DataSource = null;
            }
        }

        private void radioNotes_CheckedChanged(object sender, EventArgs e)
        {
            int index = 0;

            // Find index of note names
            if (radioFamily.Checked)
                index = 1;
            else if (radioFriends.Checked)
                index = 2;

            // Remove all existing notes
            listViewNotes.Items.Clear();

            // Get the set of strings that contain the note names
            string[] group = (string[])_notes[index];

            // Add each mail entry as an item
            foreach (string entry in group)
                listViewNotes.Items.Add(new ListViewItem(entry, 0));
        }

        private void kryptonDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
               //kryptonPageMailDetails.TextTitle
                var row = kryptonDataGridView1.Rows[e.RowIndex];

                kryptonLinkLabel1.Text = Convert.ToString(row.Cells["BGGLink"].Value);
                if (kryptonLinkLabel1.Text != string.Empty)
                    kryptonLinkLabel1.Visible = true;

                lblName.Text = Convert.ToString(row.Cells["Name"].Value);

                lblAuthor.Text = Convert.ToString(row.Cells["Author"].Value);
                if (lblAuthor.Text != string.Empty)
                    lblAuthor.Visible = true;

                lblTeam.Text = Convert.ToString(row.Cells["Team"].Value);
                if (lblTeam.Text != string.Empty)
                    lblTeam.Visible = true;


                //heroesBindingSource.DataSource = selectedAuthorViewModel.HeroesList.Where(x=>x.TeamName == model.TeamName).ToList();
                // dgAuthorHeroes.DataSource = selectedAuthorViewModel.HeroesList.Where(x => x.TeamName == model.TeamName).ToList();
            }
        }
    }
}
