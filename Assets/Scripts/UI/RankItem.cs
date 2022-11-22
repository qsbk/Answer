
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Answerquestions
{
	public class RankItem : MonoBehaviour
	{
        [SerializeField] Text numText;
        [SerializeField] Text nameText;
        [SerializeField] Text scoreText;
        [SerializeField] Image msgImage;
        private Sequence sequence;
        private bool isShut;
        private void Start() {
            gameObject.SetActive(false);
        }
        public void SetUser(User user, int rank) {
            numText.text = rank.ToString();
            if(user.Name.Length > 6) {
                nameText.fontSize = 19;
            } else {
                nameText.fontSize = 24;
            }
            nameText.text = user.Name;
            if (user.Tag >= 15) {
                nameText.color = Color.red;
            } else {
                nameText.color = Color.black;
            }
            scoreText.text = MainManger.Instance.indicator.GetScore(user.Id).ToString();
            gameObject.SetActive(true);
        }
        
        public void ShowMsg(Sprite sprite) {
            if (isShut) { return; }
            isShut = true;
            msgImage.sprite = sprite;
            msgImage.gameObject.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(msgImage.transform.DORotate(new Vector3(0, 0, -15), 0.2f));
            sequence.Append(msgImage.transform.DORotate(new Vector3(0, 0, 15), 0.2f));
            sequence.Append(msgImage.transform.DORotate(new Vector3(0, 0, 0), 0.2f));
            sequence.AppendInterval(1);
            sequence.Append(msgImage.transform.DORotate(new Vector3(0, 0, -15), 0.2f));
            sequence.Append(msgImage.transform.DORotate(new Vector3(0, 0, 15), 0.2f));
            sequence.Append(msgImage.transform.DORotate(new Vector3(0, 0, 0), 0.2f));
            sequence.AppendCallback(() => {
                msgImage.gameObject.SetActive(false);
                isShut = false;
            });

        }
	}
	
}