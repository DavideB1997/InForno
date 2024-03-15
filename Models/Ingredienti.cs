namespace InForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ingredienti")]
    public partial class Ingredienti
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingredienti()
        {
            Articoloes = new HashSet<Articolo>();
        }

        [Key]
        public int IdIngrediente { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Descrizione { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Articolo> Articoloes { get; set; }
    }
}
