using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoListNS.Objects
{
  public class Task
  {
    private string _description;
    private int _id;
    private static string Table = "tasks";
    private static SqlConnection _conn;
    
    public Task(string description, int id = 0)
    {
      _description = description;
      _id = id;
    }
    
    public string GetDescription() { return _description; }
    public int GetId() { return _id; }
    public void SetDescription(string description) { _description = description; }
    
    public override bool Equals(System.Object other)
    {
      bool output = false;
      if(!(other is Task))
      {
        output = false;
      } else {
        Task otherTask = (Task) other;
        output = GetDescription() == otherTask.GetDescription();
      }
      return output;
    }
    
    public static Task Find(int id)
    {
      string query = "WHERE id = @id";
      return Task.GetTaskFromDB(query, new List<SqlParameter> { new SqlParameter("@id", id) });
    }
    
    public static Task Find(string description)
    {
      string query = "WHERE description like @description";
      return Task.GetTaskFromDB(query, new List<SqlParameter> { new SqlParameter("@description", description) });
    }
    
    private static Task GetTaskFromDB(string query, List<SqlParameter> parameters = null)
    {
      query = "SELECT * FROM " + Task.Table + " " + query;
      SqlDataReader rdr = Task.DatabaseOperation(query, parameters);
      Task output = new Task("");
      while(rdr.Read())
      {
        output = new Task(rdr.GetString(1), rdr.GetInt32(0));
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(_conn != null)
      {
        _conn.Close();
      }
      return output;
    }
    
    public static List<Task> GetAll()
    {
      List<Task> output = new List<Task>(){};
      string query = "SELECT * FROM "+Task.Table+"";
      SqlDataReader rdr = Task.DatabaseOperation(query);
      while(rdr.Read())
      {
        output.Add(new Task(rdr.GetString(1), rdr.GetInt32(0)));
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(_conn != null)
      {
        _conn.Close();
      }
      return output;
      return new List<Task>(){};
    }
    public static void DeleteAll()
    {
      
    }
    private static SqlDataReader DatabaseOperation(string query, List<SqlParameter> parameters = null)
    {
      _conn = DB.Connection();
      _conn.Open();
      SqlCommand cmd = new SqlCommand(query, _conn);
      if(parameters != null) 
      {
        foreach(SqlParameter param in parameters)
        {
          cmd.Parameters.Add(param);
        }
      }
      
      SqlDataReader rdr = cmd.ExecuteReader();
      
      return rdr;
    }
  } // end class
} // end namespace
