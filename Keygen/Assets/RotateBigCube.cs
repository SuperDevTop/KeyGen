using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBigCube : MonoBehaviour
{
    /*
     * Die firstPressPos ist der Startpunkt der Drehung und die secondPressPos ist der Punkt, an dem sie nach einer vollen Drehung endet.
     */

    // Ein Vector2 hat eine 2D-Richtung, wie ein xy-Punkt; die Position, an der der Wischvorgang gestartet wurde
    // Dies ist die Position des Fingers des Spielers, wenn er auf dem Bildschirm nach oben wischt.
    private Vector2 firstPressPos;

    // die Position, an der der Wischvorgang beendet wurde
    private Vector2 secondPressPos;

    // der aktuelle Wischvorgang
    // bei currentSwipe berührt er gerade mit dem Finger.
    private Vector2 currentSwipe;

    // Ein Vector3 hat eine 3D-Richtung, wie ein xyz-Punkt in einem 3D-Raum
    // PreviousMousePosition speichert einen Verweis darauf, was zuvor berührt wurde, um festzustellen, wie viel Bewegung seit diesem Zeitpunkt gemacht wurde;
    private Vector3 previousMousePosition;

    // mouseDelta speichert einen Verweis darauf, in welche Richtung sich Ihr Cursor von seinem letzten Berührungspunkt bewegt hat;
    // Geschwindigkeit bestimmt, wie schnell sich Ihr Würfel um seine Achse drehen soll (in diesem Fall sind es 200 Grad pro Sekunde).
    private Vector3 mouseDelta;

    // Geschwindigkeit der Umdrehung des Würfels
    private float speed = 200f;

    // Erstellung eines Objekts um den Würfel zu drehen; 
    // ich stelle alle Pieces des Würfels in Object hinein
    public GameObject target;    


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Die Methode Swipe() und Drag() werden aufgerufen 
        Swipe();
        Drag();
    }

    /*
     * Drag(), die verwendet wird, um den Würfel zu bewegen, ohne irgendwelche Tasten zu drücken.
     * Diese Funktion prüft auf Eingaben von einer der beiden Tasten (der linken oder rechten Maustaste) 
     * und verwendet diese Eingaben, um zu bestimmen, wo als nächstes gewischt werden soll.
     * 
     * Berechnet
     * */
    void Drag()
    {
        if (Input.GetMouseButton(1))
        {
            // Während die Maus gedrückt gehalten wird, kann der Würfel um seine Mittelachse bewegt werden, um visuelles Feedback zu geben
            mouseDelta = Input.mousePosition - previousMousePosition;
            // Reduzierung der Drehzahl
            mouseDelta *= 0.1f; 
            transform.rotation = Quaternion.Euler(mouseDelta.y, -mouseDelta.x, 0) * transform.rotation;
        }
        else
        {
            // Wenn nicht, prüft es, ob ein Eingabeereignis vorhanden war oder nicht. In diesem Fall berechnet es, wie weit der Benutzer seine Maus bewegt hat,
            // und wendet dies auf eine Drehung des Objekts um seinen Mittelpunkt an.
            // es wird automatisch in die Zielposition gedreht
            if (transform.rotation != target.transform.rotation)
            {
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, step);
            }
        }
        previousMousePosition = Input.mousePosition;


    }
    /* Methode Swipe(), um mit rechten Mausclick den Würfel zu drehen in beliebiege Richtung
    // Swipe(), die verwendet wird, um den Würfel zu bewegen
    Die Funktion beginnt damit, dass überprüft wird, 
    ob der Benutzer die Maustaste gedrückt hält, und wenn dies der Fall ist, bewegt sie den Würfel in eine Richtung.

    Die Funktion Swipe() überprüft, ob der Benutzer die Maustaste gedrückt hat, 
    und setzt in diesem Fall firstPressPos auf die Stelle, an der die Maus zuletzt geklickt hat.

    Wenn nicht, wird secondPressPos auf die Stelle gesetzt, an der der Benutzer zuletzt mit der Maustaste geklickt hat.

    Richtung
    */
    void Swipe()
    {
        // mit rechten Mausclick wird der Würfel nach unten gewischt
        if (Input.GetMouseButtonDown(1))
        {
            //  Erstellen der 2D-Position des ersten Mausklicks
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            //print(firstPressPos);
        }
        // mit rechten Mausclick wird der Würfel nach oben gewischt
        if (Input.GetMouseButtonUp(1))
        {
            // Erstellen der 2D-Position des zweiten Mausklicks
            // wenn der Würfel schon umgehdereht wurde, dann muss es wieder als 2D Position sein
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            // Erstellen eines Vektors aus der ersten und zweiten Klickposition
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
            // normieren den 2d-Vektor
            currentSwipe.Normalize();

            // es wird um 90 Grad nach links umgedreht
            if (LeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 90, 0, Space.World);
            }

            // es wird um 90 Grad nach rechts umgedreht
            else if (RightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, -90, 0, Space.World);
            }

            // es wird um 90 Grad von oben nach links umgedreht
            else if (UpLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(90, 0, 0, Space.World);
            }

            // es wird um 90 Grad von oben nach rechts umgedreht
            else if (UpRightSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, -90, Space.World);
            }

            // es wird um 90 Grad von unten nach links umgedreht
            else if (DownLeftSwipe(currentSwipe))
            {
                target.transform.Rotate(0, 0, 90, Space.World);
            }

            // es wird um 90 Grad von unten nach rechts umgedreht
            else if (DownRightSwipe(currentSwipe))
            {
                target.transform.Rotate(-90, 0, 0, Space.World);
            }
        }
    }
    // alle BOOLEANS in die Swipe Methode einfügen um eine Logik zu sein

    /* nach links wischen
    * es ist richtig wenn die x-Achse ist kleiner als 0, denn es geht in die negative Richtung und somit wird nach links gewischt 
    * und nicht viel in die y-Richtung geht
    */
    bool LeftSwipe(Vector2 swipe)
    {
        return currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    /* nach rechts wischen
    * es ist richtig wenn die x-Achse ist groesser als 0, denn es geht in die positive Richtung und somit wird nach rechts gewischt 
    * und nicht viel in die y-Richtung geht
    */
    bool RightSwipe(Vector2 swipe)
    {
        return currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f;
    }

    bool UpLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x < 0f;
    }

    bool UpRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y > 0 && currentSwipe.x > 0f;
    }

    bool DownLeftSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x < 0f;
    }

    bool DownRightSwipe(Vector2 swipe)
    {
        return currentSwipe.y < 0 && currentSwipe.x > 0f;
    }
          
}
