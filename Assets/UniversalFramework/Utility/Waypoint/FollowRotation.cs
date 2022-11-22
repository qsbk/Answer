using UnityEngine;


namespace UniversalFramework {
    public class FollowRotation : MonoBehaviour {
        //定义被指向主体的Transform
        Transform m_transform;
        //定义被指向物体的Transform
        public Transform target;
        // Use this for initialization
        void Start() {
            //初始化给主体赋Transform
            m_transform = this.transform;
        }
        // Update is called once per frame
        void Update() {
            //调用旋转函数
            QuaterLook();
        }
        //四元数旋转函数
        void QuaterLook() {
            //定义一个三维变量 用来表示从本体 到 客体的向量
            Vector3 pos = target.position - m_transform.position;
            //用四元数的LookRotation() 获得转向的四元数
            Quaternion m_rotation = Quaternion.LookRotation(pos);
            //把得到的旋转量 赋给旋转本体 实现指向旋转
            m_transform.localRotation = m_rotation;
        }
    }

}