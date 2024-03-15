namespace InForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Utenti")]
    public partial class Utenti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utenti()
        {
            Carrelloes = new HashSet<Carrello>();
        }

        [Key]
        [StringLength(50)]
        public string Utente { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Ruolo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Carrello> Carrelloes { get; set; }
    }

    public class Login
    {
        public string Utente { get; set; }
        public string Password { get; set; }

    }


    public class UtenteReg
    {
        public string Utente { get; set; }
        public string Password { get; set; }
        public string Ruolo { get; set; }
        public string Email { get; set; }

    }
}
