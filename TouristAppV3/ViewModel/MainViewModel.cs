﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks; 
using TouristAppV3.Model;

namespace TouristAppV3.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<NightlifeModel> _nightlifes;
        private NightlifeModel _selectedNightlifeModel;
        private ObservableCollection<NightlifeModel> _nightlifes1;

        public MainViewModel()
        {
            #region Create Nightlife Models
            _nightlifes = new ObservableCollection<NightlifeModel>();

            NightlifeModel n1 = new NightlifeModel();
            n1.Name = "Gimle";
            n1.Address = "Helligkorsvej 2";
            n1.Description = "Gimle is a student & youth café. " +
                                 "With young energetic volunteer servers " +
                                 "the mood is like none other. Gimle also " +
                                 "hosts concerts with some of the freshest " +
                                 "artists in denmark and from across the borders.\n \n " +
                                 "Sizeable beer selection, free wi-fi and fantastic " +
                                 "music. Gimle sure has a lot to offer.";
            n1.Url = "http://gimle.dk/";

            NightlifeModel n2 = new NightlifeModel();
            n2.Name = "Roskilde Festival";
            n2.Address = "Darupvej 19";
            n2.Description = "Roskilde Festival 2013 had more than 180 performing " +
                                 "bands and gathered around 130,000 festivalgoers, with" +
                                 " more than 21,000 volunteers, 5,000 media people and 3,000 " +
                                 "artists – which means almost 160,000 people participated in" +
                                 " the festival.\n\n In 1972, the festival was taken over by the " +
                                 "Roskilde Foundation, which has since run the festival as a " +
                                 "non-profit organization for development and support of music, " +
                                 "culture and humanism.";
            n1.Url = "http://roskilde-festival.dk/";

            NightlifeModel n3 = new NightlifeModel();
            n3.Name = "IP Mary's";
            n3.Address = "Støden 9";
            n3.Description = "Most popular place for a night’s out in Roskilde. DJ’s playing" +
                                 " every friday and saturday. Cheap alcohol and even cheaper deals" +
                                 " on 10 drinks at a time this is the place to go for some serious " +
                                 "drinking. \n\nDance the night away on the spacious dance floor with" +
                                 " the music thumping until 5 or 6 in the morning on weekends.";
            n3.Url = "http://mary-s.dk/";

            NightlifeModel n4 = new NightlifeModel();
            n4.Name = "Mulligan's";
            n4.Address = "Hestetoret 1";
            n4.Description = "Mulligan’s is a solid irish pub that would satisfy the most discerning" +
                                 " irishman. There is never a dull moment at Mulligan’s. With live music " +
                                 "every friday and saturday from 21 until midnight, “Thirsty Thursdays” has" +
                                 " cheap pints for 25DKK, plenty of sports broadcasts are shown there and a" +
                                 " christmas buffet. \n\nStop by and have a seat with a Guinness cradled in" +
                                 " your hands. It will make you drunk.";
            n4.Url = "http://mulliganspub.dk";


            NightlifeModel n5 = new NightlifeModel();
            n5.Name = "Café Gulland";
            n5.Address = "Borschgade 6";
            n5.Description = "Charismatic small local café. If you want to meet danes at a real danish " +
                                 "café/pub then you should visit Café Gulland, chat with the locals and find " +
                                 "out what the ‘ligeglad dane’ is all about. \n\nIn the weekend it’s open until" +
                                 " 5 in the morning with DJ’s playing from 23:00. ";
            n5.Url = "https://www.facebook.com/pages/CAF%C3%88-GULLAND/29963469709";

            NightlifeModel n6 = new NightlifeModel();
            n6.Name = "Gustav Wieds Vinstue";
            n6.Address = "Hersegade 11";
            n6.Description = "Probably a nice place. Probably";
            n6.Url = "http://www.gustav-wieds-vinstue.dk/";
            #endregion

            

        }

        public NightlifeModel SelectNightlifeModel
        {
            get
            {
                return _selectedNightlifeModel;
            }
            set
            {
                _selectedNightlifeModel = value;
                OnPropertyChanged("SelectedNightlifeModel");
            }
        }

        public ObservableCollection<NightlifeModel> Nightlifes
        {
            get { return _nightlifes1; }
            set { _nightlifes1 = value; }
        }

        #region INotifyPropertyChanged
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
