using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NailBars.Modelo
{
    public class MainPageViewModel : BindableObject
    {
        private string _cardNumber;
        private string _expiration;
        private string _cvc;
        private string _pagardiner;
        private string _subir;

        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                _cardNumber = value;
                OnPropertyChanged();
            }
        }

        public string Expiration
        {
            get => _expiration;
            set { _expiration = value; OnPropertyChanged(); }
        }

        public string CVC
        {
            get => _cvc;
            set { _cvc = value; OnPropertyChanged(); }
        }
        public string Pagardinero
        {
            get => _pagardiner;
            set { _pagardiner = value; OnPropertyChanged(); }
        }

        public string Subir
        {
            get => _subir;
            set { _subir = value; OnPropertyChanged(); }
        }
    }
}
