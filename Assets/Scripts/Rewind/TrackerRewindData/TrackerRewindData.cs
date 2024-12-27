using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackerRewindData
{
    public Transform[] trackedCurrentEnemies;
    public Transform[] lockOnEnemies;

    public TrackerRewindData(List<Transform> _trackedCurrentEnemies, List<Transform> _lockOnEnemies)
    {
        trackedCurrentEnemies = _trackedCurrentEnemies.ToArray<Transform>();
        lockOnEnemies = _lockOnEnemies.ToArray<Transform>();
    }
}
