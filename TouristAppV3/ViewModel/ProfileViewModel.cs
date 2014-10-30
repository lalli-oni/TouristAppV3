using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Storage;
using TouristAppV3.Annotations;
using TouristAppV3.Model;

namespace TouristAppV3.ViewModel
{
    class ProfileViewModel : INotifyPropertyChanged
    {
               #region Class Variables
        private ObservableCollection<ProfileModel> _profiles;
        private ProfileModel _selectedProfileModel;
        private ProfileModel _newProfileModel;
        private ICommand _addNewProfile;
        private ICommand _removeSelectedProfile;
        private ICommand _editProfile;
        #endregion

        #region Constructor Method
        public ProfileViewModel()
        {
            _profiles = new ObservableCollection<ProfileModel>();
            _newProfileModel = new ProfileModel();
            //LoadProfileModels();
            
            _addNewProfile = new RelayCommand(AddProfile);
            _removeSelectedProfile = new RelayCommand(RemoveProfile);
        }
        #endregion

        #region LoadProfileModels()

        private async void LoadProfileModels()
        {

            StorageFile fileProfile = null;

            //try to load nightlifes xml from local storage
            try
            {
                fileProfile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("profiles.xml");
            }
            catch (Exception)
            {

            }

            //if it fails use assets folder
            if (fileProfile == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlfileProfile = @"Assets\xml\profiles.xml";
                fileProfile = await installationFolder.GetFileAsync(xmlfileProfile);
            }

            Stream profileStream = await fileProfile.OpenStreamForReadAsync();
            XDocument profileDocument = XDocument.Load(profileStream);

            IEnumerable<XElement> profileList = profileDocument.Descendants("profilelist");

            
            foreach (XElement xElement in profileList)
            {
                ProfileModel p = new ProfileModel();
                p.profileName = xElement.Element("name").Value;
                p.profilePassword = xElement.Element("password").Value;
                _profiles.Add(p);
            }
            _selectedProfileModel = _profiles[0];
            OnPropertyChanged("Profiles");
            
        }

        #endregion

        #region Property Fields

        public ObservableCollection<ProfileModel> Profiles
        {
            get { return _profiles; }
            set { _profiles = value; }
        }

        private void RemoveProfile()
        {
            _profiles.Remove(_selectedProfileModel);
        }

        private void AddProfile()
        {
            _profiles.Add(_newProfileModel);
            OnPropertyChanged("Profiles");
        }

        public ICommand EditProfile
        {
            get { return _editProfile; }
            set { _editProfile = value; }
        }

        public ICommand RemoveSelectedProfile
        {
            get { return _removeSelectedProfile; }
            set { _removeSelectedProfile = value; }
        }

        public ICommand AddNewProfile
        {
            get { return _addNewProfile; }
            set { _addNewProfile = value; }
        }

        public ProfileModel NewProfileModel
        {
            get { return _newProfileModel; }
            set { _newProfileModel = value; }
        }

        public ProfileModel SelectedProfileModel
        {
            get { return _selectedProfileModel; }
            set
            {
                _selectedProfileModel = value;
                OnPropertyChanged("SelectedProfileModel");
            }
        }
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
