using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    private TurnManager turnManager;
    private PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = GetComponent<TurnManager>();
        playerMove = FindFirstObjectByType<PlayerMove>();
    }

    public void StartRespawnProcess() {
        StopAllCoroutines();
        StartCoroutine(WaitToRespawn());
    }

    IEnumerator WaitToRespawn() {
        yield return null;
        turnManager.StopTurns();
        playerMove.canMove = false;
        yield return new WaitForSeconds(turnManager.timePerTurn - 0.1f);
        playerMove.StartShrink();
        yield return new WaitForSeconds(turnManager.timePerTurn);
        GridManager.ResetGrid();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
