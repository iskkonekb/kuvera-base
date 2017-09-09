using System.Web;
using System;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.Owin;
using kuvera108.Areas.SiteAdmin.Models;

namespace kuvera108.Data
{
    // Определение реквизитов логов
    public class Log
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Логин")]
        public string Login { get; set; }
        [Display(Name = "IP адрес")]
        public string Ip { get; set; }
        [Display(Name = "URL адрес")]
        public string Url { get; set; }
        [Required]
        [Display(Name = "Время")]
        public DateTime Dt { get; set; }
        public string FormattedDate => Dt.ToShortDateString();
        [Required]
        [Display(Name = "Описание")]
        public string Desc { get; set; }
        [Required, MaxLength(100)]
        [Display(Name = "Тип")]
        public string Typ { get; set; }
    }

    public class LogConfiguration : EntityTypeConfiguration<Log>
    {
        public LogConfiguration()
        {
            Property(p => p.Dt).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("Idx_Log_DtTyp", 1)));
            Property(p => p.Typ).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("Idx_Log_DtTyp", 2)));
        }
    }
    //Запись логов в БД
    public static class Save_log
    {
        private static ApplicationDbContext _dbContext;

        public static ApplicationDbContext DbContext
        {
            get
            {
                return _dbContext ?? HttpContext.Current.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }

        public static void Log(string des, string typ)
        {
            HttpContext context = HttpContext.Current;
            var request = context.Request;

            Log log = new Log()
            {
                Id = Convert.ToString(Guid.NewGuid()),
                Login = (request.IsAuthenticated) ? context.User.Identity.Name : "null",
                Ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                Url = request.RawUrl,
                Dt = DateTime.UtcNow,
                Desc = des,
                Typ = typ
            };

            DbContext.Logs.Add(log);
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var Response = context.Response;
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
        }
    }
    /*public class LogsManage
    {
        public void Get1()
        {
            using (Db db = new Db())
            {
                EFGenericRepository<Log> Lg = new EFGenericRepository<Log>(db);
                IEnumerable<Log> lst = Lg.GetWithInclude(x => x.Dt.Date);
                foreach (Log p in lst)
                {
                    //Console.WriteLine($"{p.Name} ({p.Company.Name}) - {p.Price}");
                }
            }
        }
    }*/
}