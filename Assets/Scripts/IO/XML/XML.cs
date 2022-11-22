using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;


namespace Answerquestions
{
	public class XML
	{
		static string root = Application.persistentDataPath;
		public static void CreateXML(string filename) {
			string path = Path.Combine(root, filename);
			XmlDocument xml;
			if (!File.Exists(path)){
				//如果xml不存在则创建
				xml = new XmlDocument();
				xml.Save(path);
			}

        }
		public static XmlDocument LoadXML(string filename) {
			//加载xml
			XmlDocument xml = new XmlDocument();
			xml.Load(Path.Combine(root, filename));
			return xml;
		}
		public static XmlNodeList GetNodeList(XmlDocument xml, string tag) {
			return xml.SelectSingleNode(tag).ChildNodes;
        }
	}
	
}