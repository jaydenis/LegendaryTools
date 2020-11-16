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


        List<Stat_Author> authorsList = new List<Stat_Author>();
        List<Heroes> heroesList = new List<Heroes>();
        List<Stat_Affiliation> teamList = new List<Stat_Affiliation>();
        List<Masterminds> mastermindsList = new List<Masterminds>();
        List<VillainGroups> villainsList = new List<VillainGroups>();
        List<Henchmen> henchmenList = new List<Henchmen>();

        static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        static string ApplicationName = "LegendaryData";
        UserCredential credential;
        String spreadsheetId = "1wE0LfYO94jlEcLapHUQ7zSv1VwyC4HxMa5EYgQc1zMI";
        SheetsService service;

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

            if (treeNode.Tag != null)
            {
                var authorModel = (Stat_Author)treeNode.Tag;
                if (teamList.Count() == 0)
                    teamList = this.repositoryTeams.GetAll().Result.ToList();

                var authorHeroesList = this.repositoryHeroes.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
                if (authorHeroesList.Count > 0)
                {
                    var tempTeams = new List<Stat_Affiliation>();
                    foreach (Heroes hero in authorHeroesList)
                    {
                        var t = teamList.Where(x => x.TeamName == hero.TeamName).FirstOrDefault();
                        if (t != null)
                        {
                            if (!tempTeams.Contains(t))
                                tempTeams.Add(t);
                        }
                    }
                    if (tempTeams.Count != 0)
                    {
                        TreeNode teamNodeRoot = new TreeNode("Teams");
                        foreach (Stat_Affiliation team in tempTeams)
                        {
                            TreeNode teamNode = new TreeNode(team.TeamName);
                            teamNode.Tag = team;
                            teamNodeRoot.Nodes.Add(teamNode);

                            TreeNode heroNodeRoot = new TreeNode("Heroes");
                            foreach (Heroes hero in authorHeroesList.Where(x => x.TeamName == team.TeamName))
                            {
                                TreeNode heroNode = new TreeNode(hero.HeroName);
                                heroNode.Tag = hero;
                                heroNodeRoot.Nodes.Add(heroNode);
                            }
                            teamNode.Nodes.Add(heroNodeRoot);
                        }

                        treeNode.Nodes.Add(teamNodeRoot);
                    }
                }

                var authorMastermindsList = this.repositoryMasterminds.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
                if (authorMastermindsList.Count > 0)
                {
                    TreeNode mastermindNodeRoot = new TreeNode("Masterminds");
                    foreach (Masterminds mm in authorMastermindsList)
                    {
                        TreeNode mastermindNode = new TreeNode(mm.MastermindName);
                        mastermindNode.Tag = mm;
                        mastermindNodeRoot.Nodes.Add(mastermindNode);
                    }

                    treeNode.Nodes.Add(mastermindNodeRoot);
                }

                var authorVillainsList = this.repositoryVillains.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
                if (authorVillainsList.Count > 0)
                {
                    TreeNode villainsNodeRoot = new TreeNode("Villains");
                    foreach (VillainGroups vg in authorVillainsList)
                    {
                        TreeNode villainNode = new TreeNode(vg.VillainGroup);
                        villainNode.Tag = vg;
                        villainsNodeRoot.Nodes.Add(villainNode);
                    }

                    treeNode.Nodes.Add(villainsNodeRoot);
                }

                var authorHenchmenList = this.repositoryHenchmen.GetAllByAuthor(authorModel.AuthorName).Result.ToList();
                if (authorHenchmenList.Count > 0)
                {
                    TreeNode henchmenNodeRoot = new TreeNode("Henchmen");
                    foreach (Henchmen hm in authorHenchmenList)
                    {
                        TreeNode henchmenNode = new TreeNode(hm.HenchmenName);
                        henchmenNode.Tag = hm;
                        henchmenNodeRoot.Nodes.Add(henchmenNode);
                    }

                    treeNode.Nodes.Add(henchmenNodeRoot);
                }
            }
            return treeNode;
        }

        private TreeNode CreateTeamsNode(TreeNode treeNode)
        {
            if (treeNode.Tag != null)
            {
                var teamModel = (Stat_Affiliation)treeNode.Tag;
                if (teamList.Count() == 0)
                    teamList = this.repositoryTeams.GetAll().Result.ToList();

                if (heroesList.Count() == 0)
                    heroesList = this.repositoryHeroes.GetAll().Result.ToList();

                foreach (Stat_Affiliation team in teamList.Where(x=>x.TeamName == teamModel.TeamName))
                {                  

                    TreeNode heroNodeRoot = new TreeNode("Heroes");

                    foreach (Heroes hero in heroesList.Where(x => x.TeamName == team.TeamName))
                    {
                        TreeNode heroNode = new TreeNode(hero.HeroName);
                        heroNode.Tag = hero;
                        heroNodeRoot.Nodes.Add(heroNode);
                    }

                    if (heroNodeRoot.Nodes.Count > 0)
                        treeNode.Nodes.Add(heroNodeRoot);

                   
                }
            }

            return treeNode;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (e.Node.Text == "Authors" && e.Node.Parent.Text == "Legendary")
            {
                tabControl1.SelectedIndex = 0;
                if (e.Node.Nodes.Count == 0)
                {
                    GetAuthors();
                    if (authorsList.Count > 0)
                    {

                        authorsList.OrderBy(o => o.AuthorName);
                        foreach (var item in authorsList)
                        {
                            TreeNode node = new TreeNode(item.AuthorName);

                            node.Tag = item;
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
                this.Cursor = Cursors.Default;
                return;
            }

            if (e.Node.Parent != null)
            {
               
                if (e.Node.Parent.Text == "Authors")
                {
                    tabControl1.SelectedIndex = 0;
                    CreateAuthorsNode(e.Node);

                    this.Cursor = Cursors.Default;
                    return;
                }
            }

            //heroes
            if (e.Node.Text == "Heroes" && e.Node.Parent.Text == "Legendary")
            {
                tabControl1.SelectedIndex = 1;
                if (e.Node.Nodes.Count == 0)
                {
                    GetHeroes();

                    if (heroesList.Count > 0)
                    {
                        heroesList.OrderBy(o => o.HeroName);
                        foreach (var item in heroesList)
                        {
                            TreeNode node = new TreeNode(item.HeroName);
                            node.Tag = item;
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
                dataGridViewHeroes.DataSource = heroesList;
                this.Cursor = Cursors.Default;
                return;
            }

            if (e.Node.NextNode != null && e.Node.Parent.Text == "Teams")
            {
                dataGridViewHeroes.DataSource = repositoryHeroes.GetAllByTeam(e.Node.Text).Result.ToList();
                tabControl1.SelectedIndex = 1;
            }

            if (e.Node.Parent != null)
            {

                if (e.Node.Parent.Text == "Heroes")
                {
                    tabControl1.SelectedIndex = 1;
                    if (e.Node.Tag != null)
                    {
                        var heroModel = (Heroes)e.Node.Tag;
                        GetHeroDetails(heroModel);
                    }

                    this.Cursor = Cursors.Default;
                    return;
                }
            }

            //teams
            if (e.Node.Text == "Teams" && e.Node.Parent.Text == "Legendary")
            {
                tabControl1.SelectedIndex = 2;
                if (e.Node.Nodes.Count == 0)
                {
                    GetTeams();

                    if (teamList.Count > 0)
                    {
                        teamList.OrderBy(o => o.TeamName);
                        foreach (var item in teamList)
                        {
                            TreeNode node = new TreeNode(item.TeamName);
                            node.Tag = item;
                            e.Node.Nodes.Add(CreateTeamsNode(node));
                        }
                    }
                }
                this.Cursor = Cursors.Default;
                return;
            }


            //masterminds
            if (e.Node.Text == "Masterminds" && e.Node.Parent.Text == "Legendary")
            {
                
                if (e.Node.Nodes.Count == 0)
                {
                    tabControl1.SelectedIndex = 3;
                    GetMasterminds();
                    if (mastermindsList.Count > 0)
                    {
                        mastermindsList.OrderBy(o => o.MastermindName);
                        foreach (var item in mastermindsList)
                        {
                            TreeNode node = new TreeNode(item.MastermindName);

                            node.Tag = item;
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
                this.Cursor = Cursors.Default;
                return;
            }

            if (e.Node.Parent != null)
            {
                
                if (e.Node.Parent.Text == "Masterminds")
                {
                    tabControl1.SelectedIndex = 3;
                    if (e.Node.Tag != null)
                    {
                        var mastermindModel = (Masterminds)e.Node.Tag;
                        GetMastermindDetails(mastermindModel);
                    }

                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            //villains
            if (e.Node.Text == "Villains" && e.Node.Parent.Text == "Legendary")
            {
                tabControl1.SelectedIndex = 4;
                if (e.Node.Nodes.Count == 0)
                {
                    GetVillains();
                    if (villainsList.Count > 0)
                    {
                        villainsList.OrderBy(o => o.VillainGroup);
                        foreach (var item in villainsList)
                        {
                            TreeNode node = new TreeNode(item.VillainGroup);

                            node.Tag = item;
                            e.Node.Nodes.Add(node);
                        }
                    }

                }
                this.Cursor = Cursors.Default;
                return;
            }

            if (e.Node.Parent != null)
            {
                
                if (e.Node.Parent.Text == "Villains")
                {
                    tabControl1.SelectedIndex = 4;
                    if (e.Node.Tag != null)
                    {
                        var villainModel = (VillainGroups)e.Node.Tag;
                        GetVillainDetails(villainModel);
                    }
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
            //henchmen
            if (e.Node.Text == "Henchmen" && e.Node.Parent.Text == "Legendary")
            {
                tabControl1.SelectedIndex = 5;
                if (e.Node.Nodes.Count == 0)
                {
                    GetHenchmen();
                    if (henchmenList.Count > 0)
                    {
                        henchmenList.OrderBy(o => o.HenchmenName);
                        foreach (var item in henchmenList)
                        {
                            TreeNode node = new TreeNode(item.HenchmenName);

                            node.Tag = item;
                            e.Node.Nodes.Add(node);
                        }
                    }
                }
                this.Cursor = Cursors.Default;
                return;
            }

            if (e.Node.Parent != null)
            {
                
                if (e.Node.Parent.Text == "Henchmen")
                {
                    tabControl1.SelectedIndex = 5;
                    if (e.Node.Tag != null)
                    {
                        var henchmenModel = (Henchmen)e.Node.Tag;
                        GetHenchmenDetails(henchmenModel);
                    }
                    this.Cursor = Cursors.Default;
                    return;
                }
            }
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
            if(heroesList.Count == 0)
            {
                heroesList = this.repositoryHeroes.GetAll().Result.ToList();
            }


            heroesBindingSource.DataSource = heroesList;
           
        }

        private void GetAuthors()
        {
            if (authorsList.Count == 0)
            {
                authorsList = this.repositoryAuthors.GetAll().Result.ToList();
            }
                     

            dataGridViewAuthors.DataSource = authorsList;
           
        }

        private void GetTeams()
        {
            if (teamList.Count == 0)
            {
                teamList = this.repositoryTeams.GetAll().Result.ToList();
            }
            
            dataGridViewTeams.DataSource = teamList;
            
        }

        private void GetMasterminds()
        {
            if (mastermindsList.Count == 0)
            {
                mastermindsList = this.repositoryMasterminds.GetAll().Result.ToList();
            }
            mastermindsBindingSource.DataSource = mastermindsList;
            
        }

        private void GetVillains()
        {
            if (villainsList.Count == 0)
            {
                villainsList = this.repositoryVillains.GetAll().Result.ToList();

                if (villainsList.Count == 0)
                    SyncVillains();
            }


            villainGroupsBindingSource.DataSource = villainsList;
            
        }

        private void GetHenchmen()
        {
            if (henchmenList.Count == 0)
            {
                henchmenList = this.repositoryHenchmen.GetAll().Result.ToList();

                if (henchmenList.Count == 0)
                    SyncHenchmen();
            }

            henchmenBindingSource.DataSource = henchmenList;
            
        }

        private void GetAuthorDetails()
        {
            //if (treeNode.Tag != null)
            //{
            //    var authorModel = (AuthorModel)treeNode.Tag;

            //    if (authorModel.HeroesList.Count == 0)
            //    {

            //        String range = "Heroes!A3:G";
            //        SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            //        ValueRange response = request.Execute();
            //        IList<IList<Object>> values = response.Values;
            //        if (values != null && values.Count > 0)
            //        {


            //            richTextBox1.Text = $"Records Found:{values.Count}{System.Environment.NewLine}{System.Environment.NewLine}";
            //            richTextBox1.Text += $"Name -  Expansion - Team{System.Environment.NewLine}";
            //            foreach (var row in values)
            //            {
            //                if (authorModel.HeroesCount > 0)
            //                {
            //                    if (Convert.ToString(row[2]) == authorModel.AuthorName && authorModel.HeroesCount > 0)
            //                    {
            //                        var item = heroesList.Where(x => x.HeroName == Convert.ToString(row[0])).FirstOrDefault();
            //                        if (item == null)
            //                        {
            //                            item = new HeroModel
            //                            {
            //                                AuthorName = authorModel.AuthorName,
            //                                HeroName = Convert.ToString(row[0]),
            //                                BGGLink = Convert.ToString(row[1]),
            //                                ExpansionName = Convert.ToString(row[3]),
            //                                Universe = Convert.ToString(row[4]),
            //                                Date = Convert.ToString(row[5]),

            //                                IsOfficial = authorModel.AuthorName == "Official" ? true : false,
            //                            };

            //                            if (row.Count == 7)
            //                            {
            //                                var team = teamList.Where(x => x.TeamName == Convert.ToString(row[6])).FirstOrDefault();

            //                                if (team == null) 
            //                                {
            //                                    team  = new TeamModel{
            //                                        TeamName = Convert.ToString(row[6]),
            //                                        Universe = Convert.ToString(row[4])
            //                                    };
            //                                }

            //                                item.Team = team;
            //                            }

            //                            if (!authorModel.HeroesList.Contains(item))
            //                            {
            //                                authorModel.HeroesList.Add(item);

            //                            }
            //                        }

            //                        // Print columns A and B, which correspond to indices 0 and 6.
            //                        richTextBox1.Text += $"{item.HeroName} - {item.ExpansionName} - {item.Team}{System.Environment.NewLine}";
            //                        //Console.WriteLine("{0}, {1}", row[0], row[6]);
            //                    }

            //                }
            //            }

            //            string json = JsonConvert.SerializeObject(authorModel);
            //            richTextBox1.Text = json;
            //        }
            //        else
            //        {
            //            richTextBox1.Text = "No data found.";
            //        }
            //    }
            //    else
            //    {
            //        richTextBox1.Text = $"Cached Records Found:{authorModel.HeroesList.Count}{System.Environment.NewLine}{System.Environment.NewLine}";
            //        richTextBox1.Text += $"Name -  Expansion - Team{System.Environment.NewLine}";
            //        foreach (var item in authorModel.HeroesList)
            //        {
            //            richTextBox1.Text += $"CACHED -  {item.HeroName} - {item.ExpansionName} - {item.Team}{System.Environment.NewLine}";
            //        }
            //    }

            //    if(authorModel.HeroesList.Count > 0)
            //    {
            //        var tempTeams = new List<TeamModel>();
            //        foreach(HeroModel hero in authorModel.HeroesList)
            //        {
            //            var t = tempTeams.Where(x => x.TeamName == hero.Team.TeamName).FirstOrDefault();
            //            if (t == null)
            //            {
            //                tempTeams.Add(hero.Team);
            //            }
            //        }
            //        TreeNode teamNodeRoot = new TreeNode("Teams");
            //        foreach (TeamModel team in tempTeams)
            //        {
            //            TreeNode teamNode = new TreeNode(team.TeamName);
            //            teamNodeRoot.Nodes.Add(teamNode);

            //            TreeNode heroNodeRoot = new TreeNode("Heroes");
            //            foreach (HeroModel hero in authorModel.HeroesList.Where(x => x.Team.TeamName == team.TeamName))
            //            {
            //                TreeNode heroNode = new TreeNode(hero.HeroName);                         
            //                heroNodeRoot.Nodes.Add(heroNode);
            //            }
            //            teamNode.Nodes.Add(heroNodeRoot);

            //        }

            //        treeNode.Nodes.Add(teamNodeRoot);
            //    }

            //}
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
                authorsList = new List<Stat_Author>();
               
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
    }
}
