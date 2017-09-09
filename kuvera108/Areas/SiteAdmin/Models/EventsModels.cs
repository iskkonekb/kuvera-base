using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using kuvera108.Data;

namespace kuvera108.Areas.SiteAdmin.Models
{
    public class EventsSearchBox
    {
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Дата С обязательно для заполнения")]
        [Display(Name = "Дата С")]
        [DefaultValue(true)]
        public String FromDate { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Дата ПОП обязательно для заполнения")]
        [Display(Name = "Дата ПО")]
        [DefaultValue(true)]
        public String ToDate { get; set; }
        public EventsSearchBox()
        {
            FromDate = DateTime.Now.Date.ToString();
            ToDate = DateTime.Now.Date.AddHours(24).AddMilliseconds(-1).ToString();
        }

}
    public class EventsModels
    {
        public EventsSearchBox SearchBox { get; set; }
        public IEnumerable<Log> Log { get; set; }
        public EventsModels()
        {
            SearchBox = new EventsSearchBox();
        }
    }
}