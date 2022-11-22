using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
using static Answerquestions.ScoreIndicator;

namespace Answerquestions
{
	public class GameDocuments {
		private static string scoreTableName = "Score" + DateTime.Now.ToString("yyyy-MM-dd");
		private readonly string server = "rm-bp1uqois4fxz484n70o.mysql.rds.aliyuncs.com";
		private readonly string id = "answer";
		private readonly string pwd = "kA%S!*gz36$Xaz_";
		private readonly string databaseName = "answer";
		private MySQLDatabase database;
		private static GameDocuments instance;
		public static GameDocuments Instance { private set { } get { 
				if(instance is null) {
					instance = new GameDocuments();
                }
				return instance; 
			} }
		/// <summary>
		/// 单例模式
		/// </summary>
		private GameDocuments() {
			database = new MySQLDatabase(server, id, pwd, databaseName);
		}
		/// <summary>
		/// 同题库中随机抽取待抽取题目
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		public List<Question> GetQuestions(int num) {
			List<Question> result = new List<Question>();
			String QUERY_QUESTION_SQL = "SELECT q.id, q.qs_describe AS 'describe', t.title AS 'type', q.qs_solution, q.qs_options AS 'options', c.title AS 'category', q.qs_solution AS 'solution'" +
																" FROM question q, question_type t, question_category c"
																+ " WHERE q.qs_type = t.id AND q.qs_category = c.id AND q.weight = true"
																+ $" ORDER BY RAND() DESC LIMIT {num};";
			var query = database.QuerySet(QUERY_QUESTION_SQL);
			if (isHasData(query)) {
				int len = Math.Min(query.Tables[0].Rows.Count, num);
				for (int i = 0; i < len; i++) {
					Question question = new Question();
					question.id = long.Parse(query.Tables[0].Rows[i]["id"].ToString());
					question.description = query.Tables[0].Rows[i]["describe"].ToString();
					question.type = query.Tables[0].Rows[i]["type"].ToString();
					string options = query.Tables[0].Rows[i]["options"].ToString();
					string answer = query.Tables[0].Rows[i]["solution"].ToString();
					/*options = Regex.Unescape(options);
					answer = Regex.Unescape(answer);
					Debug.Log(answer);*/
					question.option = JsonMapper.ToObject<List<string>>(options);
					question.answer = JsonMapper.ToObject<List<string>>(answer);
					result.Add(question);
				}

			}
			return result;
		}
		/// <summary>
		/// 判断是否还存在未使用题目
		/// </summary>
		/// <returns></returns>
		public bool isHasNotUsedQuestion() {
			String QUERY_QUESTION_SQL = "SELECT 1 FROM question WHERE weight = true;";
			var query = database.QuerySet(QUERY_QUESTION_SQL);
			return isHasData(query);
		}
		/// <summary>
		/// 将题库中的题目重置为可抽取
		/// </summary>
		public void ResetQuestionsToUse() {
			String SQL = "UPDATE question SET weight = TRUE;";
			database.QuerySet(SQL);
		}
		/// <summary>
		/// 更新题目为已抽取
		/// </summary>
		/// <param name="questions"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public void UpQuetionsWeightToUsed(List<Question> questions) {
			
            foreach (var item in questions) {
				String UP_QUESTION_WEIGHT = $"UPDATE question SET weight = 0 WHERE id = {item.id};";
				Debug.Log(UP_QUESTION_WEIGHT);
				database.QuerySet(UP_QUESTION_WEIGHT);
			}
        }
		public void UpUser(User user) {
			Debug.Log($"添加用户{user.Name}");
            string UPDATA_USER_SQL = $"INSERT INTO user (id,name,face,tag) "+
														$"VALUES('{user.Id}', '{user.Name}', '{user.FaceUrl}', '{user.Tag}') "+
														$"ON DUPLICATE KEY UPDATE name = '{user.Name}',face = '{user.FaceUrl}',tag = GREATEST(tag, {user.Tag}); ";
            database.QuerySet(UPDATA_USER_SQL);
        }
		/// <summary>
		/// 更新用户分数
		/// </summary>
		/// <param name="id"></param>
		/// <param name="score"></param>
		public void UpScore(long id, int score) {
			string UPDATA_USER_SQL;

			if (ScoreIsExists(id)) {
				UPDATA_USER_SQL = $"UPDATE user_scores SET score = {score} WHERE uid = {id} AND in_date = CURDATE();";
            } else {
                UPDATA_USER_SQL = $"INSERT INTO user_scores (uid, score) VALUES({id}, {score});";
                
            }
			database.QuerySet(UPDATA_USER_SQL);
		}
		/// <summary>
		/// 从数据库中获取分数排行前cont
		/// </summary>
		/// <param name="cont"></param>
		/// <returns></returns>
		public List<long> GetUsersTop(int cont) {
			List<long> users = new List<long>();
			//string QUERY_SCORE_TOP_SQL = $"SELECT s.uid FROM user u LEFT JOIN user_scores s ON u.id=s.uid WHERE u.tag > 0 AND s.in_date = CURDATE() ORDER BY s.score DESC  LIMIT {cont}";
			string QUERY_SCORE_TOP_SQL = $"SELECT s.uid FROM user_scores s WHERE s.in_date = CURDATE() ORDER BY s.score DESC LIMIT {cont}";
			var query = database.QuerySet(QUERY_SCORE_TOP_SQL);
			if (isHasData(query)) {
                for (int i = 0; i < query.Tables[0].Rows.Count; i++) {
					long uid = long.Parse(query.Tables[0].Rows[i]["uid"].ToString());
					users.Add(uid);
                }
			}
			return users;
		}
		/// <summary>
		/// 从数据库中查询各个阵营的分数排名
		/// </summary>
		/// <returns></returns>
		public List<CampInfo> GetCampRank() {
			List<CampInfo> rank = new List<CampInfo>();	
			/*string QUERY_CAMP_RANK_SQL = $"SELECT c.ID,c.Name,c.BColor,c.FColor,sum(s.Score) as Score " +
                $"FROM Camp c INNER JOIN User u on c.ID = u.Camp INNER JOIN '{scoreTableName}' s on s.ID = u.ID GROUP by c.ID ORDER by Score DESC;";
			var query = database.ExecuteReader(QUERY_CAMP_RANK_SQL);
			while (query.Read()) {
				CampInfo info = new CampInfo();
				info.camp = new Camp();
				info.camp.id = int.Parse(query[0].ToString());
				info.camp.name = query["Name"].ToString();
				ColorUtility.TryParseHtmlString(query["FColor"].ToString(), out info.camp.fClolor);
				ColorUtility.TryParseHtmlString(query["BColor"].ToString(), out info.camp.bClolor);
				info.Score = int.Parse(query["Score"].ToString());
				rank.Add(info);
			}
			query.Close();*/
			return rank;
		}
		/// <summary>
		/// 通过ID查找Camp
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Camp GetCampByID(int id) {
			Camp camp = null;
			string QUERY_CAMP_BY_ID = $"SELECT * FROM camp WHERE id='{id}'";
			var query = database.QuerySet(QUERY_CAMP_BY_ID);
			if (isHasData(query)) {
				camp = new Camp();
				camp.id = id; 
				camp.name = query.Tables[0].Rows[0]["name"].ToString();
				camp.oid = int.Parse(query.Tables[0].Rows[0]["creator_id"].ToString());
			}
			return camp;
			
		}
		/// <summary>
		/// 判断用户是否存在
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public bool IsUserExists(long id) {
			string QUERY_USER_EXISTS = $"SELECT 1 FROM user WHERE id={id} LIMIT 1";
			Debug.Log(database.QuerySet(QUERY_USER_EXISTS).Tables.Count);
			return database.QuerySet(QUERY_USER_EXISTS).Tables.Count > 0;
		}
		public bool ScoreIsExists(long id) {
			string QUERY_SCORE_EXISTS = $"SELECT 1 FROM user_scores WHERE in_date=CURDATE() AND uid = {id} LIMIT 1";
			return isHasData(database.QuerySet(QUERY_SCORE_EXISTS));
		}
		public String GetScoreJson() {
			return null;
        }
		/// <summary>
		/// 加入Camp
		/// </summary>
		/// <param name="uid"></param>
		/// <param name="cname"></param>
		public void JoinCamp(long uid, string cname) {
			Camp camp = GetCampByName(cname);
			if (camp == null) {return;}
			string UPDATE_USER_CAMP = $"UPDATE user SET Camp={camp.id} WHERE ID={uid};";
			database.QuerySet(UPDATE_USER_CAMP);
		}
		/// <summary>
		/// 获取用户分数
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public int GetUserScore(long id) {
			int score = 0;
			/*string QUERY_USER_SCORE = $"SELECT * FROM user u LEFT JOIN user_scores s ON u.id=s.uid WHERE u.tag > 0 AND s.in_date = CURDATE() AND u.id ={id}";*/
			string QUERY_USER_SCORE = $"SELECT * FROM user u LEFT JOIN user_scores s ON u.id=s.uid WHERE s.in_date = CURDATE() AND u.id ={id}";
			var query = database.QuerySet(QUERY_USER_SCORE);
            if (isHasData(query)) {
				score = int.Parse(query.Tables[0].Rows[0]["score"].ToString());
            }
			return score;
        }
		public int GetUserRank(long id) {
			/*string QUERY_USER_RANK = $"SELECT a1.ID, a1.Score, COUNT(a2.Score) as Score_Rank FROM " +
															$"(SELECT u.ID, u.Name, s.score FROM User as u, '{scoreTableName}' as s WHERE u.ID = s.ID AND u.Tag > 0) as a1, " +
															$"(SELECT u.ID, u.Name, s.score FROM User as u, '{scoreTableName}' as s WHERE u.ID = s.ID AND u.Tag > 0) as a2 " +
															$"WHERE a1.ID = {id} AND a1.Score <= a2.Score GROUP BY a1.ID, a1.Score ORDER BY a1.Score DESC, a1.ID DESC; ";
			var query = database.ExecuteReader(QUERY_USER_RANK);
			while (query.Read()) {
				int rank = int.Parse(query["Score_Rank"].ToString());
				return rank;
			}*/
			return -1;
        }
		/// <summary>
		/// 通过名称查找Camp
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Camp GetCampByName(string name) {
			Camp camp = null;
			string QUERY_CAMP_BY_NAME = $"SELECT * FROM camp WHERE name='{name}'";
			var query = database.QuerySet(QUERY_CAMP_BY_NAME);
            if (isHasData(query)) {
				camp = new Camp();
				camp.id = int.Parse(query.Tables[0].Rows[0]["id"].ToString());
				camp.name = name;
				camp.oid = int.Parse(query.Tables[0].Rows[0]["creator_id"].ToString());

			}
			return camp;
		}
		/// <summary>
		/// 通过ID获取用户数据
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public User GetUserByID(long id) {
			User user = null;
			string QUERY_USER_SQL = $"SELECT * FROM user WHERE id={id};";
			var query = database.QuerySet(QUERY_USER_SQL);
			if (query.Tables.Count > 0 && query.Tables[0].Rows.Count > 0) {
				
				string name = query.Tables[0].Rows[0]["name"].ToString();
				string face = query.Tables[0].Rows[0]["face"].ToString();
				int tag = int.Parse(query.Tables[0].Rows[0]["tag"].ToString());
				user = new User(id, name);
				user.FaceUrl = face;
				user.Tag = tag;
			}
			return user;
		}
        public bool isHasData(DataSet data) {
			return data.Tables.Count > 0 && data.Tables[0].Rows.Count > 0;
        }

		public void Close() {
			database.Close();
        }

	}
	
}