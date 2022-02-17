using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used on the game object that is a parent to all elements of this menu type.
/// </summary>
public class MenuContainer : MonoBehaviour
{
    public MenuType menuType;
    /// <summary>
    /// called to disable this object
    /// </summary>
    public virtual void DisableSelf() {
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// called when object is enabled, override to change behavior
    /// </summary>
    /// <param name="currentMenuType"></param>
    public virtual void AfterEnableSetup(MenuType currentMenuType) {
        
    }
}
