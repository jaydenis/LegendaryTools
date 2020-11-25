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
    public partial class Form1 : Form
    {
        private readonly IRepositoryAuthor repositoryAuthors;
        private readonly IRepositoryHeroes repositoryHeroes;
        private readonly IRepositoryMasterminds repositoryMasterminds;
        private readonly IRepositoryTeams repositoryTeams;
        private readonly IRepositoryHenchmen repositoryHenchmen;
        private readonly IRepositoryVillains  repositoryVillains;

        //List<AuthorViewModel> authorViewModels = new List<AuthorViewModel>();
        //List<Stat_Author> authorsList = new List<Stat_Author>();
        //List<Heroes> heroesList = new List<Heroes>();
        List<Stat_Affiliation> teamList = new List<Stat_Affiliation>();
        //List<Masterminds> mastermindsList = new List<Masterminds>();
        //List<VillainGroups> villainsList = new List<VillainGroups>();
        //List<Henchmen> henchmenList = new List<Henchmen>();

        GenericDictionary genericStore = new GenericDictionary();
        GlobalStore globalStore = new GlobalStore();

        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "LegendaryData";
        UserCredential credential;
        String spreadsheetId = "1wE0LfYO94jlEcLapHUQ7zSv1VwyC4HxMa5EYgQc1zMI";
        SheetsService service;

        string searchFilter = "ALL";
        string _currentBGGLink { get; set; }

        public Form1(IRepositoryAuthor repositoryAuthors, IRepositoryHeroes repositoryHeroes,  IRepositoryMasterminds repositoryMasterminds, IRepositoryTeams repositoryTeams, IRepositoryVillains repositoryVillains, IRepositoryHenchmen repositoryHenchmen)
        {
            this.repositoryAuthors = repositoryAuthors;
            this.repositoryHeroes = repositoryHeroes;
            this.repositoryMasterminds = repositoryMasterminds;
            this.repositoryTeams = repositoryTeams;
            this.repositoryVillains = repositoryVillains;
            this.repositoryHenchmen = repositoryHenchmen;

            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            SetupTree();

            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                //Console.WriteLine("Credential file saved to: " + credPath);
            }


            // Create Google Sheets API service.
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });



            LegendarySettings settings = LegendarySettings.Load();
            settings.HAS_BEEN_UPDATED = false;
            settings.Save();
            if (settings.LAST_UPDATED.HasValue)
            {
                DateTime lastUpdated = settings.LAST_UPDATED.Value;
                lblLastUpdated.Text = $"Last Updated:{lastUpdated.ToShortDateString()} - {lastUpdated.ToShortTimeString()}";
            }
            else
            {
                lblLastUpdated.Text = "Not Updated";
            }
        }

        private bool FullDataSync(bool overrideSync = false)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                bool doSync = false;
                LegendarySettings settings = LegendarySettings.Load();

                if (settings.LAST_UPDATED.HasValue)
                {
                    DateTime lastUpdated = settings.LAST_UPDATED.Value;
                    DateTime dateNow;
                    TimeSpan difference;

                    
                    dateNow = DateTime.Now;
                    difference = dateNow - lastUpdated;

                    if (difference.Days > 1)
                        doSync = true;
                }
                else
                {
                    doSync = true;                    
                }

                if (doSync || overrideSync)
                {
                    richTextBox1.Text = $"Update Started{System.Environment.NewLine}";

                    richTextBox1.Text += $"Updating Authors{System.Environment.NewLine}";
                    SyncAuthors();

                    richTextBox1.Text += $"Updating Heroes{System.Environment.NewLine}";
                    SyncHeroes();

                    richTextBox1.Text += $"Updating Masterminds{System.Environment.NewLine}";
                    SyncMasterminds();

                    richTextBox1.Text += $"Updating Teams{System.Environment.NewLine}";
                    SyncTeams();

                    richTextBox1.Text += $"Updating Villians{System.Environment.NewLine}";
                    SyncVillains();

                    richTextBox1.Text += $"Updating Henchmen{System.Environment.NewLine}";
                    SyncHenchmen();

                    richTextBox1.Text += $"Update Completed{System.Environment.NewLine}";

                    settings.HAS_BEEN_UPDATED = true;
                    settings.LAST_UPDATED = DateTime.Now;

                    DateTime lastUpdated = settings.LAST_UPDATED.Value;
                    lblLastUpdated.Text = $"Last Updated:{lastUpdated.ToShortDateString()} - {lastUpdated.ToShortTimeString()}";


                    MessageBox.Show("Remote Data Sync Complete!");

                    settings.Save();
                }              
            }
            catch (Exception ex)
            {
                richTextBox1.Text = $"ERROR!!!{System.Environment.NewLine}{ex.ToString()}";
                MessageBox.Show(ex.ToString());
                return false;
            }
            this.Cursor = Cursors.Default;
            return true;

        }

        private void btnSyncData_Click(object sender, EventArgs e)
        {
            FullDataSync(true);
        }

        private void SetupTree()
        {

            treeView1.Nodes.Clear();
            TreeNode treeRoot = new TreeNode("Legendary");

            treeRoot.Nodes.Add(new TreeNode("Authors"));
            treeRoot.Nodes.Add(new TreeNode("Heroes"));
            treeRoot.Nodes.Add(new TreeNode("Masterminds"));
            treeRoot.Nodes.Add(new TreeNode("Teams"));
            treeRoot.Nodes.Add(new TreeNode("Villains"));
            treeRoot.Nodes.Add(new TreeNode("Henchmen"));

            treeView1.Nodes.Add(treeRoot);

            this.Cursor = Cursors.Default;
        }

        private TreeNode CreateAuthorsNode(TreeNode treeNode)
        {

            //if (treeNode.Tag != null)
            //{
            //    var authorModel = (AuthorViewModel)treeNode.Tag;
                
            //        authorModel.TeamsList = this.repositoryTeams.GetAll().Result.ToList();

            //    authorModel.HeroesList = this.repositoryHeroes.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
            //    if (authorModel.HeroesList.Count() > 0)
            //    {
            //        var tempTeams = new List<Stat_Affiliation>();
            //        foreach (Heroes hero in authorModel.HeroesList)
            //        {
            //            var t = authorModel.TeamsList.Where(x => x.TeamName == hero.TeamName).FirstOrDefault();
            //            if (t != null)
            //            {
            //                if (!tempTeams.Contains(t))
            //                    tempTeams.Add(t);
            //            }
            //        }
            //        if (tempTeams.Count != 0)
            //        {
            //            TreeNode teamNodeRoot = new TreeNode("Teams");
            //            foreach (Stat_Affiliation team in tempTeams)
            //            {
            //                TreeNode teamNode = new TreeNode(team.TeamName);
            //                teamNode.Tag = team;
            //                teamNodeRoot.Nodes.Add(teamNode);

            //                TreeNode heroNodeRoot = new TreeNode("Heroes");
            //                foreach (Heroes hero in authorModel.HeroesList.Where(x => x.TeamName == team.TeamName))
            //                {
            //                    TreeNode heroNode = new TreeNode(hero.HeroName);
            //                    heroNode.Tag = hero;
            //                    heroNodeRoot.Nodes.Add(heroNode);
            //                }
            //                teamNode.Nodes.Add(heroNodeRoot);
            //            }

            //            treeNode.Nodes.Add(teamNodeRoot);
            //        }
            //    }

            //    authorModel.MastermindList = this.repositoryMasterminds.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
            //    if (authorModel.MastermindList.Count() > 0)
            //    {
            //        TreeNode mastermindNodeRoot = new TreeNode("Masterminds");
            //        foreach (Masterminds mm in authorModel.MastermindList)
            //        {
            //            TreeNode mastermindNode = new TreeNode(mm.MastermindName);
            //            mastermindNode.Tag = mm;
            //            mastermindNodeRoot.Nodes.Add(mastermindNode);
            //        }

            //        treeNode.Nodes.Add(mastermindNodeRoot);
            //    }

            //    authorModel.VillainGroupList = this.repositoryVillains.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
            //    if (authorModel.VillainGroupList.Count() > 0)
            //    {
            //        TreeNode villainsNodeRoot = new TreeNode("Villains");
            //        foreach (VillainGroups vg in authorModel.VillainGroupList)
            //        {
            //            TreeNode villainNode = new TreeNode(vg.VillainGroup);
            //            villainNode.Tag = vg;
            //            villainsNodeRoot.Nodes.Add(villainNode);
            //        }

            //        treeNode.Nodes.Add(villainsNodeRoot);
            //    }

            //    authorModel.HenchmenList = this.repositoryHenchmen.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
            //    if (authorModel.HenchmenList.Count() > 0)
            //    {
            //        TreeNode henchmenNodeRoot = new TreeNode("Henchmen");
            //        foreach (Henchmen hm in authorModel.HenchmenList)
            //        {
            //            TreeNode henchmenNode = new TreeNode(hm.HenchmenName);
            //            henchmenNode.Tag = hm;
            //            henchmenNodeRoot.Nodes.Add(henchmenNode);
            //        }

            //        treeNode.Nodes.Add(henchmenNodeRoot);
            //    }

            //    authorViewModelBindingSource.DataSource = authorModel;
            //}
            return treeNode;
        }

        private TreeNode CreateTeamsNode(TreeNode treeNode)
        {
            if (treeNode.Tag != null)
            {
                //var teamModel = (Stat_Affiliation)treeNode.Tag;
                //if (teamList.Count() == 0)
                //    teamList = this.repositoryTeams.GetAll().Result.ToList();

                //if (heroesList.Count() == 0)
                //    heroesList = this.repositoryHeroes.GetAll().Result.ToList();

                //foreach (Stat_Affiliation team in teamList.Where(x=>x.TeamName == teamModel.TeamName))
                //{                  

                //    TreeNode heroNodeRoot = new TreeNode("Heroes");

                //    foreach (Heroes hero in heroesList.Where(x => x.TeamName == team.TeamName))
                //    {
                //        TreeNode heroNode = new TreeNode(hero.HeroName);
                //        heroNode.Tag = hero;
                //        heroNodeRoot.Nodes.Add(heroNode);
                //    }

                //    if (heroNodeRoot.Nodes.Count > 0)
                //        treeNode.Nodes.Add(heroNodeRoot);

                   
                //}
            }

            return treeNode;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            TreeView tv = (TreeView)sender;
            // Update the mail heading entries
            if (tv.SelectedNode != null)
            {
                // Update the title to match the folder
                lblCurrentDataModel.Text = tv.SelectedNode.Text;

                // Set the data grid to show details from this table
                switch (tv.SelectedNode.Text)
                {
                    case "Heroes":
                        GetHeroes();
                        dataGridMain.DataSource = genericStore.GetValue<List<Heroes>>(tv.SelectedNode.Text);
                        break;
                    case "Authors":
                        GetAuthors();
                        dataGridMain.DataSource = genericStore.GetValue<List<AuthorViewModel>>(tv.SelectedNode.Text);
                        break;
                    case "Teams":
                        GetTeams();
                        dataGridMain.DataSource = genericStore.GetValue<List<Stat_Affiliation>>(tv.SelectedNode.Text);
                        break;
                    case "Masterminds":
                        GetMasterminds();
                        dataGridMain.DataSource = genericStore.GetValue<List<Masterminds>>(tv.SelectedNode.Text);
                        break;
                    case "Villains":
                        GetVillains();
                        dataGridMain.DataSource = genericStore.GetValue<List<VillainGroups>>(tv.SelectedNode.Text);
                        break;
                    case "Henchmen":
                        GetHenchmen();
                        dataGridMain.DataSource = genericStore.GetValue<List<Henchmen>>(tv.SelectedNode.Text);
                        break;
                    default:
                        GetAuthors();
                        dataGridMain.DataSource = genericStore.GetValue<List<Stat_Author>>(tv.SelectedNode.Text);
                        break;
                }
               
                //dataGridMain.DataSource = globalStore.GetFromCache<Heroes>();
            }
            else
            {
                // Update the title to a generic title
                lblCurrentDataModel.Text = "Data";

                // Nothing selected so remove any source from the data grid
                 dataGridMain.DataSource = null;
            }



            //if (e.Node.Text == "Authors" && e.Node.Parent.Text == "Legendary")
            //{
            //    tabControl1.SelectedIndex = 0;
            //    if (e.Node.Nodes.Count == 0)
            //    {
            //        GetAuthors();
            //        if (authorViewModels.Count > 0)
            //        {

            //            authorViewModels.OrderBy(o => o.AuthorName);
            //            foreach (var item in authorViewModels)
            //            {
            //                TreeNode node = new TreeNode(item.AuthorName);

            //                node.Tag = item;
            //                e.Node.Nodes.Add(node);
            //            }
            //        }
            //    }

            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            //if (e.Node.Parent != null)
            //{

            //    if (e.Node.Parent.Text == "Authors")
            //    {
            //        tabControl1.SelectedIndex = 0;
            //        CreateAuthorsNode(e.Node);

            //        this.Cursor = Cursors.Default;
            //        return;
            //    }
            //}

            ////heroes
            //if (e.Node.Text == "Heroes" && e.Node.Parent.Text == "Legendary")
            //{

            //    if (e.Node.Nodes.Count == 0)
            //    {
            //        GetHeroes();

            //        if (heroesList.Count > 0)
            //        {
            //            heroesList.OrderBy(o => o.HeroName);
            //            foreach (var item in heroesList)
            //            {
            //                TreeNode node = new TreeNode(item.HeroName);
            //                node.Tag = item;
            //                e.Node.Nodes.Add(node);
            //            }
            //        }
            //    }
            //    dataGridViewHeroes.DataSource = heroesList;
            //    tabControl1.SelectedIndex = 1;
            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            //if (e.Node.NextNode != null && e.Node.Parent.Text == "Teams")
            //{
            //    dataGridViewHeroes.DataSource = repositoryHeroes.GetAllByTeam(e.Node.Text).Result.ToList();
            //    tabControl1.SelectedIndex = 1;
            //}

            //if (e.Node.Parent != null)
            //{

            //    if (e.Node.Parent.Text == "Heroes")
            //    {
            //        tabControl1.SelectedIndex = 1;
            //        if (e.Node.Tag != null)
            //        {
            //            var heroModel = (Heroes)e.Node.Tag;
            //            GetHeroDetails(heroModel);
            //        }

            //        this.Cursor = Cursors.Default;
            //        return;
            //    }
            //}

            ////teams
            //if (e.Node.Text == "Teams" && e.Node.Parent.Text == "Legendary")
            //{
            //    tabControl1.SelectedIndex = 2;
            //    if (e.Node.Nodes.Count == 0)
            //    {
            //        GetTeams();

            //        if (teamList.Count > 0)
            //        {
            //            teamList.OrderBy(o => o.TeamName);
            //            foreach (var item in teamList)
            //            {
            //                TreeNode node = new TreeNode(item.TeamName);
            //                node.Tag = item;
            //                e.Node.Nodes.Add(CreateTeamsNode(node));
            //            }
            //        }
            //    }
            //    this.Cursor = Cursors.Default;
            //    return;
            //}


            ////masterminds
            //if (e.Node.Text == "Masterminds" && e.Node.Parent.Text == "Legendary")
            //{

            //    if (e.Node.Nodes.Count == 0)
            //    {

            //        GetMasterminds();
            //        if (mastermindsList.Count > 0)
            //        {
            //            mastermindsList.OrderBy(o => o.MastermindName);
            //            foreach (var item in mastermindsList)
            //            {
            //                TreeNode node = new TreeNode(item.MastermindName);

            //                node.Tag = item;
            //                e.Node.Nodes.Add(node);
            //            }
            //        }
            //    }
            //    dataGridViewMasterminds.DataSource = mastermindsList;
            //    tabControl1.SelectedIndex = 3;
            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            //if (e.Node.Parent != null)
            //{

            //    if (e.Node.Parent.Text == "Masterminds")
            //    {
            //        tabControl1.SelectedIndex = 3;
            //        if (e.Node.Tag != null)
            //        {
            //            var mastermindModel = (Masterminds)e.Node.Tag;
            //            GetMastermindDetails(mastermindModel);
            //        }

            //        this.Cursor = Cursors.Default;
            //        return;
            //    }
            //}
            ////villains
            //if (e.Node.Text == "Villains" && e.Node.Parent.Text == "Legendary")
            //{

            //    if (e.Node.Nodes.Count == 0)
            //    {
            //        GetVillains();
            //        if (villainsList.Count > 0)
            //        {
            //            villainsList.OrderBy(o => o.VillainGroup);
            //            foreach (var item in villainsList)
            //            {
            //                TreeNode node = new TreeNode(item.VillainGroup);

            //                node.Tag = item;
            //                e.Node.Nodes.Add(node);
            //            }
            //        }

            //    }
            //    tabControl1.SelectedIndex = 4;
            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            //if (e.Node.Parent != null)
            //{

            //    if (e.Node.Parent.Text == "Villains")
            //    {
            //        tabControl1.SelectedIndex = 4;
            //        if (e.Node.Tag != null)
            //        {
            //            var villainModel = (VillainGroups)e.Node.Tag;
            //            GetVillainDetails(villainModel);
            //        }
            //        this.Cursor = Cursors.Default;
            //        return;
            //    }
            //}
            ////henchmen
            //if (e.Node.Text == "Henchmen" && e.Node.Parent.Text == "Legendary")
            //{
            //    tabControl1.SelectedIndex = 5;
            //    if (e.Node.Nodes.Count == 0)
            //    {
            //        GetHenchmen();
            //        if (henchmenList.Count > 0)
            //        {
            //            henchmenList.OrderBy(o => o.HenchmenName);
            //            foreach (var item in henchmenList)
            //            {
            //                TreeNode node = new TreeNode(item.HenchmenName);

            //                node.Tag = item;
            //                e.Node.Nodes.Add(node);
            //            }
            //        }
            //    }
            //    this.Cursor = Cursors.Default;
            //    return;
            //}

            //if (e.Node.Parent != null)
            //{

            //    if (e.Node.Parent.Text == "Henchmen")
            //    {
            //        tabControl1.SelectedIndex = 5;
            //        if (e.Node.Tag != null)
            //        {
            //            var henchmenModel = (Henchmen)e.Node.Tag;
            //            GetHenchmenDetails(henchmenModel);
            //        }
            //        this.Cursor = Cursors.Default;
            //        return;
            //    }
            //}
            this.Cursor = Cursors.Default;
        }

        private void GetHeroDetails(Heroes heroModel)
        {
            _currentBGGLink = heroModel.BGGLink;
            linkLabel1.Text = _currentBGGLink;
            StringBuilder sb = new StringBuilder();
            sb.Append($"Name: {heroModel.HeroName}{System.Environment.NewLine}");
            sb.Append($"Author: {heroModel.Author}{System.Environment.NewLine}");
            sb.Append($"Expansion: {heroModel.ExpansionName}{System.Environment.NewLine}");
            sb.Append($"Team: {heroModel.TeamName}{System.Environment.NewLine}");
            sb.Append($"Date: {heroModel.Year}{System.Environment.NewLine}");
            sb.Append($"IsOfficial: {heroModel.IsOfficial}{System.Environment.NewLine}");
            richTextBox1.Text = sb.ToString();
        }

        private void GetMastermindDetails(Masterminds mastermindModel)
        {
            _currentBGGLink = mastermindModel.BGGLink;
            linkLabel1.Text = _currentBGGLink;
            StringBuilder sb = new StringBuilder();
            sb.Append($"Name: {mastermindModel.MastermindName}{System.Environment.NewLine}");
            sb.Append($"Author: {mastermindModel.Author}{System.Environment.NewLine}");
            sb.Append($"Expansion: {mastermindModel.ExpansionName}{System.Environment.NewLine}");
            sb.Append($"Universe: {mastermindModel.UniverseName}{System.Environment.NewLine}");
            sb.Append($"Date: {mastermindModel.Year}{System.Environment.NewLine}");
            richTextBox1.Text = sb.ToString();
        }

        private void GetVillainDetails(VillainGroups villainModel)
        {
            _currentBGGLink = villainModel.BGGLink;
            linkLabel1.Text = _currentBGGLink;
            StringBuilder sb = new StringBuilder();
            sb.Append($"Name: {villainModel.VillainGroup}{System.Environment.NewLine}");
            sb.Append($"Author: {villainModel.Author}{System.Environment.NewLine}");
            sb.Append($"Expansion: {villainModel.ExpansionName}{System.Environment.NewLine}");
            sb.Append($"Universe: {villainModel.UniverseName}{System.Environment.NewLine}");
            sb.Append($"Date: {villainModel.Year}{System.Environment.NewLine}");
            richTextBox1.Text = sb.ToString();
        }

        private void GetHenchmenDetails(Henchmen henchmenModel)
        {
            _currentBGGLink = henchmenModel.BGGLink;
            linkLabel1.Text = _currentBGGLink;
            StringBuilder sb = new StringBuilder();
            sb.Append($"Name: {henchmenModel.HenchmenName}{System.Environment.NewLine}");
            sb.Append($"Author: {henchmenModel.Author}{System.Environment.NewLine}");
            sb.Append($"Expansion: {henchmenModel.ExpansionName}{System.Environment.NewLine}");
            sb.Append($"Universe: {henchmenModel.UniverseName}{System.Environment.NewLine}");
            sb.Append($"Date: {henchmenModel.Year}{System.Environment.NewLine}");
            richTextBox1.Text = sb.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_currentBGGLink);
        }

        #region GetData

        private void GetHeroes()
        {                     
            if (!genericStore._dict.ContainsKey("Heroes"))
                genericStore.Add("Heroes", this.repositoryHeroes.GetAll().Result.ToList());
            
        }

        private void GetAuthors()
        {
            if (!genericStore._dict.ContainsKey("Authors"))
            {
                var authorViewList = new List<AuthorViewModel>();
                foreach (var author in this.repositoryAuthors.GetAll().Result.ToList())
                {
                    var authorView = new AuthorViewModel
                    {
                        Name = author.AuthorName,
                        BGGLink = author.BGGLink                        
                    };

                    

                    authorViewList.Add(authorView);
                }
                genericStore.Add("Authors", authorViewList);
            }
            
            //authorViewModelBindingSource.DataSource = authorViewModels;
            //dataGridViewAuthors.DataSource = authorsList;
           
        }

        private void GetTeams()
        {
            if (!genericStore._dict.ContainsKey("Teams"))
            {
                var teamViewList = new List<TeamsViewModel>();
                foreach(var item in this.repositoryTeams.GetAll().Result.ToList())
                {
                    var teamView = new TeamsViewModel
                    {
                        Name = item.TeamName,
                        UniverseName = item.UniverseName,
                        HeroesList = this.repositoryHeroes.GetAllByTeam(item.TeamName).Result.ToList()
                    };

                    //teamList.Add(new Stat_Affiliation { TeamName = item.TeamName, UniverseName = item.UniverseName });
                    teamViewList.Add(teamView);
                    genericStore.Add("TeamView_" + item.TeamName, teamView);
                }

                genericStore.Add("Teams", teamViewList);
            }
           
        }

        

        private void GetMasterminds()
        {
            if (!genericStore._dict.ContainsKey("Masterminds"))
                 genericStore.Add("Masterminds", this.repositoryMasterminds.GetAll().Result.ToList());            
        }

        private void GetVillains()
        {
            if (!genericStore._dict.ContainsKey("Villains"))
            {
                genericStore.Add("Villains", this.repositoryVillains.GetAll().Result.ToList());

                //if (genericStore.GetValue<List<VillainGroups>>("Villains").Count == 0)
                 //   SyncVillains();
            }

           
        }

        private void GetHenchmen()
        {
            if (!genericStore._dict.ContainsKey("Henchmen"))
            {
                genericStore.Add("Henchmen", this.repositoryHenchmen.GetAll().Result.ToList());

                //if (henchmenList.Count == 0)
                 //   SyncHenchmen();
            }
           
            
        }

        private AuthorViewModel GetAuthorDetails(AuthorViewModel author)
        {
            author.HeroesList = this.repositoryHeroes.GetAllByAuthor(author.Name).Result.ToList();
            author.MastermindList = this.repositoryMasterminds.GetAllByAuthor(author.Name).Result.ToList();
            author.VillainGroupList = this.repositoryVillains.GetAllByAuthor(author.Name).Result.ToList();
            author.HenchmenList = this.repositoryHenchmen.GetAllByAuthor(author.Name).Result.ToList();

            var authorTeams = new List<TeamsViewModel>();
            foreach(var item in author.HeroesList)
            {
               
                var teamView = new TeamsViewModel
                {
                    Name = item.TeamName,
                    UniverseName = item.UniverseName,
                    HeroesList = author.HeroesList.Where(x=>x.TeamName == item.Name).ToList()
                };
                if (!authorTeams.Contains(teamView))
                    authorTeams.Add(teamView);
            }

            author.TeamsList = authorTeams;

            return author;

        }
        #endregion

        #region SyncData
        private void SyncAuthors()
        {
            this.Cursor = Cursors.WaitCursor;
            RepositoryAuthor repositoryAuthor = new RepositoryAuthor();

            String range = "Stat_Author!A2:G";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                this.repositoryAuthors.ClearData();
                var authorsList = new List<Stat_Author>();
               
                foreach (var row in values)
                {
                    if (Convert.ToString(row[0]).Length > 0)
                    {
                        var item = new Stat_Author
                        {
                            AuthorName = Convert.ToString(row[0]),
                            BGGLink = "https://boardgamegeek.com/user/" + Convert.ToString(row[0]),

                        };
                        this.repositoryAuthors.Insert(item);
                        authorsList.Add(item);
                    }
                    
                }

                
            }
            
            this.Cursor = Cursors.Default;
        }

        private void SyncTeams()
        {
            this.Cursor = Cursors.WaitCursor;

            String range = "Stat_Affiliation!A3:C";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                this.repositoryTeams.ClearData();
                
                foreach (var row in values)
                {
                    var team = teamList.Where(x => x.TeamName == Convert.ToString(row[0])).FirstOrDefault();
                    if (team == null)
                    {
                        team = new Stat_Affiliation
                        {
                            TeamName = Convert.ToString(row[0]),
                            UniverseName = Convert.ToString(row[1])
                        };


                        if (!teamList.Contains(team))
                        {
                            this.repositoryTeams.Insert(team);
                            teamList.Add(team);
                        }
                    }
                    
                }
            }
            
            this.Cursor = Cursors.Default;
        }

        private void SyncHeroes()
        {
            this.Cursor = Cursors.WaitCursor;

            String range = "Heroes!A3:G";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                var heroesList = new List<Heroes>();
                this.repositoryHeroes.ClearData();
                foreach (var row in values)
                {
                    var item = heroesList.Where(x => x.HeroName == Convert.ToString(row[0])).FirstOrDefault();
                    if (item == null)
                    {
                        item = new Heroes
                        {
                            Author = Convert.ToString(row[2]),
                            HeroName = Convert.ToString(row[0]),
                            BGGLink = Convert.ToString(row[1]),
                            ExpansionName = Convert.ToString(row[3]),
                            Universe = Convert.ToString(row[4]),
                            IsOfficial = Convert.ToString(row[2]) == "Official" ? true : false,

                        };

                        if (item.IsOfficial.Value)
                            item.BGGLink = $"https://marveldbg.wordpress.com/?s={item.HeroName}";

                        if (row.Count >= 6)
                            item.Year = Convert.ToString(row[5]);

                        if (row.Count >= 7)
                        {
                            var team = teamList.Where(x => x.TeamName == Convert.ToString(row[6])).FirstOrDefault();
                            if (team == null)
                            {
                                team = new Stat_Affiliation
                                {
                                    TeamName = Convert.ToString(row[6]),
                                    UniverseName = Convert.ToString(row[4])
                                };
                            }

                            if (!teamList.Contains(team))
                            {
                                this.repositoryTeams.Insert(team);
                            }

                            item.TeamName = team.TeamName;


                            if (!heroesList.Contains(item))
                            {
                                this.repositoryHeroes.Insert(item);
                                heroesList.Add(item);
                            }
                            
                        }

                    }
                   
                }

            }
           

            this.Cursor = Cursors.Default;
        }

        private void SyncMasterminds()
        {
            this.Cursor = Cursors.WaitCursor;

            String range = "Masterminds!A3:F";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                this.repositoryMasterminds.ClearData();
                var mastermindsList = new List<Masterminds>();
                foreach (var row in values)
                {
                    var model = mastermindsList.Where(x => x.MastermindName == Convert.ToString(row[0])).FirstOrDefault();
                    if (model == null)
                    {
                        model = new Masterminds
                        {
                            MastermindName = Convert.ToString(row[0]),
                            BGGLink = Convert.ToString(row[1]),
                            Author = Convert.ToString(row[2]),
                            ExpansionName = Convert.ToString(row[3]),
                            UniverseName = Convert.ToString(row[4]),
                        };

                        if (model.Author == "Official")
                            model.BGGLink = $"https://marveldbg.wordpress.com/?s={model.MastermindName}";

                        if (row.Count > 5)
                            model.Year = Convert.ToString(row[5]);

                        if (!mastermindsList.Contains(model))
                        {
                            this.repositoryMasterminds.Insert(model);
                            mastermindsList.Add(model);
                        }
                        
                    }
                    
                }

              
            }
           
            this.Cursor = Cursors.Default;
        }

        private void SyncVillains()
        {
            this.Cursor = Cursors.WaitCursor;

            String range = "Villain Groups!A3:F";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                this.repositoryVillains.ClearData();
                var villainsList = new List<VillainGroups>();
                foreach (var row in values)
                {
                    var model = villainsList.Where(x => x.VillainGroup == Convert.ToString(row[0])).FirstOrDefault();
                    if (model == null)
                    {
                        model = new VillainGroups
                        {
                            VillainGroup = Convert.ToString(row[0]),
                            BGGLink = Convert.ToString(row[1]),
                            Author = Convert.ToString(row[2]),
                            ExpansionName = Convert.ToString(row[3]),
                            UniverseName = Convert.ToString(row[4]),
                        };

                        if (model.Author == "Official")
                            model.BGGLink = $"https://marveldbg.wordpress.com/?s={model.VillainGroup}";

                        if (row.Count > 5)
                            model.Year = Convert.ToString(row[5]);

                        if (!villainsList.Contains(model))
                        {
                            this.repositoryVillains.Insert(model);
                            villainsList.Add(model);
                        }
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }
        private void SyncHenchmen()
        {
            this.Cursor = Cursors.WaitCursor;

            String range = "Henchmen!A3:F";
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                this.repositoryHenchmen.ClearData();
                var henchmenList = new List<Henchmen>();
                foreach (var row in values)
                {
                    var model = henchmenList.Where(x => x.HenchmenName == Convert.ToString(row[0])).FirstOrDefault();
                    if (model == null)
                    {
                        model = new Henchmen
                        {
                            HenchmenName = Convert.ToString(row[0]),
                            BGGLink = Convert.ToString(row[1]),
                            Author = Convert.ToString(row[2]),
                            ExpansionName = Convert.ToString(row[3]),
                            UniverseName = Convert.ToString(row[4]),
                        };

                        if (model.Author == "Official")
                            model.BGGLink = $"https://marveldbg.wordpress.com/?s={model.HenchmenName}";

                        if (row.Count > 5)
                            model.Year = Convert.ToString(row[5]);

                        if (!henchmenList.Contains(model))
                        {
                            this.repositoryHenchmen.Insert(model);
                            henchmenList.Add(model);
                        }
                    }
                }
            }
            this.Cursor = Cursors.Default;
        }


        #endregion

        private void dataGridViewAuthors_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //{
            //    var row = dataGridViewAuthors.Rows[e.RowIndex];
            //    selectedAuthorViewModel = (AuthorViewModel)row.DataBoundItem;
            //    selectedAuthorViewModel.TeamsList = this.repositoryTeams.GetAll().Result.ToList();
            //    selectedAuthorViewModel.HeroesList = this.repositoryHeroes.GetAllByAuthor(selectedAuthorViewModel.AuthorName).Result.ToList();
            //    selectedAuthorViewModel.MastermindList = this.repositoryMasterminds.GetAllByAuthor(selectedAuthorViewModel.AuthorName).Result.ToList();
            //    selectedAuthorViewModel.VillainGroupList = this.repositoryVillains.GetAllByAuthor(selectedAuthorViewModel.AuthorName).Result.ToList();
            //    selectedAuthorViewModel.HenchmenList = this.repositoryHenchmen.GetAllByAuthor(selectedAuthorViewModel.AuthorName).Result.ToList();

            //    if (selectedAuthorViewModel.HeroesList.Count() > 0)
            //    {
                   
            //        var tempTeams = new List<Stat_Affiliation>();
            //        foreach (Heroes hero in selectedAuthorViewModel.HeroesList)
            //        {
            //            var t = selectedAuthorViewModel.TeamsList.Where(x => x.TeamName == hero.TeamName).FirstOrDefault();
            //            if (t != null)
            //            {
            //                if (!tempTeams.Contains(t))
            //                    tempTeams.Add(t);
            //            }
            //        }
            //        selectedAuthorViewModel.TeamsList = tempTeams;
            //    }

            //     statAffiliationBindingSource.DataSource = selectedAuthorViewModel.TeamsList;
            //    dgAuthorHeroes.DataSource = selectedAuthorViewModel.HeroesList;
            //    mastermindsBindingSource.DataSource = selectedAuthorViewModel.MastermindList;
            //    villainGroupsBindingSource.DataSource = selectedAuthorViewModel.VillainGroupList;
            //    henchmenBindingSource.DataSource = selectedAuthorViewModel.HenchmenList;
            //}
        }

        private void dgAuthorTeams_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dgAuthorTeams.Rows[e.RowIndex];
                var model = (Stat_Affiliation)row.DataBoundItem;
                //heroesBindingSource.DataSource = selectedAuthorViewModel.HeroesList.Where(x=>x.TeamName == model.TeamName).ToList();
                //dgAuthorHeroes.DataSource = selectedAuthorViewModel.HeroesList.Where(x => x.TeamName == model.TeamName).ToList();
            }
        }

        private void dataGridViewHeroes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridViewHeroes.Rows[e.RowIndex];
                var model = (Heroes)row.DataBoundItem;
                GetHeroDetails(model);
            }
            
        }

        private void dataGridViewMasterminds_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridViewMasterminds.Rows[e.RowIndex];
                var model = (Masterminds)row.DataBoundItem;

                GetMastermindDetails(model);
            }
        }

        private void dataGridViewVillains_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridViewVillains.Rows[e.RowIndex];
                var model = (VillainGroups)row.DataBoundItem;

                GetVillainDetails(model);
            }
            
        }

        private void dataGridViewHenchmen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridViewHenchmen.Rows[e.RowIndex];
                var model = (Henchmen)row.DataBoundItem;

                GetHenchmenDetails(model);
            }
        }

        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            
            //dataGridViewHeroes.Columns["ID"].Visible = false;
            //dataGridViewAuthors.Columns["ID"].Visible = false;
            //dataGridViewTeams.Columns["ID"].Visible = false;
            //dataGridViewHenchmen.Columns["ID"].Visible = false;
            //dataGridViewVillains.Columns["ID"].Visible = false;
            //dataGridViewMasterminds.Columns["ID"].Visible = false;
        }

        #region search
        private void toolStripComboSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            searchFilter = toolStripComboSearch.Text;
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            if(toolStripTextSearch.Text.Length >= 3)
            {
                this.Cursor = Cursors.WaitCursor;
                DoSearch(toolStripTextSearch.Text.ToLower());
                this.Cursor = Cursors.Default;
            }
        }

        private void DoSearch(string searchVal)
        {

            if(searchFilter == "Search All")
            {
                dataGridViewHeroes.DataSource = this.repositoryHeroes.SearchByName(searchVal).Result.ToList();
                tabControl1.SelectedIndex = 1;
            }
            else
            {

                switch (searchFilter)
                {

                    case "Authors":
                        dataGridMain.DataSource = this.repositoryAuthors.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 0;
                        break;
                    case "Heroes":
                        dataGridMain.DataSource = this.repositoryHeroes.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 1;
                        break;

                    case "Teams":
                        dataGridMain.DataSource = this.repositoryTeams.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 2;
                        break;

                    case "Masterminds":
                        dataGridMain.DataSource = this.repositoryMasterminds.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 3;
                        break;

                    case "Villain Groups":
                        dataGridMain.DataSource = this.repositoryVillains.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 4;
                        break;

                    case "Henchmen":
                        dataGridMain.DataSource = this.repositoryHenchmen.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 5;
                        break;

                    default:
                        dataGridMain.DataSource = this.repositoryHeroes.SearchByName(searchVal).Result.ToList();
                        tabControl1.SelectedIndex = 1;
                        break;
                }
            }
        }


        #endregion

        private void dataGridMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                var row = dataGridMain.Rows[e.RowIndex];
                switch (lblCurrentDataModel.Text)
                {
                    case "Heroes":
                       
                        break;
                    case "Authors":
                        var authorModel = (AuthorViewModel)row.DataBoundItem;
                        authorModel = GetAuthorDetails(authorModel);
                        dgAuthorTeams.DataSource = authorModel.TeamsList;
                        dgAuthorHeroes.DataSource = authorModel.HeroesList; 
                        tabControl1.SelectedIndex = 0;
                        
                        break;
                    case "Teams":
                        var teamModel = (Stat_Affiliation)row.DataBoundItem;
                        if (genericStore._dict.ContainsKey("TeamView_"+ teamModel.TeamName))
                        {
                            var heroTeamList = genericStore.GetValue<TeamsViewModel>("TeamView_" + teamModel.TeamName).HeroesList;
                            dataGridViewHeroes.DataSource = heroTeamList;
                            tabControl1.SelectedIndex = 1;
                        }
                        break;
                    case "Masterminds":
                        
                        break;
                    case "Villains":
                       
                        break;
                    case "Henchmen":
                        
                        break;
                    default:
                       
                        break;
                }

            }
        }
    }
}
