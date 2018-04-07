using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag.Equals ("Player")) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

}
