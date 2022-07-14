using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames{
	/// <summary>
	/// Callback event data that can be derived from.
	/// </summary>
	public class CallbackEventData {
		private Dictionary<string, object> properties;

		public CallbackEventData() {
			properties = new Dictionary<string, object>();
		}

		public void AddData(string key, object value) {
			if (properties.ContainsKey(key))
			{
				properties[key] = value;
			}else {
				properties.Add(key, value);
			}
		}

		public object GetData(string key) {
			if (properties.ContainsKey(key))
			{
				return properties[key];
			}else {
				return null;
			}
		}
	}
}