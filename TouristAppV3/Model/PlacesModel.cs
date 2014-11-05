using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TouristAppV3.Annotations;

namespace TouristAppV3.Model
{
    internal class PlacesModel : INotifyPropertyChanged
    {

        #region Variables

        public string _name;
        public string _address;
        public string _url;
        public string _description;
        public string _group;

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        #endregion

    public override string ToString()
        {
            return Name.ToString();
        }

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