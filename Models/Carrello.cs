namespace InForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Carrello")]
    public partial class Carrello
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Carrello()
        {
            ArticoloCarrelloes = new HashSet<ArticoloCarrello>();
            Ordines = new HashSet<Ordine>();
        }

        [Key]
        public int IdCarrello { get; set; }

        public int? Totale { get; set; }

        [StringLength(50)]
        public string IdUtente { get; set; }

        public int? Evaso { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticoloCarrello> ArticoloCarrelloes { get; set; }

        public virtual Utenti Utenti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ordine> Ordines { get; set; }
    }
}
