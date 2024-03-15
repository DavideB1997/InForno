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

        public virtual DbSet<Articolo> Articoloes { get; set; }
        public virtual DbSet<Carrello> Carrelloes { get; set; }
        public virtual DbSet<Ingredienti> Ingredientis { get; set; }
        public virtual DbSet<Ordine> Ordines { get; set; }
        public virtual DbSet<Utenti> Utentis { get; set; }
        public virtual DbSet<ArticoloCarrello> ArticoloCarrelloes { get; set; }

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
                .HasMany(e => e.ArticoloCarrelloes)
                .WithRequired(e => e.Articolo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Articolo>()
                .HasMany(e => e.Ingredientis)
                .WithMany(e => e.Articoloes)
                .Map(m => m.ToTable("Ingrediente_Articolo").MapLeftKey("IdArticolo").MapRightKey("IdIngrediente"));

            modelBuilder.Entity<Carrello>()
                .Property(e => e.IdUtente)
                .IsUnicode(false);

            modelBuilder.Entity<Carrello>()
                .HasMany(e => e.ArticoloCarrelloes)
                .WithRequired(e => e.Carrello)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Carrello>()
                .HasMany(e => e.Ordines)
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
                .HasMany(e => e.Carrelloes)
                .WithOptional(e => e.Utenti)
                .HasForeignKey(e => e.IdUtente);
        }
    }
}
