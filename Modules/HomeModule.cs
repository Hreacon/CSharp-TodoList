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
        return View["header.cshtml"];
      };
    }
  }
}
