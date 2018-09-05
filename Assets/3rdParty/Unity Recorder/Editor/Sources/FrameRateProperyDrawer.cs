namespace UnityEditor.Recorder
{
    [CustomPropertyDrawer(typeof(FrameRateType))]
    class FrameRateProperyDrawer : EnumProperyDrawer<FrameRateType>
    {
        protected override string ToLabel(FrameRateType value)
        {
            return FrameRateHelper.ToLable(value);
        }       
    }
    
    [CustomPropertyDrawer(typeof(ImageAspect))]
    class ImageAspectProperyDrawer : EnumProperyDrawer<ImageAspect>
    {
        protected override string ToLabel(ImageAspect value)
        {
            switch (value)
            {
                case ImageAspect.x16_9:
                    return "16 x 9";
                case ImageAspect.x16_10:
                    return "16 x 10";
                case ImageAspect.x19_10:
                    return "19 x 10";
                case ImageAspect.x5_4:
                    return "5 x 4";
                case ImageAspect.x4_3:
                    return "4 x 3";
                default:
                    return "unknown";
            }
        }       
    }
}