using UnityEngine;
using UnityEngine.UI;

public class BillboardFX : MonoBehaviour
{




    // HERE BE DRAGONS
    private GameObject screenSpaceButton;




    public float VanishDistance = 100;
    void Update()
    {
        if(Vector3.Distance(transform.position, Camera.main.transform.position) < VanishDistance)
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.back, Camera.main.transform.rotation * -Vector3.down);




            // HERE BE DRAGONS
            if (gameObject.name.Contains("Interact") && gameObject.transform.parent.gameObject.name.Contains("Tulip"))
            {
                if (screenSpaceButton != null)
                {
                    Destroy(screenSpaceButton);
                }


                GameObject canvas = GameObject.Find("CanvasControls");
                Vector2 local = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
                local *= canvas.GetComponent<CanvasScaler>().scaleFactor;
                screenSpaceButton = Instantiate(gameObject, local, Quaternion.identity, canvas.transform);
                    screenSpaceButton.transform.SetParent(canvas.transform, false);
                    screenSpaceButton.transform.position = local;
                    screenSpaceButton.transform.localScale = Vector3.one * 0.1f;
                    screenSpaceButton.transform.localRotation = Quaternion.identity;
            }




        }
        else
        {
            gameObject.GetComponent<Canvas>().enabled = false;




            // HERE BE DRAGONS
            if (screenSpaceButton != null)
            {
                Destroy(screenSpaceButton);
                screenSpaceButton = null;
            }




        }

    }
}