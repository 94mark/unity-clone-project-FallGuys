using UnityEngine;
using System.Collections;

namespace DevionGames{
	public class LookAtMainCamera : MonoBehaviour {
		public bool ignoreRaycast = true;
		private Transform target;
		private Transform mTransform;
		private bool searchCamera;

		void Start () {
			if (Camera.main != null) {
				target = Camera.main.transform;
			} else {
				StartCoroutine(SearchCamera());
			}
			mTransform = transform;
			if (ignoreRaycast) {
				gameObject.layer = 2;
			}
		}
		
		void Update () {
			if (target != null) {
				//mTransform.LookAt (target.position);
				mTransform.LookAt (mTransform.position + target.rotation * Vector3.back,
				                  target.rotation * Vector3.up);
			} else {
				if(!searchCamera){
					StartCoroutine(SearchCamera());
				}
			}
		}
		
		private IEnumerator SearchCamera(){
			searchCamera = true;
			while (target == null) {
				if (Camera.main != null) {
					target = Camera.main.transform;
				}
				yield return new WaitForSeconds(2.0f);
			}
			searchCamera = false;
		}
	}
}