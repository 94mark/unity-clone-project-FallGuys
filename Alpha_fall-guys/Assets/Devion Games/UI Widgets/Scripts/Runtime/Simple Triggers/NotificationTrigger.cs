using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//Import Namespace
using DevionGames.UIWidgets;

/// <summary>
/// Message container example.
/// </summary>
public class NotificationTrigger : MonoBehaviour {
	//Reference to the MessageContainer in scene
	private Notification m_Notification;
	//Options to display containing information about text, icon, fading duration...
	public NotificationOptions[] options;

	private void Start(){
		//Find the reference to the MessageContainer
		this.m_Notification = WidgetUtility.Find<Notification> ("Notification");
	}

	/// <summary>
	/// Called from a button OnClick event in the example
	/// </summary>
	public void AddRandomNotification(){
		//Get a random MessageOption from the array
		NotificationOptions option=options[Random.Range(0,options.Length)];
		//Add the message
		m_Notification.AddItem(option);
	}

	/// <summary>
	/// Called from a button OnClick event in the example
	/// </summary>
	public void AddNotification(InputField input){
		//Add a text message

		m_Notification.AddItem (input.text);
	}

	/// <summary>
	/// Called from a Slider OnValueChanged event in the example
	/// </summary>
	public void AddNotification(float index){
		//Round the index to int and get the option from options array.
		NotificationOptions option = options [Mathf.RoundToInt (index)];
		//Add the message
		m_Notification.AddItem (option);
	}
}
