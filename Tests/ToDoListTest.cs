using Xunit;
using ToDoListNS.Objects;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoListNS
{
  public class ToDoListTest : IDisposable
  {
     public ToDoListTest()
     {
       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
     }
     public void Dispose()
     {
       ToDoList.DeleteAll();
     }
  }
}
