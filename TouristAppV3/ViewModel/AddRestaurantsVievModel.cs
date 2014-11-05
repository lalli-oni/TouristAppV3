using System;
using System.Collections.Generic;
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
    class AddRestaurantsVievModel : INotifyPropertyChanged
    {
        private RestaurantModel _newRestaurant;
        private ICommand _addNewRestaurant;

        public AddRestaurantsVievModel()
        {
            _newRestaurant = new RestaurantModel();
            _addNewRestaurant = new RelayCommand(AddRestaurantCommand);
            AddRestaurantCommand();
        }

        private async void AddRestaurantCommand()
        {
            StorageFile file = null;
            try
            {
                file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("Restaurant.xml");
            }
            catch (Exception)
            {
            }

            if (file == null)
            {
                StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                string xmlFile = @"Assets\xml\Restaurant.xml";
                file = await installationFolder.GetFileAsync(xmlFile);
            }

            Stream loadStream = await file.OpenStreamForReadAsync();
            XDocument RestaurantDocument = XDocument.Load(loadStream);
            loadStream.Dispose();

            XElement Restaurantslist = RestaurantDocument.Element("Restaurantmodels");

            XElement restaurant = new XElement("nightlifemodel");
            restaurant.Add(new XElement("name", NewRestaurants.Name));
            restaurant.Add(new XElement("address", NewRestaurants.Address));
            restaurant.Add(new XElement("description", NewRestaurants.Description));
            restaurant.Add(new XElement("url", NewRestaurants.Url));

            Restaurantslist.LastNode.AddAfterSelf(restaurant);

            StorageFile saveFile = null;

            try
            {
                saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("Restaurants.xml");
            }
            catch (Exception)
            {
            }

            if (saveFile == null)
            {
                saveFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("Restaurants.xml");
            }

            Stream saveStream = await saveFile.OpenStreamForWriteAsync();
            RestaurantDocument.Save(saveStream);
            await saveStream.FlushAsync();
            saveStream.Dispose();
        }

        #region Properties

        public RestaurantModel NewRestaurants
        {
            get { return _newRestaurant; }
            set { _newRestaurant = value; }
        }

        public ICommand AddNewRestaurants
        {
            get { return _addNewRestaurant; }
            set { _addNewRestaurant = value; }
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
