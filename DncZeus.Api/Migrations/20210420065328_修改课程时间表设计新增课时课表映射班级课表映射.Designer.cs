﻿// <auto-generated />
using System;
using DncZeus.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DncZeus.Api.Migrations
{
    [DbContext(typeof(DncZeusDbContext))]
    [Migration("20210420065328_修改课程时间表设计新增课时课表映射班级课表映射")]
    partial class 修改课程时间表设计新增课时课表映射班级课表映射
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DncZeus.Api.Entities.ClassGrade", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<int>("IsSpecial")
                        .HasColumnType("int");

                    b.Property<string>("Memo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPeople")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.HasIndex("CourseCode");

                    b.ToTable("ClassGrade");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.ClassGradeCourseScheduleMapping", b =>
                {
                    b.Property<Guid>("ClassGradeGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseScheduleGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("ClassGradeGuid", "CourseScheduleGuid");

                    b.HasIndex("CourseScheduleGuid");

                    b.ToTable("ClassGradeCourseScheduleMapping");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseHour", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ClassGradeGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("OperationLog")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Surplus")
                        .HasColumnType("int");

                    b.Property<Guid>("TraineesGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("ClassGradeGuid");

                    b.HasIndex("CourseCode");

                    b.HasIndex("TraineesGuid");

                    b.ToTable("CourseHour");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseHourCourseScheduleMapping", b =>
                {
                    b.Property<Guid>("CourseHourGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseScheduleGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("CourseHourGuid", "CourseScheduleGuid");

                    b.HasIndex("CourseScheduleGuid");

                    b.ToTable("CourseHourCourseScheduleMapping");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseSchedule", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CourseSubjectCode")
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("CourseSubjectGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("EndDay")
                        .HasColumnType("int");

                    b.Property<int>("EndMonth")
                        .HasColumnType("int");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<int>("IsEnabled")
                        .HasColumnType("int");

                    b.Property<string>("Memo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("StartDay")
                        .HasColumnType("int");

                    b.Property<int>("StartMonth")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TeacherGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("CourseSubjectCode");

                    b.HasIndex("TeacherGuid");

                    b.ToTable("CourseSchedule");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseSubject", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("ChargeType")
                        .HasColumnType("int");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("CourseSubject");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.Customer", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("WxCity")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WxCountry")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WxHeadimgurl")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("WxNickname")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("WxOpenid")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("WxProvince")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("WxSex")
                        .HasColumnType("int");

                    b.Property<string>("WxUnionid")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Guid");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncIcon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Custom")
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DncIcon");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncMenu", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Alias")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("BeforeCloseFun")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("Component")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(800)");

                    b.Property<int?>("HideInMenu")
                        .HasColumnType("int");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("IsDefaultRouter")
                        .HasColumnType("int");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("NotCache")
                        .HasColumnType("int");

                    b.Property<Guid?>("ParentGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ParentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Sort")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Guid");

                    b.ToTable("DncMenu");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncPermission", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ActionCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(80)");

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<Guid>("MenuGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("MenuGuid");

                    b.ToTable("DncPermission");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncPermissionWithMenu", b =>
                {
                    b.Property<int>("IsDefaultRouter")
                        .HasColumnType("int");

                    b.Property<string>("MenuAlias")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("MenuGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("MenuName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermissionActionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermissionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PermissionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PermissionType")
                        .HasColumnType("int");

                    b.ToTable("DncPermissionWithMenu");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncRole", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBuiltin")
                        .HasColumnType("bit");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<bool>("IsSuperAdministrator")
                        .HasColumnType("bit");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("DncRole");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncRolePermissionMapping", b =>
                {
                    b.Property<string>("RoleCode")
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PermissionCode")
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("RoleCode", "PermissionCode");

                    b.HasIndex("PermissionCode");

                    b.ToTable("DncRolePermissionMapping");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncUser", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(255)");

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(800)");

                    b.Property<string>("DisplayName")
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<int>("IsLocked")
                        .HasColumnType("int");

                    b.Property<string>("LoginName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("UserType")
                        .HasColumnType("int");

                    b.HasKey("Guid");

                    b.ToTable("DncUser");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncUserRoleMapping", b =>
                {
                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoleCode")
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("UserGuid", "RoleCode");

                    b.HasIndex("RoleCode");

                    b.ToTable("DncUserRoleMapping");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.OperationLog", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ActionName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ControllerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descriptor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MethodName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MoudleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OperationByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OperationByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OperationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Parameter")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Guid");

                    b.ToTable("OperationLog");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.QueryModels.DncPermission.DncPermissionWithAssignProperty", b =>
                {
                    b.Property<string>("ActionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IsAssigned")
                        .HasColumnType("int");

                    b.Property<Guid?>("MenuGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleCode")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("DncPermissionWithAssignProperty");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.Teacher", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<string>("Memo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Guid");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.TeacherAttence", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseScheduleGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsAttend")
                        .HasColumnType("int");

                    b.Property<int>("IsSubstitute")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("ParentGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TeacherGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("CourseScheduleGuid");

                    b.HasIndex("ParentGuid")
                        .IsUnique()
                        .HasFilter("[ParentGuid] IS NOT NULL");

                    b.HasIndex("TeacherGuid");

                    b.ToTable("TeacherAttence");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.Trainees", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(250)");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CustomerGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("IsDeleted")
                        .HasColumnType("int");

                    b.Property<string>("Memo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.HasKey("Guid");

                    b.HasIndex("CustomerGuid");

                    b.ToTable("Trainees");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.TraineesAttence", b =>
                {
                    b.Property<Guid>("Guid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CourseHourGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CourseScheduleGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CreatedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<int>("IsAttend")
                        .HasColumnType("int");

                    b.Property<Guid?>("ModifiedByUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModifiedByUserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Guid");

                    b.HasIndex("CourseHourGuid");

                    b.HasIndex("CourseScheduleGuid");

                    b.ToTable("TraineesAttence");
                });

            modelBuilder.Entity("DncZeus.Api.Entities.ClassGrade", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.CourseSubject", "CourseSubject")
                        .WithMany("ClassGrades")
                        .HasForeignKey("CourseCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.ClassGradeCourseScheduleMapping", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.ClassGrade", "ClassGrade")
                        .WithMany("ClassGradeCourseSchedule")
                        .HasForeignKey("ClassGradeGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.CourseSchedule", "CourseSchedule")
                        .WithMany("ClassGradeCourseSchedule")
                        .HasForeignKey("CourseScheduleGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseHour", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.ClassGrade", "ClassGrade")
                        .WithMany("CourseHours")
                        .HasForeignKey("ClassGradeGuid")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DncZeus.Api.Entities.CourseSubject", "CourseSubject")
                        .WithMany("CourseHours")
                        .HasForeignKey("CourseCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.Trainees", "Trainees")
                        .WithMany("CourseHours")
                        .HasForeignKey("TraineesGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseHourCourseScheduleMapping", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.CourseHour", "CourseHour")
                        .WithMany("CourseHourCourseSchedule")
                        .HasForeignKey("CourseHourGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.CourseSchedule", "CourseSchedule")
                        .WithMany("CourseHourCourseSchedule")
                        .HasForeignKey("CourseScheduleGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.CourseSchedule", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.CourseSubject", "CourseSubject")
                        .WithMany()
                        .HasForeignKey("CourseSubjectCode");

                    b.HasOne("DncZeus.Api.Entities.Teacher", "Teacher")
                        .WithMany("CourseSchedules")
                        .HasForeignKey("TeacherGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncPermission", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.DncMenu", "Menu")
                        .WithMany("Permissions")
                        .HasForeignKey("MenuGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncRolePermissionMapping", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.DncPermission", "DncPermission")
                        .WithMany("Roles")
                        .HasForeignKey("PermissionCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.DncRole", "DncRole")
                        .WithMany("Permissions")
                        .HasForeignKey("RoleCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.DncUserRoleMapping", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.DncRole", "DncRole")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleCode")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.DncUser", "DncUser")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.TeacherAttence", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.CourseSchedule", "CourseSchedule")
                        .WithMany("TeacherAttences")
                        .HasForeignKey("CourseScheduleGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.TeacherAttence", "ParentGuidNavigation")
                        .WithOne("InverseParentGuidNavigation")
                        .HasForeignKey("DncZeus.Api.Entities.TeacherAttence", "ParentGuid")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DncZeus.Api.Entities.Teacher", "Teacher")
                        .WithMany("TeacherAttences")
                        .HasForeignKey("TeacherGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DncZeus.Api.Entities.Trainees", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.Customer", "RelationCust")
                        .WithMany("Trainees")
                        .HasForeignKey("CustomerGuid")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("DncZeus.Api.Entities.TraineesAttence", b =>
                {
                    b.HasOne("DncZeus.Api.Entities.CourseHour", "CourseHour")
                        .WithMany("TraineesAttences")
                        .HasForeignKey("CourseHourGuid")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DncZeus.Api.Entities.CourseSchedule", "CourseSchedule")
                        .WithMany("TraineesAttences")
                        .HasForeignKey("CourseScheduleGuid")
                        .OnDelete(DeleteBehavior.SetNull);
                });
#pragma warning restore 612, 618
        }
    }
}
