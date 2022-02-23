using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ChangeMenuButton : MonoBehaviour
{
    public MenuType MenuToGoTo;
    private Button MenuButton;
    private void OnEnable()
    {
        MenuButton = GetComponent<Button>();
        MenuButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked() {
        Debug.Log("HI");
        MenuManager.instance.SwitchMenu(MenuToGoTo);
    }
}
