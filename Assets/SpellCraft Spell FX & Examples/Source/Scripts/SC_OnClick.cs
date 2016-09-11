using UnityEngine;
using System.Collections;

/**
 * Mouse Click send callback.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_OnClick : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject callbackReceiver;

	[Header("Config")]
	public string callbackName;

	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			callbackReceiver.SendMessage(callbackName);
		}
	}
}
