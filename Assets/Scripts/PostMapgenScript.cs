using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*BY: ALIN
The purpose of this script is to be ran RIGHT AFTER the map is done generating, for stuff like
handling spawnign the player, the monster, etcetera. you can attach and call
other scripts from here too.
*/
public class PostMapgenScript : MonoBehaviour
{
    [Tooltip("attach the player to this, right now used to teleport him to the map once it's done loading")]
    public GameObject player;
    public GameObject RandomMapParent;
    public void Main()
    {
        player.transform.position = RandomMapParent.transform.position + new Vector3(0,20,0);
        Debug.Log("player shouldve been teleported by now");
    }
}
