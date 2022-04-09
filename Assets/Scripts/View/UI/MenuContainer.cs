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
    public virtual MenuType DisableSelf(MenuType nextMenuType) {
        this.gameObject.SetActive(false);
        return nextMenuType;
    }
    /// <summary>
    /// called when object is enabled, override to change behavior
    /// </summary>
    /// <param name="currentMenuType"></param>
    public virtual void AfterEnableSetup(MenuType currentMenuType) {

    }
}
