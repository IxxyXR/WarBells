using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UnityEditor.Recorder.Input
{
    [DisplayName("Game View")]
    [Serializable]
    public class GameViewInputSettings : StandardImageInputSettings
    {
        public GameViewInputSettings()
        {
            outputHeight = ImageHeight.Window;
        }
        
        internal override Type inputType
        {
            get { return typeof(GameViewInput); }
        }

        internal override bool ValidityCheck(List<string> errors)
        {   
            return true;
        }

        public override bool supportsTransparent
        {
            get { return false; }
        }
    }
}