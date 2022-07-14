using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames{
	/// <summary>
	/// Callback handler for custom events.
	/// </summary>
	public abstract class CallbackHandler : MonoBehaviour {
		[HideInInspector]
		public List<CallbackHandler.Entry> delegates;
		public abstract string[] Callbacks {
			get;
		}
		
		protected void Execute(string eventID,CallbackEventData eventData)
		{
			if (this.delegates != null)
			{
				int num = 0;
				int count = this.delegates.Count;
				while (num < count)
				{
					CallbackHandler.Entry item = this.delegates[num];
					if (item.eventID == eventID && item.callback != null)
					{
						item.callback.Invoke(eventData);
					}
					num++;
				}
			}
		}
		
		public void RegisterListener(string eventID,UnityAction<CallbackEventData> call){
			if (delegates == null) {
				delegates= new List<Entry>();		
			}
			Entry entry = null;
			for (int i=0; i< delegates.Count; i++) {
				Entry mEntry= delegates[i];
				if(mEntry.eventID == eventID){
					entry=mEntry;
					break;
				}
			}
			if (entry == null) {
				entry= new Entry();
				entry.eventID=eventID;
				entry.callback= new CallbackEvent();
				delegates.Add(entry);
			}
			
			entry.callback.AddListener(call);
		}
		
		public void RemoveListener(string eventID,UnityAction<CallbackEventData> call){
			if (delegates == null) {
				return;		
			}
			for (int i=0; i< delegates.Count; i++) {
				Entry entry= delegates[i];
				if(entry.eventID == eventID){
					entry.callback.RemoveListener(call);
				}
			}
		}
		
		
		[System.Serializable]
		public class Entry
		{
			public string eventID;
			
			public CallbackEvent callback;
			
			public Entry()
			{
				
			}
		}
		
		[System.Serializable]
		public class CallbackEvent:UnityEvent<CallbackEventData>{}
	}
}