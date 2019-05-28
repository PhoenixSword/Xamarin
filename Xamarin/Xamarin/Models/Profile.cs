using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Xamarin.Models
{
    public class Profile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public ImageSource ImageSource { get; set; }
        public byte[] Image { get; set; }
    }
}
