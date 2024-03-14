using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace InForno.Models
{
    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
        }

        public virtual DbSet<Articolo> Articolo { get; set; }
        public virtual DbSet<Carrello> Carrello { get; set; }
        public virtual DbSet<Ingredienti> Ingredienti { get; set; }
        public virtual DbSet<Ordine> Ordine { get; set; }
        public virtual DbSet<Utenti> Utenti { get; set; }
        public virtual DbSet<ArticoloCarrello> ArticoloCarrello { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articolo>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Articolo>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Articolo>()
                .Property(e => e.Immagine)
                .IsUnicode(false);

            modelBuilder.Entity<Articolo>()
                .HasMany(e => e.ArticoloCarrello)
                .WithRequired(e => e.Articolo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Articolo>()
                .HasMany(e => e.Ingredienti)
                .WithMany(e => e.Articolo)
                .Map(m => m.ToTable("Ingrediente_Articolo").MapLeftKey("IdArticolo").MapRightKey("IdIngrediente"));

            modelBuilder.Entity<Carrello>()
                .Property(e => e.IdUtente)
                .IsUnicode(false);

            modelBuilder.Entity<Carrello>()
                .HasMany(e => e.ArticoloCarrello)
                .WithRequired(e => e.Carrello)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Carrello>()
                .HasMany(e => e.Ordine)
                .WithRequired(e => e.Carrello)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Ingredienti>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Ingredienti>()
                .Property(e => e.Descrizione)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Indirizzo)
                .IsUnicode(false);

            modelBuilder.Entity<Ordine>()
                .Property(e => e.Note)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.Utente)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .Property(e => e.Ruolo)
                .IsUnicode(false);

            modelBuilder.Entity<Utenti>()
                .HasMany(e => e.Carrello)
                .WithOptional(e => e.Utenti)
                .HasForeignKey(e => e.IdUtente);
        }
    }
}
