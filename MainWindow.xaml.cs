using ChatApplication.EventAggregator;
using ChatApplication.ViewModel;
using Prism.Events;
using System;
using System.Windows;

namespace ChatApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            eventAggregator.GetEvent<EventMessage>().Subscribe(MainWindow_UpdateChatList);
            this.DataContext = new MainViewModel();
        }

        private void MainWindow_UpdateChatList(string obj)
        {
            this.ChatListBox.Items.Refresh();
        }

    }

}
