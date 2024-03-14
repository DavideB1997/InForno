namespace InForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Articolo")]
    public partial class Articolo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Articolo()
        {
            ArticoloCarrello = new HashSet<ArticoloCarrello>();
            Ingredienti = new HashSet<Ingredienti>();
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
        public virtual ICollection<ArticoloCarrello> ArticoloCarrello { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ingredienti> Ingredienti { get; set; }
    }


    public class AddArticoloViewModel
    {
        // Proprietà della classe AddArticoloViewModel
        public Articolo Articolo { get; set; }
        public List<Ingredienti> Ingredienti { get; set; }
        public HttpPostedFileBase ImmagineFile { get; set; }
        public AddArticoloViewModel()
        {
            // Inizializza la lista degli ingredienti
            Ingredienti = new List<Ingredienti>();
        }
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
}
