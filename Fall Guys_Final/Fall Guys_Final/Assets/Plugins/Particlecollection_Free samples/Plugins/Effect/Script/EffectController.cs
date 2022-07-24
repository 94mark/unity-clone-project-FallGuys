using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EffectData{
	public bool m_bFoldoutOpen = true;

	public float m_fTimeSec = 0.0f;
	public GameObject m_goEffect = null;

	public bool m_bTransformFoldout = true;
	public Vector3 m_goPos = new Vector3 (0, 0, 0);
	public Vector3 m_goRotation = new Vector3 (0, 0, 0);
	public Vector3 m_goScale = new Vector3 (1, 1, 1);

	public bool m_bSortingFoldout = true;
	public int m_SortingLayerID;
	public int m_SortingOrder;
}

public class EffectController : MonoBehaviour {
	public int m_nNumOfEffects = 0;			///< 特效數量.
	public bool m_bLockNums = false;		///< 特效數量鎖定.

	public List<EffectData> m_kEffectGenList = new List<EffectData>();		///< 特效設定清單.
	private int m_nNowIndex = 0;

	void Awake()
	{
		for (int i = 0; i < m_kEffectGenList.Count; i++) {
			Invoke ("GenEffect", m_kEffectGenList [i].m_fTimeSec);
		}

		Comp comparer = new Comp ();			///< 時間Comparer.
		m_kEffectGenList.Sort (comparer);		///< 依時間排序.
	}

	void Update()
	{
		CheckTransfromUpdate ();
	}

	/// <summary>
	/// 特效生成.
	/// </summary>
	void GenEffect()
	{
		EffectData effectData = m_kEffectGenList[m_nNowIndex];
		if (effectData == null)
			return;

		if(effectData.m_goEffect != null) {
			GameObject go = Instantiate (effectData.m_goEffect);
			go.transform.parent = transform;
			go.name = m_nNowIndex.ToString ();	///< 上編號.
			UpdateEffectTransformByIndex (m_nNowIndex);
			UPdateRenderLayerByIndex (m_nNowIndex);
		}
		m_nNowIndex++;
	}

	/// <summary>
	/// 原生功能更改值.
	/// </summary>
	void CheckTransfromUpdate()
	{
		foreach (Transform tf in transform) {
			int nIndex = int.Parse (tf.name);
			EffectData effectData = m_kEffectGenList[nIndex];
			if (effectData == null)
				return;

			if (tf.position != effectData.m_goPos)
				effectData.m_goPos = tf.position;
			if (tf.localRotation.eulerAngles != effectData.m_goRotation)
				effectData.m_goRotation = tf.localRotation.eulerAngles;
			if (tf.localScale != effectData.m_goScale)
				effectData.m_goScale = tf.localScale;
		}
	}

	/// <summary>
	/// 更新對應編號特效之Transform數值.
	/// </summary>
	/// <param name="nIndex">特效編號.</param>
	public void UpdateEffectTransformByIndex(int nIndex)
	{
		/// 取得特效資料.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return;
		EffectData effectData = m_kEffectGenList[nIndex];
		if (effectData == null)
			return;

		/// 設定特效物件Transform.
		tf.position = effectData.m_goPos;
		Quaternion effectObjRotation = new Quaternion ();
		effectObjRotation.eulerAngles = effectData.m_goRotation;
		tf.localRotation = effectObjRotation;
		tf.localScale = effectData.m_goScale;
	}

	/// <summary>
	/// 檢查對應編號特效是否含有粒子系統.
	/// </summary>
	/// <returns><c>true</c>,有Particle System, <c>false</c> 沒article System.</returns>
	/// <param name="nIndex">特效編號.</param>
	public ParticleSystem CheckHasParticleSystem(int nIndex)
	{
		/// 取得特效物件.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return null;

		/// 取得粒子系統.
		ParticleSystem particleSystem = tf.gameObject.GetComponent<ParticleSystem> ();
		return particleSystem;
	}

	/// <summary>
	/// 檢查對應編號特效是否使用RenderEffect.
	/// </summary>
	/// <returns>RenderEffect元件.</returns>
	/// <param name="nIndex">特效編號.</param>
	public RenderEffect CheckHasRenderEffectScript(int nIndex)
	{
		/// 取得特效物件.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return null;

		/// 取得RenderEffect元件.
		RenderEffect renderEffect = tf.gameObject.GetComponent<RenderEffect> ();
		return renderEffect;
	}

	/// <summary>
	/// 更新對應編號特效物件Render Layer.
	/// </summary>
	/// <param name="nIndex">特效編號.</param>
	public void UPdateRenderLayerByIndex(int nIndex)
	{
		/// 取得特效物件.
		Transform tf = this.transform.Find (nIndex.ToString());
		if (tf == null)
			return;
		EffectData effectData = m_kEffectGenList[nIndex];
		if (effectData == null)
			return;

		/// Render Layer 更新.
		Renderer render = tf.gameObject.GetComponent<Renderer>();
		render.sortingLayerID = effectData.m_SortingLayerID;
		render.sortingOrder = effectData.m_SortingOrder;
	}
}

/// <summary>
/// Effect Data Time comparer.
/// </summary>
public class Comp : IComparer<EffectData>
{
	public int Compare(EffectData x, EffectData y)
	{
		if (x == null) {
			if (y == null)
				return 0;
			else
				return 1;
		} else {
			if (y == null) {
				return -1;
			} else {
				float fDiff = x.m_fTimeSec.CompareTo (y.m_fTimeSec);
				if (fDiff > 0)
					return 1;
				else if (fDiff < 0)
					return -1;
				else
					return 0;
			}
		}
	}
}