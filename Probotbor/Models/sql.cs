using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace WpfApp1.Models
{
    static class sql
    {
        public static string DbName { get; set; } = "application";

        #region Добавить данные в коллекцию
        public static void GetFromDB<T>(string tableName, ObservableCollection<T> collection)
        {
            try
            {
                // Проверка наличия базы данных на сервере
                if (!ExistDB(DbName)) CreateDB(DbName);
                // Проверка наличия таблицы в базе
                if (!ExistTable(DbName, tableName))
                {
                    CreateTable<T>(DbName, tableName, typeof(T));
                }
                ReadFromSql<T>(DbName, tableName, collection);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
        #endregion

        #region Создание таблицы
        static void CreateTable<T>(string dbName, string tableName, Type type)
        {            
            var props = type.GetProperties();
            string cmd = "CREATE TABLE " + tableName + "(";
            foreach (var item in props)
            {                
                cmd += item.Name;
                cmd += " " + GetSqlTypeName(item.PropertyType);                
                cmd += ", ";
            }
            cmd = cmd.Substring(0, cmd.Length - 2);
            cmd += ");";
            SqlCommand command = new SqlCommand(cmd);
            SqlExecuteCmd(dbName, tableName, command);
        }
        #endregion

        #region Сопоставление типов данных system и sql
        static string GetSqlTypeName(Type propertyType)
        {
            switch (propertyType.Name.ToLower())
            {
                case "int":
                case "int32":
                    return "int NOT NULL default 0";
                case "string": return "nvarchar(20) NOT NULL";
                case "double": return "float NOT NULL";
                case "float": return "real NOT NULL";
                default:
                    throw new Exception($"Для типа {propertyType.Name} не определен соответствующий SQL  тип!");
            }
        } 
        #endregion

        #region Вставка записи в таблицу
        public static void InsertToTable<T>(string tableName, T cell)
        {
            SqlCommand command = new SqlCommand();            
            Type myType = cell.GetType();            
            string cmd = $"INSERT INTO {tableName}(";
            string values = "VALUES(";
            var props = myType.GetProperties();
            string paramName = null;
            foreach (var item in props)
            { 
                cmd = cmd + item.Name + ", ";               
                paramName = $"@{item.Name}";
                command.Parameters.Add(new SqlParameter(paramName, myType.GetProperty(item.Name).GetValue(cell)));
                values += paramName;                
                values += ", ";
            }
            cmd = cmd.Substring(0, cmd.Length - 2);
            cmd += ")";
            values = values.Substring(0, values.Length - 2);
            values += ")";
            cmd = cmd + values;
            cmd += ";";
            command.CommandText = cmd;
            SqlExecuteCmd(DbName, tableName, command);
        } 
        #endregion

        #region Проверка наличия базы данных на сервере
        /// <summary>
        /// Проверка наличия базы данных на сервере
        /// </summary>
        /// <param name="dbName">Имя БД</param>
        /// <returns>true, если БД существует, иначе false</returns>
        static bool ExistDB(string dbName)
        {
            string connectionString = "Server=LENOVOX1CARBON;Database=master;Connect Timeout=44;Trusted_Connection=True;";            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = @"if Exists(select 1 from master.dbo.sysdatabases where name= @db)
                       select 1 else select 0";
                command.Connection = connection;
                command.Parameters.Add("@db", SqlDbType.NVarChar).Value = dbName;
                object count = command.ExecuteScalar();
                return (int)count == 1;
            }
        }
        #endregion

        #region Создание базы данных
        static void CreateDB(string dbName)
        {
            string connectionString = "Server=LENOVOX1CARBON;Database=master;Connect Timeout=44;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = "CREATE DATABASE application";
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        }
        #endregion

        #region Проверка наличия таблицы
        /// <summary>
        /// Проверка наличия таблицы в БД
        /// </summary>
        /// <param name="DBName">Имя БД</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <returns>true, если таблица существует</returns>
        static bool ExistTable(string DBName, string tableName)
        {
            string connectionString = $"Server=LENOVOX1CARBON;Database={DBName};Connect Timeout=44;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"SELECT CONVERT(BIT, COUNT(*)) FROM sys.sysobjects WHERE name = '{tableName}';";
                command.Connection = connection;                
                object count = command.ExecuteScalar();
                return (bool)count;
            }
        }
        #endregion

        #region Чтение данных из таблицы
        static void ReadFromSql<T>(string dbName, string tableName, ObservableCollection<T> collection )
        {            
            string connectionString = $"Server=LENOVOX1CARBON;Database={dbName};Connect Timeout=44;Trusted_Connection=True;";            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                collection.Clear();
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM {tableName};",connection);
                SqlDataReader reader = command.ExecuteReader();
                Type type = typeof(T);
                var props = type.GetProperties();                
                while (reader.Read())
                {
                    T cell = (T)Activator.CreateInstance(type);
                    for (int i = 0; i < props.Length; i++)
                    {
                        type.GetProperty(props[i].Name).SetValue(cell, reader.GetValue(i));
                    }
                    collection.Add(cell);
                }                
                
            }           
        }
        #endregion

        #region Удление записи из таблицы
        public static void DeleteFromSql(string tableName, string propertyName, object propertyValue)
        { 
            string propertyValueString = (propertyValue.GetType()!= typeof(string)) ? propertyValue.ToString() : "'" + propertyValue.ToString() + "'";
            string cmd = $"DELETE FROM {tableName} WHERE {propertyName} = {propertyValueString};";
            SqlCommand command = new SqlCommand(cmd);
            SqlExecuteCmd(DbName, tableName, command);
        }
        #endregion

        #region Изменение записи 
        public static void UpdateSql(string tableName, string idName, object idValue, string setName, object setValue)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add(new SqlParameter("@idValue", idValue));
            command.Parameters.Add(new SqlParameter("@setValue", setValue));
            command.CommandText = $"UPDATE {tableName} SET {setName} = @setValue WHERE {idName} = @idValue;";
            SqlExecuteCmd(DbName, tableName, command);
        }
        #endregion

        #region Выполнить команду без возврата значения
        /// <summary>
        /// Выполнить команду без возврата значения
        /// </summary>
        /// <param name="dbName">Имя БД</param>
        /// <param name="tableName">Имя таблицы</param>
        /// <param name="cmd">Sql запрос</param>
        static void SqlExecuteCmd(string dbName, string tableName, SqlCommand command)
        {
            string connectionString = $"Server=LENOVOX1CARBON;Database={dbName};Connect Timeout=44;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                command.Connection = connection;
                command.ExecuteNonQuery();
            }
        } 
        #endregion


    }
}
