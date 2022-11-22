using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Answerquestions {
    /// <summary>
    /// 计分器类
    /// </summary>
    public class ScoreIndicator {
        Dictionary<long, int> scores = new Dictionary<long, int>();
        List<long> rank = new List<long>();
        public ScoreIndicator() {
        }
        public int GetScore(long id) {
            return GameDocuments.Instance.GetUserScore(id);
        }
        public void AddScore(long id, int score) {
            GameDocuments.Instance.UpScore(id, score);
        }
        public void AddScore(long id) {
            AddScore(id, 0);
        }
        public bool ContainsKey(long id) {
            return scores.ContainsKey(id);
        }
        /// <summary>
        /// 获取排名的前n项
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<long> GetRank(int count) {
                return GameDocuments.Instance.GetUsersTop(count);
        }
        /// <summary>
        /// 更新分数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="score"></param>
        public void UpScore(long id, int score) {
            GameDocuments.Instance.UpScore(id, score);
        }
        /// <summary>
        /// 分数类
        /// </summary>
        public class Score : IComparable<Score> {
            public long uid;
            public int score;
            public Score() { }

            public int CompareTo(Score other) {
                return score.CompareTo(other.score);
            }
        }

    }
}
