using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Ops.Infra.ReadModels;

namespace Ops.Infra.Sql
{
    public class TableReadmodelInterface
    {
        static SqlConnection myConnection =
            new SqlConnection("Data Source=LT-257\\SQLEXPRESS;Initial Catalog=\"WhiteSpot-Dev\";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        static string nspace = "Ops.ReadModels";
        public static void CheckForTables()
        {
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == nspace
                    select t;
            myConnection.Open();
            SqlCommand command = myConnection.CreateCommand();
            myConnection.Close();
            foreach (var readModel in q)
            {
                string[] key = readModel.Name.ToString().Split("Read");
                int table = readModel.Name.ToString().Split("Table").Length;
                if (key.Length == 1 && key[0][0] != '<' && key[0] != "<>o__0" && table > 1)
                {
                    try
                    {
                        myConnection.Open();
                        string methodName = readModel.Name.ToString();
                        string nameSpace = readModel.Namespace.ToString();
                        string fullClassName = nameSpace + "." + methodName;
                        object classToInvoke = Activator.CreateInstance(Type.GetType(fullClassName));
                        string dboParams = "";
                        foreach (PropertyInfo propertyInfo in classToInvoke.GetType().GetProperties())
                        {
                            if (propertyInfo.Name.ToString() != "Id")
                            {
                                dboParams = dboParams + propertyInfo.Name.ToString() + " varchar(50), ";
                            }
                        }
                        string dropCommandText = "IF EXISTS(SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[" + key[0] + "]'))"
                           + Environment.NewLine +
                           " Drop TABLE " + key[0];
                        command.CommandText = dropCommandText;
                        command.ExecuteReader();
                        myConnection.Close();
                        myConnection.Open();
                        string commandtext = "IF NOT EXISTS(SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[" + key[0] + "]'))"
                            + Environment.NewLine +
                            " CREATE TABLE " + key[0] + "(" +
                            " Id varchar(50) PRIMARY KEY, " +
                            dboParams +
                            ");";
                        command.CommandText = commandtext;
                        var test = command.ExecuteReader();
                        myConnection.Close();
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }
                }
            }
        }

        public static void UpdateTable(ReadModelData readModelData, string table)
        {
            var l = readModelData.GetType().ToString();
            if (l.Split("Table").Length > 1 || 1 != 0)
            {
                string commandText = "INSERT INTO " + "dbo." + table.ToLower() + "Data" + Environment.NewLine + "Values(";
                string dboParams = "'" + readModelData.Id + "', ";
                string clearRow = "";
                clearRow = "DELETE FROM " + table.ToLower() + "Data" + Environment.NewLine + "WHERE Id='" + readModelData.Id + "'";
                Type typedReadmodel = readModelData.GetType();
                foreach (PropertyInfo propertyInfo in typedReadmodel.GetProperties())
                {
                    if (propertyInfo.Name != "Id")
                    {
                        var value = GetValues(readModelData, propertyInfo.Name);
                        dboParams = dboParams + "'" + value + "', ";
                    }
                }
                char[] comma = { ',' };
                if (dboParams.EndsWith(",")) dboParams = dboParams.Remove(dboParams.Length - 1);
                dboParams = dboParams.Substring(0, dboParams.Length - 2);
                commandText = commandText + dboParams + ")";
                myConnection.Open();
                try
                {
                    SqlCommand sqlCommand = myConnection.CreateCommand();
                    sqlCommand.CommandText = clearRow;
                    var test = sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = commandText;
                    try
                    {
                        var builder = sqlCommand.ExecuteReader();
                    }
                    catch (Exception e) { Console.Write(e); }
                }
                catch (Exception e)
                {
                    Console.Write(e);
                }

                myConnection.Close();
            }
        }
        private static object GetValues(ReadModelData readModelData, string name)
        {
            return readModelData.GetType().GetProperties()
                .Single(pi => pi.Name == name)
                .GetValue(readModelData, null);
        }
    }
}
