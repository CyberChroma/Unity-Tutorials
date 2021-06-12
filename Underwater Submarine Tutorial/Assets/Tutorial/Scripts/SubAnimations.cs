using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubAnimations : MonoBehaviour
{
    public float turbineSpinSpeed; // The speed the turbine spins at max speed
    public float turbineSmoothing; // The smoothing applied to the changing of the turbine speed
    private float lastTurbineTurn; // Stores the speed of the turbine from the last frame

    public float finTurnAmount; // The amount the fins turn
    public float finSmoothing; // The smoothing applied to the turning of the fins
    private float lastFinTurn; // Stores the rotation of the fins from the last frame

    public float drillTurnAmount; // The amount the drill turns
    public float drillSmoothing; // The smoothing applied to the turning of the drill
    private float lastDrillTurn; // Stores the rotation of the drill from the last frame

    public Transform turbine; // Reference to the turbine
    public Transform fins; // Reference to the fins
    public Transform drill; // Reference to the drill

    public void Spin (float dir)
    {
        float curTurn = Mathf.MoveTowards(lastTurbineTurn, turbineSpinSpeed * dir, turbineSmoothing * Time.deltaTime); // Calculate how much to turn the turbine
        turbine.Rotate(new Vector3(0, 0, curTurn)); // Spin the turbine
        lastTurbineTurn = curTurn; // Store the speed of the turbine to be used next frame
    }

    public void FinTurn (float dir)
    {
        float curTurn = Mathf.Lerp(lastFinTurn, finTurnAmount * dir, finSmoothing * Time.deltaTime); // Calculate how much to turn the fins
        fins.localRotation = Quaternion.Euler(new Vector3(-90 + curTurn, 0, 0)); // Turn the fins
        lastFinTurn = curTurn; // Store the rotation of the fins to be used next frame
    }

    public void DrillTurn (float dir)
    {
        float curTurn = Mathf.Lerp(lastDrillTurn, drillTurnAmount * dir, drillSmoothing * Time.deltaTime); // Calculate how much to turn the drill
        drill.localRotation = Quaternion.Euler(new Vector3(-90, curTurn, 0)); // Turns the drill
        lastDrillTurn = curTurn; // Store the rotation of the drill to be used next frame
    }
}
