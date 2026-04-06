using UnityEngine;

public class PlayerWillpower : MonoBehaviour
{

    public int currentWillpower;
    public int maxWillpower = 50;
    float willpowerBaseTime = 1;
    float willpowerTimer = 1;
    float willPowerDrainTimer = 0.1f;
    int willpowerDecrementAuto = 2;
    [Header("DEV")]
    public bool autotick = true;
    PlayerControls controls;
    void Awake()
    {
        currentWillpower = maxWillpower;
        controls = new PlayerControls();
        controls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!autotick) return;
        if (willpowerTimer <= 0)
        {
            currentWillpower -= willpowerDecrementAuto;
            willpowerTimer = willpowerBaseTime;
            UIEvents.SetWillpower();
            CheckWillpowerEmpty();
        }
        if (controls.Player.DrainWP.IsPressed())
        {
            willPowerDrainTimer -= Time.deltaTime;
            if (willPowerDrainTimer <= 0)
            {
                willPowerDrainTimer = 0.1f;
                AddWillpower(-1);
                PlayerManager.Instance.health.Heal(1);
            }
        }
        willpowerTimer -= Time.deltaTime;
    }

    public void AddWillpowerTime (float s)
    {
        willpowerTimer += s;
    }

    public void AddWillpower(int amount)
    {
        currentWillpower = Mathf.Min(currentWillpower + amount, maxWillpower);
        UIEvents.SetWillpower();
        CheckWillpowerEmpty();
    }

    void CheckWillpowerEmpty ()
    {
        if (currentWillpower <= 0)
        {
            DealMenu.Instance.OpenMenu();
            currentWillpower = maxWillpower;
        }
    }

}
