using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Storage;
using Windows.UI.Popups;
using TouristAppV3.Model;
using TouristAppV3.Annotations;

namespace TouristAppV3.ViewModel
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        #region Class Variables
        private ObservableCollection<NightlifeModel> _nightlifes;
        private NightlifeModel _selectedNightlifeModel;
        private NightlifeModel _newNightlifeModel;
        private ICommand _addNewNightlife;
        private ICommand _removeSelectedNightlife;
        private ICommand _editNightlife;

        #endregion

        #region Constructor Method
        public MainViewModel()
        {
            #region Create Nightlife Models(COMMENTED OUT)

            //_nightlifes = new ObservableCollection<NightlifeModel>();

            //NightlifeModel n1 = new NightlifeModel();
            //n1.Name = "Gimle";
            //n1.Address = "Helligkorsvej 2";
            //n1.Description = "Gimle is a student & youth café. " +
            //                     "With young energetic volunteer servers " +
            //                     "the mood is like none other. Gimle also " +
            //                     "hosts concerts with some of the freshest " +
            //                     "artists in denmark and from across the borders.\n \n " +
            //                     "Sizeable beer selection, free wi-fi and fantastic " +
            //                     "music. Gimle sure has a lot to offer.";
            //n1.Url = "http://gimle.dk/";
            //_nightlifes.Add(n1);

            //NightlifeModel n2 = new NightlifeModel();
            //n2.Name = "Roskilde Festival";
            //n2.Address = "Darupvej 19";
            //n2.Description = "Roskilde Festival 2013 had more than 180 performing " +
            //                     "bands and gathered around 130,000 festivalgoers, with" +
            //                     " more than 21,000 volunteers, 5,000 media people and 3,000 " +
            //                     "artists – which means almost 160,000 people participated in" +
            //                     " the festival.\n\n In 1972, the festival was taken over by the " +
            //                     "Roskilde Foundation, which has since run the festival as a " +
            //                     "non-profit organization for development and support of music, " +
            //                     "culture and humanism.";
            //n1.Url = "http://roskilde-festival.dk/";

            //NightlifeModel n3 = new NightlifeModel();
            //n3.Name = "IP Mary's";
            //n3.Address = "Støden 9";
            //n3.Description = "Most popular place for a night’s out in Roskilde. DJ’s playing" +
            //                     " every friday and saturday. Cheap alcohol and even cheaper deals" +
            //                     " on 10 drinks at a time this is the place to go for some serious " +
            //                     "drinking. \n\nDance the night away on the spacious dance floor with" +
            //                     " the music thumping until 5 or 6 in the morning on weekends.";
            //n3.Url = "http://mary-s.dk/";

            //NightlifeModel n4 = new NightlifeModel();
            //n4.Name = "Mulligan's";
            //n4.Address = "Hestetoret 1";
            //n4.Description = "Mulligan’s is a solid irish pub that would satisfy the most discerning" +
            //                     " irishman. There is never a dull moment at Mulligan’s. With live music " +
            //                     "every friday and saturday from 21 until midnight, “Thirsty Thursdays” has" +
            //                     " cheap pints for 25DKK, plenty of sports broadcasts are shown there and a" +
            //                     " christmas buffet. \n\nStop by and have a seat with a Guinness cradled in" +
            //                     " your hands. It will make you drunk.";
            //n4.Url = "http://mulliganspub.dk";


            //NightlifeModel n5 = new NightlifeModel();
            //n5.Name = "Café Gulland";
            //n5.Address = "Borschgade 6";
            //n5.Description = "Charismatic small local café. If you want to meet danes at a real danish " +
            //                     "café/pub then you should visit Café Gulland, chat with the locals and find " +
            //                     "out what the ‘ligeglad dane’ is all about. \n\nIn the weekend it’s open until" +
            //                     " 5 in the morning with DJ’s playing from 23:00. ";
            //n5.Url = "https://www.facebook.com/pages/CAF%C3%88-GULLAND/29963469709";

            //NightlifeModel n6 = new NightlifeModel();
            //n6.Name = "Gustav Wieds Vinstue";
            //n6.Address = "Hersegade 11";
            //n6.Description = "Probably a nice place. Probably";
            //n6.Url = "http://www.gustav-wieds-vinstue.dk/";

            #endregion


            _nightlifes = new ObservableCollection<NightlifeModel>();
            _newNightlifeModel = new NightlifeModel();
            LoadNightlifeModels();
            
            _addNewNightlife = new RelayCommand(AddNightlife);
            _removeSelectedNightlife = new RelayCommand(RemoveNightlife);
        }
        #endregion

        #region LoadNightlifeModelsXML()
        private async void LoadNightlifeModels()
        {

            StorageFile fileNightlife = null;

            //try to load nightlifes xml from local storage
            try
            {
                fileNightlife = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("nightlife.xml");
            }
            catch (Exception)
            {

            }

            //if it fails use assets folder
            if (fileNightlife == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlfileNightlife = @"Assets\xml\nightlife.xml";
                fileNightlife = await installationFolder.GetFileAsync(xmlfileNightlife);
            }

            Stream nightlifeStream = await fileNightlife.OpenStreamForReadAsync();
            XDocument nightlifeDocument = XDocument.Load(nightlifeStream);

            IEnumerable<XElement> nightlifeList = nightlifeDocument.Descendants("nightlifemodel");

            
            foreach (XElement xElement in nightlifeList)
            {
                NightlifeModel e = new NightlifeModel();
                e.Name = xElement.Element("name").Value;
                e.Address = xElement.Element("address").Value;
                e.Description = xElement.Element("description").Value;
                e.Url = xElement.Element("url").Value;
                _nightlifes.Add(e);
            }
            _selectedNightlifeModel = _nightlifes[0];
            OnPropertyChanged("Nightlifes");
        }
        #endregion

        #region Property Fields

        public ObservableCollection<NightlifeModel> Nightlifes
        {
            get { return _nightlifes; }
            set { _nightlifes = value; }
        }

        private void RemoveNightlife()
        {
            _nightlifes.Remove(_selectedNightlifeModel);
        }

        private void AddNightlife()
        {
            _nightlifes.Add(_newNightlifeModel);
            OnPropertyChanged("Nightlifes");
        }

        public ICommand EditNightlife
        {
            get { return _editNightlife; }
            set { _editNightlife = value; }
        }

        public ICommand RemoveSelectedNightlife
        {
            get { return _removeSelectedNightlife; }
            set { _removeSelectedNightlife = value; }
        }

        public ICommand AddNewNightlife
        {
            get { return _addNewNightlife; }
            set { _addNewNightlife = value; }
        }

        public NightlifeModel NewNightlifeModel
        {
            get { return _newNightlifeModel; }
            set { _newNightlifeModel = value; }
        }

        public NightlifeModel SelectedNightlifeModel
        {
            get { return _selectedNightlifeModel; }
            set
            {
                _selectedNightlifeModel = value;
                OnPropertyChanged("SelectedNightlifeModel");
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