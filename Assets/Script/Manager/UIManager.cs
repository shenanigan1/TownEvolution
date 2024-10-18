using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public UnityEvent<int> GoldHaveChange = new UnityEvent<int>();
    public UnityEvent<int> PeopleHaveChange = new UnityEvent<int>();
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI peopleText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        GoldHaveChange.AddListener(SetGoldText);
        PeopleHaveChange.AddListener(SetPeopleText);
    }

    private void SetGoldText(int amount)
    {
        goldText.text = "Gold : " + amount; 
    }

    private void SetPeopleText(int amount)
    {
        peopleText.text = "People : " + amount;
    }


}
