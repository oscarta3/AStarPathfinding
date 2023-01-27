using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public int size;
    public GridGenerator _gridGenerator;
    public bool colorUpdate = true;

    void Update()
    {
        if (colorUpdate)
        {
            if (size == 1)
            {
                GetComponent<Renderer>().material.color = Color.black;
            }
            else if (size == 2)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }
            else if (size == 3)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
            }
        }

    }

    void OnMouseDown()
    {
        if (colorUpdate)
        {
            if (size > 3)
            {
                size = 0;
            }
            size++;

            _gridGenerator.UpdateGrid();
        }
    }
}
