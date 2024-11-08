using System.Collections;
using UnityEngine;

public class IncomeManager : MonoBehaviour
{
    private bool m_generateIncome = true;
    private RessourcesManager m_ressourcesManager;

    // Start is called before the first frame update
    void Start()
    {
        m_ressourcesManager = RessourcesManager.Instance;
        StartCoroutine(GenerateIncome());
        m_ressourcesManager.AddRessources<Gold>(10000);
    }

    private IEnumerator GenerateIncome()
    {
        while(m_generateIncome)
        {
            yield return new WaitForSeconds(1.0f);
            GenerateGoldIncome();
            GenerateRessourceIncome<Energy>();
            GenerateRessourceIncome<Water>();
        }
         
    }

    private void GenerateGoldIncome()
    {
        float percentage = Mathf.Clamp(m_ressourcesManager.GetRessources<Impot>().quantity / 100f, 0.0001f, 1.0f);
        int goldToAdd = Mathf.CeilToInt(10 * m_ressourcesManager.GetRessources<People>().UpdatePeopleNumber() * percentage);
        m_ressourcesManager.AddRessources<Gold>(goldToAdd);
    }

    private void GenerateRessourceIncome<T>() where T : AutoConsume
    {
        int income = m_ressourcesManager.GetRessources<T>().UpdateProduction();
        Debug.Log("Income :: "+ income); 
        if (income == 0)
            return;
        if (income < 0)
        {
            Debug.Log("Ressources en energie insuffisante");
            return;
        }            
        m_ressourcesManager.AddRessources<Gold>(income);
            
    }

}
