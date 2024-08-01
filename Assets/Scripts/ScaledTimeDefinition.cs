using System.Collections.Generic;
using UnityEngine;

public class PlayTime
{
    public static float Scale { get { return Time.timeScale; } }

    public static readonly TimeScaledNull ScaledNull = new();

    private static Dictionary<float, WaitForSeconds> _WaitForSeconds = new();

    public static WaitForSeconds ScaledWaitForSeconds(float seconds)
    {
        if (!_WaitForSeconds.TryGetValue(seconds, out var waitForSeconds))
        {
            waitForSeconds = new WaitForSeconds(seconds);
            _WaitForSeconds.Add(seconds, waitForSeconds);
        }

        return waitForSeconds;
    }

}

public class TimeScaledNull : CustomYieldInstruction
{
    public override bool keepWaiting
    {
        get
        {
            return Time.timeScale == 0;
        }
    }
}