using Xunit;
using ToDoListNS.Objects;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoListNS
{
  public class TaskTest : IDisposable
  {
     public TaskTest()
     {
       DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
     }
     public void Dispose()
     {
       Task.DeleteAll();
     }
  }
}
