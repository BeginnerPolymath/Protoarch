using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace TriInspector.Utilities
{
    public class TriUnityInspectorUtilities
    {
        private static readonly Dictionary<int, string> StandardArrayElementNames = new Dictionary<int, string>();
        
        private static readonly FieldInfo GUIStyleNameBackingField = typeof(GUIStyle)
            .GetField("m_Name", BindingFlags.Instance | BindingFlags.NonPublic);

        public static bool MustDrawWithUnity(TriProperty property)
        {
            if (property.FieldType == typeof(GUIStyle) ||
                property.FieldType == typeof(RectOffset))
            {
                return true;
            }

            return !property.IsArray && property.TryGetAttribute(out DrawWithUnityAttribute _);
        }

        public static string GetStandardArrayElementName(TriProperty property)
        {
            var index = property.IndexInArray;

            if (!StandardArrayElementNames.TryGetValue(index, out var name))
            {
                StandardArrayElementNames[index] = name = $"Element {index}";
            }

            return name;
        }

        public static bool TryGetSpecialArrayElementName(TriProperty property, out string name)
        {
            if (property.FieldType == typeof(GUIStyle) && property.Value is GUIStyle guiStyle)
            {
                GUIStyleNameBackingField?.SetValue(guiStyle, null);
                name = guiStyle.name;
                return true;
            }

            if(property.Parent.Value.GetType().ToString() == "System.Collections.Generic.List`1[ComponentScript]")
            {
                MethodInfo getItemMethod = property.Parent.Value.GetType().GetMethod("get_Item");

                if((int)property.Parent.Value.GetType().GetProperty("Count").GetValue(property.Parent.Value) == 0)
                {
                    name = "";
                    return false;
                }

                object element = getItemMethod.Invoke(property.Parent.Value, new object[] { property.IndexInArray });

                MethodInfo getObjectMethod = element.GetType().GetField("Component").GetType().GetMethod("GetObject");

                object component = element.GetType().GetField("Component").GetValue(element);

                object value = component.GetType().GetField("Value").GetValue(component);


                name = element.GetType().GetField("Name").GetValue(element).ToString() + " | " 
                + element.GetType().GetField("Component").GetValue(element) + " | " + value;

                return true;
            }

            if(property.Parent.Value.GetType().ToString() == "System.Collections.Generic.List`1[SystemScript]")
            {
                MethodInfo getItemMethod = property.Parent.Value.GetType().GetMethod("get_Item");

                object element = getItemMethod.Invoke(property.Parent.Value, new object[] { property.IndexInArray });

                name = element.GetType().GetField("Name").GetValue(element).ToString();

                return true;
            }

            if(property.Parent.Value.GetType().ToString() == "System.Collections.Generic.List`1[EntityScript]")
            {
                MethodInfo getItemMethod = property.Parent.Value.GetType().GetMethod("get_Item");

                if((int)property.Parent.Value.GetType().GetProperty("Count").GetValue(property.Parent.Value) - 1 < property.IndexInArray)
                {
                    name = "";
                    return false;
                }

                object element = getItemMethod.Invoke(property.Parent.Value, new object[] { property.IndexInArray });

                name = element.GetType().GetField("Name").GetValue(element).ToString();

                return true;
            }


            foreach (var field in property.Parent.Value.GetType().GetFields())
            {
                Debug.Log(field.Name);
            }

            if (property.PropertyType == TriPropertyType.Generic &&
                property.ChildrenProperties.Count > 0 &&
                property.ChildrenProperties[0] is var firstChild &&
                firstChild.ValueType == typeof(string) &&
                firstChild.Value is string firstChildValueStr &&
                !string.IsNullOrEmpty(firstChildValueStr))
            {
                name = firstChildValueStr;
                return true;
            }

            name = default;
            return false;
        }
    }
}