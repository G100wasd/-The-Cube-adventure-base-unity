using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Player;
    Vector2 playerPos;
    void Update()
    {
        playerPos = Player.transform.position;
        this.transform.position = new Vector3(playerPos.x, playerPos.y + 2, -10);
    }
}
