using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Map = Xamarin.Essentials.Map;

namespace NailBars.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsNormal : ContentPage
    {
        public MapsNormal()
        {
            InitializeComponent();
            var images = new List<string>
{
                "https://muchosnegociosrentables.com/wp-content/uploads/2021/01/como-abrir-un-salon-de-belleza-guia.jpg",
                "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRm3JPgUJMqsO-RgqEvt3AobjwD3cl--hKlYw&usqp=CAU",
                "https://map.viamichelin.com/map/carte?map=viamichelin&z=10&lat=14.60343&lon=-87.84041&width=550&height=382&format=png&version=latest&layer=background&debug_pattern=.*"
};
            MainCarouselView.ItemsSource = images;
            Device.StartTimer(TimeSpan.FromSeconds(6), (Func<bool>)(() =>
            {
                MainCarouselView.Position = (MainCarouselView.Position + 1) % images.Count;
                return true;
            }));

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    Title = "Active la Localización GPS";
                }
                else
                {
                    Title = "GPS esta Activado";
                }
            }
            catch (FeatureNotSupportedException)
            {
            }
            catch (FeatureNotEnabledException)
            {
            }
            catch (PermissionException)
            {
            }
            catch (System.Exception)
            {
            }
        }


        private async void btnRutaMapa_Clicked(object sender, EventArgs e)
        {
            //Rutas para el rastreo de la dirección
            //var location = new Location(16.351835, -86.464745); 14.941765388971756, -88.32727787206794   sb = 14.918282184707866, -88.23540685731157 sps=15.507161583112486, -88.01707569793233
            var location = new Location(14.6166700, -87.8333300);
            var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

            await Map.OpenAsync(location, options);
        }
    }
}


