using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using MapKit;
using UIKit;

namespace Haritalar.iOS
{
    public class OzelMKAnnotationView:MKAnnotationView
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public OzelMKAnnotationView(IMKAnnotation annotation, string id) : base(annotation, id)
        {

        }
    }
}