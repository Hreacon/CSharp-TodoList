using Nancy;
using ToDoListNS.Objects;
using System.Collections.Generic;
using System;

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
        return View["forward.cshtml", "/cat/"+task.GetCategoryId()];
      };
      Get["/uncheck/{id}"] = x => {
        Task task = Task.Find(int.Parse(x.id));
        task.SetChecked(0);
        task.Save();
        
        return View["forward.cshtml", "/cat/"+task.GetCategoryId()];
      };
      Get["/delete/{id}"] = x => {
        Task t = Task.Find(int.Parse(x.id));
        int catId = t.GetCategoryId();
        Task.Delete(int.Parse(x.id));
        return View["forward.cshtml", "/cat/"+catId];
      };
      Get["/cat/{id}/delete"] = x => {
        int id = int.Parse(x.id);
        Console.WriteLine("Deleting Category: " + id);
        Category.Delete(int.Parse(x.id));
        return View["forward.cshtml", "/"];
      };
      Post["/cat/{id}/addTask"] = x => {
        int catId = int.Parse(x.id);
        Task task = new Task(Request.Form["description"], catId);
        task.Save();
        return View["forward.cshtml", "/cat/"+catId];
      };
      Post["/addCategory"] = _ => {
        string name = Request.Form["name"];
        Category cat = new Category(name);
        cat.Save();
        return View["forward.cshtml", "/"];
      };
      
    }
  }
}
