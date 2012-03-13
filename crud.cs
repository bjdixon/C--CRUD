using System;
using System.Data;
using System.Web;
using System.IO;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;

public class CRUD {
    private SqlConnection CRUDconnection;

    public CRUD() {
        // database connection
        CRUDconnection = new SqlConnection("user id=USER; password=PASSWORD; Data Source=SERVERNAME; database=DBNAME;");
    }

    public CRUD(string connectionString) {
        // database connection
        CRUDconnection = new SqlConnection(connectionString);
    }

    public bool Create(string tableName, string[] columnNames, string[] insertValues) {
        string columnsString = " (";
        string valuesString = " VALUES (";
        string query = "INSERT INTO " + tableName;
        if (columnNames.Length != insertValues.Length) {
            //check if the amount of columns and amount of values match. If not we'll return false without executing any sql.
            //Console.WriteLine("Number of Columns and number of Values mismatch.");
            return false;
        }
        //build the query
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + ", ";
            valuesString += "@" + columnNames[i] + ", ";
        }
        //remove trailing commars
        query += columnsString.Substring(0, columnsString.Length - 2) + ") ";
        query += valuesString.Substring(0, valuesString.Length - 2) + ") ";
        //open the connection
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, CRUDconnection);
            for (int i = 0; i < columnNames.Length; i++) {
                CRUDcommand.Parameters.Add(new SqlParameter("@" + columnNames[i], insertValues[i]));
            }
            CRUDcommand.ExecuteNonQuery();
            CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return true;
    }

    // read operations.
    // 1. select *
    // 2. select specified column
    // 3. select specified columns
    // 4. select specified column where arg (arg string has to be in the form of "field = 'argument'")
    // 5. select specified columns where arg

    //1.
    public DataTable Read(string tableName) {
        string query = "SELECT * FROM " + tableName;
        return executeRead(query);
    }
    //2.
    public DataTable Read(string tableName, string columnName) {
        string query = "SELECT " + columnName + " FROM " + tableName;
        return executeRead(query);
    }
    //3.
    public DataTable Read(string tableName, string[] columnNames) {
        string columnsString = "";
        string query = "SELECT ";
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + ", ";
        }
        query += columnsString.Substring(0, columnsString.Length - 1);
        query += " FROM " + tableName;
        return executeRead(query);
    }
    //4.
    public DataTable Read(string tableName, string columnName, string condition) {
        string query = "SELECT ";
        query += columnName + " FROM ";
        query += tableName + " WHERE " + condition;
        return executeRead(query);
    }
    //5.
    public DataTable Read(string tableName, string[] columnNames, string condition) {
        string columnsString = "";
        string query = "SELECT ";
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + ", ";
        }
        query += columnsString.Substring(0, columnsString.Length - 1);
        query += " FROM ";
        query += tableName + " WHERE " + condition;
        return executeRead(query);
    }

    public bool Update(string tableName, string[] columnNames, string[] insertValues, string condition) {
        string columnsString = " SET ";
        //string valuesString = " Values (";
        string query = "UPDATE " + tableName;
        if (columnNames.Length != insertValues.Length) {
            //check if the amount of columns and amount of values match. If not we'll return false without executing any sql.
            //Console.WriteLine("Number of Columns and number of Values mismatch.");
            return false;
        }
        //build the query
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + "=";
            columnsString += "@" + columnNames[i] + ", ";
        }
        //remove trailing commars
        query += columnsString.Substring(0, columnsString.Length - 2);
        query += " WHERE " + condition;
        //query += valuesString.Substring(0, valuesString.Length - 2) + ") ";
        //open the connection
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, CRUDconnection);
            for (int i = 0; i < columnNames.Length; i++) {
                CRUDcommand.Parameters.Add(new SqlParameter("@" + columnNames[i], insertValues[i]));
            }
            CRUDcommand.ExecuteNonQuery();
            CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return true;
    }

    public bool Delete(string tableName, string condition) {
        string query = "DELETE FROM" + tableName + " WHERE " + condition;
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, CRUDconnection);
            CRUDcommand.ExecuteNonQuery();
            CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return true;
    }
    
    protected DataTable executeRead(string query) {
        SqlDataReader CRUDreader = null;
        DataTable dt = new DataTable();
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, CRUDconnection);
            dt.Load(CRUDcommand.ExecuteReader());
            CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return dt;
    }
}