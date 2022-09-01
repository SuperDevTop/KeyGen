using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeMap : MonoBehaviour
{

    /*
     * Es durchläuft zuerst alle Flächen des Würfels und aktualisiert dann die Position jeder Fläche, indem er alle damit verbundenen Transformationen durchläuft
     */
    private CubeState cubeState;

    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Diese Methode ist um die Farben mir der Karte zu aktualisieren 
    public void Set()
    {

        // Der Code durchläuft die Liste der Gesichter und Transformationen und aktualisiert jedes einzelne
        cubeState = FindObjectOfType<CubeState>();


        // Das Seite wird aktualisiert, indem UpdateMap darauf mit einem Transform aufgerufen wird, das die zu aktualisierende Seite enthält 
        UpdateMap(cubeState.front, front);
        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);
    }

    /*
     * Diese Methode durchläuft jede der untergeordneten Innenseiten der Würfelkarte
     * Das heißt es wird verwendet das erste Zeichen des Namens des Gesichts z.B U für Up am gleichen Index,
     * um die Farbe der Würfelkarte zu aktualisieren
     * 
     */

    // jede Farbe des Würfels wird in die Würfelkarte eingefügt
    void UpdateMap(List<GameObject> face, Transform side)
    {
        int i = 0;
        foreach (Transform map in side)
        {
            // Front ist orange
            if (face[i].name[0] == 'F')
            {   
                map.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1);
            }
            // Bottom ist rot
            if (face[i].name[0] == 'B')
            {
                map.GetComponent<Image>().color = Color.red;
            }
            // Up ist gelb
            if (face[i].name[0] == 'U')
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            // Down ist weiss
            if (face[i].name[0] == 'D')
            {
                map.GetComponent<Image>().color = Color.white;
            }
            // Left ist grün
            if (face[i].name[0] == 'L')
            {
                map.GetComponent<Image>().color = Color.green;
            }
            // Right ist blau
            if (face[i].name[0] == 'R')
            {
                map.GetComponent<Image>().color = Color.blue;
            }
            i++;
        }               
    }
}
