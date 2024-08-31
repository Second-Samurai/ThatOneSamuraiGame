using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindEntity : MonoBehaviour
{
    public bool isTravelling = false;
    public int currentIndex = 0;
    [SerializeField]
    public List<PositionalTimeData> transformDataList;
    public Transform thisTransform;

    public RewindManager _rewindInput;


    //[Header("TimeThreashold")]

    //public TimeThreasholdReferance timeThreasholdVariable;

    // Start is called before the first frame update
    //protected void Awake()
    //{
    //    timeThreasholdVariable.Variable.TimeThreashold = 10f;
    //}

    protected void Start()
    {
        /*
        transformDataList = new List<PositionalTimeData>();
        thisTransform = gameObject.transform;

        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
        _rewindInput.rewindObjects.Add(this);
        _rewindInput.StepForward += StepForward;
        _rewindInput.StepBack += StepBack;

        _rewindInput.Reset += ResetTimeline;

        */
       
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        /*
        if (_rewindInput.isTravelling == false) 
        {
            RecordPast();
            
        }
        */
    }


    [Obsolete("Deprecated", false)]
    public void RecordPast() 
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (transformDataList.Count > _rewindInput.rewindTime) 
        {
            transformDataList.RemoveAt(transformDataList.Count - 1);
        }
       
        transformDataList.Insert(0, new PositionalTimeData(thisTransform.position, thisTransform.rotation));
    }

    [Obsolete("Deprecated", false)]
    public void ResetTimeline() 
    {
        for (int i = currentIndex; i >= 0; i--) 
        {
            if (currentIndex <= transformDataList.Count - 1)
            {
                transformDataList.RemoveAt(i);
            }
        }
        currentIndex = 0;
        transformDataList.TrimExcess();
    }

    [Obsolete("Deprecated", false)]
    public virtual void StepBack() 
    {

        if (transformDataList.Count > 0) 
        {
            if (currentIndex < transformDataList.Count - 1) 
            {
                currentIndex++;
                if (currentIndex >= transformDataList.Count - 1)
                {
                    currentIndex = transformDataList.Count - 1;
                }
                SetPosition();
                
            }
            //Debug.Log("StepBack");
        }
    }

    [Obsolete("Deprecated", false)]
    public virtual void StepForward()
    {
        if (transformDataList.Count > 0)
        {
            if (currentIndex > 0)
            {
                SetPosition();
                currentIndex--;
            }
           // Debug.Log("StepForward");
        }
    }

    [Obsolete("Deprecated", false)]
    public void SetPosition() 
    {
        if (currentIndex <= transformDataList.Count - 1)
        {
            thisTransform.position = transformDataList[currentIndex].position;
            thisTransform.rotation = transformDataList[currentIndex].rotation;
        }

    }

    [Obsolete("Deprecated", false)]
    public virtual void ApplyData()
    {

    }
    protected void OnDestroy()
    {
        /*
        _rewindInput.Reset -= ResetTimeline; 
        _rewindInput.OnEndRewind -= ApplyData;
        _rewindInput.StepForward -= StepForward;
        _rewindInput.StepBack -= StepBack;
        */
    }
}
