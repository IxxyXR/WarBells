using System;

namespace UnityEditor.Recorder
{
    enum FrameRateType
    {
        FR_23, // 24 * 1000 / 1001
        FR_24,
        FR_25,
        FR_29, // 30 * 1000 / 1001,
        FR_30,
        FR_50,
        FR_59, // 60 * 1000 / 1001,
        FR_60,
        FR_CUSTOM,
    }
    
    static class FrameRateHelper
    {
        public static float ToFloat(FrameRateType frameRateType, float customValue)
        {
            switch (frameRateType)
            {
                case FrameRateType.FR_CUSTOM:
                    return customValue;
                case FrameRateType.FR_23:
                    return 24 * 1000 / 1001f;
                case FrameRateType.FR_24:
                    return 24;
                case FrameRateType.FR_25:
                    return 25;
                case FrameRateType.FR_29:
                    return 30 * 1000 / 1001f;
                case FrameRateType.FR_30:
                    return 30;
                case FrameRateType.FR_50:
                    return 50;
                case FrameRateType.FR_59:
                    return 60 * 1000 / 1001f;
                case FrameRateType.FR_60:
                    return 60;
                default:
                    throw new ArgumentOutOfRangeException("frameRateType", frameRateType, null);
            }
        }

        internal static string ToLable(FrameRateType value)
        {
            switch (value)
            {
                case FrameRateType.FR_23:
                    return "23.97";
                case FrameRateType.FR_24:
                    return "Film (24)";
                case FrameRateType.FR_25:
                    return "PAL (25)";
                case FrameRateType.FR_29:
                    return "NTSC (29.97)";
                case FrameRateType.FR_30:
                    return "30";
                case FrameRateType.FR_50:
                    return "50";
                case FrameRateType.FR_59:
                    return "59.94" ;
                case FrameRateType.FR_60:
                    return "60";
                case FrameRateType.FR_CUSTOM:
                    return "Custom";
                    
                default:
                    return "unknown";
            }
        }       
    }
}
