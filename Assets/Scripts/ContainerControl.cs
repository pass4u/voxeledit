using UnityEngine;
using System.Collections;

public class ContainerControl : MonoBehaviour {
	public GameObject Container;

	public GameObject camera;

	public int i_bottom_guide_row, i_botton_guide_cell;
	public GameObject pref_guide_item;
	private Vector3 q;
	private GameObject container_anchor, bottom_guide;
	// Use this for initialization
	void Start () {
        //ViewCenter ();
        //ViewAllObject ();

		container_anchor = Container.transform.FindChild ("Anchor").gameObject;
		GenerateGuide ();
	}

	/// <summary>
	/// generate guide
	/// </summary>
	public void GenerateGuide(){

		if (container_anchor.transform.FindChild ("bottom_guide") == null) {
			// generate !!
			bottom_guide = new GameObject();
			bottom_guide.name = "bottom_guide";
			bottom_guide.transform.parent = container_anchor.transform;
			bottom_guide.transform.localPosition = Vector3.down*0.5f;
			bottom_guide.transform.localRotation = Quaternion.Euler (Vector3.zero);

		}

		int row_count = 0;
		GameObject pref;
		for (int i = 0; i < i_botton_guide_cell; ++i) {
			for (int j = 0; j < i_bottom_guide_row; ++j) {
				pref = Instantiate (pref_guide_item);
				pref.transform.parent = bottom_guide.transform;
				pref.transform.localPosition = new Vector3(-1*row_count,0,(-1*j));

			}
			row_count += 1;
		}
	}

	public void horizonSCBHandler(float value){
		Debug.Log (value);
//		value -> 0 = 0->180
//		value -> 1 = 360->180

		Vector3 q = Container.transform.rotation.eulerAngles;
		Vector3 v = new Vector3 (q.x, Mathf.Lerp (180, -180, value), q.z);
        Container.transform.rotation = Quaternion.Euler(v);
	}

	public void verticalSCBHandler(float value){
		Debug.Log (value);		
		Quaternion q = Container.transform.rotation;
        //Vector3 v = new Vector3 (Mathf.Lerp (180, -180, value),q.y, q.z);
        Container.transform.rotation = Quaternion.Euler(new Vector3(Mathf.Lerp(180, -180, value), q.y, q.z));

	}

	public void ViewCenter(){
		camera.transform.position=Vector3.zero;
		setCenterForObject ();
	}


	// set center for all child gameobject
	void setCenterForObject()
	{
//		Debug.Log (Container.transform.FindChild ("Anchor").localPosition);


		Vector3 centroid = Vector3.zero;
		if ( Container.transform.FindChild("Anchor").childCount > 0 )
		{
			Transform allChildren = Container.transform.FindChild("Anchor").GetComponentInChildren<Transform>();
			foreach (Transform child in allChildren)
			{
				centroid += child.localPosition;
			}
			centroid /= -allChildren.childCount;
		}


//		Debug.Log ("centroid :: " + centroid);

		if (Container.transform.FindChild ("Anchor").localPosition != centroid) {
			Container.transform.FindChild("Anchor").localPosition = centroid;
		}

	}

	public void ViewAllObject(){
		Vector3 centroid = Vector3.zero;
		Transform allChildren = Container.transform.FindChild("Anchor").GetComponentInChildren<Transform>();
		foreach (Transform child in allChildren)
		{
			centroid += child.localScale;
		}

		float max = Mathf.Max (centroid.x, centroid.y, centroid.z);
		if (max <= 1) {
			max = 1;
		} else {
			max /=2;
		}
		Debug.Log ("max :: " + max);
		camera.transform.FindChild ("Main Camera").GetComponent<Camera> ().orthographicSize = max;

	}

}
