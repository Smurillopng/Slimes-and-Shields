using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class NewLog
    {
        public static string Colore(this string myStr, string color)
        {
            return $"<color={color}>{myStr}</color>";
        }

        private static void DoLog(Action<string, Object> logFunction, string prefix, Object myObj, params object[] msg)
        {
#if UNITY_EDITOR
            var name = (myObj ? myObj.name : "NullObject").Colore("cyan");
            logFunction($"{prefix}[{name}]: {String.Join("; ", msg)}\n ", myObj);
#endif
        }

        public static void Log(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "▶".Colore("cyan"), myObj, msg);
        }

        public static void LogError(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogError, "♦️".Colore("red"), myObj, msg);
        }

        public static void LogWarning(this Object myObj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".Colore("yellow"), myObj, msg);
        }

        public static void LogSuccess(this Object myObj, params object[] msg)
        {
            DoLog(Debug.Log, "☻".Colore("green"), myObj, msg);
        }
    }
}