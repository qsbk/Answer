using OpenBLive.Runtime.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalFramework;

namespace Answerquestions
{
	/// <summary>
	/// 答题系统 维护一个用户List 及 本轮答题信息Dic
	/// 
	/// </summary>
	public class AnswerSys : MonoBehaviour
	{
		QuestionBank bank;
		//private static List<User> rank = new List<User>();
		private List<long> rightUser = new List<long>();
		private List<long> errUser = new List<long>();
		private bool shut;
		private  Question currQuestion;
		const int RANK_COUNT = 18;
		private void Awake() {
			bank = new QuestionBank();
			EventManager.AddEvent<Dm>("Message", OnDamuMsg);
			EventManager.AddEvent("START_SETTLE", settle);
			EventManager.AddEvent("SETTLE_END", next);
        }
        private void Start() {
			EventManager.TriggerEvent("SYSTEM_CONNECT");
			UpUserRank();
		}
        public void next() {
			shut = false;
			currQuestion = bank.getNextQuestion();
			EventManager.TriggerEvent("UPDATE_QUESTION", currQuestion);
        }
		public void run() {
			shut = false;
			currQuestion = bank.getQuestion(0);
			EventManager.TriggerEvent("UPDATE_QUESTION", currQuestion);
		}
		/// <summary>
		/// 更新单人排行榜
		/// </summary>
		private void UpUserRank() {
			List<long> fuser = MainManger.Instance.indicator.GetRank(RANK_COUNT);
			EventManager.TriggerEvent("UP_RANK", fuser);
		}
/*		/// <summary>
		/// 试图将某个用户加入排行榜
		/// </summary>
		/// <param name="user"></param>
		public void TryAddRank (User user){
			//if (!user.IsTag) { return; };
			rank.Remove(user);
			int inser = 0;
			int count = rank.Count;
			///逆序遍历 比较大小判断插入点及是否可插入
			for (int i = count -1; i >= 0 ; i--) {
				if (rank[i].Sore > user.Sore) { break; }
				inser--;
            }
			if ((count + inser) < RANK_COUNT) {
				rank.Insert(rank.Count + inser, user);
			}
		}*/
		/// <summary>
		/// 结算分数
		/// </summary>
		public void settle() {
			shut = true; 
			int len = rightUser.Count;
            for (int i = 0; i < len; i++) {
				User user = UserManager.GetUser(rightUser[i]);
                UserInfo userInfo = new UserInfo {
                    id = user.Id,
                    name = user.Name,
                    face = user.FaceUrl,
                    basicScore = SettleScores(i),
                    extraScore = user.extraScore,
                    weight = user.weight,
					tag = user.Tag
                };
                int c = userInfo.basicScore +userInfo.extraScore;
				userInfo.Score = (int) (c * userInfo.weight);
				EventManager.TriggerEvent("ADD_USER_TO_TOP", userInfo);
				user.weight = 1;
				//更新用户分数到计分器
				MainManger.Instance.indicator.UpScore(user.Id, MainManger.Instance.indicator.GetScore(user.Id) + userInfo.Score);
				
            }
			UpUserRank();
			EventManager.TriggerEvent("SETTLE", currQuestion.answer[0]);
			rightUser.Clear();
			errUser.Clear();
        }
        /// <summary>
        /// 接受脚本事件
        /// </summary>
        /// <param name="result"></param>
        private void OnDamuMsg(Dm danmu) {
			User user = UserManager.GetUser(danmu.uid);
			if (shut || user is null) { return; }
            ///当已经选择错误，无法重新选择
            if (currQuestion.type.Equals("选择题")) {
                danmu.msg = danmu.msg.ToUpper();
            }

			///判断答案列表
            foreach (var item in currQuestion.answer) {
				if (danmu.msg.Equals(item)) {
					Debug.Log("答案正确：" + user.Name);
					user.TryGetFace();
					errUser.Remove(user.Id);
					if (!rightUser.Contains(user.Id)) { rightUser.Add(user.Id); }
					return;
				}
			}
            if (!rightUser.Contains(user.Id)) {
                Debug.Log("答案错误：" + user.Name);
                errUser.Add(user.Id);
            }
			
		}
        /// <summary>
        /// 结算分数
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public static int SettleScores(int rank) {
			int c = 0;
			if (rank == 0) {
				c = 47;
			} else if (rank == 1) {
				c = 42;
			} else if (rank == 2) {
				c = 37;
			} else if (rank < 10) {
				c = 27;
			} else {
				c = 22;
			}
			return c;
		}
	}
	
}