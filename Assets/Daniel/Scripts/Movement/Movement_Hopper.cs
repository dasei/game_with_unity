using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Hopper : Movement {

    private GameObject player;

    public override void PerformMovement()
    {
		if (player == null)
			player = GameObject.FindGameObjectWithTag ("Player");

        Vector2 direction = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;

        Move(direction.x, direction.y, true);
    }
}
