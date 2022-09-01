using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeState : MonoBehaviour
{
    // sides
    // eine Liste von GameObjects, die verwendet werden, um die Vorderseite, Rückseite, oben, unten, links und rechts des Würfels darzustellen.
    // es wird Listen verwendet, um die Würfel so zu speichern, dass sie von anderen Teilen des Spiels leicht zugänglich sind
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    // Automate Script
    public static bool autoRotating = false;
    public static bool started = false;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }


    // die PickUp-Funktion beim Ausführen transform.parent auf jeder Seite der Reihe nach aufruft und prüft,
    // ob sie nicht gleich cubeSide[4] ist, weil der mittlere Teil kann man nicht umdrehen
    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            // Hängt das Elternteil jedes Gesichts an (den kleinen Würfel) zum Elternteil des 4. Index (der kleine Würfel in der Mitte)
            // Es sei denn, es ist bereits der 4. Index
            // Die erste Bedingung prüft, auf welcher Seite des Objekts Sie sich gerade befinden 
            if (face != cubeSide[4])
            {

                // Wenn dies der Fall ist, wird transform.parent auf dieser Fläche so eingestellt, dass es gleich cubeSide ist
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }
        }
    }    

    // wenn die Seite umgedreht ist, alle kleine Würfel werden wieder Childs vom mittleren Würfel sein (auf vorherigem Zustand)

    public void PutDown(List<GameObject> littleCubes, Transform pivot)
    {
        foreach (GameObject littleCube in littleCubes)
        {
            if (littleCube != littleCubes[4])
            {
                littleCube.transform.parent.transform.parent = pivot;
            }
        }
    }

    // faces to strings
    string GetSideString(List<GameObject> side)
    {
        string sideString = "";
        foreach (GameObject face in side)
        {
            // statt UP, DOWN ... , wird U, D ausgegeben
            sideString += face.name[0].ToString();
        }
        return sideString;
    }

   
    // am ende von einer Liste machen wir Strings, da wir nicht mehr 1,2,3 brauchen aber UP,...
    public string GetStateString()
    {
        string stateString = "";
        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);
        return stateString;
    }
}
