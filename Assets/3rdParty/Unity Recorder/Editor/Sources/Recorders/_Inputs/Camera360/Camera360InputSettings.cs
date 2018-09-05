#if UNITY_2018_1_OR_NEWER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    [DisplayName("360 View")]
    [Serializable]
    public class Camera360InputSettings : ImageInputSettings
    {
        public ImageSource source = ImageSource.MainCamera;
        public string cameraTag;
        public bool flipFinalOutput = false;
        public bool renderStereo = true;
        public float stereoSeparation = 0.065f;
        public int mapSize = 1024;
        public int outputWidth = 1024;
        public int outputHeight = 2048;

        internal override Type inputType
        {
            get { return typeof(Camera360Input); }
        }
        
        public override Vector2 resolution
        {
            get { return new Vector2(outputWidth, outputHeight); }
        }

        internal override bool ValidityCheck(List<string> errors)
        {
            var ok = true;

            if (source == ImageSource.TaggedCamera && string.IsNullOrEmpty(cameraTag))
            {
                ok = false;
                errors.Add("Missing camera tag");
            }

            if (outputWidth != (1 << (int)Math.Log(outputWidth, 2)))
            {
                ok = false;
                errors.Add("Output width must be a power of 2.");
            }

            if (outputWidth < 128 || outputWidth > 8 * 1024)
            {
                ok = false;
                errors.Add( string.Format( "Output width must fall between {0} and {1}.", 128, 8*1024 ));
            }

            if (outputHeight != (1 << (int)Math.Log(outputHeight, 2)))
            {
                ok = false;
                errors.Add("Output height must be a power of 2.");
            }

            if (outputHeight < 128 || outputHeight > 8 * 1024)
            {
                ok = false;
                errors.Add( string.Format( "Output height must fall between {0} and {1}.", 128, 8*1024 ));
            }

            if (mapSize != (1 << (int)Math.Log(mapSize, 2)))
            {
                ok = false;
                errors.Add("Cube Map size must be a power of 2.");
            }

            if( mapSize < 16 || mapSize > 8 * 1024 )
            {
                ok = false;
                errors.Add( string.Format( "Cube Map size must fall between {0} and {1}.", 16, 8*1024 ));
            }

            if (renderStereo && stereoSeparation < float.Epsilon)
            {
                ok = false;
                errors.Add("Stereo separation value is too small.");
            }

            return ok;
        }
    }

}

#endif