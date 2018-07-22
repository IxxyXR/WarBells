using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class SingletonLoopSeek : SingletonLoopSeekBase<SingletonLoopSeek>
{
    PlayableDirector playableDirector;

    [SerializeField]
    Dictionary<int, double> dicLabelTime = new Dictionary<int, double>();

    bool canSetTime = true;
    private double originalSpeed = 1;
    
    void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    void Start() { }

    public void ClearDic()
    {
        dicLabelTime.Clear();
    }

    public void AddLabelTime(int label, double time)
    {
        dicLabelTime.Add(label, time);
    }

    public void ErrorCheck(int label)
    {
        if (!dicLabelTime.ContainsKey(label))
            Debug.LogError("dicLabelTime does not contain label:" + label.ToString() + " !", transform);
    }
    
    public void Pause()
    {
        if (playableDirector != null)
        {
            originalSpeed = playableDirector.playableGraph.GetRootPlayable(0).GetSpeed();
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        }
    }

    public void Resume()
    {
        if (playableDirector != null)
        {
            playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(originalSpeed);
        }
    }

    public void Goto(int label)
    {
        SetTime(label, true);
    }

    // TimelineのLoopSeekBehaviourからの呼び出しの場合trueを指定する。
    // それ以外の場合は
    public void SetTime(int label, bool fromTimeline = false)
    {
        if (fromTimeline)
        {
            if (canSetTime)
            {
                ErrorCheck(label);
                playableDirector.time = dicLabelTime[label];
            }
        }
        else
        {
            StartCoroutine("SetTimeCoroutine", label);
        }
    }

    IEnumerator SetTimeCoroutine(int _label)
    {
        ErrorCheck(_label);

        canSetTime = false;

        playableDirector.time = dicLabelTime[_label];

        yield return new WaitForSeconds(0.1f);

        canSetTime = true;
    }

    //[Inspect(0), Title(FontStyle.Bold, "Test"), Spacing(Before = 3)]
    int testLabel;

    //[Inspect(1)]
    void Test()
    {
        StartCoroutine("SetTimeCoroutine", testLabel);
    }
}
