using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    /// <inheritdoc />
    /// <summary>
    /// Optional base class for Image related inputs.
    /// </summary>
    public abstract class ImageInputSettings : RecorderInputSettings
    {
        public abstract Vector2 resolution { get; }
        
        public virtual bool supportsTransparent
        {
            get { return true; }
        }
        
        public bool allowTransparency;
    }
    
    /// <inheritdoc />
    /// <summary>
    /// Regroups settings needed to specify the size of an Image input using a size and an aspect ratio
    /// </summary>
    public abstract class StandardImageInputSettings : ImageInputSettings
    { 
        public ImageHeight outputHeight = ImageHeight.x720p_HD;
        public ImageAspect outputAspect = ImageAspect.x16_9;
        
        [SerializeField] internal bool forceEvenSize;
        
        internal ImageHeight maxSupportedSize { get; set; }

        protected StandardImageInputSettings()
        {
            maxSupportedSize = ImageHeight.x4320p_8K;
        }

        public override Vector2 resolution
        {
            get
            {               
                var h = (int)outputHeight;
                var w = (int)(h * AspectRatioHelper.GetRealAspect(outputAspect));

                return new Vector2(w, h);
            }
        }
        
        internal override bool ValidityCheck(List<string> errors)
        {
            var ok = true;

            if (outputHeight > maxSupportedSize)
            {
                ok = false;
                errors.Add("Output size exceeds maximum supported size: " + (int)maxSupportedSize );
            }

            return ok;
        }
    }
}