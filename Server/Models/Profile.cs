using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Server.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }
}
