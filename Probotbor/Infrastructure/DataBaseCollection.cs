using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Probotbor.Infrastructure
{
    class DataBaseCollection<T> where T : NotifyPropertyChanged
    {
        /// <summary>
        /// Событие ошибки работы с SQL
        /// </summary>
        public Action<string> SqlErrorEvent;
        private string DBName { get; } = "application";
        public readonly string TableName;

        #region Конструктор
        public DataBaseCollection(string tableName, T defaultCell)
        {
            TableName = tableName;
            Data = new ObservableCollection<T>();
            // Загружаем данные из БД
            GetFromDB();
            if (Data.Count == 0 && defaultCell != null)
                Data.Add(defaultCell);
            // Подписка на изменение коллекции
            Data.CollectionChanged += UpdateCollection;
            // Подписка на изменение свойтсва каждого элемента коллекции
            foreach (var item in Data) item.PropertyChanged += EditCellSql;
        } 
        #endregion
        public ObservableCollection<T> Data { get; }
        
        #region Взять данные из базы данных
        /// <summary>
        /// Взять данные из базы данных
        /// </summary>
        void GetFromDB()
        {
            try
            {
                // Проверка наличия базы данных на сервере
                if (!ExistDB()) CreateDb();
                // Проверка наличия таблицы в базе
                if (!ExistTable()) CreateTable();
                ReadFromSql();
            }
            catch (Exception ex)
            {
                SqlErrorEvent?.Invoke(ex.Message);
            }
        }
        #endregion
        #region Проверка наличия базы данных на сервере
        /// <summary>
        /// Проверка наличия базы данных на сервере
        /// </summary>
        /// <returns>true, если существует</returns>
        bool ExistDB()
        {
            string connectionString = "Server=LENOVOX1CARBON;Database=master;Connect Timeout=44;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = @"if Exists(select 1 from master.dbo.sysdatabases where name= @db)
                       select 1 else select 0";
                command.Connection = connection;
                command.Parameters.Add("@db", SqlDbType.NVarChar).Value = DBName;
                object count = command.ExecuteScalar();
                return (int)count == 1;
            }
        }
        #endregion
        #region Проверка наличия таблицы в баз данных
        /// <summary>
        /// Проверка наличия таблицы в базе данных
        /// </summary>
        /// <returns>true, если существует</returns>
        bool ExistTable()
        {
            string connectionString = $"Server=LENOVOX1CARBON;Database={DBName};Connect Timeout=44;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = $"SELECT CONVERT(BIT, COUNT(*)) FROM sys.sysobjects WHERE name = '{TableName}';";
                command.Connection = connection;
                object count = command.ExecuteScalar();
                return (bool)count;
            }
        }
        #endregion
        #region Создание базы данных
        /// <summary>
        /// Создание базы данных
        /// </summary>
        void CreateDb()
        {
            var props = typeof(T).GetProperties();
            string cmd = "CREATE TABLE " + TableName + "(";
            foreach (var item in props)
            {
                cmd += item.Name;
                cmd += " " + GetSqlTypeName(item.PropertyType);
                cmd += ", ";
            }
            cmd = cmd.Substring(0, cmd.Length - 2);
            cmd += ");";
            SqlCommand command = new SqlCommand(cmd);
            SqlExecuteCmd(command);
        }
        #endregion
        #region Создание таблицы в базе данных
        /// <summary>
        /// Создание таблицы в базе данных
        /// </summary>
        void CreateTable()
        {
            var props = typeof(T).GetProperties();
            string cmd = "CREATE TABLE " + TableName + "(";
            foreach (var item in props)
            {
                cmd += item.Name;
                cmd += " " + GetSqlTypeName(item.PropertyType);
                cmd += ", ";
            }
            cmd = cmd.Substring(0, cmd.Length - 2);
            cmd += ");";
            SqlCommand command = new SqlCommand(cmd);
            SqlExecuteCmd(command);
        }
        #endregion
        #region Сопоставление имен system и sql
        /// <summary>
        /// Сопоставление имен system и sql
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public static string GetSqlTypeName(Type propertyType)
        {
            switch (propertyType.Name.ToLower())
            {
                case "int":
                case "int32":
                    return "int NOT NULL default 0";
                case "string": return "nvarchar(40) NOT NULL";
                case "double": return "float NOT NULL";
                case "float": return "real NOT NULL";
                default:
                    throw new Exception($"Для типа {propertyType.Name} не определен соответствующий SQL  тип!");
            }
        }

        #endregion
        #region Выполнить sql команду
        /// <summary>
        /// Выполнить sql команду
        /// </summary>
        /// <param name="command"></param>
        void SqlExecuteCmd(SqlCommand command)
        {
            try
            {
                string connectionString = $"Server=LENOVOX1CARBON;Database={DBName};Connect Timeout=44;Trusted_Connection=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    command.Connection = connection;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                SqlErrorEvent?.Invoke(ex.Message);
            }
        }
        #endregion
        #region Чтение данных из БД
        void ReadFromSql()
        {
            string connectionString = $"Server=LENOVOX1CARBON;Database={DBName};Connect Timeout=44;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Data.Clear();
                connection.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM {TableName};", connection);
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
                    Data.Add(cell);
                }
            }
        }
        #endregion
        #region Удаление данных из таблицы
        void DeleteFromSql(string propertyName, object propertyValue)
        {
            string propertyValueString = (propertyValue.GetType() != typeof(string)) ? propertyValue.ToString() : "'" + propertyValue.ToString() + "'";
            string cmd = $"DELETE FROM {TableName} WHERE {propertyName} = {propertyValueString};";
            SqlCommand command = new SqlCommand(cmd);
            SqlExecuteCmd(command);
        }
        #endregion
        #region Добвление в таблицу
        void InsertToTable(T cell)
        {
            SqlCommand command = new SqlCommand();
            Type myType = cell.GetType();
            string cmd = $"INSERT INTO {TableName}(";
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
            SqlExecuteCmd(command);
        }
        #endregion
        #region Редактирование данных
        void UpdateSql(string idName, object idValue, string setName, object setValue)
        {
            SqlCommand command = new SqlCommand();
            command.Parameters.Add(new SqlParameter("@idValue", idValue));
            command.Parameters.Add(new SqlParameter("@setValue", setValue));
            command.CommandText = $"UPDATE {TableName} SET {setName} = @setValue WHERE {idName} = @idValue;";
            SqlExecuteCmd(command);
        }
        #endregion
        #region Добавление и удаление данных из коллекции
        /// <summary>
        /// Добавление и удаление данных из коллекции
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="tableName"></param>
        void UpdateCollection(object sender, NotifyCollectionChangedEventArgs e)
        {
            Type type = typeof(T);
            var props = type.GetProperties();
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ObservableCollection<T> collection = sender as ObservableCollection<T>;
                if (collection != null && collection.Count > 1 && ContainStringArr(props, "Id"))
                {
                    var index = (int)type.GetProperty("Id").GetValue(collection[collection.Count - 2]) + 1;
                    type.GetProperty("Id").SetValue((T)e.NewItems[0], index);
                    InsertToTable((T)e.NewItems[0]);
                    (e.NewItems[0] as T).PropertyChanged += EditCellSql;
                }                
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (ContainStringArr(props, "Id"))
                {
                    foreach (var deleteItem in e.OldItems)
                    {
                        var index = (int)type.GetProperty("Id").GetValue((T)deleteItem);
                        DeleteFromSql("Id", index);
                    }
                }
            }
        }
        #endregion
        #region Проверка наличия свойства в классе
        bool ContainStringArr(PropertyInfo[] props, string name)
        {
            foreach (var item in props)
            {
                if (item.Name == name) return true;
            }
            return false;
        }
        #endregion
        #region Изменение данных в коллекции
        void EditCellSql(object sender, PropertyChangedEventArgs e)
        {
            Type type = typeof(T);
            var props = type.GetProperties();
            if (ContainStringArr(props, "Id") && ContainStringArr(props, e.PropertyName) && type == sender.GetType())
            {
                int index = (int)type.GetProperty("Id").GetValue((T)sender);
                var propValue = type.GetProperty(e.PropertyName).GetValue((T)sender);
                UpdateSql("Id", index, e.PropertyName, propValue);
            }
        } 
        #endregion
    }
}
