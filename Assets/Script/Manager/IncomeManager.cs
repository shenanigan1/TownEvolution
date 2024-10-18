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
        m_ressourcesManager.AddRessources<Gold>(10 * RessourcesManager.Instance.GetRessources<People>().UpdatePeopleNumber() + 1);
    }

    private void GenerateRessourceIncome<T>() where T : AutoConsume
    {
        int income = m_ressourcesManager.GetRessources<T>().UpdateProduction();
        if (income == 0) { return; }
        if (income > 0)
        {
            m_ressourcesManager.AddRessources<Gold>(income);
        }
        else
            Debug.Log("Ressources en energie insuffisante");
    }

}
