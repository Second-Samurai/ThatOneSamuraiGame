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
        _rewindInput = GameManager.instance.RewindManager.GetComponent<RewindManager>();
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
            if (currentIndex <= ProjectileDataList.Count - 1)
            {
                ProjectileDataList.RemoveAt(i);
            }
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
        ProjectileDataList.Insert(0, new ProjectileTimeData(projectile.direction, projectile.rb.linearVelocity, projectile.collider.enabled, projectile.hitEnemies));

        base.RecordPast();
    }

    public override void StepBack()
    {

        if (ProjectileDataList.Count > 0)
        {
            if (currentIndex < ProjectileDataList.Count - 1)
            {
                currentIndex++;
                if (currentIndex >= ProjectileDataList.Count - 1)
                {
                    currentIndex = ProjectileDataList.Count - 1;
                }
                SetPosition();
            }
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
        }
    }

    public new void SetPosition()
    {
        if (currentIndex <= ProjectileDataList.Count - 1)
        {
            projectile.direction = ProjectileDataList[currentIndex].direction;
            projectile.arrowModel.SetActive(ProjectileDataList[currentIndex].isActive);
        }
        base.SetPosition();
    }

    public override void ApplyData()
    {
        projectile.rb.linearVelocity = ProjectileDataList[currentIndex].velocity;
        projectile.collider.enabled = ProjectileDataList[currentIndex].isActive;
        projectile.hitEnemies = ProjectileDataList[currentIndex].hitEnemys;

    }
}
