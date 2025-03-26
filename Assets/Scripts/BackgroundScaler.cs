using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    [SerializeField] bool ScaleOnAwake = true;
    [SerializeField] bool SetPosToVectorZeroOnAwake = true;
    void Awake()
    {
        if (!ScaleOnAwake) return;
        ScaleToScreenSize();
        if (!SetPosToVectorZeroOnAwake) return;
        SetPosToVectorZero();
    }

    public void ScaleToScreenSize()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        SpriteRenderer background_sr = gameObject.GetComponent<SpriteRenderer>();

        gameObject.transform.localScale = new Vector3(worldScreenWidth / background_sr.sprite.bounds.size.x, worldScreenHeight / background_sr.sprite.bounds.size.y, 1f);
    }

    public void SetPosToVectorZero()
    {
        gameObject.transform.position = new Vector3(0, 0, gameObject.transform.position.z);
    }
}
