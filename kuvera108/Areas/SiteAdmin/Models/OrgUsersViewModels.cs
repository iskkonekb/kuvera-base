using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using kuvera108.Data;
using System.Web.Mvc;

namespace kuvera108.Areas.SiteAdmin.Models
{
    public class OrgUsers
    {
        public string Id { get; set; }
        public string Org_Id { get; set; }
        public string User_Id { get; set; }
        public string Owner { get; set; }
    }

    [Table("OrgsUsers_MainView")]
    public class OrgUsersViewModel
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Общество обязательно для заполнения")]
        [Display(Name = "Общество")]
        public string Org_Id { get; set; }
        public string User_Id { get; set; }
        public string Owner { get; set; }
        [Display(Name = "Название общества")]
        public string OrgName { get; set; }
        [Display(Name = "Описание общества")]
        public string OrgDescr { get; set; }
        [Display(Name = "Логин пользователя")]
        public string User_Name { get; set; }
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        [Required(ErrorMessage = "Адрес электронной почты обязателен для заполнения")]
        public string User_Email { get; set; }
        [Display(Name = "Имя пользователя")]
        public string User_FullName { get; set; }
        [Display(Name = "Логин владельца")]
        public string Owner_Name { get; set; }
        [Display(Name = "Эл. почта владельца")]
        public string Owner_Email { get; set; }
        [Display(Name = "Имя владельца")]
        public string Owner_FullName { get; set; }
    }

    public class OrgUsersSearchAndCreateModel
    {
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Общество обязательно для заполнения")]
        [Display(Name = "Общество")]
        public string Org_Id { get; set; }
        [EmailAddress]
        [Display(Name = "Адрес электронной почты")]
        [Required(ErrorMessage = "Адрес электронной почты обязателен для заполнения")]
        public string User_Email { get; set; }
        public string Owner { get; set; }
        public SelectList OrgList { get; set; }
    }
}