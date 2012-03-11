using System;
using System.Data;
using System.Web;
using System.IO;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;

public class CRUD {
    private sqlConnection CRUDconnection;

    public CRUD() {
        // database connection
        SqlConnection this.CRUDconnection = new SqlConnection("user id=umbracouser; password=PurellSanitizer#58501; Data Source=CLARISRV; database=IACDB;");
    }

    public CRUD(string connectionString) {
        // database connection
        SqlConnection this.CRUDconnection = new SqlConnection(connectionString);
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
            this.CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, this.CRUDconnection);
            this.CRUDconnection.ExecuteNonQuery();
            this.CRUDconnection.Close();
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
        return executeReader(query, CRUDreader);
    }
    //2.
    public SqlDataReader read(string tableName, string columnName) {
        SqlDataReader CRUDreader = null;
        string query = "SELECT " + columnName + " FROM " + tableName;
        return executeReader(query, CRUDreader);
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
        return executeReader(query, CRUDreader);
    }
    //4.
    public SqlDataReader read(string tableName, string columnName, string condition) {
        SqlDataReader CRUDreader = null;
        string query = "SELECT ";
        query += columnName " FROM ";
        query += tableName + " WHERE " + condition;
        return executeReader(query, CRUDreader);
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
        return executeReader(query, CRUDreader);
    }

    public update() {
        
    }

    public bool delete(string tableName, string condition) {
        string query = "DELETE FROM" + tableName + " WHERE " + condition;
        try {
            this.CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, this.CRUDconnection);
            this.CRUDconnection.ExecuteNonQuery();
            this.CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return true;
    }
    
    protected SqlDataReader executeRead(string query, SqlDataReader CRUDreader) {
        try {
            this.CRUDconnection.Open();
            SqlCommand CRUDcommand = new SqlCommand(query, this.CRUDconnection);
            CRUDreader = this.CRUDcommand.ExecuteReader();
            this.CRUDconnection.Close();
        } catch (Exception e) {
            //Console.WriteLine(e.ToString());
        }
        return CRUDreader;
    }
}
