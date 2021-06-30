using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProblemGenerator : MonoBehaviour
{
	public GameObject container;

    // Start is called before the first frame update
    void Start()
    {
		GameObject referenceAddedObject = (GameObject)Instantiate(Resources.Load("AddedObject"));

		for (int i = 0; i < 15; i++)
		{
			GameObject newObj = (GameObject)Instantiate(referenceAddedObject);
			newObj.transform.parent = container.transform;
			newObj.GetComponent<Image>().color = Random.ColorHSV();
		}

		Destroy(referenceAddedObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
