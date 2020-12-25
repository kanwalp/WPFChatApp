namespace ChatApplication.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dbo.Messages")]
    public partial class Message
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Column("Message")]
        [Required]
        public string Message1 { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
