using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileRewindEntity : RewindEntity
{
    public List<ProjectileTimeData> ProjectileDataList;
    public Rigidbody gameObjectRigidbody;

    public Projectile projectile;


    protected new void Start()
    {
        _rewindInput = GameManager.instance.rewindManager.GetComponent<RewindManager>();
        ProjectileDataList = new List<ProjectileTimeData>();

        projectile = gameObject.GetComponent<Projectile>();

        _rewindInput.Reset += ResetTimeline;
        _rewindInput.OnStartRewind += DisableEvents;
        _rewindInput.OnEndRewind += EnableEvents;

        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();


        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (_rewindInput.isTravelling == false)
        {
            RecordPast();

            // animator.enabled = true;
        }
        else
        {


        }


    }

    public void DisableEvents()
    {
        gameObjectRigidbody.isKinematic = true;

        //base.DisableEvents();

    }

    public void EnableEvents()
    {
        gameObjectRigidbody.isKinematic = false;

        //base.EnableEvents();

    }

    public new void ResetTimeline()
    {
        for (int i = currentIndex; i >= 0; i--)
        {
            ProjectileDataList.RemoveAt(i);
        }
        ProjectileDataList.TrimExcess();
    }

    public new void RecordPast()
    {
        //how much data is cached before list starts being culled (currently 10 seconds)
        if (ProjectileDataList.Count > _rewindInput.rewindTime)
        {
            ProjectileDataList.RemoveAt(ProjectileDataList.Count - 1);
        }

        //move to animation rewind entity
        ProjectileDataList.Insert(0, new ProjectileTimeData(projectile.direction, projectile.rb));

        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).normalizedTime + "   :   " + m_CurrentClipInfo[0].clip.name);

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (ProjectileDataList.Count > 0)
        {
            if (currentIndex < ProjectileDataList.Count - 1)
            {
                SetPosition();
                currentIndex++;
            }
            //Debug.LogWarning("animStepBack");
        }
    }

    public override void StepForward()
    {
        if (ProjectileDataList.Count > 0)
        {
            if (currentIndex > 0)
            {
                SetPosition();
                currentIndex--;
            }
            // Debug.LogWarning("animStepForward");
        }
    }

    public new void SetPosition()
    {
        projectile.direction = ProjectileDataList[currentIndex].direction;
        projectile.rb = ProjectileDataList[currentIndex].rb;
        base.SetPosition();
    }
}
