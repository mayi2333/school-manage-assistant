/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using DncZeus.Api.Entities.QueryModels.DncPermission;
using Microsoft.EntityFrameworkCore;

namespace DncZeus.Api.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class DncZeusDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public DncZeusDbContext(DbContextOptions<DncZeusDbContext> options) : base(options)
        {

        }
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<DncUser> DncUser { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<DncRole> DncRole { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<DncMenu> DncMenu { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public DbSet<DncIcon> DncIcon { get; set; }

        /// <summary>
        /// 用户-角色多对多映射
        /// </summary>
        public DbSet<DncUserRoleMapping> DncUserRoleMapping { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public DbSet<DncPermission> DncPermission { get; set; }
        /// <summary>
        /// 角色-权限多对多映射
        /// </summary>
        public DbSet<DncRolePermissionMapping> DncRolePermissionMapping { get; set; }

        /// <summary>
        /// 客户
        /// </summary>
        public DbSet<Customer> Customer { get; set; }
        /// <summary>
        /// 学员
        /// </summary>
        public DbSet<Trainees> Trainees { get; set; }
        /// <summary>
        /// 课程科目
        /// </summary>
        public DbSet<CourseSubject> CourseSubject { get; set; }
        /// <summary>
        /// 课时
        /// </summary>
        public DbSet<CourseHour> CourseHour { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public DbSet<ClassGrade> ClassGrade { get; set; }
        /// <summary>
        /// 老师
        /// </summary>
        public DbSet<Teacher> Teacher { get; set; }
        /// <summary>
        /// 老师考勤
        /// </summary>
        public DbSet<TeacherAttence> TeacherAttence { get; set; }
        /// <summary>
        /// 学员考勤
        /// </summary>
        public DbSet<TraineesAttence> TraineesAttence { get; set; }
        /// <summary>
        /// 课程时间表
        /// </summary>
        public DbSet<CourseSchedule> CourseSchedule { get; set; }
        /// <summary>
        /// 操作日志
        /// </summary>
        public DbSet<OperationLog> OperationLog { get; set; }
        /// <summary>
        /// 班级课程表映射
        /// </summary>
        public DbSet<ClassGradeCourseScheduleMapping> ClassGradeCourseScheduleMapping { get; set; }
        /// <summary>
        /// 学员课时课程表映射
        /// </summary>
        public DbSet<CourseHourCourseScheduleMapping> CourseHourCourseScheduleMapping { get; set; }
        /// <summary>
        /// 人脸特征记录表
        /// </summary>
        public DbSet<FaceFeature> FaceFeature { get; set; }
        /// <summary>
        /// 试听课预约
        /// </summary>
        public DbSet<AuditionCourse> AuditionCourse { get; set; }
        ///// <summary>
        ///// 系统配置
        ///// </summary>
        //public DbSet<SysConfig> SysConfig { get; set; }
        /// <summary>
        /// 微信模板消息
        /// </summary>
        public DbSet<TemplateMsg> TemplateMsg { get; set; }

        #region DbQuery
        /// <summary>
        /// 
        /// </summary>
        public DbQuery<DncPermissionWithAssignProperty> DncPermissionWithAssignProperty { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbQuery<DncPermissionWithMenu> DncPermissionWithMenu { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<DncUser>()
            //    .Property(x => x.Status);
            //modelBuilder.Entity<DncUser>()
            //    .Property(x => x.IsDeleted);


            modelBuilder.Entity<DncRole>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
            });

            modelBuilder.Entity<DncMenu>(entity =>
            {
                //entity.haso
            });


            modelBuilder.Entity<DncUserRoleMapping>(entity =>
            {
                entity.HasKey(x => new
                {
                    x.UserGuid,
                    x.RoleCode
                });

                entity.HasOne(x => x.DncUser)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.UserGuid)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.DncRole)
                    .WithMany(x => x.UserRoles)
                    .HasForeignKey(x => x.RoleCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<DncPermission>(entity =>
            {
                entity.HasIndex(x => x.Code)
                    .IsUnique();

                entity.HasOne(x => x.Menu)
                    .WithMany(x => x.Permissions)
                    .HasForeignKey(x => x.MenuGuid);
            });

            modelBuilder.Entity<DncRolePermissionMapping>(entity =>
            {
                entity.HasKey(x => new
                {
                    x.RoleCode,
                    x.PermissionCode
                });

                entity.HasOne(x => x.DncRole)
                    .WithMany(x => x.Permissions)
                    .HasForeignKey(x => x.RoleCode)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.DncPermission)
                    .WithMany(x => x.Roles)
                    .HasForeignKey(x => x.PermissionCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasMany(x => x.Trainees)
                    .WithOne(x => x.RelationCust)
                    .HasForeignKey(x => x.CustomerGuid)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Trainees>(entity =>
            {
                entity.HasOne(x => x.RelationCust)
                    .WithMany(x => x.Trainees);
                    //.HasForeignKey(x => x.CustomerGuid)
                    //.OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasMany(x => x.CourseHours)
                    .WithOne(x => x.Trainees)
                    .HasForeignKey(x => x.TraineesGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.TraineesAttences)
                    .WithOne(x => x.Trainees)
                    .HasForeignKey(x => x.TraineesGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.FaceFeature)
                    .WithOne(x => x.Trainees)
                    .HasForeignKey<Trainees>(x => x.FaceFeatureGuid)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(x => x.AuditionCourses)
                    .WithOne(x => x.Trainees)
                    .HasForeignKey(x => x.TraineesGuid)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CourseSubject>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
                entity.HasMany(x => x.ClassGrades)
                    .WithOne(x => x.CourseSubject)
                    .HasForeignKey(x => x.CourseCode)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.CourseHours)
                    .WithOne(x => x.CourseSubject)
                    .HasForeignKey(x => x.CourseCode)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.CourseSchedules)
                    .WithOne(x => x.CourseSubject)
                    .HasForeignKey(x => x.CourseCode)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.AuditionCourses)
                    .WithOne(x => x.CourseSubject)
                    .HasForeignKey(x => x.CourseCode)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<CourseHour>(entity =>
            {
                entity.HasOne(x => x.CourseSubject)
                    .WithMany(x => x.CourseHours);
                entity.HasOne(x => x.ClassGrade)
                    .WithMany(x => x.CourseHours);
                entity.HasOne(x => x.Trainees)
                    .WithMany(x => x.CourseHours);
                entity.HasMany(x => x.TraineesAttences)
                    .WithOne(x => x.CourseHour)
                    .HasForeignKey(x => x.CourseHourGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.CourseHourCourseSchedule)
                    .WithOne(x => x.CourseHour)
                    .HasForeignKey(x => x.CourseHourGuid)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ClassGrade>(entity =>
            {
                entity.HasOne(x => x.CourseSubject)
                    .WithMany(x => x.ClassGrades);
                    //.HasForeignKey(x => x.CourseCode)
                    //.OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.CourseHours)
                    .WithOne(x => x.ClassGrade)
                    .HasForeignKey(x => x.ClassGradeGuid)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(x => x.ClassGradeCourseSchedule)
                    .WithOne(x => x.ClassGrade)
                    .HasForeignKey(x => x.ClassGradeGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.AuditionCourses)
                    .WithOne(x => x.ClassGrade)
                    .HasForeignKey(x => x.ClassGradeGuid)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.HasMany(x => x.TeacherAttences)
                    .WithOne(x => x.Teacher)
                    .HasForeignKey(x => x.TeacherGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.CourseSchedules)
                    .WithOne(x => x.Teacher)
                    .HasForeignKey(x => x.TeacherGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.FaceFeature)
                    .WithOne(x => x.Teacher)
                    .HasForeignKey<Teacher>(x => x.FaceFeatureGuid)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TeacherAttence>(entity =>
            {
                entity.HasOne(x => x.Teacher)
                    .WithMany(x => x.TeacherAttences);
                entity.HasOne(x => x.CourseSchedule)
                    .WithMany(x => x.TeacherAttences);
                entity.HasOne(x => x.ParentGuidNavigation)
                    .WithOne(x => x.InverseParentGuidNavigation)
                    .HasForeignKey<TeacherAttence>(x => x.ParentGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.TraineesAttences)
                    .WithOne(x => x.TeacherAttence)
                    .HasForeignKey(x => x.TeacherAttenceGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.AuditionCourses)
                    .WithOne(x => x.TeacherAttence)
                    .HasForeignKey(x => x.TeacherAttenceGuid)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<TraineesAttence>(entity =>
            {
                entity.HasOne(x => x.CourseHour)
                    .WithMany(x => x.TraineesAttences);
                entity.HasOne(x => x.CourseSchedule)
                    .WithMany(x => x.TraineesAttences);
                entity.HasOne(x => x.TeacherAttence)
                    .WithMany(x => x.TraineesAttences);
                entity.HasOne(x => x.Trainees)
                    .WithMany(x => x.TraineesAttences);
            });

            modelBuilder.Entity<CourseSchedule>(entity =>
            {
                entity.HasMany(x => x.TeacherAttences)
                    .WithOne(x => x.CourseSchedule)
                    .HasForeignKey(x => x.CourseScheduleGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.TraineesAttences)
                    .WithOne(x => x.CourseSchedule)
                    .HasForeignKey(x => x.CourseScheduleGuid)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(x => x.Teacher)
                    .WithMany(x => x.CourseSchedules);
                entity.HasMany(x => x.ClassGradeCourseSchedule)
                    .WithOne(x => x.CourseSchedule)
                    .HasForeignKey(x => x.CourseScheduleGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(x => x.CourseHourCourseSchedule)
                    .WithOne(x => x.CourseSchedule)
                    .HasForeignKey(x => x.CourseScheduleGuid)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.CourseSubject)
                    .WithMany(x => x.CourseSchedules);
            });

            modelBuilder.Entity<OperationLog>(entity =>
            {
            });

            modelBuilder.Entity<ClassGradeCourseScheduleMapping>(entity =>
            {
                entity.HasKey(x => new
                {
                    x.ClassGradeGuid,
                    x.CourseScheduleGuid
                });
                entity.HasOne(x => x.ClassGrade)
                    .WithMany(x => x.ClassGradeCourseSchedule);
                entity.HasOne(x => x.CourseSchedule)
                    .WithMany(x => x.ClassGradeCourseSchedule);
            });
            modelBuilder.Entity<CourseHourCourseScheduleMapping>(entity =>
            {
                entity.HasKey(x => new
                {
                    x.CourseHourGuid,
                    x.CourseScheduleGuid
                });
                entity.HasOne(x => x.CourseHour)
                    .WithMany(x => x.CourseHourCourseSchedule);
                entity.HasOne(x => x.CourseSchedule)
                    .WithMany(x => x.CourseHourCourseSchedule);
            });
            modelBuilder.Entity<FaceFeature>(entity =>
            {
                entity.HasOne(x => x.Teacher)
                    .WithOne(x => x.FaceFeature);
                entity.HasOne(x => x.Trainees)
                    .WithOne(x => x.FaceFeature);
            });
            modelBuilder.Entity<AuditionCourse>(entity =>
            {
                entity.HasOne(x => x.Trainees)
                    .WithMany(x => x.AuditionCourses);
                entity.HasOne(x => x.CourseSubject)
                    .WithMany(x => x.AuditionCourses);
                entity.HasOne(x => x.ClassGrade)
                    .WithMany(x => x.AuditionCourses);
                entity.HasOne(x => x.TeacherAttence)
                    .WithMany(x => x.AuditionCourses);
            });
            //modelBuilder.Entity<SysConfig>(entity =>
            //{
            //});
            modelBuilder.Entity<TemplateMsg>(entity =>
            {
            });
            base.OnModelCreating(modelBuilder);
        }
        //#if DEBUG
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    var loggerFactory = new LoggerFactory();
        //    loggerFactory.AddProvider(new EFLoggerProvider());
        //    optionsBuilder.UseLoggerFactory(loggerFactory);

        //    base.OnConfiguring(optionsBuilder);
        //}
        //#endif
    }
}
