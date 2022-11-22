using UnityEngine;
using System.Data;
using Answerquestions;

public class Test : MonoBehaviour {
    private void Start() {
        MySQLDatabase mySql = new MySQLDatabase("rm-bp1uqois4fxz484n70o.mysql.rds.aliyuncs.com", "answer", "kA%S!*gz36$Xaz_", "answer");
        mySql.CreateTableAutoID("tableTest", new string[] { "id", "name", "age" }, new string[] { "int", "text", "text" });
        mySql.InsertInto("tableTest", new string[] { "name", "age" }, new string[] { "张三", "28" });
        mySql.InsertInto("tableTest", new string[] { "name", "age" }, new string[] { "李四", "20" });
        for (int i = 1; i < 3; i++) {
            DataSet ds = mySql.Select("tableTest", new string[] { "name", "age" }, new string[] { "id" }, new string[] { "=" }, new string[] { i.ToString() });
            if (ds != null) {
                DataTable table = ds.Tables[0];
                foreach (DataRow row in table.Rows) {
                    foreach (DataColumn column in table.Columns) {
                        Debug.Log(row[column]);
                    }
                }
            }
        }
        mySql.Close();
    }
}
