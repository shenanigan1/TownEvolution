using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinessManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GetBatimentWithDisability());
    }

    private IEnumerator GetBatimentWithDisability()
    {
        while (true)
        {
            Debug.Log("Percentage :: " + GridManager.Instance.GetPercentageOfBatimentDisable());
            yield return new WaitForSeconds(10);
        }
    }
}
