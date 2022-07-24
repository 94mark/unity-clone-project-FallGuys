using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraTarget : MonoBehaviour {
	public Transform m_TargetOffset;

	void LateUpdate(){
		transform.LookAt (m_TargetOffset);
	}

}
