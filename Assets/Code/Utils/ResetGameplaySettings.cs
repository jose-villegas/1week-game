#if UNITY_EDITOR
using System;
using System.Reflection;
using General;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    class ResetGameplaySettings : MonoBehaviour
    {
        [MenuItem("Gameplay Settings/Reset to Default", true)]
        private static bool ValidateResetToDefault()
        {
            return Selection.activeObject is GameplaySettings;
        }

        [MenuItem("Gameplay Settings/Reset to Default")]
        private static void ResetToDefault()
        {
            // a new instance has the default values
            var defGameplay =
                ScriptableObject.CreateInstance<GameplaySettings>();
            var gameplay = Selection.activeObject as GameplaySettings;

            if (gameplay == null) return;

            Type type = gameplay.GetType();
            BindingFlags flags = BindingFlags.Public |
                                 BindingFlags.NonPublic |
                                 BindingFlags.Instance |
                                 BindingFlags.Default |
                                 BindingFlags.DeclaredOnly;
            PropertyInfo[] properties = type.GetProperties(flags);

            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    try
                    {
                        property.SetValue(gameplay, property.GetValue(defGameplay,
                                          null), null);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }

            FieldInfo[] fields = type.GetFields(flags);

            foreach (var fieldInfo in fields)
            {
                fieldInfo.SetValue(gameplay, fieldInfo.GetValue(defGameplay));
            }

            DestroyImmediate(defGameplay);
        }
    }
}
#endif