using Mono.Data.Sqlite;
using UnityEngine;
using System;

namespace Answerquestions {
    public class Database {
        private string PATH_DATABASE { get; } = "Data.db";
        public string tableName;
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SqliteConnection SqlConnection;
        /// <summary>
        /// 数据库读取
        /// </summary>
        private SqliteDataReader SqlDataReader;
        /// <summary>
        /// 建立数据库连接
        /// </summary>
        public Database() {
            try {
                SqlConnection = new SqliteConnection(GetDataPath(PATH_DATABASE));
                SqlConnection.Open();
            } catch (Exception e) {
                Debug.Log(e.ToString());
            }
        }
        /// <summary>
        /// 新增，修改，删除
        /// </summary>
        /// <returns></returns>
        public int Updata() {
            return 0;
        }
        /// <summary>
        /// 表格是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool ExistTable(string tableName) {
            string sql = $"SELECT COUNT(*) FROM sqlite_master where type='table' and name='{tableName}';";
            return ExecuteScalar(sql);
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void Close() {

            if (SqlDataReader != null) {
                SqlDataReader.Close();
                SqlDataReader = null;
            }

            if (SqlConnection != null) {
                SqlConnection.Close();
                SqlConnection = null;
            }
        }
        public bool ExecuteScalar(string sql) {
#if UNITY_EDITOR
            Debug.Log("SQL:ExecuteScalar " + sql);
#endif
            using (var command = SqlConnection.CreateCommand()) {
                try {
                    //执行数据库操作             
                    command.CommandText = sql;
                    int result = System.Convert.ToInt32(command.ExecuteScalar());
                    return (result > 0);
                } catch (SqliteException e) {             //回滚            
                    throw e;
                }
            }
        }
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public SqliteDataReader ExecuteReader(string sql) {
#if UNITY_EDITOR
            Debug.Log("SQL:ExecuteReader " + sql);
#endif
            using (var command = SqlConnection.CreateCommand()) {
                try {
                    //执行数据库操作             
                    command.CommandText = sql;
                    SqliteDataReader reader = command.ExecuteReader();
                    return reader;
                } catch (SqliteException e) {             //回滚            
                    return null;
                }
            }
        }
        public void ExecuteNonQuery(string sql) {
#if UNITY_EDITOR
            Debug.Log("SQL:ExecuteNonQuery " + sql);
#endif
            using (var command = SqlConnection.CreateCommand()) {
                try {
                    //执行数据库操作             
                    command.CommandText = sql;
                    command.ExecuteNonQuery();

                } catch (SqliteException e) {             //回滚            
                    Debug.LogError(e.Message);
                }
            }
        }
        public string GetDataPath(string databasePath) {
#if UNITY_EDITOR 
            return $"data source={Application.streamingAssetsPath}/{databasePath}";
#endif
#if UNITY_ANDROID
            return $"URI=file:{Application.streamingAssetsPath}/{databasePath}";
#endif
#if UNITY_IOS
            return $"data source={Application.streamingAssetsPath}/{databasePath}";
#endif
        }

    }

}