using UnityEngine;
using System.Collections;

/**
 * Sets the spell tooltip description.
 * 
 * @author j@gamemechanix.io
 * @project SpellCraft
 * @copyright GameMechanix.io 2016
 **/
public class SC_SpellTooltip : MonoBehaviour {

	[Header("Config")]
	public string spellTooltipDescription;

	void Start () {
		GUIText spellTooltip = GameObject.FindWithTag ("Spell Tooltip").GetComponent<GUIText> ();
		spellTooltip.text = spellTooltipDescription.Replace ("<br>", "\n");
	}
}
