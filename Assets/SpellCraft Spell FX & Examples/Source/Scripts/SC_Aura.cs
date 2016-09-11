using UnityEngine;
using System.Collections;

/**
 * Script for Auras.
 * Aura's are always on - Not click based.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Aura : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject callbackReceiver;
	
	[Header("Config")]
	public string callbackName;
	
	void Start () {
		callbackReceiver.SendMessage(callbackName);
	}
}
