using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        turnManager = GetComponent<TurnManager>();
    }

    public void LoadNextLevel() {
        StartCoroutine(LevelTransition());
    }

    IEnumerator LevelTransition() {
        yield return null;
        turnManager.StopTurns();
        yield return new WaitForSeconds(turnManager.timePerTurn * 2);
        GridManager.ResetGrid();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
