using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

	private Vector3 ResetCamera;
	private Vector3 Origin;
	private Vector3 Diference;
	private bool Drag = false;

	public float zoomSpeed = 2;
	public float targetOrtho;
	public float smoothSpeed = 2.0f;
	public float minOrtho = 2.0f;
	public float maxOrtho = 10.0f;

	// Start is called before the first frame update
	void Start()
    {
		ResetCamera = Camera.main.transform.position;
		targetOrtho = Camera.main.orthographicSize;
	}

    // Update is called once per frame
    void LateUpdate()
    {
		if (Input.GetMouseButton(0))
		{
			Diference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
			if (Drag == false)
			{
				Drag = true;
				Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
		}
		else
		{
			Drag = false;
		}
		if (Drag == true)
		{
			Camera.main.transform.position = Origin - Diference;
		}
		//RESET CAMERA TO STARTING POSITION WITH RIGHT CLICK
		if (Input.GetMouseButton(1))
		{
			Collider2D hitCollider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
			if (hitCollider == null)
			{
				Camera.main.transform.position = ResetCamera;
			}
		}

		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0.0f)
		{
			targetOrtho -= scroll * zoomSpeed;
			targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);
		}

		Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);

	}
}
