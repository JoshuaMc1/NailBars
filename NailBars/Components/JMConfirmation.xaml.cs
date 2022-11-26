using Rg.Plugins.Popup.Extensions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NailBars.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JMConfirmation : Rg.Plugins.Popup.Pages.PopupPage
    {
        public static int Success = 1, Danger = 2, Warning = 3, Information = 4;
        string ColorSuccess = "#ccf4d0", ColorSuccessText = "#101c1f";
        string ColorWarning = "#e6c978", ColorWarningText = "#4f401c";
        string ColorDanger = "#f4bcb8", ColorDangerText = "#641414";
        string ColorInfo = "#ece0f8", ColorInfoText = "#4c348c";
        public EventHandler<JMConfirmationResult> OnDialogClosed;

        public JMConfirmation(string Title, string Message, int Type)
        {
            InitializeComponent();
            txtTitulo.Text = Title;
            txtMensaje.Text = Message;
            if (Type == 1)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorSuccess);
                txtTitulo.TextColor = Color.FromHex(ColorSuccessText);
                txtMensaje.TextColor = Color.FromHex(ColorSuccessText);
                btnAceptar.BackgroundColor = Color.FromHex(ColorSuccessText);
                btnAceptar.TextColor = Color.FromHex("#FFF");
                btnCancelar.BackgroundColor = Color.FromHex(ColorSuccessText);
                btnCancelar.TextColor = Color.FromHex("#FFF");
            }
            else if (Type == 2)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorDanger);
                txtTitulo.TextColor = Color.FromHex(ColorDangerText);
                txtMensaje.TextColor = Color.FromHex(ColorDangerText);
                btnAceptar.BackgroundColor = Color.FromHex(ColorDangerText);
                btnAceptar.TextColor = Color.FromHex("#FFF");
                btnCancelar.BackgroundColor = Color.FromHex(ColorDangerText);
                btnCancelar.TextColor = Color.FromHex("#FFF");
            }
            else if (Type == 3)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorWarning);
                txtTitulo.TextColor = Color.FromHex(ColorWarningText);
                txtMensaje.TextColor = Color.FromHex(ColorWarningText);
                btnAceptar.BackgroundColor = Color.FromHex(ColorWarningText);
                btnAceptar.TextColor = Color.FromHex(ColorWarning);
                btnCancelar.BackgroundColor = Color.FromHex(ColorWarningText);
                btnCancelar.TextColor = Color.FromHex(ColorWarning);
            }
            else if (Type == 4)
            {
                txtVentana.BackgroundColor = Color.FromHex(ColorInfo);
                txtTitulo.TextColor = Color.FromHex(ColorInfoText);
                txtMensaje.TextColor = Color.FromHex(ColorInfoText);
                btnAceptar.BackgroundColor = Color.FromHex(ColorInfoText);
                btnAceptar.TextColor = Color.FromHex(ColorInfo);
                btnCancelar.BackgroundColor = Color.FromHex(ColorInfoText);
                btnCancelar.TextColor = Color.FromHex(ColorInfo);
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

        private void btnCancelar_Clicked(object sender, EventArgs e)
        {
            OnDialogClosed?.Invoke(this, new JMConfirmationResult { Success = false, Message = "Cancel" });
            App.Current.MainPage.Navigation.PopPopupAsync(true);
        }

        private void btnAceptar_Clicked(object sender, EventArgs e)
        {
            OnDialogClosed?.Invoke(this, new JMConfirmationResult { Success = true, Message = "Accept" });
            App.Current.MainPage.Navigation.PopPopupAsync(true);
        }
    }

    public class JMConfirmationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}