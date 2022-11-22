using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

    /// <summary>
    /// 显示的log等级
    /// </summary>
public enum LogLevel {
    /// <summary>
    /// 调试
    /// </summary>
    Debug = 0x000000,
    /// <summary>
    /// 信息
    /// </summary>
    Info = 0xff5700,
    /// <summary>
    /// 警告
    /// </summary>
    Warning = 0xfee033,
    /// <summary>
    /// 错误
    /// </summary>
    Erro = 0xfd2423,
    /// <summary>
    /// 严重
    /// </summary>
    Fatal = 0x307e16,
    /// <summary>
    /// 不打印
    /// </summary>
    Nothing
}


[DebuggerNonUserCode]
[DebuggerStepThrough]
public class Log {
    public static LogLevel logLevel = LogLevel.Debug;
    public static void Debug(object @object) {
        LogString(@object.ToString(), LogLevel.Debug, "#ffffff");
    }
    public static void Info(object @object) {
        LogString(@object.ToString(), LogLevel.Info, "#ff5700");
    }
    public static void Warning(object @object) {
        LogString(@object.ToString(), LogLevel.Warning, "#fee033");
    }
    public static void Erro(object @object) {
        LogString(@object.ToString(), LogLevel.Erro, "#fd2423");
    }
    public static void Fatal(object @object) {
        LogString(@object.ToString(), LogLevel.Fatal, "#307e16");
    }
    private static void LogString(string msg, LogLevel level, string color) {
        if (level < logLevel) { return; }
        Color m_Color;
        if(!ColorUtility.TryParseHtmlString(color,out m_Color)) {return;}
        UnityEngine.Debug.Log(string.Format("<color={0}><b><size=14>" + 
            level.ToString() + " : " + msg + "</size></b></color>", color));
    }
        
}

