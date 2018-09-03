#if UNITY_EDITOR
 
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;

namespace UnityEngine.Recorder.Examples
{
    public class RecorderExample : MonoBehaviour
    {
       RecorderController m_RecorderController;
    
       void OnEnable()
       {
           var controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
           m_RecorderController = new RecorderController(controllerSettings);
    
           var outputFolder = Application.dataPath + "/SampleRecordings";
    
           // Video
           var videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
           videoRecorder.name = "My Video Recorder";
           videoRecorder.enabled = true;
    
           videoRecorder.outputFormat = VideoRecorderOutputFormat.MP4;
           videoRecorder.videoBitRateMode = VideoBitrateMode.Low;
    
           videoRecorder.imageInputSettings = new GameViewInputSettings
           {
               outputAspect = ImageAspect.x16_9,
               outputHeight = ImageHeight.x720p_HD
           };
    
           videoRecorder.audioInputSettings.preserveAudio = true;
    
           videoRecorder.outputFile = outputFolder + "/video";
    
           // Animation
           var animationRecorder = ScriptableObject.CreateInstance<AnimationRecorderSettings>();
           animationRecorder.name = "My Animation Recorder";
           animationRecorder.enabled = true;
    
           var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    
           animationRecorder.animationInputSettings = new AnimationInputSettings
           {
               gameObject = sphere, 
               recursive = true,
           };
           
           animationRecorder.animationInputSettings.AddComponentToRecord(typeof(Transform));
           
           animationRecorder.outputFile = outputFolder + "/animation_" + DefaultWildcard.GeneratePattern("GameObject");
    
           // Image Sequence
           var imageRecorder = ScriptableObject.CreateInstance<ImageRecorderSettings>();
           imageRecorder.name = "My Image Recorder";
           imageRecorder.enabled = true;
    
           imageRecorder.outputFormat = ImageRecorderOutputFormat.PNG;
           imageRecorder.captureAlpha = true;
           
           imageRecorder.outputFile = outputFolder + "/image_" + DefaultWildcard.Frame;
    
           imageRecorder.imageInputSettings = new CameraInputSettings
           {
               source = ImageSource.MainCamera,
               outputAspect = ImageAspect.x16_9,
               outputHeight = ImageHeight.x720p_HD,
               captureUI = true
           };
    
           // Setup Recording
           controllerSettings.AddRecorderSettings(videoRecorder);
           controllerSettings.AddRecorderSettings(animationRecorder);
           controllerSettings.AddRecorderSettings(imageRecorder);
    
           controllerSettings.SetRecordModeToManual();
           controllerSettings.frameRate = 60.0f;
    
           m_RecorderController.verbose = true;
           m_RecorderController.StartRecording();
       }
    
       void OnDisable()
       {
           m_RecorderController.StopRecording();
       }
    }
 }
    
 #endif
