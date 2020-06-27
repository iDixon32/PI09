using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OZO.Models
{
    public partial class PI09Context : DbContext
    {
        

        public PI09Context(DbContextOptions<PI09Context> options)
            : base(options)
        {
        }

        public virtual DbSet<NatječajPoslodavac> NatječajPoslodavac { get; set; }
        public virtual DbSet<Natječaji> Natječaji { get; set; }
        public virtual DbSet<Obrazovanje> Obrazovanje { get; set; }
        public virtual DbSet<Oprema> Oprema { get; set; }
        public virtual DbSet<OpremaIzvještaji> OpremaIzvještaji { get; set; }
        public virtual DbSet<PosaoOprema> PosaoOprema { get; set; }
        public virtual DbSet<Poslodavac> Poslodavac { get; set; }
        public virtual DbSet<Poslovi> Poslovi { get; set; }
        public virtual DbSet<PosloviIzvjestaji> PosloviIzvjestaji { get; set; }
        public virtual DbSet<ReferentniTip> ReferentniTip { get; set; }
        public virtual DbSet<Usluge> Usluge { get; set; }
        public virtual DbSet<UslugePoslodavac> UslugePoslodavac { get; set; }
        public virtual DbSet<Zaposlenici> Zaposlenici { get; set; }
        public virtual DbSet<ZaposleniciZanimanja> ZaposleniciZanimanja { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NatječajPoslodavac>(entity =>
            {
                entity.HasKey(e => e.IdNatječajPoslodavac);

                entity.ToTable("NATJEČAJ_POSLODAVAC");

                entity.Property(e => e.IdNatječajPoslodavac).HasColumnName("ID_NATJEČAJ_POSLODAVAC");

                entity.Property(e => e.IdNatječaja).HasColumnName("ID_NATJEČAJA");

                entity.Property(e => e.IdPoslodavca).HasColumnName("ID_POSLODAVCA");

                entity.HasOne(d => d.IdNatječajaNavigation)
                    .WithMany(p => p.NatječajPoslodavac)
                    .HasForeignKey(d => d.IdNatječaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NATJEČAJ_POSLODAVAC_Natječaji");

                entity.HasOne(d => d.IdPoslodavcaNavigation)
                    .WithMany(p => p.NatječajPoslodavac)
                    .HasForeignKey(d => d.IdPoslodavca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NATJEČAJ_POSLODAVAC_POSLODAVAC");
            });

            modelBuilder.Entity<Natječaji>(entity =>
            {
                entity.HasKey(e => e.IdNatječaji)
                    .HasName("PK_Natječaji");

                entity.ToTable("NATJEČAJI");

                entity.Property(e => e.IdNatječaji).HasColumnName("ID_NATJEČAJI");

                entity.Property(e => e.Cijena)
                    .HasColumnName("CIJENA")
                    .HasColumnType("money");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("OPIS")
                    .HasMaxLength(50);

                entity.Property(e => e.VremenskiRok)
                    .HasColumnName("VREMENSKI_ROK")
                    .HasColumnType("date");

                entity.HasOne(d => d.IdReferentniTipNavigation)
                    .WithMany(p => p.Natječaji)
                    .HasForeignKey(d => d.IdReferentniTip)
                    .HasConstraintName("FK_Natječaji_REFERENTNI_TIP");
            });

            modelBuilder.Entity<Obrazovanje>(entity =>
            {
                entity.HasKey(e => e.IdObrazovanje);

                entity.ToTable("OBRAZOVANJE");

                entity.Property(e => e.IdObrazovanje).HasColumnName("ID_OBRAZOVANJE");

                entity.Property(e => e.IdZaposlenici).HasColumnName("ID_ZAPOSLENICI");

                entity.Property(e => e.NazivŠkole)
                    .HasColumnName("NAZIV_ŠKOLE")
                    .HasMaxLength(50);

                entity.Property(e => e.PoloženiTečaji)
                    .HasColumnName("POLOŽENI_TEČAJI")
                    .HasMaxLength(50);

                entity.Property(e => e.StručnaSprema)
                    .HasColumnName("STRUČNA_SPREMA")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdZaposleniciNavigation)
                    .WithMany(p => p.Obrazovanje)
                    .HasForeignKey(d => d.IdZaposlenici)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OBRAZOVANJE_ZAPOSLENICI");
            });

            modelBuilder.Entity<Oprema>(entity =>
            {
                entity.HasKey(e => e.IdOprema);

                entity.ToTable("OPREMA");

                entity.Property(e => e.IdOprema).HasColumnName("ID_OPREMA");

                entity.Property(e => e.Dostupnost).HasColumnName("DOSTUPNOST");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdReferentniTipNavigation)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.IdReferentniTip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OPREMA_REFERENTNI_TIP");
            });

            modelBuilder.Entity<OpremaIzvještaji>(entity =>
            {
                entity.HasKey(e => e.IdOpremaIzvještaji);

                entity.ToTable("OPREMA_IZVJEŠTAJI");

                entity.Property(e => e.IdOpremaIzvještaji).HasColumnName("ID_OPREMA_IZVJEŠTAJI");

                entity.Property(e => e.Cijena)
                    .HasColumnName("CIJENA")
                    .HasColumnType("money");

                entity.Property(e => e.IdOprema).HasColumnName("ID_OPREMA");

                entity.Property(e => e.Sadržaj).HasColumnName("SADRŽAJ");

                entity.HasOne(d => d.IdOpremaNavigation)
                    .WithMany(p => p.OpremaIzvještaji)
                    .HasForeignKey(d => d.IdOprema)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OPREMA_IZVJEŠTAJI_OPREMA");
            });

            modelBuilder.Entity<PosaoOprema>(entity =>
            {
                entity.HasKey(e => e.IdPosaoOprema);

                entity.ToTable("POSAO_OPREMA");

                entity.Property(e => e.IdPosaoOprema).HasColumnName("ID_POSAO_OPREMA");

                entity.Property(e => e.IdOprema).HasColumnName("ID_OPREMA");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.HasOne(d => d.IdOpremaNavigation)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.IdOprema)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSAO_OPREMA_OPREMA");

                entity.HasOne(d => d.IdPosloviNavigation)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.IdPoslovi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSAO_OPREMA_POSLOVI");
            });

            modelBuilder.Entity<Poslodavac>(entity =>
            {
                entity.HasKey(e => e.IdPoslodavac);

                entity.ToTable("POSLODAVAC");

                entity.Property(e => e.IdPoslodavac).HasColumnName("ID_POSLODAVAC");

                entity.Property(e => e.ImeFirme)
                    .IsRequired()
                    .HasColumnName("IME_FIRME")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Poslovi>(entity =>
            {
                entity.HasKey(e => e.IdPoslovi);

                entity.ToTable("POSLOVI");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.Property(e => e.IdNatječaji).HasColumnName("ID_NATJEČAJI");

                entity.Property(e => e.IdUsluge).HasColumnName("ID_USLUGE");

                entity.Property(e => e.Mjesto)
                    .HasColumnName("MJESTO")
                    .HasMaxLength(50);

                entity.Property(e => e.Naziv)
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.Property(e => e.VrijemeTrajanja)
                    .HasColumnName("VRIJEME_TRAJANJA")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdNatječajiNavigation)
                    .WithMany(p => p.Poslovi)
                    .HasForeignKey(d => d.IdNatječaji)
                    .HasConstraintName("FK_POSLOVI_Natječaji");

                entity.HasOne(d => d.IdUslugeNavigation)
                    .WithMany(p => p.Poslovi)
                    .HasForeignKey(d => d.IdUsluge)
                    .HasConstraintName("FK_POSLOVI_USLUGE");
            });

            modelBuilder.Entity<PosloviIzvjestaji>(entity =>
            {
                entity.HasKey(e => e.IdPosloviIzvještaji)
                    .HasName("PK_POSLOVI_IZVJEŠTAJI");

                entity.ToTable("POSLOVI_IZVJESTAJI");

                entity.Property(e => e.IdPosloviIzvještaji).HasColumnName("ID_POSLOVI_IZVJEŠTAJI");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.Property(e => e.Sadržaj).HasColumnName("SADRŽAJ");

                entity.HasOne(d => d.IdPosloviNavigation)
                    .WithMany(p => p.PosloviIzvjestaji)
                    .HasForeignKey(d => d.IdPoslovi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSLOVI_IZVJEŠTAJI_POSLOVI");
            });

            modelBuilder.Entity<ReferentniTip>(entity =>
            {
                entity.HasKey(e => e.IdReferentniTip);

                entity.ToTable("REFERENTNI_TIP");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.Naziv)
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usluge>(entity =>
            {
                entity.HasKey(e => e.IdUsluge);

                entity.ToTable("USLUGE");

                entity.Property(e => e.IdUsluge).HasColumnName("ID_USLUGE");

                entity.Property(e => e.Cijena)
                    .HasColumnName("CIJENA")
                    .HasColumnType("money");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.NazivUsluge)
                    .IsRequired()
                    .HasColumnName("NAZIV_USLUGE")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .HasColumnName("OPIS")
                    .HasMaxLength(50);

                entity.Property(e => e.VremenskiRok)
                    .HasColumnName("VREMENSKI_ROK")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdReferentniTipNavigation)
                    .WithMany(p => p.Usluge)
                    .HasForeignKey(d => d.IdReferentniTip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USLUGE_REFERENTNI_TIP");
            });

            modelBuilder.Entity<UslugePoslodavac>(entity =>
            {
                entity.HasKey(e => e.IdUslugePoslodavac);

                entity.ToTable("USLUGE_POSLODAVAC");

                entity.Property(e => e.IdUslugePoslodavac).HasColumnName("ID_USLUGE_POSLODAVAC");

                entity.Property(e => e.IdPoslodavac).HasColumnName("ID_POSLODAVAC");

                entity.Property(e => e.IdUsluge).HasColumnName("ID_USLUGE");

                entity.HasOne(d => d.IdPoslodavacNavigation)
                    .WithMany(p => p.UslugePoslodavac)
                    .HasForeignKey(d => d.IdPoslodavac)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USLUGE_POSLODAVAC_POSLODAVAC");

                entity.HasOne(d => d.IdUslugeNavigation)
                    .WithMany(p => p.UslugePoslodavac)
                    .HasForeignKey(d => d.IdUsluge)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USLUGE_POSLODAVAC_USLUGE");
            });

            modelBuilder.Entity<Zaposlenici>(entity =>
            {
                entity.HasKey(e => e.IdZaposlenici);

                entity.ToTable("ZAPOSLENICI");

                entity.Property(e => e.IdZaposlenici).HasColumnName("ID_ZAPOSLENICI");

                entity.Property(e => e.DatumRođenja)
                    .HasColumnName("DATUM_ROĐENJA")
                    .HasColumnType("date");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.Property(e => e.Ime)
                    .HasColumnName("IME")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Prezime)
                    .HasColumnName("PREZIME")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TrošakZaposlenika)
                    .HasColumnName("TROŠAK_ZAPOSLENIKA")
                    .HasColumnType("money");

                entity.HasOne(d => d.IdPosloviNavigation)
                    .WithMany(p => p.Zaposlenici)
                    .HasForeignKey(d => d.IdPoslovi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZAPOSLENICI_POSLOVI");
            });

            modelBuilder.Entity<ZaposleniciZanimanja>(entity =>
            {
                entity.HasKey(e => e.IdZaposleniciZanimanja);

                entity.ToTable("ZAPOSLENICI_ZANIMANJA");

                entity.Property(e => e.IdZaposleniciZanimanja).HasColumnName("ID_ZAPOSLENICI_ZANIMANJA");

                entity.Property(e => e.IdZaposlenici).HasColumnName("ID_ZAPOSLENICI");

                entity.Property(e => e.Naziv)
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdZaposleniciNavigation)
                    .WithMany(p => p.ZaposleniciZanimanja)
                    .HasForeignKey(d => d.IdZaposlenici)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZAPOSLENICI_ZANIMANJA_ZAPOSLENICI");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
