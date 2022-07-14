using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DevionGames{
	public interface IJsonSerializable {
        void GetObjectData(Dictionary<string,object> data);
		void SetObjectData(Dictionary<string,object> data);
	}
}