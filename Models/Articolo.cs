namespace InForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Articolo")]
    public partial class Articolo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articolo()
        {
            ArticoloCarrelloes = new HashSet<ArticoloCarrello>();
            Ingredientis = new HashSet<Ingredienti>();
        }

        [Key]
        public int IdArticolo { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Descrizione { get; set; }

        public int Prezzo { get; set; }

        [StringLength(255)]
        public string Immagine { get; set; }

        public int TempoConsegna { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArticoloCarrello> ArticoloCarrelloes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ingredienti> Ingredientis { get; set; }
    }

    public class ArticoloLista
    {
        public int IdArticolo { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public int Prezzo { get; set; }
        public int TempoConsegna { get; set; }
        public List<Ingredienti> Ingredienti { get; set; }
    }


    public partial class ArticoloDaCarrello
    {
        public int IdArticolo { get; set; }
        public int IdCarrello { get; set; }
        public int Quantità { get; set; }
        public string Nome { get; set; }
        public string Descrizione { get; set; }
        public int Prezzo { get; set; }
        public int TempoConsegna { get; set; }
        public int Evaso { get; set; }

        public int Totale { get; set; }
    }


    public partial class ArticoloOrdine
    {
        public string Note { get; set; }
        public string Indirizzo { get; set; }
        public int Evaso { get; set; }
        public int Totale { get; set; }
        public string Utente { get; set; }
    }
}
