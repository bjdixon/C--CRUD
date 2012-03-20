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
                string commar = ", ";
                if (columnNames.Length != insertValues.Length) {
                    //check if the amount of columns and amount of values match. If not we'll return false without executing any sql.
                    //Console.WriteLine("Number of Columns and number of Values mismatch.");
                    return false;
                }
                //build the query
                for (int i = 0; i < columnNames.Length; i++) {
                    if (i == columnNames.Length -1) {
                        commar = "";
                    } 
                    columnsString += columnNames[i] + commar;
                    valuesString += "@" + columnNames[i] + commar;
                }
                //remove trailing commars
                query += columnsString + ") ";
                query += valuesString + ") ";
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

            // 1.
            public DataTable Read(string tableName) {
                string query = "SELECT * FROM " + tableName;
                return executeRead(query);
            }
            // 2.
            public DataTable Read(string tableName, string columnName) {
                string query = "SELECT " + columnName + " FROM " + tableName;
                return executeRead(query);
            }
            // 3.
            public DataTable Read(string tableName, string[] columnNames) {
                string columnsString = "";
                string commar = ", ";
                string query = "SELECT ";
                for (int i = 0; i < columnNames.Length; i++) {
                    if (i == columnNames.Length -1) {
                        commar = "";
                    } 
                    columnsString += columnNames[i] + commar;
                }
                query += columnsString.Substring(0, columnsString.Length - 2);
                query += " FROM " + tableName;
                return executeRead(query);
            }
            
            // Select specified columns where conditionColumns == conditionValues
            public DataTable Read_Where(string tableName, string[] columnNames, string[] conditionColumns, string[] conditionValues) {
                string columnsString = "";
                string condition = "";
                string query = "SELECT ";
                for (int i = 0; i < columnNames.Length; i++) {
                    columnsString += columnNames[i] + ", ";
                }
                query += columnsString.Substring(0, columnsString.Length - 2);
                query += " FROM " + tableName + " WHERE ";
                for (int i = 0; i < conditionColumns.Length; i++) {
                    condition += conditionColumns[i] + " = " + "@" + conditionColumns[i];
                    if (i != conditionColumns.Length -1) {
                        condition += " AND ";
                    }
                }
                query += condition;
                return executeRead(query, conditionColumns, conditionValues);;
            }
            
            // Select * where conditionColumns == conditionValues
            public DataTable Read_Where(string tableName, string[] conditionColumns, string[] conditionValues) {
                string columnsString = "";
                string condition = "";
                string query = "SELECT *";
                query += " FROM " + tableName + " WHERE ";
                for (int i = 0; i < conditionColumns.Length; i++) {
                    condition += conditionColumns[i] + " = " + "@" + conditionColumns[i];
                    if (i != conditionColumns.Length -1) {
                        condition += " AND ";
                    }
                }
                query += condition;
                return executeRead(query, conditionColumns, conditionValues);
            }

            public bool Update(string tableName, string[] columnNames, string[] insertValues, string condition) {
                string columnsString = " SET ";
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

            protected DataTable executeRead(string query, string[] conditionColumns, string[] conditionValues) {
                SqlDataReader CRUDreader = null;
                DataTable dt = new DataTable();
                try {
                    CRUDconnection.Open();
                    SqlCommand CRUDcommand = new SqlCommand(query, CRUDconnection);
                    for (int i = 0; i < conditionColumns.Length; i++) {
                        CRUDcommand.Parameters.Add(new SqlParameter("@" + conditionColumns[i], conditionValues[i]));
                    }
                    dt.Load(CRUDcommand.ExecuteReader());
                    CRUDconnection.Close();
                } catch (Exception e) {
                    //Console.WriteLine(e.ToString());
                }
                return dt;
            }
        }
}