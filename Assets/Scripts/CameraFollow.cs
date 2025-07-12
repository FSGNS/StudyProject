using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public List<Transform> players;
    public Vector3 offset;

    private void LateUpdate()
    {
        Transform leadPlayer = GetLeadingPlayer();

        if (leadPlayer == null) return;

        transform.position = new Vector3(leadPlayer.position.x, transform.position.y, transform.position.z) + offset;
    }
    
    private Transform GetLeadingPlayer()
    {
        float maxX = float.MinValue;
        Transform leader = null;

        foreach (Transform player in players)
        {
            if (player != null && player.gameObject.activeInHierarchy)
            {
                if (player.position.x > maxX)
                {
                    maxX = player.position.x;
                    leader = player;
                }
            }
        }

        return leader;
    }
}
