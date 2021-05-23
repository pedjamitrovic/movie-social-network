﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieSocialNetworkApi.Database;

namespace MovieSocialNetworkApi.Migrations
{
    [DbContext(typeof(MovieSocialNetworkDbContext))]
    [Migration("20210523154157_ExtendedTypeChangeMigration")]
    partial class ExtendedTypeChangeMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Ban", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BannedEntityId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("BannedFrom")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("BannedUntil")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BannedEntityId");

                    b.ToTable("Bans");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.ChatRoom", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("Id");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.ChatRoomMembership", b =>
                {
                    b.Property<int>("ChatRoomId")
                        .HasColumnType("int");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.HasKey("ChatRoomId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("ChatRoomMemberships");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Content", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Contents");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Content");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.GroupAdmin", b =>
                {
                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.HasKey("GroupId", "AdminId");

                    b.HasIndex("AdminId");

                    b.ToTable("GroupAdmins");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChatRoomId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ChatRoomId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Extended")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RecepientId")
                        .HasColumnType("int");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RecepientId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Reaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContentId")
                        .HasColumnType("int");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ContentId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Reactions");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Relation", b =>
                {
                    b.Property<int>("FollowingId")
                        .HasColumnType("int");

                    b.Property<int>("FollowerId")
                        .HasColumnType("int");

                    b.HasKey("FollowingId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("Relations");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Report", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("IssuedBanId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ReportedContentId")
                        .HasColumnType("int");

                    b.Property<int>("ReportedSystemEntityId")
                        .HasColumnType("int");

                    b.Property<int>("ReporterId")
                        .HasColumnType("int");

                    b.Property<bool>("Reviewed")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("IssuedBanId");

                    b.HasIndex("ReportedContentId");

                    b.HasIndex("ReportedSystemEntityId");

                    b.HasIndex("ReporterId");

                    b.ToTable("Reports");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.SystemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CoverImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SystemEntities");

                    b.HasDiscriminator<string>("Discriminator").HasValue("SystemEntity");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Comment", b =>
                {
                    b.HasBaseType("MovieSocialNetworkApi.Entities.Content");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.HasIndex("CreatorId");

                    b.HasIndex("PostId");

                    b.HasDiscriminator().HasValue("Comment");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Post", b =>
                {
                    b.HasBaseType("MovieSocialNetworkApi.Entities.Content");

                    b.Property<int?>("CreatorId")
                        .HasColumnType("int")
                        .HasColumnName("Post_CreatorId");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ForGroupId")
                        .HasColumnType("int");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ForGroupId");

                    b.HasDiscriminator().HasValue("Post");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Group", b =>
                {
                    b.HasBaseType("MovieSocialNetworkApi.Entities.SystemEntity");

                    b.Property<string>("Subtitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("Group");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.User", b =>
                {
                    b.HasBaseType("MovieSocialNetworkApi.Entities.SystemEntity");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Ban", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "BannedEntity")
                        .WithMany("Bans")
                        .HasForeignKey("BannedEntityId");

                    b.Navigation("BannedEntity");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.ChatRoomMembership", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.ChatRoom", "ChatRoom")
                        .WithMany("Memberships")
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Member")
                        .WithMany("ChatRoomMemberships")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ChatRoom");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.GroupAdmin", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.User", "Admin")
                        .WithMany("GroupAdmin")
                        .HasForeignKey("AdminId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MovieSocialNetworkApi.Entities.Group", "Group")
                        .WithMany("GroupAdmin")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Admin");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Message", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.ChatRoom", "ChatRoom")
                        .WithMany("Messages")
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatRoom");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Notification", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Recepient")
                        .WithMany("ReceivedNotifications")
                        .HasForeignKey("RecepientId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Sender")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Recepient");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Reaction", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.Content", "Content")
                        .WithMany("Reactions")
                        .HasForeignKey("ContentId");

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId");

                    b.Navigation("Content");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Relation", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Follower")
                        .WithMany("Following")
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Following")
                        .WithMany("Followers")
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Follower");

                    b.Navigation("Following");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Report", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.Ban", "IssuedBan")
                        .WithMany("Reports")
                        .HasForeignKey("IssuedBanId");

                    b.HasOne("MovieSocialNetworkApi.Entities.Content", "ReportedContent")
                        .WithMany("ReportedReports")
                        .HasForeignKey("ReportedContentId");

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "ReportedSystemEntity")
                        .WithMany("ReportedReports")
                        .HasForeignKey("ReportedSystemEntityId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Reporter")
                        .WithMany("ReporterReports")
                        .HasForeignKey("ReporterId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("IssuedBan");

                    b.Navigation("ReportedContent");

                    b.Navigation("ReportedSystemEntity");

                    b.Navigation("Reporter");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Comment", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Creator")
                        .WithMany("Comments")
                        .HasForeignKey("CreatorId");

                    b.HasOne("MovieSocialNetworkApi.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");

                    b.Navigation("Creator");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Post", b =>
                {
                    b.HasOne("MovieSocialNetworkApi.Entities.SystemEntity", "Creator")
                        .WithMany("Posts")
                        .HasForeignKey("CreatorId");

                    b.HasOne("MovieSocialNetworkApi.Entities.Group", "ForGroup")
                        .WithMany()
                        .HasForeignKey("ForGroupId");

                    b.Navigation("Creator");

                    b.Navigation("ForGroup");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Ban", b =>
                {
                    b.Navigation("Reports");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.ChatRoom", b =>
                {
                    b.Navigation("Memberships");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Content", b =>
                {
                    b.Navigation("Reactions");

                    b.Navigation("ReportedReports");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.SystemEntity", b =>
                {
                    b.Navigation("Bans");

                    b.Navigation("ChatRoomMemberships");

                    b.Navigation("Comments");

                    b.Navigation("Followers");

                    b.Navigation("Following");

                    b.Navigation("Posts");

                    b.Navigation("ReceivedNotifications");

                    b.Navigation("ReportedReports");

                    b.Navigation("ReporterReports");

                    b.Navigation("SentNotifications");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Post", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.Group", b =>
                {
                    b.Navigation("GroupAdmin");
                });

            modelBuilder.Entity("MovieSocialNetworkApi.Entities.User", b =>
                {
                    b.Navigation("GroupAdmin");
                });
#pragma warning restore 612, 618
        }
    }
}
