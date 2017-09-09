using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace kuvera108.Areas.SiteAdmin.Models
{
    // Определение реквизитов Обществ
    public class Org
    {
        public string Id { get; set; }
        public string Owner { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Название общества обязательно для заполнения")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Descr { get; set; }
    }
    /*public interface IOrgInterface<TEntity> where TEntity : class
    {
    }
    
    public class OrgInterface
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;
        public DbSet DbSet { get { return _dbSet; } }
        public OrgInterface(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
        public TEntity FindByEmail(string User_Email)
        {
            OrgUsers 
            return _dbSet.Find(User_Email);
        }

                public IEnumerable<TEntity> Get()
                {
                    return _dbSet.AsNoTracking().ToList();
                }
       
    }*/
}