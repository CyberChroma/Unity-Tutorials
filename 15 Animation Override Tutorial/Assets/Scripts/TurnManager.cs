using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public float timePerTurn = 0.3f;

    private bool turnIsEven = false;
    private PlayerMove playerMove;
    private List<EnemyMove> enemies = new List<EnemyMove>();
    private List<LargeEnemyMove> largeEnemies = new List<LargeEnemyMove>();
    private List<BoulderMove> boulders = new List<BoulderMove>();
    private List<ArrowSpin> spinArrows = new List<ArrowSpin>();
    private List<FallingDebris> fallingDebris = new List<FallingDebris>();

    // Start is called before the first frame update
    void Start()
    {
        playerMove = FindAnyObjectByType<PlayerMove>();
        StartCoroutine(WaitForTurn());
    }

    IEnumerator WaitForTurn() {
        while (true) {
            yield return new WaitForSeconds(timePerTurn);
            if (turnIsEven) {
                foreach (ArrowSpin spinArrow in spinArrows) {
                    spinArrow.Turn();
                }
            }
            foreach (FallingDebris fallingSingleDebris in fallingDebris) {
                fallingSingleDebris.Fall();
            }
            playerMove.Move();
            foreach (EnemyMove enemy in enemies) {
                enemy.Move();
            }
            foreach (LargeEnemyMove largeEnemy in largeEnemies) {
                largeEnemy.Move();
            }
            foreach (BoulderMove boulder in boulders) {
                boulder.Move();
            }
            turnIsEven = !turnIsEven;
        }
    }

    public void AddEnemy(EnemyMove newEnemy) {
        if (enemies.Contains(newEnemy)) {
            return;
        }
        enemies.Add(newEnemy);
    }

    public void RemoveEnemy(EnemyMove enemy) {
        if (!enemies.Contains(enemy)) {
            return;
        }
        enemies.Remove(enemy);
    }

    public void AddLargeEnemy(LargeEnemyMove newLargeEnemy) {
        if (largeEnemies.Contains(newLargeEnemy)) {
            return;
        }
        largeEnemies.Add(newLargeEnemy);
    }

    public void RemoveLargeEnemy(LargeEnemyMove largeEnemy) {
        if (!largeEnemies.Contains(largeEnemy)) {
            return;
        }
        largeEnemies.Remove(largeEnemy);
    }

    public void AddBoulder(BoulderMove newBoulder) {
        if (boulders.Contains(newBoulder)) {
            return;
        }
        boulders.Add(newBoulder);
    }

    public void RemoveBoulder(BoulderMove boulder) {
        if (!boulders.Contains(boulder)) {
            return;
        }
        boulders.Remove(boulder);
    }

    public void AddArrow(ArrowSpin newSpinArrow) {
        if (spinArrows.Contains(newSpinArrow)) {
            return;
        }
        spinArrows.Add(newSpinArrow);
    }

    public void AddDebris(FallingDebris newFallingDebris) {
        if (fallingDebris.Contains(newFallingDebris)) {
            return;
        }
        fallingDebris.Add(newFallingDebris);
    }

    public void StopTurns() {
        StopAllCoroutines();
        StartCoroutine(FinalTurn());
    }

    IEnumerator FinalTurn() {
        yield return new WaitForSeconds(timePerTurn);
        playerMove.FinalTurn();
        foreach (EnemyMove enemy in enemies) {
            enemy.FinalTurn();
        }
        foreach (LargeEnemyMove largeEnemy in largeEnemies) {
            largeEnemy.FinalTurn();
        }
        foreach (BoulderMove boulder in boulders) {
            boulder.FinalTurn();
        }
    }
}
