using System;
using System.Reflection;
using System.Web.Mvc;
/* Фильтр обработки кнопки post из формы:
 * Пример: Страница
<form action="" method="post">
    <input type="submit" value="Publish1" name="action:Publish1" />
    <input type="submit" value="Publish0" name="action:Publish0" />
</form>
Контроллер
using kuvera108.Filters;
[HttpPost]
[MultipleButton(Name = "action", Argument = "Publish1")]
public ActionResult PressPublish1Button()
{
    return RedirectToAction("test", "home");
}
 */
namespace kuvera108.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }
}