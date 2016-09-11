using UnityEngine;
using System.Collections;

/**
 * Heals current game object for an amount with each attacking hit.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_BuffLifesteal : MonoBehaviour {

	[Header("Config")]
	public int lifestealAmount = 10;
	
	void Start () {
		// Add life steal to the minion
	}
}
