using System.Collections;
using UnityEngine;

/// <summary>
/// Manages periodic generation of income and resource updates.
/// </summary>
public class IncomeManager : MonoBehaviour
{
    private bool m_generateIncome = true;
    private RessourcesManager m_ressourcesManager;

    private void Start()
    {
        // Cache singleton instance reference
        m_ressourcesManager = RessourcesManager.Instance;

        // Initial large gold allocation for testing / start game balance
        m_ressourcesManager.AddRessources<Gold>(10000);

        // Start recurring income generation every second
        StartCoroutine(GenerateIncome());
    }

    /// <summary>
    /// Coroutine that runs indefinitely while generating income each second.
    /// </summary>
    private IEnumerator GenerateIncome()
    {
        while (m_generateIncome)
        {
            yield return new WaitForSeconds(1f);
            GenerateGoldIncome();
            GenerateRessourceIncome<Energy>();
            GenerateRessourceIncome<Water>();
        }
    }

    /// <summary>
    /// Calculates gold income based on population and tax rate (Impot).
    /// </summary>
    private void GenerateGoldIncome()
    {
        // Get tax percentage clamped between 0.0001 and 1.0
        float percentage = Mathf.Clamp(m_ressourcesManager.GetRessources<Impot>().quantity / 100f, 0.0001f, 1f);

        // Calculate gold based on population update and tax percentage
        int goldToAdd = Mathf.CeilToInt(10 * m_ressourcesManager.GetRessources<People>().UpdatePeopleNumber() * percentage);

        m_ressourcesManager.AddRessources<Gold>(goldToAdd);
    }

    /// <summary>
    /// Generates income for generic resource type T which extends AutoConsume.
    /// Adds the resulting income to gold if positive.
    /// </summary>
    /// <typeparam name="T">Type of resource inheriting AutoConsume</typeparam>
    private void GenerateRessourceIncome<T>() where T : AutoConsume
    {
        int income = m_ressourcesManager.GetRessources<T>().UpdateProduction();
        Debug.Log($"Income for {typeof(T).Name} :: {income}");

        if (income == 0)
            return;

        if (income < 0)
        {
            Debug.LogWarning($"Insufficient resources for {typeof(T).Name} income generation.");
            return;
        }

        m_ressourcesManager.AddRessources<Gold>(income);
    }
}
