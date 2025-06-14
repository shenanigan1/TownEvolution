using System.Collections;
using UnityEngine;

/// <summary>
/// Manages periodic logging or handling of the percentage of disabled buildings.
/// </summary>
public class HappinessManager : MonoBehaviour
{
    private const float LogIntervalSeconds = 10f;

    private void Start()
    {
        // Start the coroutine that periodically checks and logs disabled buildings percentage
        StartCoroutine(LogDisabledBuildingsPercentageRoutine());
    }

    /// <summary>
    /// Coroutine that runs indefinitely, logging the percentage of disabled buildings every 10 seconds.
    /// </summary>
    private IEnumerator LogDisabledBuildingsPercentageRoutine()
    {
        while (true)
        {
            float disabledPercentage = GridManager.Instance.GetPercentageOfBuildingDisabled();
            Debug.Log($"Percentage of disabled buildings: {disabledPercentage * 100f:0.00}%");
            yield return new WaitForSeconds(LogIntervalSeconds);
        }
    }
}
