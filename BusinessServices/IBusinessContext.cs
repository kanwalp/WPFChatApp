using ChatApplication.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApplication.BusinessServices
{
    public interface IBusinessContext 
    {
        void AddMessagesEntity(Message message);
        void InsertToTriggerChat(Message message);
        void dispose();
        void InitiateContext();
        DbSet<Message> GetMessages();
        DbSet<Users> GetUsers();
    }
}
