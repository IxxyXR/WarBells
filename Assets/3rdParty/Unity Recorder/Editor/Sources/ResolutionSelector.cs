using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityEditor.Recorder
{
    static class ResolutionSelector
    {
        static readonly string[] s_MaskedNames;
        static readonly Dictionary<ImageHeight, int> s_ImageDimensionToIndex = new Dictionary<ImageHeight, int>();

        static ResolutionSelector()
        {
            s_MaskedNames = EnumHelper.ClipOutEnumNames<ImageHeight>((int)ImageHeight.Window, (int)ImageHeight.x4320p_8K, ToLabel);

            var values = Enum.GetValues(typeof(ImageHeight));
            for (int i = 0; i < values.Length; ++i)
                s_ImageDimensionToIndex[(ImageHeight)values.GetValue(i)] = i;
        }

        public static int Popup(string label, ImageHeight max, int intValue)
        {              
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                var index = EnumHelper.GetClippedIndexFromEnumValue<ImageHeight>(intValue, (int)ImageHeight.Window, (int)max);
                index = EditorGUILayout.Popup(label, index, s_MaskedNames.Take(s_ImageDimensionToIndex[max] + 1).ToArray());

                if (check.changed)
                    intValue = EnumHelper.GetEnumValueFromClippedIndex<ImageHeight>(index, (int)ImageHeight.Window, (int)max);

                if (intValue > (int)max)
                    intValue = (int)max;
            }

            return intValue;
        }

        static string ToLabel(ImageHeight value)
        {
            switch (value)
            {
                case ImageHeight.x4320p_8K:
                    return "8K - 4320p";
                case ImageHeight.x2880p_5K:
                    return "5K - 2880p";
                case ImageHeight.x2160p_4K:
                    return "4K - 2160p";
                case ImageHeight.x1440p_QHD:
                    return "QHD - 1440p";
                case ImageHeight.x1080p_FHD:
                    return "FHD - 1080p";
                case ImageHeight.x720p_HD:
                    return "HD - 720p";
                case ImageHeight.x480p:
                    return "SD - 480p";
                case ImageHeight.x240p:
                    return "240p";
                case ImageHeight.Window:
                    return "Match Window Size";
                default:
                    return "unknown";
            }
        }      
    }
}