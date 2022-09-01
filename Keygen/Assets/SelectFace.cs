using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFace : MonoBehaviour
{
    // Membervariable erstellen um Zugriff auf CubeState und ReadCube zu haben
    private CubeState cubeState;
    private ReadCube readCube;

    // diese LayerMask ist nur für die Flächen des Würfels gedacht
    private int layerMask = 1 << 8;


    // Start is called before the first frame update
    // In der Start-Klasse werden alle Methoden aufgerufen bzw. eine Verknüpfung erstellen uwischen readState-script und cubeState-script
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Die Update-Funktion prüft zuerst, ob Input.GetMouseButtonDown(0) && !CubeState.autoRotating ist, was bedeutet,
    // dass der Spieler auf seine Maustaste geklickt hat und kein Autorotationsmodus aktiv ist 
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating)
        {
            // den aktuellen Zustand des Würfels lesen           
            readCube.ReadState();

            // Raycast von der Maus in Richtung des Würfels, um zu sehen, ob ein Gesicht getroffen wird
            // wenn wir mit dem Maus auf einer Fläche drücken, soll die Raycasts das erkennen und die richtige Fläche drehen wenn wir das wollen
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                GameObject face = hit.collider.gameObject;

                // Erstellen einer Liste aller Seiten (Listen von Gesichts-GameObjects)
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back
                };
                // Wenn der Gesichtstreffer innerhalb einer Seite existiert
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    if (cubeSide.Contains(face))
                    {
                        // es wird abgeholt
                        cubeState.PickUp(cubeSide);
                        // Starte die Seitenrotationslogik
                        // an das Elternteil jeder Fläche anhängen (der kleine Würfel) 
                        // an das Elternteil des 4. Index (der kleine Würfel in der Mitte)
                        cubeSide[4].transform.parent.GetComponent<PivotRotation>().Rotate(cubeSide);
                    }
                }
            }
        }
    }
}
