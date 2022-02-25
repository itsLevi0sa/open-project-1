using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController_old : MonoBehaviour
{

	public GameObject FirstPerson;
	public GameObject FlyThrough;
    // Start is called before the first frame update
    void Start()
    {
		FirstPerson.SetActive(true);
		FlyThrough.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			FirstPerson.SetActive(!FirstPerson.activeSelf);
			FlyThrough.SetActive(!FlyThrough.activeSelf);

		}
	}
}
