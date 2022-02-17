using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuContainer : MonoBehaviour
{
    public MenuType menuType;
    public virtual void DisableSelf() {
        this.gameObject.SetActive(false);
    }
    public virtual void AfterEnableSetup(MenuType currentMenuType, string menuSetting) {
        
    }
}
