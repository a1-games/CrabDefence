using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Transform healthbar;

    public void RefreshHealthbar()
    {
        float tmp = GameManager.AskFor.playerHealth / GameManager.AskFor.maxPlayerHealth;
        if (tmp <= 0) tmp = 0;
        healthbar.localScale = new Vector3(tmp, 1, 1);
    }

}
