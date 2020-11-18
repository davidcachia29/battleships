using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBoxController : MonoBehaviour
{
    bool highlighted = false;
    Color currentColor;
    public int indexX, indexY;

    private void Start()
    {
        currentColor = GetComponent<SpriteRenderer>().color;
        
        
    }




    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //horizontal
            Debug.Log("Horizontal"+indexX + " " + indexY);
            flipColor();
        }
        if (Input.GetMouseButtonDown(1))
        {
            //horizontal
            Debug.Log("Vertical" + indexX + " " + indexY);
            flipColor();
        }


    }

    void flipColor()
    {
        // Destroy the gameObject after clicking on it
        highlighted = !highlighted;



        //  Debug.Log(highlighted);

       

        if(highlighted)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }else
        {
            GetComponent<SpriteRenderer>().color = currentColor;
        }
    }
}
