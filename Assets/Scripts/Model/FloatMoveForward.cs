using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatMoveForward : MonoBehaviour
{
    private float startingpoint;
    private float startingz;


    [SerializeField] public float distance;
    [SerializeField] public float speed;

    [SerializeField] public float waitTime = 5f;
    //private float period = .1f;

    [SerializeField] public float sideMoveRange;
    [SerializeField] public float sideMoveSpeed;
    [SerializeField] public bool isfish = false;

  //  private System.Random randomNumGenerator;
    private Material m_Material;   
    private Color m_Color;

    private bool fadein = true;
    private bool fadeout = true;
    private bool reset = true;

   
    
    // Start is called before the first frame update
    void Start()
    {
        startingpoint = gameObject.transform.localPosition.x;
        startingz = gameObject.transform.localPosition.z;
        //randomNumGenerator = new System.Random();
        // Get reference to object's material.
        m_Material = GetComponent<Renderer>().material;

        // Get material's starting color value.
        m_Color = m_Material.color;

       
    }

    // Update is called once per frame
    void Update()
    {


        if (startingpoint + distance > (gameObject.transform.localPosition.x))
        {
           
            if(gameObject.transform.localPosition.x < startingpoint + distance * (.1f))
            {
                
                //fadein
                if (fadein) {
                    fadein = false;
                    StartCoroutine(FadeIn());
                }
                

            }
            if (gameObject.transform.localPosition.x > startingpoint + distance * (.8f))
            {
                //fadeout
                if (fadeout)
                {
                    fadeout = false;
                    StartCoroutine(FadeOut());
                }
            }

            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + (speed * Time.deltaTime), gameObject.transform.localPosition.y, startingz + Side());
        }
        else
        {
           // nextActionTime += period;
           if (reset)
            {
                reset = false;
                StartCoroutine(WaitReset());

            }



        }

    }



    IEnumerator WaitReset()
    {
        yield return new WaitForSeconds(waitTime);

        gameObject.transform.localPosition = new Vector3(startingpoint, gameObject.transform.localPosition.y, startingz);
        fadein = true;
        fadeout = true;
        reset = true;
    }
    public float Side()
    {
        float speed = Mathf.Sin(sideMoveSpeed * (Time.time));
        return sideMoveRange * Mathf.Sin(speed * (Time.time));
    }


    IEnumerator FadeIn()
    {
        float alpha = 0f;
        while (alpha < 1f)
        {
            
            alpha += .5f * Time.deltaTime;
            m_Material.color = new Color(m_Color.r, m_Color.g, m_Color.b, alpha);
            if (isfish)
            {
                gameObject.GetComponent<Collider>().enabled = true;
            }
            yield return null;
        }

    }

    IEnumerator FadeOut()
    {
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= .5f * Time.deltaTime;
            m_Material.color = new Color(m_Color.r, m_Color.g, m_Color.b, alpha);
            if (isfish)
            {
                gameObject.GetComponent<Collider>().enabled = false;
            }
            yield return null;
        }

    }

}