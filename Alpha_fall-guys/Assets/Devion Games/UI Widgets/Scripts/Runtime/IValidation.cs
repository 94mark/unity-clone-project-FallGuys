using UnityEngine;
using System.Collections;

namespace DevionGames.UIWidgets{
	public interface IValidation<T> {
		bool Validate(T item);
	}
}