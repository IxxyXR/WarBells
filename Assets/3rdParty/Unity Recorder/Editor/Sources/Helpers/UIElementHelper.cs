using UnityEngine.Experimental.UIElements;

namespace UnityEditor.Recorder
{
    static class UIElementHelper
    {
        internal static void SetFlex(VisualElement element, float value)
        {
            #if UNITY_2018_3_OR_NEWER
                element.style.flex = new Flex(value);
            #else
                element.style.flex = value;
            #endif
        }

        internal static bool GetToggleValue(Toggle toggle)
        {
            #if UNITY_2018_2_OR_NEWER
                return toggle.value;
            #else
                return toggle.on;
            #endif
        }

        internal static void SetToggleValue(Toggle toggle, bool value)
        {
            #if UNITY_2018_2_OR_NEWER
                toggle.value = value;
            #else
                toggle.on = value;
            #endif
        }
    }
}