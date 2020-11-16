
using LegendaryData.Repositories;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LegendaryData
{
    static class Program
    {
        private static Container container;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Bootstrap();
            Application.Run(container.GetInstance<Form1>());
        }

        private static void Bootstrap()
        {
            // Create the container as usual.
            container = new Container();

            // Register your types, for instance:
            container.Register<IRepositoryAuthor, RepositoryAuthor>(Lifestyle.Singleton);
            container.Register<IRepositoryHeroes, RepositoryHeroes>(Lifestyle.Singleton);
            container.Register<IRepositoryMasterminds, RepositoryMasterminds>(Lifestyle.Singleton);
            container.Register<IRepositoryTeams, RepositoryTeams>(Lifestyle.Singleton);
            container.Register<IRepositoryVillains, RepositoryVillains>(Lifestyle.Singleton);
            container.Register<IRepositoryHenchmen, RepositoryHenchmen>(Lifestyle.Singleton);

            //container.Register<IUserContext, WinFormsUserContext>();
            container.Register<Form1>();
            var registration = container.GetRegistration(typeof(Form1)).Registration;

            registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "whatever");

            // Optionally verify the container.
            container.Verify();
        }
    }
}
