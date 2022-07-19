 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LHS_RagdollChanger : MonoBehaviour
{
    // 캐릭터 오브젝트
    public GameObject charObj;
    // 랙돌 오브젝트
    public GameObject ragdollObj;

    // 힘을 가할 rigidbody
    public Rigidbody spine;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ChangeRagdoll();
        }
    }

    public void ChangeRagdoll()
    {
        CopyCharacterTransformToRagdoll(charObj.transform, ragdollObj.transform);  

        charObj.SetActive(false);
        ragdollObj.SetActive(true);

        spine.AddForce(new Vector3(0f, 100f, -100f), ForceMode.Impulse);
    }

    private void CopyCharacterTransformToRagdoll(Transform origin, Transform ragdoll)
    {
        for(int i = 0; i < origin.childCount; i++)
        {
            if(origin.childCount != 0)
            {
                CopyCharacterTransformToRagdoll(origin.GetChild(i), ragdoll.GetChild(i));
            }

            ragdoll.GetChild(i).localPosition = origin.GetChild(i).localPosition;
            ragdoll.GetChild(i).localRotation = origin.GetChild(i).localRotation;
        }
    }

}
