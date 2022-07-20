/******************************************
 * AUTHOR:          Rector
 * CREATEDON:       2018-09-26
 * OFFICIAL_SITE:    码友网(https://codedefault.com)--专注.NET/.NET Core
 * 版权所有，请勿删除
 ******************************************/

using AutoMapper;
using DncZeus.Api.Entities;
using DncZeus.Api.ViewModels.Rbac.DncIcon;
using DncZeus.Api.ViewModels.Rbac.DncMenu;
using DncZeus.Api.ViewModels.Rbac.DncPermission;
using DncZeus.Api.ViewModels.Rbac.DncRole;
using DncZeus.Api.ViewModels.Rbac.DncUser;
using DncZeus.Api.ViewModels.教务中心.ClassGrade;
using DncZeus.Api.ViewModels.教务中心.CourseHour;
using DncZeus.Api.ViewModels.教务中心.CourseSchedule;
using DncZeus.Api.ViewModels.教务中心.CourseSubject;
using DncZeus.Api.ViewModels.教务中心.Teacher;
using DncZeus.Api.ViewModels.教务中心.TeacherAttence;
using DncZeus.Api.ViewModels.教务中心.Trainees;
using DncZeus.Api.ViewModels.教务中心.TraineesAttence;
using DncZeus.Api.ViewModels.系统管理.OperationLog;
using DncZeus.Api.ViewModels.营销中心.AuditionCourse;
using DncZeus.Api.ViewModels.营销中心.Customer;
using System;
using System.Linq;

namespace DncZeus.Api.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public MappingProfile()
        {
            #region DncUser
            CreateMap<DncUser, UserJsonModel>();
            CreateMap<UserCreateViewModel, DncUser>();
            CreateMap<UserEditViewModel, DncUser>();
            CreateMap<DncUser, UserEditViewModel>();
            #endregion

            #region DncRole
            CreateMap<DncRole, RoleJsonModel>();
            CreateMap<RoleCreateViewModel, DncRole>();
            CreateMap<DncRole, RoleCreateViewModel>();
            #endregion

            #region DncMenu
            CreateMap<DncMenu, MenuJsonModel>();
            CreateMap<MenuCreateViewModel, DncMenu>();
            CreateMap<MenuEditViewModel, DncMenu>();
            CreateMap<DncMenu, MenuEditViewModel>();
            #endregion

            #region DncIcon
            CreateMap<DncIcon, IconCreateViewModel>();
            CreateMap<DncIcon, IconJsonModel>();
            CreateMap<IconCreateViewModel, DncIcon>();
            #endregion

            #region DncPermission
            CreateMap<DncPermission, PermissionJsonModel>()
                .ForMember(d => d.MenuName, s => s.MapFrom(x => x.Menu.Name))
                .ForMember(d => d.PermissionTypeText, s => s.MapFrom(x => x.Type.ToString()));
            CreateMap<PermissionCreateViewModel, DncPermission>();
            CreateMap<PermissionEditViewModel, DncPermission>();
            CreateMap<DncPermission, PermissionEditViewModel>();
            #endregion

            #region Customer
            CreateMap<Customer, CustomerJsonModel>()
                .ForMember(d => d.BindTraineesCount, s => s.MapFrom(x => x.Trainees.Count));
            #endregion

            #region Trainees
            CreateMap<Trainees, TraineesJsonModel>()
                .ForMember(d => d.ClassName, s => s.MapFrom(x => x.GetClassName()))
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.GetCourseName()))
                .ForMember(d => d.IdCardBind, s => s.MapFrom(x => (string.IsNullOrEmpty(x.IdCardBindInfo)) ? false : true))
                .ForMember(d => d.FaceBind, s => s.MapFrom(x => (x.FaceFeatureGuid == null || x.FaceFeatureGuid == Guid.Empty) ? false : true));
            CreateMap<Trainees, TraineesCreateOrEditViewModel>();
            CreateMap<TraineesCreateOrEditViewModel, Trainees>();
            #endregion

            #region ClassGrade
            CreateMap<ClassGrade, ClassGradeJsonModel>()
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.CourseSubject.CourseName))
                .ForMember(d => d.TraineesCount, s => s.MapFrom(x => x.CourseHours.Count));
            #endregion

            #region CourseSubject
            CreateMap<CourseSubject, CourseSubjectJsonModel>()
                .ForMember(d => d.ClassGradesCount, s => s.MapFrom(x => x.ClassGrades.Count));
            #endregion

            #region CourseHour
            CreateMap<CourseHour, CourseHourJsonModel>()
                .ForMember(d => d.FullName, s => s.MapFrom(x => x.Trainees.FullName))
                .ForMember(d => d.Telephone, s => s.MapFrom(x => x.Trainees.Telephone))
                .ForMember(d => d.ClassName, s => s.MapFrom(x => x.ClassGrade.ClassName))
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.CourseSubject.CourseName))
                .ForMember(d => d.ExpiryDate, s => s.MapFrom(x => x.ExpiryDate == DateTime.MaxValue ? "无期限" : x.ExpiryDate.ToString("yyyy-MM-dd")));
            CreateMap<CourseHourCreateOrEditViewModel, CourseHour>()
                .ForMember(d => d.ExpiryDate, s => s.MapFrom(x => x.IsMaxExpiryDate ? DateTime.MaxValue : DateTime.Parse(x.ExpiryDate)));
            CreateMap<CourseHour, CourseHourCreateOrEditViewModel>()
                .ForMember(d => d.IsMaxExpiryDate, s => s.MapFrom(x => x.ExpiryDate == DateTime.MaxValue));
            #endregion

            #region OperationLog
            CreateMap<OperationLog, OperationLogJsonModel>();
            #endregion

            #region Teacher
            CreateMap<Teacher, TeacherJsonModel>()
                .ForMember(d => d.Attendance, s => s.MapFrom(x => x.TeacherAttences.Count))
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.GetCourseName()))
                .ForMember(d => d.IdCardBind, s => s.MapFrom(x => (string.IsNullOrEmpty(x.IdCardBindInfo)) ? false : true))
                .ForMember(d => d.FaceBind, s => s.MapFrom(x => (x.FaceFeatureGuid == null || x.FaceFeatureGuid == Guid.Empty) ? false : true));
            CreateMap<TeacherCreateOrEditViewModel, Teacher>();
            CreateMap<Teacher, TeacherCreateOrEditViewModel>();
            #endregion

            #region CourseSchedule
            CreateMap<CourseSchedule, CourseScheduleJsonModel>()
                .ForMember(d => d.TeacherName, s => s.MapFrom(x => x.Teacher.FullName))
                .ForMember(d => d.DisplayTime, s => s.MapFrom(x => x.StartDate.ToString("yyyy年MM月dd日")
                + " 至 " + x.EndDate.ToString("yyyy年MM月dd日")
                + " " + x.StartTime.ToString(@"hh\:mm")
                + "-" + x.EndTime.ToString(@"hh\:mm")
                + " " + x.DayOfWeek.ToString()));
            CreateMap<CourseScheduleCreateViewModel, CourseSchedule>();
            CreateMap<CourseSchedule, CourseScheduleEditViewModel>();
            #endregion

            #region TeacherAttence
            CreateMap<TeacherAttence, TeacherAttenceJsonModel>()
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.CourseSchedule.CourseName))
                .ForMember(d => d.TeacherName, s => s.MapFrom(x => x.Teacher.FullName))
                .ForMember(d => d.SubstituteName, s => s.MapFrom(x => x.IsSubstitute == Entities.Enums.CommonEnum.YesOrNo.Yes ?
                x.ParentGuidNavigation.Teacher.FullName
                : x.IsAttend == Entities.Enums.CommonEnum.YesOrNo.No
                ? x.InverseParentGuidNavigation.Teacher.FullName
                : ""));
            #endregion

            #region TraineesAttence
            CreateMap<TraineesAttence, TraineesAttenceJsonModel>()
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.CourseSchedule.CourseName))
                .ForMember(d => d.TraineesName, s => s.MapFrom(x => x.Trainees.FullName));
            #endregion

            #region AuditionCourse
            CreateMap<AuditionCourse, AuditionCourseJsonModel>()
                .ForMember(d => d.CourseName, s => s.MapFrom(x => x.CourseSubject.CourseName))
                .ForMember(d => d.Telephone, s => s.MapFrom(x => x.Trainees.Telephone))
                .ForMember(d => d.FullName, s => s.MapFrom(x => x.Trainees.FullName))
                .ForMember(d => d.ClassName, s => s.MapFrom(x => x.ClassGrade.ClassName));
            #endregion
        }
    }
}
