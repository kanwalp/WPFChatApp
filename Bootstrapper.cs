using Prism.Ioc;
using Prism.Unity;
using System;
using System.Windows;
namespace ChatApplication
{
    public class Bootstrapper : PrismBootstrapper
    {
        public Bootstrapper()
        {
        }

        protected override DependencyObject CreateShell()
        {
            var shell = Container.Resolve<MainWindow>();
            return shell;
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
