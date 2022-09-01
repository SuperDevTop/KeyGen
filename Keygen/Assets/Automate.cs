using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    public static List<string> moveList = new List<string>() { };

    // Liste namens "allMoves", die alle möglichen Züge enthält, die in diesem Spiel gemacht werden können.
    // es werden die Seiten bestimmt die automatisch gedreht werden 
    // U-normal nach Uhrzeiger drehen 90 Grad; U2-180 Grad; U' - Bewegungen gegen den Uhrzeigersinn
    private readonly List<string> allMoves = new List<string>()
        { "U", "D", "L", "R", "F", "B",
          "U2", "D2", "L2", "R2", "F2", "B2",
          "U'", "D'", "L'", "R'", "F'", "B'" 
        };

    // speichert Informationen darüber, in welchem Zustand sich der aktuelle Würfel des Spielers befindet
    private CubeState cubeState;

    // zum Speichern von Informationen darüber, was es vom Bildschirm liest
    private ReadCube readCube;

    // Es beginnt mit dem Lesen des Cube-Zustands
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Die Update-Funktion beginnt mit der Suche nach dem CubeState-Objekt, um seine Positions- und Rotationswerte zu erhalten.
    void Update()
    {
        // wenn eine Bewegung gemacht wird, dann 
        if (moveList.Count > 0  && !CubeState.autoRotating && CubeState.started)
        {
            // Mach den Zug am ersten Index aus;
            DoMove(moveList[0]);

            // den Zug am ersten Index entfernen
            moveList.Remove(moveList[0]);
        }
    }

    // Es erstellt eine Liste von Zügen und mischt sie
    public void Shuffle()
    {
        // es wird eine Random Liste erstellt
        // Die Länge dieses Mischens wird zufällig zwischen 10 und 30 bestimmt
        List<string> moves = new List<string>();
        int shuffleLength = Random.Range(10, 30);
        for (int i = 0; i < shuffleLength; i++)
        {
            int randomMove = Random.Range(0, allMoves.Count);
            moves.Add(allMoves[randomMove]);
        }
        moveList = moves;
    }

    // das basiert auf der bewegung, die wir machen
    void DoMove(string move)
    {
        // Zustand vom Würfel
        readCube.ReadState();
        CubeState.autoRotating = true;
        if (move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        if (move == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        if (move == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (move == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        if (move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        if (move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
        if (move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        if (move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
    }

    // es dreht das Seitenobjekt um einen bestimmten Winkel
    void RotateSide(List<GameObject> side, float angle)
    {
        // es ruft zuerst die Pivot-Rotationskomponente des Seitenobjekts ab und beginnt dann mit der automatischen Drehung.
        // die Seite automatisch um den Winkel drehen
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.StartAutoRotate(side, angle);        
    }

}
