using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Models
{
    public class vwRolYetkiView
    {

        public int RefKey { get; set; }

        public string RolRefKey { get; set; }

        public string RolAdi { get; set; }

        public int ModulRefKey { get; set; }

        public string ModulDeger { get; set; }

        public string ModulAdi { get; set; }

        public int? FormRefKey { get; set; }

        public string FormDeger { get; set; }

        public string FormAdi { get; set; }

        public bool Okuma { get; set; }

        public bool Yazma { get; set; }

        public bool Degistirme { get; set; }

        public bool Silme { get; set; }

        public int IsletmeRefKey { get; set; }

    }
}
