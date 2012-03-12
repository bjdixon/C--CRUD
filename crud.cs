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
        CRUDconnection = new SqlConnection("user id=USER; password=PASSWORD; Data Source=SERVER; database=DBNAME;");
    }

    public CRUD(string connectionString) {
        // database connection
        CRUDconnection = new SqlConnection(connectionString);
    }

    public bool create(string tableName, string[] columnNames, string[] insertValues) {
        string columnsString = " (";
        string valuesString = " Values (";
        string query = "INSERT INTO " + tableName;
        if (columnNames.Length != insertValues.Length) {
            //check if the amount of columns and amount of values match. If not we'll return false without executing any sql.
            //Console.WriteLine("Number of Columns and number of Values mismatch.");
            return false;
        }
        //build the query
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + ", ";
            valuesString += insertValues[i] + ", ";
        }
        //remove trailing commars
        query += columnsString.Substring(0, columnsString.Length - 1) + ") ";
        query += valuesString.Substring(0, valuesString.Length - 1) + ")";
        //open the connection
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, this.CRUDconnection);
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
    public SqlDataReader read(string tableName) {
        SqlDataReader CRUDreader = null;
        string query = "SELECT * FROM " + tableName;
        return executeRead(query, CRUDreader);
    }
    //2.
    public SqlDataReader read(string tableName, string columnName) {
        SqlDataReader CRUDreader = null;
        string query = "SELECT " + columnName + " FROM " + tableName;
        return executeRead(query, CRUDreader);
    }
    //3.
    public SqlDataReader read(string tableName, string[] columnNames) {
        SqlDataReader CRUDreader = null;
        string columnsString = "";
        string query = "SELECT ";
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + ", ";
        }
        query += columnsString.Substring(0, columnsString.Length - 1);
        query += " FROM " + tableName;
        return executeRead(query, CRUDreader);
    }
    //4.
    public SqlDataReader read(string tableName, string columnName, string condition) {
        SqlDataReader CRUDreader = null;
        string query = "SELECT ";
        query += columnName + " FROM ";
        query += tableName + " WHERE " + condition;
        return executeRead(query, CRUDreader);
    }
    //5.
    public SqlDataReader read(string tableName, string[] columnNames, string condition) {
        SqlDataReader CRUDreader = null;
        string columnsString = "";
        string query = "SELECT ";
        for (int i = 0; i < columnNames.Length; i++) {
            columnsString += columnNames[i] + ", ";
        }
        query += columnsString.Substring(0, columnsString.Length - 1);
        query += " FROM ";
        query += tableName + " WHERE " + condition;
        return executeRead(query, CRUDreader);
    }

    public bool update() {
        return true;
    }

    public bool delete(string tableName, string condition) {
        string query = "DELETE FROM" + tableName + " WHERE " + condition;
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, this.CRUDconnection);
            CRUDcommand.ExecuteNonQuery();
            CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return true;
    }
    
    protected SqlDataReader executeRead(string query, SqlDataReader CRUDreader) {
        try {
            CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, this.CRUDconnection);
            CRUDreader = CRUDcommand.ExecuteReader();
            CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return CRUDreader;
    }
}