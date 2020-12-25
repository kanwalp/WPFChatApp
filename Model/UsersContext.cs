namespace ChatApplication.Model
{
    using System.Data.Entity;

    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("name=UsersContext")
        {
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}