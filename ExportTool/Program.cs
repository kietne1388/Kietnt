using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        string connStr = "Server=MSI\\KIETNTTB01357;Database=CoffeeDB;Trusted_Connection=True;TrustServerCertificate=True";
        string outputFilePath = "export_data.sql";

        Console.WriteLine("Starting data export...");

        try
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                // Get all user tables
                DataTable tablesSchema = conn.GetSchema("Tables");

                using (StreamWriter writer = new StreamWriter(outputFilePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("-- Data Export Script for CoffeeDB");
                    writer.WriteLine("-- Generated at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    writer.WriteLine("USE [CoffeeDB];");
                    writer.WriteLine("GO\n");

                    foreach (DataRow row in tablesSchema.Rows)
                    {
                        string tableType = row["TABLE_TYPE"].ToString();
                        if (tableType == "BASE TABLE")
                        {
                            string tableName = row["TABLE_NAME"].ToString();
                            string schemaName = row["TABLE_SCHEMA"].ToString();

                            if (tableName == "__EFMigrationsHistory") continue;

                            Console.WriteLine($"Exporting table: {schemaName}.{tableName}");

                            // Generate INSERTs
                            string query = $"SELECT * FROM [{schemaName}].[{tableName}]";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    writer.WriteLine($"-- Table: {schemaName}.{tableName}");
                                    
                                    // Turn on identity insert if there is an identity column
                                    bool hasIdentity = CheckHasIdentity(connStr, tableName, schemaName);
                                    if (hasIdentity)
                                    {
                                        writer.WriteLine($"SET IDENTITY_INSERT [{schemaName}].[{tableName}] ON;");
                                    }

                                    while (reader.Read())
                                    {
                                        StringBuilder columns = new StringBuilder();
                                        StringBuilder values = new StringBuilder();

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            columns.Append($"[{reader.GetName(i)}], ");

                                            if (reader.IsDBNull(i))
                                            {
                                                values.Append("NULL, ");
                                            }
                                            else
                                            {
                                                string typeName = reader.GetDataTypeName(i).ToLower();
                                                object val = reader.GetValue(i);

                                                if (typeName.Contains("char") || typeName.Contains("text") || typeName.Contains("date") || typeName.Contains("time") || typeName == "uniqueidentifier")
                                                {
                                                    string cleanVal = val.ToString().Replace("'", "''");
                                                    values.Append($"N'{cleanVal}', ");
                                                }
                                                else if (typeName == "bit")
                                                {
                                                    values.Append(((bool)val ? "1" : "0") + ", ");
                                                }
                                                else if (typeName == "decimal" || typeName == "numeric" || typeName == "money" || typeName == "float" || typeName == "real")
                                                {
                                                    values.Append(val.ToString().Replace(",", ".") + ", ");
                                                }
                                                else if (typeName == "binary" || typeName == "varbinary" || typeName == "image")
                                                {
                                                    byte[] bytes = (byte[])val;
                                                    values.Append($"0x{BitConverter.ToString(bytes).Replace("-", "")}, ");
                                                }
                                                else
                                                {
                                                    values.Append($"{val}, ");
                                                }
                                            }
                                        }

                                        columns.Length -= 2; // Remove last comma
                                        values.Length -= 2;  // Remove last comma

                                        writer.WriteLine($"INSERT INTO [{schemaName}].[{tableName}] ({columns}) VALUES ({values});");
                                    }

                                    if (hasIdentity)
                                    {
                                        writer.WriteLine($"SET IDENTITY_INSERT [{schemaName}].[{tableName}] OFF;");
                                    }
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                }

                Console.WriteLine($"Export complete! File saved to {Path.GetFullPath(outputFilePath)}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static bool CheckHasIdentity(string connStr, string tableName, string schemaName)
    {
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            string query = "SELECT OBJECTPROPERTY(OBJECT_ID(@TableName), 'TableHasIdentity')";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TableName", $"[{schemaName}].[{tableName}]");
                object result = cmd.ExecuteScalar();
                return result != DBNull.Value && (int)result == 1;
            }
        }
    }
}
