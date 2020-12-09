using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace JobBoard.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required]
        public string SenderName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string SenderEmail { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string ReceiverEmail { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string ReceiverName { get; set; }

        public string Subject { get; set; }

        [Required]
        [DisplayFormat(NullDisplayText = "*Message cannot be empty")]
        [UIHint("MultilineText")]
        public string Message { get; set; }
    }
}