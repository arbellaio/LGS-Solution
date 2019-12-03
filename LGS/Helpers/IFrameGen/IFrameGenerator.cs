using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LGS.Helpers.IFrameGen
{
    public class FrameGenerator
    {
        public static string GenerateIFrame(string location)
        {
            string iFrameTemplate = "<iframe src='https://www.google.com/maps?q=" + location + "&output=embed' style='width: 100%;"+
            "height: 400px';></iframe>";
            return iFrameTemplate;
        }
    }
}