using ChatApplication.Model;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;

namespace ChatApplication.BusinessServices
{
    public class BusinessContext : IBusinessContext
    {
        private UsersContext _dataContext;
        public BusinessContext()
        {
            _dataContext = new UsersContext();
        }

        public void AddMessagesEntity(Message message)
        {
            _dataContext.Messages.Add(message);
            _dataContext.SaveChanges();
        }

        public void InsertToTriggerChat(Message message)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["UsersContext"].ConnectionString.ToString()))
            {
                conn.Open();

                string strQueryUpdate = "INSERT INTO Messages (UserId, Message, ReceiverId, CreatedOn) VALUES('{0}', '{1}', '{2}', GETDATE())";
                using (SqlCommand cm = new SqlCommand(string.Format(strQueryUpdate, message.UserId, message.Message1, message.ReceiverId), conn))
                {
                    cm.ExecuteNonQuery();
                }
            }
        }

        public void InitiateContext()
        {
            _dataContext = new UsersContext();
        }

        public void dispose()
        {
            _dataContext = null;
        }

        public DbSet<Message> GetMessages()
        {
            return _dataContext.Messages;
        }

        public DbSet<Users> GetUsers()
        {
            return _dataContext.Users;
        }
    }
}
