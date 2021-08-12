using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiktok_organizer.Models
{
    public class Video
    {
        public string VideoLink { get; set; }

        public Bitmap VideoThumb { get; set; }

        public string category;
    }
}
