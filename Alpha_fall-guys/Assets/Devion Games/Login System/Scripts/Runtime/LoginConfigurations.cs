using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DevionGames.LoginSystem
{
	[System.Serializable]
	public class LoginConfigurations : ScriptableObject
	{
		public List<Configuration.Settings> settings = new List<Configuration.Settings>();
	}
}