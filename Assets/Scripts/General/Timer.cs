using UnityEngine;

public class Timer
{
    public float timeLength;
    public float startingValue;

    private float remainingTime;

    public void CalculateRemainingTime()
    {
        remainingTime -= Time.deltaTime;
    }

    public float GetRemainingTime()
    {
        return remainingTime;
    }

    public void ResetTimer()
    {
        remainingTime = timeLength;
    }
}
