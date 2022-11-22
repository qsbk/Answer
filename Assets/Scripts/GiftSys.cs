using System.Collections;
using System.Collections.Generic;
using OpenBLive.Runtime.Data;
using UnityEngine;


namespace Answerquestions
{
	public class GiftSys : MonoBehaviour, ISys<SendGift>
	{

        
        private bool isRun = true;
        /// <summary>
        /// 处理用户的礼物积分充值
        /// </summary>
        /// <param name="gift"></param>
        private void OnSendGift(SendGift gift) {
            
            if (gift.paid) {
                //Debug.Log(gift.fansMedalName);
                User user = new User(gift.uid, gift.userName, gift.userFace, 0);
                
                if (gift.giftName.Equals("小花花")) {
                    Debug.LogError(user.Name + ": " + "小花花");
                    if (user.weight < 1.5f) { user.weight = 1.5f; }

                } else if (gift.giftName.Equals("这个好诶")) {
                    Debug.LogError(user.Name + ": " + "这个好诶");
                    if (user.weight < 2f) { user.weight = 2; }

                }else if (gift.giftName.Equals("粉丝团灯牌")) {
                    user.Tag = 1;
                }
                UserManager.UpUser(user);
            }
            
            

        }

        public void Run() {
        }

        public void Stop() {
        }

        public void Close() {
            Destroy(this.gameObject);
        }

        public void Work(SendGift data) {
            if (!isRun) { return; }
            OnSendGift(data);
        }

    }

}