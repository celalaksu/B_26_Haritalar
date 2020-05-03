

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Haritalar
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        bool konumIzniVarmi = false;

        public MainPage()
        {
            InitializeComponent();       
           
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            KonumaGitPinEkle();
        }

        async void KonumaGitPinEkle()
        {
            //Pin ekler
            var konumAl = await Geolocation.GetLastKnownLocationAsync();
            Pin pin = new Pin{
                Label="Ben burdayim",
                Type = PinType.Place,                
                Position = new Position(konumAl.Latitude,konumAl.Longitude)
            };
            haritaMap.Pins.Add(pin);

            //Kullanıcı konumuna gider.
            var konum = new Position(konumAl.Latitude, konumAl.Longitude);
            var span = new MapSpan(konum, 1, 1);
            haritaMap.MoveToRegion(span);
        }

        async void AdresleriGoster()
        {
            
            var konumAl = await Geolocation.GetLastKnownLocationAsync();
            var konum = new Position(konumAl.Latitude, konumAl.Longitude);
            Geocoder geoCoder = new Geocoder();
            IEnumerable<string> olabilecekAdresler = await geoCoder.GetAddressesForPositionAsync(konum);
            adreslerListView.ItemsSource = olabilecekAdresler.ToList();
                     
        }

        private void HaritaMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            AdresleriGoster();
        }

        private async void adreslerListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            adresLabel.Text = (String)e.SelectedItem;
            string adress = (String)e.SelectedItem;

            if (Device.RuntimePlatform == Device.iOS)
            {
                // https://developer.apple.com/library/ios/featuredarticles/iPhoneURLScheme_Reference/MapLinks/MapLinks.html
                await Launcher.OpenAsync("http://maps.apple.com/?daddr={adress}");
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                // opens the 'task chooser' so the user can pick Maps, Chrome or other mapping app
                await Launcher.OpenAsync("http://maps.google.com/?daddr={adres}");
            }
        }

        private async void konumAraSB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar konumAra = (SearchBar)sender;
            adreslerListView.ItemsSource = await AdresAra(konumAra.Text);
            
            

        }

        async Task<List<string>> AdresAra(string adres)
        {
            List<string> yerListesi = new List<string>();

            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(adres);
            List<Position> konumListesi = new List<Position>(approximateLocations);
            if (konumListesi.Count != 0)
            {
                var position = konumListesi.FirstOrDefault<Position>();
                yerListesi = new List<string>(await geoCoder.GetAddressesForPositionAsync(position));
            }
            return yerListesi;
        }
    }
}
