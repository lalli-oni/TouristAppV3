using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using TouristAppV3.Model;

namespace TouristAppV3.ViewModel
{
    class XmlReader
    {
        private readonly StorageFolder _storageFolder = ApplicationData.Current.LocalFolder;
        private readonly StorageFolder _installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        public ObservableCollection<NightlifeModel> populateList;

        public async void XmlRead()
        {
            ObservableCollection<NightlifeModel> _populateList = new ObservableCollection<NightlifeModel>();
            //create stream to file
            Stream readStream = await _storageFolder.OpenStreamForReadAsync("nightlife.xml");

            StreamReader streamReader = new StreamReader(readStream);

            XDocument nightlifeDocument = XDocument.Load(streamReader);

            IEnumerable<XElement> nightlifelist = nightlifeDocument.Descendants("nightlifemodel");

            foreach (XElement xElement in nightlifelist)
            {
                NightlifeModel a = new NightlifeModel();
                a.Name = xElement.Element("name").Value;
                a.Address = xElement.Element("address").Value;
                a.Description = xElement.Element("description").Value;
                a.Url = xElement.Element("url").Value;
                _populateList.Add(a);
            }
            readStream.Dispose();
        }
    }
}
