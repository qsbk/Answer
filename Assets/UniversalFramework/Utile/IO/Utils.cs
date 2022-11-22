using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniversalFramework
{
	public static class Utils
	{
	    /// <summary>
	    /// 判断Gui阻挡射线
	    /// </summary>
	    /// <returns></returns>
	    public static bool CheckGuiRaycastObjects() {
	        // PointerEventData eventData = new PointerEventData(Main.Instance.eventSystem);
	
	        PointerEventData eventData = new PointerEventData(EventSystem.current);
	        eventData.pressPosition = Input.mousePosition;
	        eventData.position = Input.mousePosition;
	
	        List<RaycastResult> list = new List<RaycastResult>();
	        // Main.Instance.graphicRaycaster.Raycast(eventData, list);
	        EventSystem.current.RaycastAll(eventData, list);
	        //Debug.Log(list.Count);
	        return list.Count > 0;
	
	    }
	    
	    /// <summary>
	    /// 数组转栈
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="arry"></param>
	    /// <returns></returns>
	
	    public static Stack<T> arrayToStack<T>(T[] arry) {
	        Stack<T> stack = new Stack<T>();
	
	        int len = arry.Length;
	        Debug.Log(len);
	        for (int i = 0; i < len; i++) {
	            stack.Push(arry[i]);
	        }
	
	        return stack;
	    }
	
	
	}
	
}