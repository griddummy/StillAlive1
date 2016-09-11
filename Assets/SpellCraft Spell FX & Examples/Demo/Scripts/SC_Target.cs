using UnityEngine;
using System.Collections;

/**
 * UI Target script for unit selection and hover over.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_Target : MonoBehaviour {

	[Header("Config")]
	public bool isTargetable = true;
	public Texture2D cursorTexture;
	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;

	void OnMouseEnter () {
		if (isTargetable) {
			Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

			// Selection Ring
			if (gameObject.tag == "Ally") {
				SC_Helper.FindChildWithTag (gameObject, "Selection Ring Green").SetActive (true);
			} else if (gameObject.tag == "Enemy") {
				SC_Helper.FindChildWithTag (gameObject, "Selection Ring Red").SetActive (true);
			}
		}
	}

	void OnMouseExit () {
		if (isTargetable) {
			Cursor.SetCursor (null, Vector2.zero, cursorMode);

			// Selection Ring
			if (gameObject.tag == "Ally") {
				SC_Helper.FindChildWithTag (gameObject, "Selection Ring Green").SetActive (false);
			} else if (gameObject.tag == "Enemy") {
				SC_Helper.FindChildWithTag (gameObject, "Selection Ring Red").SetActive (false);
			}
		}
	}
}
