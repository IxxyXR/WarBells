using System;

namespace UnityEditor.Recorder
{
    /// <summary>
    /// Helper to describe a standard image height. 
    /// </summary>
    public enum ImageHeight
    {
        x4320p_8K = 4320,
        x2880p_5K = 2880,
        x2160p_4K = 2160,
        x1440p_QHD = 1440,
        x1080p_FHD = 1080,
        x720p_HD = 720,
        x480p = 480,
        x240p = 240,
        Window = 0,
    }

    /// <summary>
    /// Helper to describe a standard image aspect ratio. 
    /// </summary>
    public enum ImageAspect
    {
        x16_9,
        x16_10,
        x19_10,
        x5_4,
        x4_3,
    }

    static class AspectRatioHelper
    {
        internal static float GetRealAspect(ImageAspect imageAspect)
        {
            switch (imageAspect)
            {
                case ImageAspect.x16_9:
                    return 16.0f / 9.0f;
                case ImageAspect.x16_10:
                    return 16.0f / 10.0f;
                case ImageAspect.x19_10:
                    return 19.0f / 10.0f;
                case ImageAspect.x5_4:
                    return 5.0f / 4.0f;
                case ImageAspect.x4_3:
                    return 4.0f / 3.0f;
                default:
                    throw new ArgumentOutOfRangeException("imageAspect", imageAspect, null);
            }
        }
    }
}
