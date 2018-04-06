using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SelectRandomSprite : MonoBehaviour {

	public Sprite[] spriteSelection;

	// Use this for initialization
	void Awake () {
		assignRandomSprite();
	}

	void assignRandomSprite(){
		SpriteRenderer render = GetComponent<SpriteRenderer> ();
		Sprite sprite = spriteSelection[(int) Random.Range(0, spriteSelection.GetLength (0))];
		render.sprite = sprite;
	}

}
