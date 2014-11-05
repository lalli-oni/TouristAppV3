using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TouristAppV3.Annotations;

namespace TouristAppV3.Model
{
    class RestaurantModel : PlacesModel
    {
        #region Variables

        public PlacesModel _name;
        public PlacesModel _address;
        public PlacesModel _url;
        public PlacesModel _description;


        //private string _name;
        //private string _address;
        //private string _url;
        //private string _description;

        #endregion
        #region INotify

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
