//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EforahWebapp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Gebruiker
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Gebruiker()
        {
            this.Lid = new HashSet<Lid>();
        }
    
        public int gebruikerId { get; set; }
        public int locatieId { get; set; }
        public string gebruikersnaam { get; set; }
        public string wachtwoord { get; set; }
        public string email { get; set; }
        public string telefoonnummer { get; set; }
        public string voornaam { get; set; }
        public string achternaam { get; set; }
        public string telefoonnummerAlt { get; set; }
        public string foto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lid> Lid { get; set; }
        public virtual Locatie Locatie { get; set; }
    }
}
