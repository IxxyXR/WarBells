using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    [DisplayName("Render Texture Asset")]
    [Serializable]
    public class RenderTextureInputSettings : ImageInputSettings
    {
        public RenderTexture renderTexture;
        
        internal override Type inputType
        {
            get { return typeof(RenderTextureInput); }
        }
        
        public override Vector2 resolution
        {
            get
            {
                return renderTexture == null ? Vector2.zero : new Vector2(renderTexture.width, renderTexture.height);
            }
        }

        internal override bool ValidityCheck(List<string> errors)
        {
            var ok = true;

            if (renderTexture == null)
            {
                ok = false;
                errors.Add("Missing source render texture object/asset.");
            }

            return ok;
        }
    }
}
