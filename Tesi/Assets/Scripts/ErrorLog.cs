using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorLog : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void StartFade()
	{
		StartCoroutine(Fade());
	}

	public IEnumerator Fade()
	{

		for (int i = 0; i < 255; i++)
		{
			//TODO - Fade
			yield return null;
		}
		DestroyObject(gameObject);
	}

	public void StartMove()
	{
		StartCoroutine(Move());
	}

	public IEnumerator Move()
	{
		for (int i = 0; i < 50; i++)
		{
			gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 2f);
			yield return null;
		}
	}
}
