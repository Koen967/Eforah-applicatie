using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EforahWebapp.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Gebruikersnaam")]
        public string Gebruikersnaam { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Wachtwoord { get; set; }

        [Display(Name = "Ingelogd blijven?")]
        public bool blijvendLogin { get; set; }
    }
}
