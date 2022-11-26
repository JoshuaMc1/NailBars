using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JMDialog : Rg.Plugins.Popup.Pages.PopupPage
    {
        public static int Success = 1, Danger = 2, Warning = 3, Information = 4;
        string ColorSuccess = "#ccf4d0", ColorSuccessText = "#101c1f";
        string ColorWarning = "#e6c978", ColorWarningText = "#4f401c";
        string ColorDanger = "#f4bcb8", ColorDangerText = "#641414";
        string ColorInfo = "#ece0f8", ColorInfoText = "#4c348c";

        public JMDialog(string Title, string Message, int Type)
        {
            InitializeComponent();
            txtTitulo.Text = Title;
            txtMensaje.Text = Message;
            if (Type == 1)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorSuccess);
                txtTitulo.TextColor = Color.FromHex(ColorSuccessText);
                txtMensaje.TextColor = Color.FromHex(ColorSuccessText);
                btnOk.BackgroundColor = Color.FromHex(ColorSuccessText);
                btnOk.TextColor = Color.FromHex("#FFF");
            }
            else if (Type == 2)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorDanger);
                txtTitulo.TextColor = Color.FromHex(ColorDangerText);
                txtMensaje.TextColor = Color.FromHex(ColorDangerText);
                btnOk.BackgroundColor = Color.FromHex(ColorDangerText);
                btnOk.TextColor = Color.FromHex("#FFF");
            }
            else if (Type == 3)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorWarning);
                txtTitulo.TextColor = Color.FromHex(ColorWarningText);
                txtMensaje.TextColor = Color.FromHex(ColorWarningText);
                btnOk.BackgroundColor = Color.FromHex(ColorWarningText);
                btnOk.TextColor = Color.FromHex(ColorWarning);
            }
            else if (Type == 4)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorInfo);
                txtTitulo.TextColor = Color.FromHex(ColorInfoText);
                txtMensaje.TextColor = Color.FromHex(ColorInfoText);
                btnOk.BackgroundColor = Color.FromHex(ColorInfoText);
                btnOk.TextColor = Color.FromHex(ColorInfo);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }

        private void btnOk_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }
}