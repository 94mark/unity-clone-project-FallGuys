using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace DevionGames.UIWidgets
{
	/// <summary>
	/// UIWidget is responsible for the management of widgets as well as animating them. 
	/// Your custom widgets should extend from this class or from child classes. 
	/// This way you can always track existing widgets by name in your game using WidgetUtility.Find<T>(name).
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class UIWidget : CallbackHandler
	{
		/// <summary>
		/// Name of the widget.
		/// </summary>
		[Tooltip("Name of the widget. You can find a reference to a widget with WidgetUtility.Find<T>(name).")]
		[SerializeField]
		protected new string name;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// Callbacks for Inspector.
		/// </summary>
		public override string[] Callbacks
		{
			get
			{
				return new string[]{
					"OnShow",
					"OnClose",
				};
			}
		}

		/// <summary>
		/// Widgets with higher priority will be prefered when used with WidgetUtility.Find<T>(name).
		/// </summary>
		[Tooltip("Widgets with higher priority will be prefered when used with WidgetUtility.Find<T>(name).")]
		[Range(0, 100)]
		public int priority;

		/// <summary>
		/// Key to toggle show and close
		/// </summary>
		[Header("Appearence")]
		[Tooltip("Key to show or close this widget.")]
		[SerializeField]
		protected KeyCode m_KeyCode = KeyCode.None;

		[Tooltip("Easing equation type used to tween this widget.")]
		[SerializeField]
		private EasingEquations.EaseType m_EaseType = EasingEquations.EaseType.EaseInOutBack;

		/// <summary>
		/// The duration to tween this widget.
		/// </summary>
		[Tooltip("The duration to tween this widget.")]
		[SerializeField]
		protected float m_Duration = 0.7f;

		[SerializeField]
		protected bool m_IgnoreTimeScale = true;
		public bool IgnoreTimeScale { get { return this.m_IgnoreTimeScale; } }

        /// <summary>
        /// The AudioClip that will be played when this widget shows.
        /// </summary>
		[Tooltip("The AudioClip that will be played when this widget shows.")]
		[SerializeField]
        protected AudioClip m_ShowSound;

		/// <summary>
		/// The AudioClip that will be played when this widget closes.
		/// </summary>
		[Tooltip("The AudioClip that will be played when this widget closes.")]
		[SerializeField]
		protected AudioClip m_CloseSound;

		/// <summary>
		/// Brings this window to front in Show()
		/// </summary>
		[Tooltip("Focus the widget. This will bring the widget to front when it is shown.")]
		[SerializeField]
        protected bool m_Focus = true;

        /// <summary>
        /// If true deactivates the gameobject when closed.
        /// </summary>
		[Tooltip("If true, deactivates the game object when it gets closed. This prevets Update() to be called every frame.")]
        [SerializeField]
		protected bool m_DeactivateOnClose = true;

		[Tooltip("Enables Cursor when this window is shown. Hides it again when the window is closed or character moves.")]
		[SerializeField]
		protected bool m_ShowAndHideCursor = false;
		[SerializeField]
		protected string m_CameraPreset = "UI";
		[Tooltip("Close this widget when the player moves.")]
		[SerializeField]
		protected bool m_CloseOnMove = true;
		/*[Tooltip("When the key is pressed, show and hide cursor funtionality will be disabled.")]
		[SerializeField]
		protected KeyCode m_Deactivate= KeyCode.LeftControl;*/
		[Tooltip("This option allows to focus and rotate player. This functionality only works with the included ThirdPersonCamera and FocusTarget component!")]
		[SerializeField]
		protected bool m_FocusPlayer = false;

		protected static CursorLockMode m_PreviousCursorLockMode;
		protected static bool m_PreviousCursorVisibility;
		protected Transform m_CameraTransform;
		protected MonoBehaviour m_CameraController;
		protected MonoBehaviour m_ThirdPersonController;
		protected static bool m_PreviousCameraControllerEnabled;
		protected static List<UIWidget> m_CurrentVisibleWidgets=new List<UIWidget>();

		/// <summary>
		/// Gets a value indicating whether this widget is visible.
		/// </summary>
		/// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
		public bool IsVisible { 
			get {
				if (this.m_CanvasGroup == null)
					this.m_CanvasGroup = GetComponent<CanvasGroup>();
				return m_CanvasGroup.alpha == 1f; 
			} 
		}

		/// <summary>
		/// The RectTransform of the widget.
		/// </summary>
		protected RectTransform m_RectTransform;
		/// <summary>
		/// The CanvasGroup of the widget.
		/// </summary>
		protected CanvasGroup m_CanvasGroup;
		/// <summary>
		/// Checks if Show() is already called. This prevents from calling Show() multiple times when the widget is not finished animating. 
		/// </summary>
		protected bool m_IsShowing;

		private TweenRunner<FloatTween> m_AlphaTweenRunner;
		private TweenRunner<Vector3Tween> m_ScaleTweenRunner;

		protected Scrollbar[] m_Scrollbars;

		protected bool m_IsLocked = false;
		public bool IsLocked
		{
			get { return this.m_IsLocked; }
		}


		private void Awake ()
		{
			//Register the KeyCode to show or close the widget.
			WidgetInputHandler.RegisterInput(this.m_KeyCode, this);
			m_RectTransform = GetComponent<RectTransform> ();
			m_CanvasGroup = GetComponent<CanvasGroup> ();
			this.m_Scrollbars = GetComponentsInChildren<Scrollbar>();
			this.m_CameraTransform = Camera.main.transform;
			this.m_CameraController = this.m_CameraTransform.GetComponent("ThirdPersonCamera") as MonoBehaviour;
			PlayerInfo playerInfo = new PlayerInfo("Player");

			if (playerInfo.gameObject != null)
				this.m_ThirdPersonController = playerInfo.gameObject.GetComponent("ThirdPersonController") as MonoBehaviour;


			if (!IsVisible) {
				//Set local scale to zero, when widget is not visible. Used to correctly animate the widget.
				m_RectTransform.localScale = Vector3.zero;
			}
			if (this.m_AlphaTweenRunner == null)
				this.m_AlphaTweenRunner = new TweenRunner<FloatTween> ();
			this.m_AlphaTweenRunner.Init (this);

			if (this.m_ScaleTweenRunner == null)
				this.m_ScaleTweenRunner = new TweenRunner<Vector3Tween> ();
			this.m_ScaleTweenRunner.Init (this);
            m_IsShowing = IsVisible;

			OnAwake ();
		}

		protected virtual void OnAwake ()
		{
		}

		private void Start ()
		{
			OnStart ();
			StartCoroutine (OnDelayedStart ());
		}

		protected virtual void OnStart ()
		{
		}

		private IEnumerator OnDelayedStart ()
		{
			yield return null;
			if (!IsVisible && m_DeactivateOnClose) {
				gameObject.SetActive (false);
			}
		}

		protected virtual void Update() {
			if (this.m_ShowAndHideCursor && this.IsVisible && this.m_CloseOnMove && (this.m_ThirdPersonController == null || this.m_ThirdPersonController.enabled) && (Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f))
			{
				Close();
			}
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		public virtual void Show ()
		{
            if (this.m_IsShowing) {
                return;
            }
            this.m_IsShowing = true;
			gameObject.SetActive (true);
            if (this.m_Focus) {
				Focus ();
			}
			TweenCanvasGroupAlpha (m_CanvasGroup.alpha, 1f);
			TweenTransformScale (Vector3.ClampMagnitude (m_RectTransform.localScale, 1.9f), Vector3.one);
			
			WidgetUtility.PlaySound (this.m_ShowSound, 1.0f);
			m_CanvasGroup.interactable = true;
			m_CanvasGroup.blocksRaycasts = true;
			Canvas.ForceUpdateCanvases();
			for (int i = 0; i < this.m_Scrollbars.Length; i++)
			{
				this.m_Scrollbars[i].value = 1f;
			}
			if (this.m_ShowAndHideCursor) {
				/*if (m_CurrentVisibleWidgets.Count == 0) {
					m_PreviousCursorLockMode = Cursor.lockState;
					m_PreviousCursorVisibility = Cursor.visible;
					if (m_CameraController != null)
						m_PreviousCameraControllerEnabled = m_CameraController.enabled;
				}*/
				m_CurrentVisibleWidgets.Add(this);
				if (m_CameraController != null && m_CurrentVisibleWidgets.Count == 1)
				{
					//this.m_CameraController.enabled = false;
					this.m_CameraTransform.SendMessage("Activate", this.m_CameraPreset, SendMessageOptions.DontRequireReceiver);

					if(this.m_FocusPlayer && !this.m_IsLocked)
						this.m_CameraController.SendMessage("Focus", true, SendMessageOptions.DontRequireReceiver);
				}
				//Cursor.lockState = CursorLockMode.None;
				//Cursor.visible = true;
			}

			Execute("OnShow", new CallbackEventData());
		}

		/// <summary>
		/// Close this widget.
		/// </summary>
		public virtual void Close ()
		{
            if (!m_IsShowing) {
                return;
            }
            m_IsShowing = false;
			TweenCanvasGroupAlpha (m_CanvasGroup.alpha, 0f);
			TweenTransformScale (m_RectTransform.localScale, Vector3.zero);
			
			WidgetUtility.PlaySound (this.m_CloseSound, 1.0f);
			m_CanvasGroup.interactable = false;
			m_CanvasGroup.blocksRaycasts = false;
			if (this.m_ShowAndHideCursor) {
				m_CurrentVisibleWidgets.Remove(this);
				if (m_CurrentVisibleWidgets.Count == 0) {
					//Cursor.lockState = m_PreviousCursorLockMode;
					//Cursor.visible = m_PreviousCursorVisibility;
					if (this.m_CameraController != null)
					{
						this.m_CameraTransform.SendMessage("Deactivate", this.m_CameraPreset, SendMessageOptions.DontRequireReceiver);
						//this.m_CameraController.enabled = m_PreviousCameraControllerEnabled;
						if (this.m_CameraController.enabled && this.m_FocusPlayer) {
							this.m_CameraController.SendMessage("Focus", false, SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}

			Execute("OnClose", new CallbackEventData());

		}

		private void TweenCanvasGroupAlpha (float startValue, float targetValue)
		{
				FloatTween alphaTween = new FloatTween {
					easeType = m_EaseType,
					duration = m_Duration,
					startValue = startValue,
					targetValue = targetValue,
					ignoreTimeScale = m_IgnoreTimeScale
				};

				alphaTween.AddOnChangedCallback ((float value) => {
					m_CanvasGroup.alpha = value;
				});
				alphaTween.AddOnFinishCallback (() => {
					if (alphaTween.startValue > alphaTween.targetValue) {
						if (m_DeactivateOnClose && !this.m_IsShowing) {
							gameObject.SetActive (false);
						}
					} 
				});
			
			m_AlphaTweenRunner.StartTween (alphaTween);
		}

		private void TweenTransformScale (Vector3 startValue, Vector3 targetValue)
		{
            Vector3Tween scaleTween = new Vector3Tween
            {
                easeType = m_EaseType,
                duration = m_Duration,
                startValue = startValue,
                targetValue = targetValue,
				ignoreTimeScale = m_IgnoreTimeScale
			};
            scaleTween.AddOnChangedCallback((Vector3 value) => {
                m_RectTransform.localScale = value;
            });

            m_ScaleTweenRunner.StartTween(scaleTween);
        }

		/// <summary>
		/// Toggle the visibility of this widget.
		/// </summary>
		public virtual void Toggle ()
		{
			if (!IsVisible) {
				Show ();
			} else {
				Close ();
			}
		}

		/// <summary>
		/// Brings the widget to the top
		/// </summary>
		public virtual void Focus ()
		{
			m_RectTransform.SetAsLastSibling ();
		}

		protected virtual void OnDestroy() {
			//Unregister input key
			WidgetInputHandler.UnregisterInput(this.m_KeyCode, this);
		}

		public void Lock(bool state)
		{
			this.m_IsLocked = state;
		}

		public static void LockAll(bool state) {
			UIWidget[] widgets = WidgetUtility.FindAll<UIWidget>();
			for (int i = 0; i < widgets.Length; i++)
			{
				widgets[i].Lock(state);
			}
		}
	}
}