using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public PlayerManager player;
    public bool damage;
    float timeScale;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timeScale = player.timeScale;
    }

    public void MapPattern1()
    {
        spriteRenderer.color = new Color(0, 1, 0);
        Invoke("MapPattern2", 2 / timeScale);
    }
    void MapPattern2()
    {
        spriteRenderer.color = new Color(1, 1, 0);
        Invoke("MapPattern3", 2 / timeScale);
    }
    void MapPattern3()
    {
        spriteRenderer.color = new Color(1, 0, 0);
        Invoke("MapPattern4", 2 / timeScale);
    }
    void MapPattern4()
    {
        spriteRenderer.color = new Color(0, 0, 0);
        damage = true;
        Invoke("MapPattern5", 2 / timeScale);
    }
    void MapPattern5()
    {
        spriteRenderer.color = new Color(1, 1, 1);
        damage = false;
    }
}
