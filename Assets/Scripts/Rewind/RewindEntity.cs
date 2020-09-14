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
        transformDataList = new List<PositionalTimeData>();
        thisTransform = gameObject.transform;

        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        _rewindInput.rewindObjects.Add(this);
        _rewindInput.StepForward += StepForward;
        _rewindInput.StepBack += StepBack;

        _rewindInput.Reset += ResetTimeline;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if (isTravelling == false) 
        {
            RecordPast();
            
        }
        Debug.Log(_rewindInput.rewindTime);
    }

    public void RecordPast() 
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (transformDataList.Count > _rewindInput.rewindTime) 
        {
            transformDataList.RemoveAt(transformDataList.Count - 1);
        }
       
        transformDataList.Insert(0, new PositionalTimeData(thisTransform.position, thisTransform.rotation));
    }

    public void ResetTimeline() 
    {
        for (int i = currentIndex; i >= 0; i--) 
        {
            transformDataList.RemoveAt(i);
        }
        currentIndex = 0;
        transformDataList.TrimExcess();
    }

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

    public void SetPosition() 
    {
        thisTransform.position = transformDataList[currentIndex].position;
        thisTransform.rotation = transformDataList[currentIndex].rotation;


    }

    public virtual void ApplyData()
    {

    }

}
