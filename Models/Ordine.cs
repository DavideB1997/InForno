namespace InForno.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ordine")]
    public partial class Ordine
    {
        [Key]
        public int IdOrdine { get; set; }

        public bool Evaso { get; set; }

        [Required]
        [StringLength(255)]
        public string Indirizzo { get; set; }

        [StringLength(255)]
        public string Note { get; set; }

        public int IdCarrello { get; set; }

        public virtual Carrello Carrello { get; set; }
    }
}
