using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadCube : MonoBehaviour
{
    // lesen des Würfels
    // Die Variablen sind ein Transform, ein Objekt in Unity, das verschoben und skaliert werden kann, um seine Größe zu ändern.
    // die Strahltransformationen werden für jede der sechs Seiten des Würfels festgelegt

    // manipulate the position and rotation (nach rechs nach links ...)
    public Transform tUp;
    public Transform tDown;
    public Transform tLeft;
    public Transform tRight;
    public Transform tFront;
    public Transform tBack;

    // Die Variablen haben  private Felder, da es sich um Listen mit GameObjects handelt.
    // Sie werden als Liste deklariert und gleicheitig als Klasse, die verwendet wird, um den Zustand eines Würfels darzustellen.
    private List<GameObject> frontRays = new List<GameObject>();
    private List<GameObject> backRays = new List<GameObject>();
    private List<GameObject> upRays = new List<GameObject>();
    private List<GameObject> downRays = new List<GameObject>();
    private List<GameObject> leftRays = new List<GameObject>();
    private List<GameObject> rightRays = new List<GameObject>();

    // diese LayerMask ist nur für die Flächen des Würfels gedacht
    private int layerMask = 1 << 8;


    // Cube zustand erstellen
    // Die Variable cubeState ist vom Typ CubeState, der verwendet wird, um Informationen über den Status der Cubes auf dem Bildschirm zu speichern.
    CubeState cubeState;

    // reference to cubemap
    CubeMap   cubeMap;

    // Die Variable emptyGO ist ein GameObject, das verwendet wird, um das Objekt zu speichern, das in dieser Klasse erstellt wurde.
    public GameObject emptyGO;
       
    // Start is called before the first frame update
    // In der Start-Klasse werden alle Methoden aufgerufen bzw. eine Verknüpfung erstellen uwischen cubeState-script und cubemap-script
    void Start()
    {

        // Diese Funktion richtet alle Transformationen für jeden Strahl ein, um sicherzustellen, dass sie richtig eingerichtet sind
        SetRayTransforms();

        // sucht den Zustand des Würfels und wird somit mit der Mappe synchronisiert
        // weist sie auch cubeState zu
        cubeState = FindObjectOfType<CubeState>();

        // Instanz von CubeMap und weist sie CubeMap zu
        cubeMap = FindObjectOfType<CubeMap>();

        // ReadState() wird aufgerufen, um alle Daten von beiden Objekten in den Speicher liest, damit sie später bei Update() verarbeitet werden können
        ReadState();
        CubeState.started = true;
       

    }

    // Update() aktualisiert einfach, was auf dem Bildschirm passiert, während die Zeit vergeht, während wir unser Spiel spielen
    void Update() {
    }

    public void ReadState() {

        // // Das cubeState-Objekt wird verwendet, um den aktuellen Zustand der Strahltransformationen zu speichern.
        cubeState = FindObjectOfType<CubeState>();
        cubeMap = FindObjectOfType<CubeMap>();

        
        // Dies sind temporäre Variablen, die Werte von ReadFace()-Funktionen enthalten
        // setze den Zustand jeder Position in der Liste der Seiten, damit wir es wissen welche Farbe ist an welcher Position
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        // aktualisiere die Karte mit den gefundenen Positionen
        cubeMap.Set();

    }

    /* Diese Funktion kümmert sich um die Einrichtung aller Strahltransformationen 
     * für jede Fläche auf der Würfelkarte unter Verwendung der zuvor in dieser Funktion erstellten temporären Variablen.
     */
    void SetRayTransforms()
    {
        
        // Ich baue 6 grids mit 9 Rennstartpunkten, die jeweils auf den Würfel zeigen, und speichere sie in ihrer jeweiligen Strahlenliste
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));
    }

    // es wird sozusagen ein Index für jeden kleinen Quadrat erstellt. 9 Indexe für eine Seite
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        // Die Anzahl der Strahlen wird verwendet, um die Strahlen zu benennen, damit wir sicher sein können, dass sie in der richtigen Reihenfolge sind
        // die Anzahl der erstellten Strahlen zu verfolgen
        int rayCount = 0;

        // es wird eine Liste von GameObjects erstellt, die Strahlen sind 
        List<GameObject> rays = new List<GameObject>();


        // Dies erzeugt 9 Strahlen in Form der Würfelseite mit
        // Ray 0 oben links und Ray 8 unten rechts:
        //  |0|1|2|
        //  |3|4|5|
        //  |6|7|8|

        // Für jede Iteration erstellt der Code mit BuildRays() ein neues GameObject und weist es der aktuellen Position auf der Achse der Ray-Transformation zu
        // durchläuft dann die x- und y-Achse und fügt jeden Strahl in der Reihenfolge von links nach rechts zur Liste hinzu
        for (int y = 1; y > -2; y--)
        {

            for (int x = -1; x < 2; x++)
            {
                // Startposition von wo wir anfangen
                Vector3 startPos = new Vector3( rayTransform.localPosition.x + x,
                                                rayTransform.localPosition.y + y,
                                                rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);

                // wir wissen, welcher Strahl welcher ist und stellen sicher,
                // dass die Rotation der Strahlen nach oben mit einer Fläche in der Würfelkarte repräsentiert
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);

                // Dies wird verwendet, um die Anzahl der Strahlen zu verfolgen, die gebaut werden
                rayCount++;
            }
        }

        // Um einen neuen Strahl zu erzeugen, müssen wir dann ein leeres Spielobjekt mit dieser Position instanziieren und seine Drehung auf die aktuelle Richtung einstellen
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;

    }
    // es wird die ganze Seite des Würfels gelesen
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        // RayCast Logik
        // eine Liste zu sehen ob wir die Seite getroffen haben
        List<GameObject> facesHit = new List<GameObject>();

        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 ray = rayStart.transform.position;
            RaycastHit hit;

            // Durchschneidet der Strahl irgendwelche Objekte in der LayerMask?
            // ein Check ob RayCast funktioniert und wird in Console gelb ausgegeben wenn es richtig ist
            if (Physics.Raycast(ray, rayTransform.forward, out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(ray, rayTransform.forward * hit.distance, Color.yellow);
                facesHit.Add(hit.collider.gameObject);
                //print(hit.collider.gameObject.name);
            }
            // wenn raycasts nicht die Seiten des Würfel lesen kann, dann wird in die Console Debug grün ausgegeben
            else
            {
                Debug.DrawRay(ray, rayTransform.forward * 1000, Color.green);
            }
        }
        return facesHit;
    }

}
