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
    class EditNightlifeViewModel : INotifyPropertyChanged
    {
        private NightlifeModel _selectedNightlifeModel;
        private ICommand _editSelectedNightlifeModel;
        private ObservableCollection<NightlifeModel> _nightlifeList;

        #region Constructor

        public EditNightlifeViewModel()
        {
            _editSelectedNightlifeModel = new RelayCommand(EditSelectedNightlifeModelCommand);
        }
        #endregion

        private async void EditSelectedNightlifeModelCommand()
        {
            StorageFile file = null;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("nightlife.xml");
            }
            catch (Exception)
            {
            }

            if (file == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlFile = @"Assets\xml\allnightlife.xml";
                file = await installationFolder.GetFileAsync(xmlFile);
            }

            Stream loadStream = await file.OpenStreamForReadAsync();
            XDocument nightlifeDocument = XDocument.Load(loadStream);
            loadStream.Dispose();

            IEnumerable<XElement> nightlifelist = nightlifeDocument.Descendants("nightlifemodels");

            foreach (XElement xElement in nightlifelist)
            {
                NightlifeModel e = new NightlifeModel();
                e.Name = xElement.Element("name").Value;
                e.Address = xElement.Element("address").Value;
                e.Description = xElement.Element("description").Value;
                e.Url = xElement.Element("url").Value;
                _nightlifeList.Add(e);
            }

        }

        #region Properties

        public NightlifeModel SelectedNightlifeModel
        {
            get { return _selectedNightlifeModel; }
            set { _selectedNightlifeModel = value; }
        }

        public ICommand EditSelectedNightlifeModel
        {
            get { return _editSelectedNightlifeModel; }
            set { _editSelectedNightlifeModel = value; }
        }

        public ObservableCollection<NightlifeModel> NightlifeList
        {
            get { return _nightlifeList; }
            set { _nightlifeList = value; }
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
