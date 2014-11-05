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
        private StorageFile file = null;
        private string _selectedName = "loading";

        #region Constructor

        public EditNightlifeViewModel()
        {
            _nightlifeList = new ObservableCollection<NightlifeModel>();
            _editSelectedNightlifeModel = new RelayCommand(EditSelectedNightlife);
            LoadEditSelectionList();
        }
        #endregion

        private async void EditSelectedNightlife()
        {

            NightlifeModel editedNightlifeModel = new NightlifeModel();
            editedNightlifeModel.Name = SelectedNightlifeModel.Name;
            editedNightlifeModel.Address = SelectedNightlifeModel.Address;
            editedNightlifeModel.Description = SelectedNightlifeModel.Description;
            editedNightlifeModel.Url = SelectedNightlifeModel.Url;

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
            XDocument editnightlifeDocument = XDocument.Load(loadStream);

            loadStream.Dispose();

            IEnumerable<XElement> editnightlifelist = editnightlifeDocument.Descendants("nightlifemodel");

            XElement addedNightlife = new XElement("nightlifemodel");
            addedNightlife.Add(new XElement("name", editedNightlifeModel.Name));
            addedNightlife.Add(new XElement("address", editedNightlifeModel.Address));
            addedNightlife.Add(new XElement("description", editedNightlifeModel.Description));
            addedNightlife.Add(new XElement("url", editedNightlifeModel.Url));

            foreach (XElement xElement in editnightlifelist)
            {
                if (xElement.DescendantsAndSelf().Any(e => e.Value == selectedName))
                {
                    xElement.ReplaceWith(addedNightlife);

                    StorageFile saveFile = null;

                    try
                    {
                        saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("nightlife.xml");
                    }
                    catch (Exception)
                    {
                    }

                    if (saveFile == null)
                    {
                        saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("nightlife.xml");
                    }

                    Stream saveStream = await saveFile.OpenStreamForWriteAsync();
                    editnightlifeDocument.Save(saveStream);
                    await saveStream.FlushAsync();
                    saveStream.Dispose();
                    break;
                }
            }
        }

        private async void LoadEditSelectionList()
        {
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

            IEnumerable<XElement> nightlifelist = nightlifeDocument.Descendants("nightlifemodel");

            foreach (XElement xElement in nightlifelist)
            {
                NightlifeModel a = new NightlifeModel();
                a.Name = xElement.Element("name").Value;
                a.Address = xElement.Element("address").Value;
                a.Description = xElement.Element("description").Value;
                a.Url = xElement.Element("url").Value;
                _nightlifeList.Add(a);
            }
            OnPropertyChanged("NightlifeList");

            if (SelectedNightlifeModel == null)
            {
                SelectedNightlifeModel = NightlifeList[0];
            }

        }

        #region Properties
        public NightlifeModel SelectedNightlifeModel
        {
            get { return _selectedNightlifeModel; }
            set
            {

                _selectedNightlifeModel = value;
                if (_selectedNightlifeModel != null)
                {
                    _selectedName = _selectedNightlifeModel.Name;
                }
                OnPropertyChanged("SelectedNightlifeModel");
            }
        }

        public string selectedName
        {
            get { return _selectedName; }
            set { _selectedName = value; }
        }

        public ICommand EditSelectedNightlifeModel
        {
            get { return _editSelectedNightlifeModel; }
            set
            {
                _editSelectedNightlifeModel = value;
                OnPropertyChanged("SelectedNightlifeModel");
            }
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
