using UnityEngine;


namespace UniversalFramework {
    public class FollowRotation : MonoBehaviour {
        //���屻ָ�������Transform
        Transform m_transform;
        //���屻ָ�������Transform
        public Transform target;
        // Use this for initialization
        void Start() {
            //��ʼ�������帳Transform
            m_transform = this.transform;
        }
        // Update is called once per frame
        void Update() {
            //������ת����
            QuaterLook();
        }
        //��Ԫ����ת����
        void QuaterLook() {
            //����һ����ά���� ������ʾ�ӱ��� �� ���������
            Vector3 pos = target.position - m_transform.position;
            //����Ԫ����LookRotation() ���ת�����Ԫ��
            Quaternion m_rotation = Quaternion.LookRotation(pos);
            //�ѵõ�����ת�� ������ת���� ʵ��ָ����ת
            m_transform.localRotation = m_rotation;
        }
    }

}