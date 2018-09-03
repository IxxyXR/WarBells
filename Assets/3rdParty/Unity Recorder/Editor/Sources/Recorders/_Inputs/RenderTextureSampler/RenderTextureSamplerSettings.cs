using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace UnityEditor.Recorder.Input
{
    /// <exclude/>
    public enum SuperSamplingCount
    {
        X1 = 1,
        X2 = 2,
        X4 = 4,
        X8 = 8,
        X16 = 16,
    }

    [DisplayName("Texture Sampling")]
    [Serializable]
    public class RenderTextureSamplerSettings : StandardImageInputSettings
    {
        public ImageSource source = ImageSource.ActiveCamera;
        public ImageHeight renderSize = ImageHeight.x720p_HD;
        public SuperSamplingCount superSampling = SuperSamplingCount.X1;
        public float superKernelPower = 16f;
        public float superKernelScale = 1f;
        public string cameraTag;
        public ColorSpace colorSpace = ColorSpace.Gamma;
        public bool flipFinalOutput = false;

        internal override Type inputType
        {
            get { return typeof(RenderTextureSampler); }
        }

        internal override bool ValidityCheck(List<string> errors)
        {
            return true;
        }
    }
}