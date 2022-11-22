using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Answerquestions
{
	public class User
	{
		private long id;
		private string name;
		private string faceUrl;
        public Sprite face;
        private long tag;
        public float weight = 1f;
        public int extraScore = 0;  //额外分数
        public string FaceUrl { get => faceUrl; set {faceUrl = value;} }
        public long Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public long Tag { get => tag;
            set { 
                tag = value;
                if (tag >= 15) {
                    extraScore = 5;
                }
            } }
        public void TryGetFace() {
            if (FaceUrl.Equals(string.Empty)) { return; }
            BiliBIliAPI.LoadSprite(faceUrl, (s) => {
                face = s;
            });
        }
        public User(long id, string name) {
            this.Id = id;
            this.Name = name;
        }

        public User(long id, string name, string face) {
            this.id = id;
            this.Name = name;
            this.FaceUrl = face;
        }

        public User(long id, string name, string face, long Tag) {
            this.id = id;
            this.Name = name;
            this.FaceUrl = face;
            this.Tag = Tag;
        }
    }
	
}