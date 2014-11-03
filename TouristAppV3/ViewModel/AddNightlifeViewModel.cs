using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Storage;
using TouristAppV3.Annotations;
using TouristAppV3.Common;
using TouristAppV3.Model;

namespace TouristAppV3.ViewModel
{
    class AddNightlifeViewModel : INotifyPropertyChanged
    {
        private NightlifeModel _newNightlife;
        private ICommand _addNewNightlife;

        #region Constructor
        public AddNightlifeViewModel()
        {
            _newNightlife = new NightlifeModel();
            _addNewNightlife = new RelayCommand(AddNightlifeCommand);
        }
        #endregion

        private async void AddNightlifeCommand()
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

            XElement nightlifelist = nightlifeDocument.Element("nightlifemodels");

            XElement nightlife = new XElement("nightlifemodel");
            nightlife.Add(new XElement("name", NewNightlife.Name));
            nightlife.Add(new XElement("address", NewNightlife.Address));
            nightlife.Add(new XElement("description", NewNightlife.Description));
            nightlife.Add(new XElement("url", NewNightlife.Url));

            nightlifelist.LastNode.AddAfterSelf(nightlife);

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
            nightlifeDocument.Save(saveStream);
            await saveStream.FlushAsync();
            saveStream.Dispose();
        }

        #region Properties

        public NightlifeModel NewNightlife
        {
            get { return _newNightlife; }
            set { _newNightlife = value; }
        }

        public ICommand AddNewNightlife
        {
            get { return _addNewNightlife; }
            set { _addNewNightlife = value; }
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
