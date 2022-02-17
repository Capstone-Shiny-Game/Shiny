using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContainer : MonoBehaviour
{
    public MenuType menuType;
    public void DisableSelf() {
        this.gameObject.SetActive(false);
    }
    public void AfterEnableSetup(MenuType previousMenuType) {
        
    }
}
