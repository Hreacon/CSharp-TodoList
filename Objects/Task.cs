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
    private DateTime _due;
    public static string Table = "tasks";
    private static string DescriptionColumn = "description";
    private static string CategoryIdColumn = "category_id";
    private static string CheckedColumn = "checked";
    private static string DueColumn = "duedate";
    private static SqlConnection _conn;
    
    public Task(string description, int category_id, DateTime due, int id = 0)
    {
      _description = description;
      _category_id = category_id;
      _id = id;
      _due = due;
      _checked = 0;
    }
    
    public string GetDescription() { return _description; }
    public int GetId() { return _id; }
    public void SetDescription(string description) { _description = description; }
    public int GetCategoryId() { return _category_id; }
    public int GetChecked() { return _checked; }
    public DateTime GetDueDate() { return _due; }
    public void SetChecked(int checkedIn) { _checked = checkedIn; }
    
    public void Save()
    {
      string query = "";
      List<SqlParameter> queryparams = new List<SqlParameter> { 
        new SqlParameter("@Description", GetDescription()),
        new SqlParameter("@checked", GetChecked()),
        new SqlParameter("@DueDate", GetDueDate()) };
      Console.WriteLine("Saving id of " + _id);
      if(_id > 0) // already exists in database, update table
      {
        query = "UPDATE " + Task.Table + " SET " + Task.DescriptionColumn + "=@Description, " 
        + Task.CheckedColumn + "=@checked, " 
        + Task.DueColumn + "=@DueDate WHERE id = " + GetId();
      } else { // doesnt exist in databaes, insert
        query = "INSERT INTO "+Task.Table+" ("+Task.DescriptionColumn+", " + Task.CategoryIdColumn + ", " + Task.CheckedColumn + ", " + Task.DueColumn + ") OUTPUT INSERTED.id values (@Description, @categoryId, @Checked, @DueDate);";
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
      Task output = new Task("", 0, new DateTime());
      while(rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string descr = rdr.GetString(1);
        int category_id =  rdr.GetInt32(2);
        int checkedInt = rdr.GetInt32(3);
        DateTime due = rdr.GetDateTime(4);
        Console.WriteLine("Loading Task: ID: " + id + " desc: " + descr + " catId " + category_id + " checked " + checkedInt);
        output = new Task(descr, category_id, due, id);
        output.SetChecked(checkedInt);
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
        Task t = new Task(rdr.GetString(1), rdr.GetInt32(2), rdr.GetDateTime(4), rdr.GetInt32(0));
        t.SetChecked(rdr.GetInt32(3));
        output.Add(t);
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
    public static void Delete(int id)
    {
      string query = "DELETE FROM " + Task.Table + " WHERE id = @id";
      SqlDataReader rdr = Task.DatabaseOperation(query, new List<SqlParameter> { new SqlParameter("@id", id)});
      if(rdr != null)
      {
        rdr.Close();
      }
      if(_conn != null)
      {
        _conn.Close();
      }
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
