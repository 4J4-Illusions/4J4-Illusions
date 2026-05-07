using UnityEngine;

public class MachineCalibration : MonoBehaviour
{
    Animator anim;
    BoxCollider collision;
    float cooldown = -1;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        collision = GetComponent<BoxCollider>();

        BreakMachine();
    }
    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldown = Mathf.Max(cooldown, 0);
        }
        else if (cooldown == 0)
        {
            BreakMachine();
            cooldown = -1;
        }
    }



    /// <summary>
    /// Joue l'animation de la machine qui se casse et active le collider pour que le joueur puisse interagir avec la machine
    /// </summary>
    void BreakMachine()
    {
        collision.enabled = true;
        anim.SetTrigger("TriggerBreak");
    }
    /// <summary>
    /// Joue l'animation de la machine qui se répare et désactive le collider pour empêcher l'interaction jusqu'à ce qu'elle se casse à nouveau
    /// </summary>
    public void SuccessfulRepairMachine()
    {
        collision.enabled = false;
        anim.SetTrigger("TriggerRepair");
        cooldown = Random.Range(15, 30);
        //cooldown = Random.Range(1, 3);
    }
    public void FailedRepairMachine()
    {
        collision.enabled = false;
        cooldown = Random.Range(20, 40);
        //cooldown = Random.Range(2, 4);
    }
}
