using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMUSIC : MonoBehaviour {

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
