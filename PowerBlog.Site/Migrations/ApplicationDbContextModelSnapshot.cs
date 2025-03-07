﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PowerBlog.Site.Data;

#nullable disable

namespace PowerBlog.Site.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PowerBlog.Site.Models.Blog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublish")
                        .HasColumnType("bit");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("TextBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("View")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("BlogId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsConfirmation")
                        .HasColumnType("bit");

                    b.Property<string>("TextBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Favorite", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("BlogId")
                        .HasColumnType("bigint");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("UserId");

                    b.ToTable("Favorites");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.OfferPay", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreatDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDisable")
                        .HasColumnType("bit");

                    b.Property<decimal?>("OfferAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("OfferPercentage")
                        .HasColumnType("int");

                    b.Property<string>("OfferWord")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OfferPays");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Order", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("BlogId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OfferPayId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("PayDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentStatus")
                        .HasColumnType("int");

                    b.Property<string>("TransactionId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("OfferPayId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.ReactionBlog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("BlogId")
                        .HasColumnType("bigint");

                    b.Property<long?>("CommentId")
                        .HasColumnType("bigint");

                    b.Property<int?>("LikeOrDisLike")
                        .HasColumnType("int");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BlogId");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("ReactionBlogs");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Photo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("UserRole")
                        .HasColumnType("int");

                    b.Property<decimal?>("Wallet")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Blog", b =>
                {
                    b.HasOne("PowerBlog.Site.Models.Category", "Category")
                        .WithMany("Blogs")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.User", "User")
                        .WithMany("Blogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Comment", b =>
                {
                    b.HasOne("PowerBlog.Site.Models.Blog", "Blog")
                        .WithMany("Comments")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Blog");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Favorite", b =>
                {
                    b.HasOne("PowerBlog.Site.Models.Blog", "Blog")
                        .WithMany("Favorites")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.User", "User")
                        .WithMany("Favorites")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Blog");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Order", b =>
                {
                    b.HasOne("PowerBlog.Site.Models.Blog", "Blog")
                        .WithMany("Orders")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.OfferPay", "OfferPay")
                        .WithMany("Orders")
                        .HasForeignKey("OfferPayId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Blog");

                    b.Navigation("OfferPay");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.ReactionBlog", b =>
                {
                    b.HasOne("PowerBlog.Site.Models.Blog", "Blog")
                        .WithMany("ReactionBlogs")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.Comment", "Comment")
                        .WithMany("ReactionBlogs")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("PowerBlog.Site.Models.User", "User")
                        .WithMany("ReactionBlogs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Blog");

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Blog", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Favorites");

                    b.Navigation("Orders");

                    b.Navigation("ReactionBlogs");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Category", b =>
                {
                    b.Navigation("Blogs");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.Comment", b =>
                {
                    b.Navigation("ReactionBlogs");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.OfferPay", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("PowerBlog.Site.Models.User", b =>
                {
                    b.Navigation("Blogs");

                    b.Navigation("Comments");

                    b.Navigation("Favorites");

                    b.Navigation("Orders");

                    b.Navigation("ReactionBlogs");
                });
#pragma warning restore 612, 618
        }
    }
}
