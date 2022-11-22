using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager {
    private static Dictionary<string, List<Delegate>> events = new Dictionary<string, List<Delegate>>();
    private static EventManager eventManager = new EventManager();
    private EventManager() {}
//    public static EventManager GetInstance { get {return eventManager;} }
    /// <summary>
    /// 通用的添加Event
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
   private static void CommonAdd(string eventName, Delegate callback) {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions)){
            actions.Add(callback);
        } else {
            actions = new List<Delegate>();
            actions.Add(callback);
            events.Add(eventName, actions);
        }
    }
    /// <summary>
    /// 没有参数的订阅
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void AddEvent(string eventName, Action callback) {
        CommonAdd(eventName, callback);
    }
    /// <summary>
    /// 一个参数的订阅
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void AddEvent<T>(string eventName, Action<T> callback) {
        CommonAdd(eventName, callback);
    }
    /// <summary>
    /// 两个参数的订阅
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void AddEvent<T, T1>(string eventName, Action<T, T1> callback) {
        CommonAdd(eventName, callback);
    }
    /// <summary>
    /// 带三个参数的添加订阅方法
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    public static void AddEvent<T, T1, T2>(string eventName, Action<T, T1, T2> callback) {
        CommonAdd(eventName, callback);
    }
    /// <summary>
    /// 带四个参数的添加订阅方法
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
    private static void CommonRemove(string eventName, Delegate callback) {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions))
            {
                actions.Remove(callback);
                if (actions.Count == 0)
                    {
                        events.Remove(eventName);
                    }
            }
    }

    /// <summary>
    /// 删除Event
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveEvent(string eventName, Action callback) {
        CommonRemove(eventName, callback);
    }
    /// <summary>
    /// 删除带参数的Event
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="listener"></param>
    public void RemoveEvent<T>(string eventName, Action<T> callback) {
        CommonRemove(eventName, callback);
    }
    /// <summary>
    /// 删除两个参数的Event
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="callback"></param>
     public static void RemoveEvent<T, T1>(string eventName, Action<T, T1> callback)
     {
         CommonRemove(eventName, callback);
     }
    /// <summary>
    /// 删除所有事件
    /// </summary>
    public static void RemoveAllEvents() {
        events.Clear();
    }
    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="eventName"></param>
    public static void TriggerEvent(string eventName) {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions)) {
            foreach (var item in actions) {
                item.DynamicInvoke();
            }
        }
    }
    /// <summary>
    /// 触发带参数的事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="arg"></param>
    public static void TriggerEvent<T>(string eventName, T arg) {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions)) {
            foreach (var item in actions) {
                item.DynamicInvoke(arg);
            }
        }
    }
    /// <summary>
    /// 触发带参数的事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="arg"></param>
    public static void TriggerEvent<T, T1>(string eventName, T arg, T1 arg1) {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions)) {
            foreach (var item in actions) {
                item.DynamicInvoke(arg, arg1);
            }
        }
    } 
    /// <summary>
    /// 触发带参数的事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="arg"></param>
    public static void TriggerEvent<T, T1, T2>(string eventName, T arg, T1 arg1, T2 arg2) {
        List<Delegate> actions = null;
        if (events.TryGetValue(eventName, out actions)) {
            foreach (var item in actions) {
                item.DynamicInvoke(arg, arg1, arg2);
            }
        }
    }
}