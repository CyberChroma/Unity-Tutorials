using UnityEngine;

public class FallingDebris : MonoBehaviour
{
    public bool isDangerous = false;
    public GameObject debris;

    private Animator anim;
    private TurnManager turnManager;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        GridManager.AddToGrid(gameObject);
        turnManager = GetComponentInParent<TurnManager>();
        turnManager.AddDebris(this);
        if (isDangerous) {
            anim.SetTrigger("Fall");
        }
    }

    public void Fall() {
        isDangerous = !isDangerous;
        if (isDangerous) {
            anim.SetTrigger("Fall");
        }
    }
}
