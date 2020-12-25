using ChatApplication.BusinessServices;
using ChatApplication.Model;
using ChatApplication.ViewModel;
using ChatApplication.ViewModels;
using System.Windows;
using TableDependency.SqlClient;
using Unity;

namespace ChatApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private SqlTableDependency<Message> _sqlTableDependency;
        UnityContainer _container;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new UnityContainer();
            RegisterTypes();
            Bootstrapper bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }

        private void RegisterTypes()
        {
            _container.RegisterType<IMainViewModel, MainViewModel>();
            _container.RegisterType<IBusinessContext, BusinessContext>();
        }

        protected virtual void OnExit(ExitEventArgs e)
        {
            _sqlTableDependency.Stop();
        }
    }
}
