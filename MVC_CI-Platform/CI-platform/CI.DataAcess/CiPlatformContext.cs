using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CI.Models;

public partial class CiPlatformContext : DbContext
{
    public CiPlatformContext()
    {
    }

    public CiPlatformContext(DbContextOptions<CiPlatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CmsPage> CmsPages { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<FavoriteMission> FavoriteMissions { get; set; }

    public virtual DbSet<GoalMission> GoalMissions { get; set; }

    public virtual DbSet<Mission> Missions { get; set; }

    public virtual DbSet<MissionApplication> MissionApplications { get; set; }

    public virtual DbSet<MissionDocument> MissionDocuments { get; set; }

    public virtual DbSet<MissionInvite> MissionInvites { get; set; }

    public virtual DbSet<MissionMedia> MissionMedia { get; set; }

    public virtual DbSet<MissionRating> MissionRatings { get; set; }

    public virtual DbSet<MissionSkill> MissionSkills { get; set; }

    public virtual DbSet<MissionTheme> MissionThemes { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Story> Stories { get; set; }

    public virtual DbSet<StoryInvite> StoryInvites { get; set; }

    public virtual DbSet<StoryMedia> StoryMedia { get; set; }

    public virtual DbSet<StoryView> StoryViews { get; set; }

    public virtual DbSet<Timesheet> Timesheets { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSkill> UserSkills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=PCA107\\SQL2017;DataBase=Platform;User ID=sa;Password=Tatva@123;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("PK__admin__43AA414134B2702F");

            entity.ToTable("admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__banner__10373C34B2D145EA");

            entity.ToTable("banner");

            entity.Property(e => e.BannerId).HasColumnName("banner_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Image)
                .HasColumnType("text")
                .HasColumnName("image");
            entity.Property(e => e.SortOrder).HasColumnName("sort_order");
            entity.Property(e => e.Text)
                .HasColumnType("text")
                .HasColumnName("text");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__city__031491A8DE0995ED");

            entity.ToTable("city");

            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__city__country_id__45F365D3");
        });

        modelBuilder.Entity<CmsPage>(entity =>
        {
            entity.HasKey(e => e.CmsPageId).HasName("PK__cms_page__B46D5B5223D794CA");

            entity.ToTable("cms_page");

            entity.Property(e => e.CmsPageId).HasColumnName("cms_page_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Slug)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("slug");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValueSql("('1')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comment__E79576871CB6CC9B");

            entity.ToTable("comment");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("approval_status");
            entity.Property(e => e.Comment1)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Comments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comment__mission__6AEFE058");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comment__user_id__69FBBC1F");
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.ContactId);

            entity.ToTable("contact_us");

            entity.Property(e => e.ContactId).HasColumnName("contact_id");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.Subject)
                .HasColumnType("text")
                .HasColumnName("subject");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CountryId).HasName("PK__country__7E8CD0557DF4E00A");

            entity.ToTable("country");

            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Iso)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("ISO");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<FavoriteMission>(entity =>
        {
            entity.HasKey(e => e.FavoriteMissionId).HasName("PK__favorite__B4CF33120E4280F3");

            entity.ToTable("favorite_mission");

            entity.Property(e => e.FavoriteMissionId).HasColumnName("favorite_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.FavoriteMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__favorite___missi__5D95E53A");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteMissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__favorite___user___5CA1C101");
        });

        modelBuilder.Entity<GoalMission>(entity =>
        {
            entity.HasKey(e => e.GoalMissionId).HasName("PK__goal_mis__358E02C73C741366");

            entity.ToTable("goal_mission");

            entity.Property(e => e.GoalMissionId).HasColumnName("goal_mission_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.GoalObjectiveText)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("goal_objective_text");
            entity.Property(e => e.GoalValue).HasColumnName("goal_value");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.GoalMissions)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__goal_miss__missi__55F4C372");
        });

        modelBuilder.Entity<Mission>(entity =>
        {
            entity.HasKey(e => e.MissionId).HasName("PK__mission__B5419AB2C6422352");

            entity.ToTable("mission");

            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Achieved).HasColumnName("achieved");
            entity.Property(e => e.Availability)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("availability");
            entity.Property(e => e.AvbSeat).HasColumnName("avb_seat");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Deadline)
                .HasColumnType("date")
                .HasColumnName("deadline");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasMaxLength(2056)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.GoalObject)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("goal_object");
            entity.Property(e => e.MissionType)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("mission_type");
            entity.Property(e => e.OrganizationDetail)
                .HasColumnType("text")
                .HasColumnName("organization_detail");
            entity.Property(e => e.OrganizationName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("organization_name");
            entity.Property(e => e.ShortDescription)
                .HasMaxLength(1028)
                .IsUnicode(false)
                .HasColumnName("short_description");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.ThemeId).HasColumnName("theme_id");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.TotalSeats).HasColumnName("total_seats");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.City).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission__city_id__797309D9");

            entity.HasOne(d => d.Country).WithMany(p => p.Missions)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission__country__7B5B524B");

            entity.HasOne(d => d.Theme).WithMany(p => p.Missions)
                .HasForeignKey(d => d.ThemeId)
                .HasConstraintName("FK__mission__theme_i__778AC167");
        });

        modelBuilder.Entity<MissionApplication>(entity =>
        {
            entity.HasKey(e => e.MissionApplicationId).HasName("PK__mission___DF92838A7A11B80D");

            entity.ToTable("mission_application");

            entity.Property(e => e.MissionApplicationId).HasColumnName("mission_application_id");
            entity.Property(e => e.AppliedAt)
                .HasColumnType("datetime")
                .HasColumnName("applied_at");
            entity.Property(e => e.ApprovalStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("approval_status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_a__missi__09A971A2");

            entity.HasOne(d => d.User).WithMany(p => p.MissionApplications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_a__user___0A9D95DB");
        });

        modelBuilder.Entity<MissionDocument>(entity =>
        {
            entity.HasKey(e => e.MissionDocumentId).HasName("PK__mission___E80E0CC8FED94F57");

            entity.ToTable("mission_document");

            entity.Property(e => e.MissionDocumentId).HasColumnName("mission_document_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DocumentName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("document_name");
            entity.Property(e => e.DocumentPath)
                .HasColumnType("text")
                .HasColumnName("document_path");
            entity.Property(e => e.DocumentType)
                .HasColumnType("text")
                .HasColumnName("document_type");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionDocuments)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_d__missi__123EB7A3");
        });

        modelBuilder.Entity<MissionInvite>(entity =>
        {
            entity.HasKey(e => e.MissionInviteId).HasName("PK__mission___A97ED158BD0C62FC");

            entity.ToTable("mission_invite");

            entity.Property(e => e.MissionInviteId).HasColumnName("mission_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.FromUser).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__from___18EBB532");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionInvites)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_i__missi__17F790F9");
        });

        modelBuilder.Entity<MissionMedia>(entity =>
        {
            entity.HasKey(e => e.MissionMediaId).HasName("PK__mission___848A78E8AAD3C2BC");

            entity.ToTable("mission_media");

            entity.Property(e => e.MissionMediaId).HasColumnName("mission_media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Default)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasDefaultValueSql("('1')")
                .HasColumnName("default");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MediaName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("media_name");
            entity.Property(e => e.MediaPath)
                .HasColumnType("text")
                .HasColumnName("media_path");
            entity.Property(e => e.MediaType)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("media_type");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionMedia)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_m__missi__1F98B2C1");
        });

        modelBuilder.Entity<MissionRating>(entity =>
        {
            entity.HasKey(e => e.MissionRatingId).HasName("PK__mission___320E51722FD48EB3");

            entity.ToTable("mission_rating");

            entity.Property(e => e.MissionRatingId).HasColumnName("mission_rating_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_r__missi__282DF8C2");

            entity.HasOne(d => d.User).WithMany(p => p.MissionRatings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_r__user___2739D489");
        });

        modelBuilder.Entity<MissionSkill>(entity =>
        {
            entity.HasKey(e => e.MissionSkillId).HasName("PK__mission___827120082099173A");

            entity.ToTable("mission_skill");

            entity.Property(e => e.MissionSkillId).HasColumnName("mission_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Mission).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__mission_s__missi__2FCF1A8A");

            entity.HasOne(d => d.Skill).WithMany(p => p.MissionSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("FK__mission_s__skill__2EDAF651");
        });

        modelBuilder.Entity<MissionTheme>(entity =>
        {
            entity.HasKey(e => e.MissionThemeId).HasName("PK__mission___4925C5AC162D56BC");

            entity.ToTable("mission_theme");

            entity.Property(e => e.MissionThemeId).HasColumnName("mission_theme_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.ToTable("password_reset");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Token)
                .HasMaxLength(191)
                .IsUnicode(false)
                .HasColumnName("token");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__skill__FBBA83796EAAC7D3");

            entity.ToTable("skill");

            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.SkillName)
                .HasMaxLength(64)
                .IsUnicode(false)
                .HasColumnName("skill_name");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<Story>(entity =>
        {
            entity.HasKey(e => e.StoryId).HasName("PK__story__66339C56DD504640");

            entity.ToTable("story");

            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.PublishedAt)
                .HasColumnType("date")
                .HasColumnName("published_at");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('DRAFT')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Mission).WithMany(p => p.Stories)
                .HasForeignKey(d => d.MissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story__mission_i__367C1819");

            entity.HasOne(d => d.User).WithMany(p => p.Stories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story__user_id__3587F3E0");
        });

        modelBuilder.Entity<StoryInvite>(entity =>
        {
            entity.HasKey(e => e.StoryInviteId).HasName("PK__story_in__04497867CFD5982B");

            entity.ToTable("story_invite");

            entity.Property(e => e.StoryInviteId).HasColumnName("story_invite_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.FromUserId).HasColumnName("from_user_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.ToUserId).HasColumnName("to_user_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<StoryMedia>(entity =>
        {
            entity.HasKey(e => e.StoryMediaId).HasName("PK__story_me__29BD053C646530A9");

            entity.ToTable("story_media");

            entity.Property(e => e.StoryMediaId).HasColumnName("story_media_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Path)
                .HasColumnType("text")
                .HasColumnName("path");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryMedia)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__story_med__story__40F9A68C");
        });

        modelBuilder.Entity<StoryView>(entity =>
        {
            entity.HasKey(e => e.ViewId).HasName("PK__story_vi__B5A34EE2D4ED1665");

            entity.ToTable("story_views");

            entity.Property(e => e.ViewId).HasColumnName("view_id");
            entity.Property(e => e.StoryId).HasColumnName("story_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Story).WithMany(p => p.StoryViews)
                .HasForeignKey(d => d.StoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_story_views_story");

            entity.HasOne(d => d.User).WithMany(p => p.StoryViews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_story_views_user");
        });

        modelBuilder.Entity<Timesheet>(entity =>
        {
            entity.HasKey(e => e.TimesheetId).HasName("PK__timeshee__7BBF5068CE1161BB");

            entity.ToTable("timesheet");

            entity.Property(e => e.TimesheetId).HasColumnName("timesheet_id");
            entity.Property(e => e.Action).HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DateVolunteered)
                .HasColumnType("datetime")
                .HasColumnName("date_volunteered");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.MissionId).HasColumnName("mission_id");
            entity.Property(e => e.Notes)
                .HasColumnType("text")
                .HasColumnName("notes");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('PENDING')")
                .HasColumnName("status");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__user__B9BE370FC27A2B08");

            entity.ToTable("user");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Availablity)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("availablity");
            entity.Property(e => e.Avatar)
                .HasDefaultValueSql("('data:image/png;base64,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')")
                .HasColumnType("text")
                .HasColumnName("avatar");
            entity.Property(e => e.CityId).HasColumnName("city_id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("department");
            entity.Property(e => e.Email)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("employee_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.LinkedInUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("linked_in_url");
            entity.Property(e => e.Manager)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("manager");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("phone_number");
            entity.Property(e => e.ProfileText)
                .HasColumnType("text")
                .HasColumnName("profile_text");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasDefaultValueSql("('1')")
                .HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.WhyIVolunteer)
                .HasColumnType("text")
                .HasColumnName("why_i_volunteer");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK__user__city_id__5629CD9C");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .HasConstraintName("FK__user__country_id__571DF1D5");
        });

        modelBuilder.Entity<UserSkill>(entity =>
        {
            entity.HasKey(e => e.UserSkillId).HasName("PK__user_ski__FD3B576BFC5EE35F");

            entity.ToTable("user_skill");

            entity.Property(e => e.UserSkillId).HasColumnName("user_skill_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.SkillId).HasColumnName("skill_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Skill).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_skil__skill__503BEA1C");

            entity.HasOne(d => d.User).WithMany(p => p.UserSkills)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__user_skil__user___4F47C5E3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
