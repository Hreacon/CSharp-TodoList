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
      Get["/check/{id}"] = x => {
        Task task = Task.Find(int.Parse(x.id));
        task.SetChecked(1);
        task.Save();
        return View["viewCategory.cshtml", Category.Find(task.GetCategoryId())];
      };
      Get["/uncheck/{id}"] = x => {
        Task task = Task.Find(int.Parse(x.id));
        task.SetChecked(0);
        task.Save();
        return View["viewCategory.cshtml", Category.Find(task.GetCategoryId())];
      };
      Post["/cat/{id}/addTask"] = x => {
        int catId = int.Parse(x.id);
        Task task = new Task(Request.Form["description"], catId);
        task.Save();
        return View["viewCategory.cshtml", Category.Find(catId)];
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
