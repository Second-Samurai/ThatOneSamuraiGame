using UnityEngine;

public class HideTutorialsOnTrigger : MonoBehaviour
{
    public GameEvent[] gameEvents;
    private bool bHasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !bHasTriggered)
        {
            bHasTriggered = true;
            foreach (var gameEvent in gameEvents)
            {
                gameEvent.Raise();
            }
        }
    }
}
