using System;
using UnityEngine;

namespace HB.Utilities
{
    public static class LoggerUtility
    {
        static LoggerUtility()
        {
            _isLogEnabled = false;
#if UNITY_EDITOR
            _isLogEnabled = true;
#elif UNITY_ANDROID || UNITY_IOS
#if LOGS_ENABLED
            _isLogEnabled = true;
#endif
#endif
            Debug.unityLogger.logEnabled = _isLogEnabled;
        }

        private static bool _isLogEnabled;

        internal static void Log(object message)
        {
            if (_isLogEnabled)
            {
                Debug.Log(message);
            }
        }

        internal static void LogWarning(object message)
        {
            if (_isLogEnabled)
            {
                Debug.LogWarning(message);
            }
        }

        internal static void LogError(object message)
        {
            if (_isLogEnabled)
            {
                Debug.LogError(message);
            }
        }

        internal static void LogFormat(string format, params object[] args)
        {
            if (_isLogEnabled)
            {
                Debug.LogFormat(format);
            }
        }

        internal static void LogException(Exception exception)
        {
            if (_isLogEnabled)
            {
                Debug.LogException(exception);
            }
        }

        internal static void PrettyLog(object message, Color32 messageColor = default)
        {
            var _ = $"#{messageColor.r:X2}{messageColor.g:X2}{messageColor.b:X2}";

            if (_isLogEnabled) Debug.Log($"<color={_}>{message}</color>");
        }
    }
}
