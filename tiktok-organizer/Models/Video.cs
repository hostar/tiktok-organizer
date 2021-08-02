using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tiktok_organizer.Models
{
    public class Video
    {
        public string VideoLink { get; set; }

        public Image VideoThumb { get; set; }

        public string category;
    }
}
