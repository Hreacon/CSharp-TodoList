using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoListNS.Objects
{
  public class Category
  {
    private string _name;
    private int _id;
    private static string Table = "categories";
    private static string NameColumn = "name";
    private static SqlConnection _conn;
    
    public Category(string description, int id = 0)
    {
      _name = description;
      _id = id;
    }
    
    public string GetName() { return _name; }
    public int GetId() { return _id; }
    public void SetName(string Name) { _name = Name; }
    
    public void Save()
    {
      string query = "INSERT INTO "+Category.Table+" ("+Category.NameColumn+") OUTPUT INSERTED.id values (@Name);";
      SqlDataReader rdr = Category.DatabaseOperation(query, new List<SqlParameter> { new SqlParameter("@Name", GetName()) });
      
      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      Category.DatabaseCleanup(rdr, _conn);
    }
    
    public override bool Equals(System.Object other)
    {
      bool output = false;
      if(!(other is Category))
      {
        output = false;
      } else {
        Category otherCategory = (Category) other;
        output = GetName() == otherCategory.GetName();
      }
      return output;
    }
    
    public List<Task> GetTasks()
    {
      string query = "SELECT * FROM " + Task.Table + " WHERE category_id = @id;";
      SqlDataReader rdr = Category.DatabaseOperation(query, new List<SqlParameter> { new SqlParameter("@id", GetId())});
      List<Task> output = new List<Task>(){};
      while(rdr.Read())
      {
        Task t = new Task(rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(0));
        t.SetChecked(rdr.GetInt32(3));
        output.Add(t);
        
      }
      return output;
    }
    public static Category Find(int id)
    {
      string query = "WHERE id = @id";
      return Category.GetCategoryFromDB(query, new List<SqlParameter> { new SqlParameter("@id", id) });
    }
    
    public static Category Find(string description)
    {
      string query = "WHERE description like @description";
      return Category.GetCategoryFromDB(query, new List<SqlParameter> { new SqlParameter("@description", description) });
    }
    
    private static Category GetCategoryFromDB(string query, List<SqlParameter> parameters = null)
    {
      query = "SELECT * FROM " + Category.Table + " " + query;
      SqlDataReader rdr = Category.DatabaseOperation(query, parameters);
      Category output = new Category("");
      while(rdr.Read())
      {
        output = new Category(rdr.GetString(1), rdr.GetInt32(0));
      }
      Category.DatabaseCleanup(rdr, _conn);
      return output;
    }
    
    public static List<Category> GetAll()
    {
      List<Category> output = new List<Category>(){};
      string query = "SELECT * FROM "+Category.Table+"";
      SqlDataReader rdr = Category.DatabaseOperation(query);
      while(rdr.Read())
      {
        output.Add(new Category(rdr.GetString(1), rdr.GetInt32(0)));
      }
      Category.DatabaseCleanup(rdr, _conn);
      return output;
    }
    public static void Delete(int id)
    {
      // make sure to delete tasks first, otherwise cause sql error
      List<Task> tasks = Category.Find(id).GetTasks();
      foreach(Task task in tasks)
      {
        Task.Delete(task.GetId());
      }
      string query = "DELETE FROM "+Category.Table+" WHERE id = @id";
      SqlDataReader rdr = Category.DatabaseOperation(query, new List<SqlParameter> { new SqlParameter("@id", id)});
      Category.DatabaseCleanup(rdr, _conn);
    }
    public static void DeleteAll()
    {
      string query = "DELETE FROM "+Category.Table+"";
      SqlDataReader rdr = Category.DatabaseOperation(query);
      Category.DatabaseCleanup(rdr, _conn);
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
    private static void DatabaseCleanup(SqlDataReader rdr, SqlConnection conn)
    {
      if(rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }
  } // end class
} // end namespace
