using UnityEngine;

public class ChangeMenu : MonoBehaviour
{
    [SerializeField] private GameObject NextMenu;

    public void DoChangeMenu() 
    {
        NextMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
