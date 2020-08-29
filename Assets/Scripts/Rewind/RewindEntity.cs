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

    RewindInput _rewindInput;
   

    // Start is called before the first frame update
    protected void Start()
    {
        transformDataList = new List<PositionalTimeData>();
        thisTransform = gameObject.transform;

        _rewindInput = gameObject.GetComponent<RewindInput>();
        _rewindInput.StepForward += StepForward;
        _rewindInput.StepBack += StepBack;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        if (isTravelling == false) 
        {
            RecordPast();
            
        }
    }

    public void RecordPast() 
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (transformDataList.Count > Mathf.Round(10f * (1f / Time.fixedDeltaTime))) 
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
    }

    public virtual void StepBack() 
    {

        if (transformDataList.Count > 0) 
        {
            SetPosition();
            if (currentIndex < transformDataList.Count - 1) 
            {
                currentIndex++;
            }
            //Debug.Log("StepBack");
        }
    }

    public virtual void StepForward()
    {
        if (transformDataList.Count > 0)
        {
            SetPosition();
            if (currentIndex > 0)
            {
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


}
