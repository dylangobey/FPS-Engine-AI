using Unity.VisualScripting;
using UnityEngine;

public class AIHealthManager : Enemy
{
    bool isDead = false;

    public override void Damage(float damage)
    {
        if (isDead) return;
        base.Damage(damage);
    }

    public override void Die()
    {
        if (isDead) return;
        isDead = true;
        events.OnDeath.Invoke();

        if (shieldSlider != null) shieldSlider.gameObject.SetActive(false);
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        if (UI.GetComponent<UIController>().displayEvents)
        {
            UI.GetComponent<UIController>().AddKillfeed(name);
        }

        //if (transform.parent.GetComponent<CompassElement>() != null) transform.parent.GetComponent<CompassElement>().Remove();

        base.Die();

        //Destroy(gameObject);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Start()
    {
        base.Start();
    }
}
