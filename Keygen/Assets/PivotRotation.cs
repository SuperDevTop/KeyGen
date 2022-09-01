using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    // welche seite wird gedreht
    private List<GameObject> activeSide;

    // speichert die aktuelle Position des Mauszeigers im Weltraum.
    private Vector3 localForward;

    // mit maus kann man so oft den Würfel drehen, wie viel man will
    private Vector3 mouseRef;

    // man muss wissen ob der Maus zieht den Würfel oder nicht 
    private bool dragging = false;

    // die Seite soll automatisch sich drehen
    // es ist true wenn wir das sagen
    private bool autoRotating = false;

    // wie oft der Würfel wurde gedreht
    private float sensitivity = 0.4f;

    // Die Geschwindigkeit für die automatsiche Umdrehung 
    private float speed = 300f;
    private Vector3 rotation;

    // für den Zielwinkel, der automatisch gedreht wird
    private Quaternion targetQuaternion;

    // ein Objekt vom Typ ReadCube, das verwendet wird, um den Würfel zu speichern, der herumgezogen wird.
    // wenn die Seite gedreht wird, muss die Karte auch synchronisiert werden 
    private ReadCube readCube;

    // Dann findet es das CubeState-Objekt und setzt seine Referenz auf dieses Objekt und speichert sie im Arbeitsspeicher
    private CubeState cubeState;
       
    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }


    // alle Aktualisierungen übernimmt, die durchgeführt werden müssen, damit das Spiel weiterläuft
    // prüft, ob das Ziehen wahr ist und ob autoRotating ausgeschaltet wurde
    void LateUpdate()
    {
        if (dragging && !autoRotating)
        {

            // Wenn beides zutrifft, dreht die Funktion SpinSide() die aktive Seite des Würfels so, dass sie dem Spieler zugewandt ist
            SpinSide(activeSide);
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                RotateToRightAngle();
            }
        }
        if (autoRotating)
        {
            AutoRotate();
        }
                
    }
    // die Seite, die wir drehen wollen
    private void SpinSide(List<GameObject> side)
    {
        // die Drehung zurücksetzen
        rotation = Vector3.zero;

        // Anfangswert
        // aktuelle Mausposition minus letzte Mausposition
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);        

        // es werden die Seiten bestimmt in welche Richtungen sie drehen sollen
        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * -1;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x + mouseOffset.y) * sensitivity * 1;
        }

        // Danach prüft es auf Eingaben von Ihren Mausbewegungen und aktualisiert seine Position entsprechend
        // rotate
        transform.Rotate(rotation, Space.Self);

        // mouse speichern
        mouseRef = Input.mousePosition;
    }

    // Egal welche Seite wir gedreht haben, diese Methode wird zur aktiven Seite
    public void Rotate(List<GameObject> side)
    {
        // wehche seite wird gedreht, ist die aktive Seite
        activeSide = side;

        // mit maus kann man de nWürfel so oft drehen, wie oft man will
        mouseRef = Input.mousePosition;

        // mit dem Maus kann man den Würfel ziehen 
        dragging = true;

        // Erzeuge einen Vektor um ihn zu drehen
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
    }

    // wir müssen wissen, welche Seite gewählt wird
    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        Vector3 localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;
        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;
        activeSide = side;
        autoRotating = true;
    }

    // es wird genau um 90 Grad automatisch gedreht
    public void RotateToRightAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        // Runde vec auf die nächsten 90 Grad
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
        autoRotating = true;
    }

    // Die Funktion autoRotate wird aufgerufen, wenn der Spieler mit der linken Maustaste klickt
    // wenn man nicht ganz den Würfel ziehen will, dann wird die AutoRotate Methode aufgerufen
    private void AutoRotate()
    {
        dragging = false;
        var step = speed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        // wenn innerhalb eines Grades, Winkel auf Zielwinkel setzen, dann wird es nicht automatsich gedreht
        // 1 erreicht hat oder nicht, bevor Sie mit weiteren Berechnungen fortfahren, da er sich sonst ewig weiter drehen würde
        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            // die kleinen Würfel auflösen
            cubeState.PutDown(activeSide, transform.parent);
            readCube.ReadState();
            CubeState.autoRotating = false;
            autoRotating = false;
            dragging = false;                                                               
        }
    }         
}
