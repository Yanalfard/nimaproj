using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DataLayer.Models
{
    public partial class NimaProjContext : DbContext
    {
        public NimaProjContext()
        {
        }

        public NimaProjContext(DbContextOptions<NimaProjContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblBannerAndSlide> TblBannerAndSlides { get; set; }
        public virtual DbSet<TblBlog> TblBlogs { get; set; }
        public virtual DbSet<TblBlogCommentRel> TblBlogCommentRels { get; set; }
        public virtual DbSet<TblCatagory> TblCatagories { get; set; }
        public virtual DbSet<TblClient> TblClients { get; set; }
        public virtual DbSet<TblComment> TblComments { get; set; }
        public virtual DbSet<TblConfig> TblConfigs { get; set; }
        public virtual DbSet<TblContactU> TblContactUs { get; set; }
        public virtual DbSet<TblDiscount> TblDiscounts { get; set; }
        public virtual DbSet<TblImage> TblImages { get; set; }
        public virtual DbSet<TblOfflineOrder> TblOfflineOrders { get; set; }
        public virtual DbSet<TblOrder> TblOrders { get; set; }
        public virtual DbSet<TblOrderDetail> TblOrderDetails { get; set; }
        public virtual DbSet<TblProduct> TblProducts { get; set; }
        public virtual DbSet<TblProductCommentRel> TblProductCommentRels { get; set; }
        public virtual DbSet<TblProductImageRel> TblProductImageRels { get; set; }
        public virtual DbSet<TblProductPropertyRel> TblProductPropertyRels { get; set; }
        public virtual DbSet<TblProperty> TblProperties { get; set; }
        public virtual DbSet<TblRegularQuestion> TblRegularQuestions { get; set; }
        public virtual DbSet<TblRole> TblRoles { get; set; }
        public virtual DbSet<TblSpecialOffer> TblSpecialOffers { get; set; }
        public virtual DbSet<TblVisit> TblVisits { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder
   .UseLazyLoadingProxies()
   .UseSqlServer("Data Source=103.216.62.27;Initial Catalog=NimaProj;User ID=Yanal;Password=1710ahmad.fard");
//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Data Source=103.216.62.27;Initial Catalog=NimaProj;User ID=Yanal;Password=1710ahmad.fard");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblBlog>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblBlogCommentRel>(entity =>
            {
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.TblBlogCommentRels)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_TblBlogCommentRel_TblBlog");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TblBlogCommentRels)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK_TblBlogCommentRel_TblComment");
            });

            modelBuilder.Entity<TblCatagory>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_TblCatagory_TblCatagory");
            });

            modelBuilder.Entity<TblClient>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblClients)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblClient_TblRole");
            });

            modelBuilder.Entity<TblComment>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.TblComments)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblComment_TblClient");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_TblComment_TblComment");
            });

            modelBuilder.Entity<TblOfflineOrder>(entity =>
            {
                entity.HasKey(e => e.OnlineOrderId)
                    .HasName("PK_TblOrder");

                entity.Property(e => e.DateSubmited).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.TblOfflineOrders)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_TblOrder_TblClient");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblOfflineOrders)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblOfflineOrder_TblProduct");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.HasKey(e => e.OrdeId)
                    .HasName("PK_TblOrder_1");

                entity.Property(e => e.DateSubmited).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SendPrice).HasComment("0");

                entity.Property(e => e.Status).HasComment("0 is making; 1 is on its way; 2 is done;");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.ClientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblOrder_TblClient1");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_TblOrder_TblDiscount");
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId)
                    .HasName("PK_TblClientProductRel");

                entity.Property(e => e.Count).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FinalOrder)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.FinalOrderId)
                    .HasConstraintName("FK_TblOrderDetail_TblOrder");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblOrderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TblOrderDetail_TblProduct");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Catagory)
                    .WithMany(p => p.TblProducts)
                    .HasForeignKey(d => d.CatagoryId)
                    .HasConstraintName("FK_TblProduct_TblCatagory");
            });

            modelBuilder.Entity<TblProductCommentRel>(entity =>
            {
                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.TblProductCommentRels)
                    .HasForeignKey(d => d.CommentId)
                    .HasConstraintName("FK_TblProductCommentRel_TblComment");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductCommentRels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TblProductCommentRel_TblProduct");
            });

            modelBuilder.Entity<TblProductImageRel>(entity =>
            {
                entity.HasOne(d => d.Image)
                    .WithMany(p => p.TblProductImageRels)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_TblProductImageRel_TblImage");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductImageRels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TblProductImageRel_TblProduct");
            });

            modelBuilder.Entity<TblProductPropertyRel>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblProductPropertyRels)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TblProductPropertyRel_TblProduct");

                entity.HasOne(d => d.Property)
                    .WithMany(p => p.TblProductPropertyRels)
                    .HasForeignKey(d => d.PropertyId)
                    .HasConstraintName("FK_TblProductPropertyRel_TblProperty");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<TblSpecialOffer>(entity =>
            {
                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblSpecialOffers)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK_TblSpecialOffer_TblProduct");
            });

            modelBuilder.Entity<TblVisit>(entity =>
            {
                entity.Property(e => e.Date).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
