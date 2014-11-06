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
    class ProfileModel : INotifyPropertyChanged
    {
        #region Variables
        private string _profileName;
        private string _profilePassword;

        private int _colourGreen;
        private int _colourRed;
        private int _colourBlue;
        #endregion

        #region Properties
        public string profileName
        {
            get { return _profileName; }
            set { _profileName = value; }
        }

        public string profilePassword
        {
            get { return _profilePassword; }
            set { _profilePassword = value; }
        }

        public int ColourGreen
        {
            get { return _colourGreen; }
            set { _colourGreen = value; }
        }
        public int ColourBlue
        {
            get { return _colourBlue; }
            set { _colourBlue = value; }
        }
        public int ColourRed
        {
            get { return _colourRed; }
            set { _colourRed = value; }
        }
        #endregion

        #region INotifyProp
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
