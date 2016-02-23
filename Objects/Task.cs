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
    private int _category_id;
    private int _checked;
    public static string Table = "tasks";
    private static string DescriptionColumn = "description";
    private static string CategoryIdColumn = "category_id";
    private static string CheckedColumn = "checked";
    private static SqlConnection _conn;
    
    public Task(string description, int category_id, int id = 0)
    {
      _description = description;
      _category_id = category_id;
      _id = id;
      _checked = 0;
    }
    
    public string GetDescription() { return _description; }
    public int GetId() { return _id; }
    public void SetDescription(string description) { _description = description; }
    public int GetCategoryId() { return _category_id; }
    public int GetChecked() { return _checked; }
    public void SetChecked(int checkedIn) { _checked = checkedIn; }
    
    public void Save()
    {
      string query = "";
      List<SqlParameter> queryparams = new List<SqlParameter> { 
        new SqlParameter("@Description", GetDescription()),
        new SqlParameter("@checked", GetChecked()) };
      Console.WriteLine("Saving id of " + _id);
      if(_id > 0) // already exists in database, update table
      {
        query = "UPDATE " + Task.Table + " SET " + Task.DescriptionColumn + "=@Description, " + Task.CheckedColumn + "=@checked WHERE id = " + GetId();
      } else { // doesnt exist in databaes, insert
        query = "INSERT INTO "+Task.Table+" ("+Task.DescriptionColumn+", " + Task.CategoryIdColumn + ", " + Task.CheckedColumn + ") OUTPUT INSERTED.id values (@Description, @categoryId, @Checked);";
        queryparams.Add(new SqlParameter("@categoryId", GetCategoryId()));
      }
      SqlDataReader rdr = Task.DatabaseOperation(query, queryparams);
      
      while(rdr.Read() && _id == 0)
      {
        this._id = rdr.GetInt32(0);
      }
      if(rdr != null)
      {
        rdr.Close();
      }
      if(_conn != null)
      {
        _conn.Close();
      }
    }
    
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
      Task output = new Task("", 0);
      while(rdr.Read())
      {
        output = new Task(rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(0));
        output.SetChecked(rdr.GetInt32(3));
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
    }
    public static void DeleteAll()
    {
      string query = "DELETE FROM "+Task.Table+"";
      SqlDataReader rdr = Task.DatabaseOperation(query);
      if(rdr != null)
      {
        rdr.Close();
      }
      if(_conn != null)
      {
        _conn.Close();
      }
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
