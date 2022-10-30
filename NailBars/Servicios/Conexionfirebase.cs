using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace NailBars.Servicios
{
    public class Conexionfirebase
    {
        public static FirebaseClient firebase = new FirebaseClient("https://nailbars-9dde3-default-rtdb.firebaseio.com/");
       
        public const string WebapyFirebase = "AIzaSyBy6Cc5gDJunzUI5Z7Vn0Sv_GHwxeblPRs";
    }
}
