using Nancy;
using ToDoListNS.Objects;
using System.Collections.Generic;
namespace ToDoListNS
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["viewCategories.cshtml", Category.GetAll()];
      };
      Get["/cat/{id}"] = x => {
        
        return View["viewCategory.cshtml", Category.Find(int.Parse(x.id))];
      };
      Post["/addCategory"] = _ => {
        string name = Request.Form["name"];
        Category cat = new Category(name);
        cat.Save();
        return View["ViewCategories.cshtml", Category.GetAll()];
      };
    }
  }
}
