using ChatApplication.BusinessServices;
using ChatApplication.Command;
using ChatApplication.Model;
using ChatApplication.ViewModels;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Windows.Input;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using Unity;

namespace ChatApplication.ViewModel
{
    public class MainViewModel : IMainViewModel, INotifyPropertyChanged
    {
        public ICommand SendBtnCommand { get; set; }
        public IBusinessContext _context { get; set; }
        private SqlTableDependency<Message> _sqlTableDependency;
        UnityContainer container;
        IEventAggregator ea;
        public MainViewModel()
        {
            container = new UnityContainer();
            _context = container.Resolve<BusinessContext>();
            this.SendBtnCommand = new RelayCommand(ExecuteMethod, CanExecuteMethod);
            ReceiversChat = new ObservableCollection<ReceiversChat>();
            ChatList = new ObservableCollection<ChatList>();
            FetchReceivers();
            SubscribeNotificationService();
        }

        public void SubscribeNotificationService()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["UsersContext"].ConnectionString.ToString();
                var mapper = new ModelToTableMapper<Message>();
                mapper.AddMapping(c => c.Message1, "Message");
                mapper.AddMapping(c => c.ReceiverId, "ReceiverId");
                mapper.AddMapping(c => c.UserId, "UserId");

                _sqlTableDependency = new SqlTableDependency<Message>(connectionString, "Messages", "dbo", mapper);
                _sqlTableDependency.OnChanged += TableDependency_OnChanged;
                _sqlTableDependency.Start();
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "ErrorLog.txt", "error during sql table dependency subscription: " + e.InnerException.Message);
            }
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Message> e)
        {
            if (e.Entity.ReceiverId == Convert.ToInt16(ConfigurationManager.AppSettings["UserID"]))
            {
                SetSelectedReceiver(e.Entity.UserId);
                OnReceiverChanged();
            }
        }

        public MainViewModel(UnityContainer container, IEventAggregator ea)
        {
            ea = container.Resolve<IEventAggregator>();
        }

        public int CurrentUserID
        {
            get
            {
                return Convert.ToInt16(ConfigurationManager.AppSettings["UserID"]);
            }
        }

        public string CurrentUserName
        {
            get
            {
                var username = "";
                if (_context != null && _context.GetUsers().ToList().Count > 0)
                {
                    username = _context.GetUsers().FirstOrDefault(s => s.Id == CurrentUserID).Name;
                }
                return username;
            }
        }


        private ObservableCollection<ReceiversChat> _receiversChat;
        public ObservableCollection<ReceiversChat> ReceiversChat
        {
            get { return _receiversChat; }
            set
            {
                _receiversChat = value;
                OnPropertyChanged("ReceiversChat");
            }
        }


        private ObservableCollection<ChatList> _chatList;
        public ObservableCollection<ChatList> ChatList
        {
            get { return _chatList; }
            set
            {
                _chatList = value;
                OnPropertyChanged("ChatList");
            }
        }

        private ReceiversChat _selectedItem;
        public ReceiversChat SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    SelectedReceiverID = _selectedItem.ReceiverId;
                }
                OnPropertyChanged("SelectedItem");
            }
        }


        private int _selectedReceiverID;
        public int SelectedReceiverID
        {
            get { return _selectedReceiverID; }
            set
            {
                _selectedReceiverID = value;
                if (_selectedReceiverID > 0)
                {
                    OnReceiverChanged();
                }
                OnPropertyChanged("SelectedReceiverID");
            }
        }

        public static int GlobalReceiver { get; set; }

        private string _messageInput;
        public string MessageInput
        {
            get { return _messageInput; }
            set
            {
                _messageInput = value;
                OnPropertyChanged("MessageInput");
            }
        }

        public bool CanExecuteMethod(object parameter)
        {
            return !string.IsNullOrEmpty(MessageInput) && (SelectedReceiverID > 0 || GlobalReceiver > 0);
        }

        public void ExecuteMethod(object parameter)
        {
            if (!string.IsNullOrEmpty(MessageInput))
            {
                var msg = new Message()
                {
                    UserId = CurrentUserID,
                    ReceiverId = GlobalReceiver,
                    Message1 = MessageInput,
                    CreatedOn = DateTime.Now
                };
                _context.InsertToTriggerChat(msg);
                _context.dispose();
                _context.InitiateContext();
                SelectedReceiverID = GlobalReceiver;
                MessageInput = "";
                FetchParticularReceiverChat(SelectedReceiverID);
                OnReceiverChanged();
            }
        }

        public void FetchParticularReceiverChat(int id)
        {
            var user = _context.GetUsers().FirstOrDefault(s => s.Id == id);
            var message = _context.GetMessages().Where(s => s.ReceiverId == id && s.UserId == CurrentUserID).OrderByDescending(s => s.CreatedOn).FirstOrDefault();
            var receiverchat = new ReceiversChat()
            {
                UsersName = user.Name,
                UserIconName = user.IconName,
                ReceiverId = message.ReceiverId,
                UserPreviousMessage = message.Message1,
                UserMessageTime = message.CreatedOn
            };
            if (ReceiversChat.Count > 0)
            {
                var chat = ReceiversChat.FirstOrDefault(s => s.ReceiverId == message.ReceiverId);
                ReceiversChat.Remove(chat);
            }
            ReceiversChat.Add(receiverchat);
        }

        public void FetchReceivers()
        {
            var uniqueReceivers = _context.GetMessages().Where(s => s.UserId == CurrentUserID).Select(s => s.ReceiverId).Distinct().ToList();
            foreach (var item in uniqueReceivers)
            {
                var user = _context.GetUsers().FirstOrDefault(s => s.Id == item);
                _context = container.Resolve<BusinessContext>();
                var message = _context.GetMessages().Where(s => s.ReceiverId == item && s.UserId == CurrentUserID).OrderByDescending(s => s.CreatedOn).FirstOrDefault();
                var receiverchat = new ReceiversChat()
                {
                    UsersName = user.Name,
                    UserIconName = user.IconName,
                    ReceiverId = message.ReceiverId,
                    UserPreviousMessage = message.Message1,
                    UserMessageTime = message.CreatedOn
                };
                ReceiversChat.Add(receiverchat);
            }
        }
        public void SetSelectedReceiver(int ReceiverId)
        {
            this.SelectedReceiverID = ReceiverId;
        }

        public void OnReceiverChanged()
        {
            if (SelectedReceiverID > 0)
            {
                GlobalReceiver = SelectedReceiverID;
                ChatList = new ObservableCollection<ChatList>();
                var receivedMessages = _context.GetMessages().Where(s => s.UserId == CurrentUserID && s.ReceiverId == SelectedReceiverID).ToList();
                var sentMessages = _context.GetMessages().Where(s => s.UserId == SelectedReceiverID && s.ReceiverId == CurrentUserID).ToList();
                receivedMessages.AddRange(sentMessages);
                var chatMessages = receivedMessages.OrderBy(s => s.CreatedOn);
                foreach (var item in chatMessages)
                {
                    var chat = new ChatList()
                    {
                        ChatContent = item.Message1,
                        ChatTime = item.CreatedOn,
                        AlignmentType = item.UserId == CurrentUserID ? AlignmentType.Right.ToString() : AlignmentType.Left.ToString()
                    };
                    ChatList.Add(chat);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



}
