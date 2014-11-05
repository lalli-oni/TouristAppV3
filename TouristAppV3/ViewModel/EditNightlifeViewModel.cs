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
            LoadEditSelectionList();
            _editSelectedNightlifeModel = new RelayCommand(EditSelectedNightlife);
        }
        #endregion

        private async void EditSelectedNightlife()
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

            Stream editStream = await file.OpenStreamForWriteAsync();
            XDocument editnightlifeDocument = XDocument.Load(editStream);


            var targetElement = editnightlifeDocument.Element("nightlifemodels")
                .Elements("nightlifemodel")
                .Where(e => e.Element("name").Value == selectedName).Single();
            targetElement.Element("name").Value = SelectedNightlifeModel.Name;
            targetElement.Element("address").Value = SelectedNightlifeModel.Address;
            targetElement.Element("description").Value = SelectedNightlifeModel.Description;
            targetElement.Element("url").Value = SelectedNightlifeModel.Url;

            await editStream.FlushAsync();
            editnightlifeDocument.Save(editStream);
            editStream.Dispose();

                //(new XElement("name",SelectedNightlifeModel.Name),
                //new XElement("address",SelectedNightlifeModel.Address),
                //new XElement("description",SelectedNightlifeModel.Description),
                //new XElement("url",SelectedNightlifeModel.Url));

            //IEnumerable<XElement> editnightlifelist = editnightlifeDocument.Descendants("nightlifemodel");

            //XElement addedNightlife = new XElement("nightlifemodel");
            //addedNightlife.Add(new XElement("name", SelectedNightlifeModel.Name));
            //addedNightlife.Add(new XElement("address", SelectedNightlifeModel.Address));
            //addedNightlife.Add(new XElement("description", SelectedNightlifeModel.Description));
            //addedNightlife.Add(new XElement("url", SelectedNightlifeModel.Url));

            //foreach (XElement xElement in editnightlifelist)
            //{
            //    if (xElement.DescendantsAndSelf().Any(e => e.Value == selectedName))
            //    {
            //        xElement.Descendants().Remove();
            //        xElement.Add(addedNightlife.Descendants());
            //        break;

            //    }
            //}

            //StorageFile saveFile = null;

            //try
            //{
            //    saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("nightlife.xml");
            //}
            //catch (Exception)
            //{
            //}

            //if (saveFile == null)
            //{
            //    saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("nightlife.xml");
            //}

            //Stream saveStream = await saveFile.OpenStreamForWriteAsync();

            //editnightlifeDocument.Save(saveStream);
            
            ////This breakpoint corrupts when removing chars
            //await saveStream.FlushAsync();
            //saveStream.Dispose();
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

            if (SelectedNightlifeModel == null)
            {
                SelectedNightlifeModel = NightlifeList[0];
            }
            OnPropertyChanged("NightlifeList");

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
