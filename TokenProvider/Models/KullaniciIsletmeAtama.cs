using System.ComponentModel.DataAnnotations;

namespace ArcelikAuthProvider.Models
{
    public class KullaniciIsletmeAtama
    {
        [Key]
        public int RefKey { get; set; }

        public string KullaniciRefKey { get; set; }

        public int IsletmeRefKey { get; set; }

    }
}
