using System;

namespace UnityEditor.Recorder.Input
{
    class RenderTextureInput : BaseRenderTextureInput
    {
        RenderTextureInputSettings cbSettings
        {
            get { return (RenderTextureInputSettings)settings; }
        }

        public override void BeginRecording(RecordingSession session)
        {
            if (cbSettings.renderTexture == null)
                throw new Exception("No Render Texture object provided as source");

            outputHeight = cbSettings.renderTexture.height;
            outputWidth = cbSettings.renderTexture.width;
            outputRT = cbSettings.renderTexture;
        }
    }
}