using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAppV3.Model;

namespace TouristAppV3.ViewModel
{
    class CategoryModel: INotifyCollectionChanged
    {                   
        
            private String _imageUrl;
            private List<PlacesModel> _placesModels;
            public String Name { get; set; }
            public String Category { get; set; }

            public List<PlacesModel> Places
            {
                get { return _placesModels; }
                set { _placesModels = value; }
            }

            public override string ToString()
            {
                return Name.ToString();
            }

            public String ImageUrl
            {
                get { return _imageUrl; }
                set { _imageUrl = value; }
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged;
        
    }
}
