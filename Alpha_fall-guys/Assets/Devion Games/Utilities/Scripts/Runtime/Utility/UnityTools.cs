using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

namespace DevionGames
{
    public static class UnityTools
    {
        private static CoroutineHandler m_CoroutineHandler;
        private static CoroutineHandler Handler
        {
            get
            {
                if (m_CoroutineHandler == null)
                {
                    GameObject handlerObject = new GameObject("Coroutine Handler");
                    m_CoroutineHandler = handlerObject.AddComponent<CoroutineHandler>();
                }
                return m_CoroutineHandler;
            }
        }

        private static AudioSource audioSource;
        /// <summary>
        /// Play an AudioClip.
        /// </summary>
        /// <param name="clip">Clip.</param>
        /// <param name="volume">Volume.</param>
        public static void PlaySound(AudioClip clip, float volumeScale, AudioMixerGroup audioMixerGroup=null)
        {
            if (clip == null)
            {
                return;
            }
            if (audioSource == null)
            {
                AudioListener listener = GameObject.FindObjectOfType<AudioListener>();
                if (listener != null)
                {
                    audioSource = listener.GetComponent<AudioSource>();
                    if (audioSource == null)
                    {
                        audioSource = listener.gameObject.AddComponent<AudioSource>();
                    }
                }
            }
            if (audioSource != null)
            {
                audioSource.outputAudioMixerGroup = audioMixerGroup;
                audioSource.PlayOneShot(clip, volumeScale);
            }
        }

        public static bool IsPointerOverUI()
        {
            if (EventSystem.current == null || EventSystem.current.currentInputModule == null)
            {
                return false;
            }
            Type type = EventSystem.current.currentInputModule.GetType();
            MethodInfo methodInfo;
            methodInfo = type.GetMethod("GetLastPointerEventData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (methodInfo == null)
            {
                return false;
            }
            PointerEventData eventData = (PointerEventData)methodInfo.Invoke(EventSystem.current.currentInputModule, new object[] { PointerInputModule.kMouseLeftId });
            if (eventData != null && eventData.pointerEnter)
            {
                return eventData.pointerEnter.layer == 5;
            }
            return false;
        }

        /// <summary>
        /// Converts a color to hex.
        /// </summary>
        /// <returns>Hex string</returns>
        /// <param name="color">Color.</param>
        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        /// <summary>
        /// Converts a hex string to color.
        /// </summary>
        /// <returns>Color</returns>
        /// <param name="hex">Hex.</param>
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "");
            hex = hex.Replace("#", "");
            byte a = 255;
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Colors the string.
        /// </summary>
        /// <returns>The colored string.</returns>
        /// <param name="value">Value.</param>
        /// <param name="color">Color.</param>
        public static string ColorString(string value, Color color)
        {
            return "<color=#" + UnityTools.ColorToHex(color) + ">" + value + "</color>";
        }

        /// <summary>
        /// Replaces a string ignoring case.
        /// </summary>
        /// <param name="source">Source.</param>
        /// <param name="oldString">Old string.</param>
        /// <param name="newString">New string.</param>
        public static string Replace(string source, string oldString, string newString)
        {
            int index = source.IndexOf(oldString, StringComparison.CurrentCultureIgnoreCase);

            // Determine if we found a match
            bool MatchFound = index >= 0;

            if (MatchFound)
            {
                // Remove the old text
                source = source.Remove(index, oldString.Length);

                // Add the replacemenet text
                source = source.Insert(index, newString);
            }
            return source;
        }

        /// <summary>
        /// Determines if the object is numeric.
        /// </summary>
        /// <returns><c>true</c> if is numeric the specified expression; otherwise, <c>false</c>.</returns>
        /// <param name="expression">Expression.</param>
        public static bool IsNumeric(object expression)
        {
            if (expression == null)
                return false;

            double number;
            return Double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out number);
        }

        public static bool IsInteger(Type value)
        {
            return (value == typeof(SByte) || value == typeof(Int16) || value == typeof(Int32)
                    || value == typeof(Int64) || value == typeof(Byte) || value == typeof(UInt16)
                    || value == typeof(UInt32) || value == typeof(UInt64));
        }

        public static bool IsFloat(Type value)
        {
            return (value == typeof(float) | value == typeof(double) | value == typeof(Decimal));
        }

        /// <summary>
        /// Finds the child by name.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="target">Target.</param>
        /// <param name="name">Name.</param>
        /// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
        public static GameObject FindChild(this GameObject target, string name, bool includeInactive)
        {
            if (target != null)
            {
                if (target.name == name && includeInactive || target.name == name && !includeInactive && target.activeInHierarchy)
                {
                    return target;
                }
                for (int i = 0; i < target.transform.childCount; ++i)
                {
                    GameObject result = target.transform.GetChild(i).gameObject.FindChild(name, includeInactive);

                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public static void Stretch(this RectTransform rectTransform, RectOffset offset)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = new Vector2(-(offset.right + offset.left), -(offset.bottom + offset.top));
            rectTransform.anchoredPosition = new Vector2(offset.left + rectTransform.sizeDelta.x * rectTransform.pivot.x, -offset.top - rectTransform.sizeDelta.y * (1f - rectTransform.pivot.y));
        }

        public static void Stretch(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
        }

        public static void SetActiveObjectsOfType<T>(bool state) where T : Component
        {

            T[] objects = GameObject.FindObjectsOfType<T>();
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].gameObject.SetActive(state);
            }
        }

        public static void IgnoreCollision(GameObject gameObject1, GameObject gameObject2)
        {
            Collider collider = gameObject2.GetComponent<Collider>();
            if (collider == null) return;
            Collider[] colliders = gameObject1.GetComponentsInChildren<Collider>(true);
            for (int i = 0; i < colliders.Length; i++)
            {
                Physics.IgnoreCollision(colliders[i], collider);
            }
        }

        public static Bounds GetBounds(GameObject obj)
        {
            Bounds bounds = new Bounds();
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            if (renderers.Length > 0)
            {
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds = renderer.bounds;
                        break;
                    }
                }
                foreach (Renderer renderer in renderers)
                {
                    if (renderer.enabled)
                    {
                        bounds.Encapsulate(renderer.bounds);
                    }
                }
            }
            return bounds;
        }

        public static string KeyToCaption(KeyCode key)
        {
            switch (key)
            {
                case KeyCode.None: return "None";
                case KeyCode.Backspace: return "Backspace";
                case KeyCode.Tab: return "Tab";
                case KeyCode.Clear: return "Clear";
                case KeyCode.Return: return "Return";
                case KeyCode.Pause: return "Pause";
                case KeyCode.Escape: return "Esc";
                case KeyCode.Space: return "Space";
                case KeyCode.Exclaim: return "!";
                case KeyCode.DoubleQuote: return "\"";
                case KeyCode.Hash: return "#";
                case KeyCode.Dollar: return "$";
                case KeyCode.Ampersand: return "&";
                case KeyCode.Quote: return "'";
                case KeyCode.LeftParen: return "(";
                case KeyCode.RightParen: return ")";
                case KeyCode.Asterisk: return "*";
                case KeyCode.Plus: return "+";
                case KeyCode.Comma: return ",";
                case KeyCode.Minus: return "-";
                case KeyCode.Period: return ".";
                case KeyCode.Slash: return "/";
                case KeyCode.Alpha0: return "0";
                case KeyCode.Alpha1: return "1";
                case KeyCode.Alpha2: return "2";
                case KeyCode.Alpha3: return "3";
                case KeyCode.Alpha4: return "4";
                case KeyCode.Alpha5: return "5";
                case KeyCode.Alpha6: return "6";
                case KeyCode.Alpha7: return "7";
                case KeyCode.Alpha8: return "8";
                case KeyCode.Alpha9: return "9";
                case KeyCode.Colon: return ":";
                case KeyCode.Semicolon: return ";";
                case KeyCode.Less: return "<";
                case KeyCode.Equals: return "=";
                case KeyCode.Greater: return ">";
                case KeyCode.Question: return "?";
                case KeyCode.At: return "@";
                case KeyCode.LeftBracket: return "[";
                case KeyCode.Backslash: return "\\";
                case KeyCode.RightBracket: return "]";
                case KeyCode.Caret: return "^";
                case KeyCode.Underscore: return "_";
                case KeyCode.BackQuote: return "`";
                case KeyCode.A: return "A";
                case KeyCode.B: return "B";
                case KeyCode.C: return "C";
                case KeyCode.D: return "D";
                case KeyCode.E: return "E";
                case KeyCode.F: return "F";
                case KeyCode.G: return "G";
                case KeyCode.H: return "H";
                case KeyCode.I: return "I";
                case KeyCode.J: return "J";
                case KeyCode.K: return "K";
                case KeyCode.L: return "L";
                case KeyCode.M: return "M";
                case KeyCode.N: return "N";
                case KeyCode.O: return "O";
                case KeyCode.P: return "P";
                case KeyCode.Q: return "Q";
                case KeyCode.R: return "R";
                case KeyCode.S: return "S";
                case KeyCode.T: return "T";
                case KeyCode.U: return "U";
                case KeyCode.V: return "V";
                case KeyCode.W: return "W";
                case KeyCode.X: return "X";
                case KeyCode.Y: return "Y";
                case KeyCode.Z: return "Z";
                case KeyCode.Delete: return "Del";
                case KeyCode.Keypad0: return "K0";
                case KeyCode.Keypad1: return "K1";
                case KeyCode.Keypad2: return "K2";
                case KeyCode.Keypad3: return "K3";
                case KeyCode.Keypad4: return "K4";
                case KeyCode.Keypad5: return "K5";
                case KeyCode.Keypad6: return "K6";
                case KeyCode.Keypad7: return "K7";
                case KeyCode.Keypad8: return "K8";
                case KeyCode.Keypad9: return "K9";
                case KeyCode.KeypadPeriod: return ".";
                case KeyCode.KeypadDivide: return "/";
                case KeyCode.KeypadMultiply: return "*";
                case KeyCode.KeypadMinus: return "-";
                case KeyCode.KeypadPlus: return "+";
                case KeyCode.KeypadEnter: return "NT";
                case KeyCode.KeypadEquals: return "=";
                case KeyCode.UpArrow: return "UP";
                case KeyCode.DownArrow: return "DN";
                case KeyCode.RightArrow: return "LT";
                case KeyCode.LeftArrow: return "RT";
                case KeyCode.Insert: return "Ins";
                case KeyCode.Home: return "Home";
                case KeyCode.End: return "End";
                case KeyCode.PageUp: return "PU";
                case KeyCode.PageDown: return "PD";
                case KeyCode.F1: return "F1";
                case KeyCode.F2: return "F2";
                case KeyCode.F3: return "F3";
                case KeyCode.F4: return "F4";
                case KeyCode.F5: return "F5";
                case KeyCode.F6: return "F6";
                case KeyCode.F7: return "F7";
                case KeyCode.F8: return "F8";
                case KeyCode.F9: return "F9";
                case KeyCode.F10: return "F10";
                case KeyCode.F11: return "F11";
                case KeyCode.F12: return "F12";
                case KeyCode.F13: return "F13";
                case KeyCode.F14: return "F14";
                case KeyCode.F15: return "F15";
                case KeyCode.Numlock: return "Num";
                case KeyCode.CapsLock: return "Caps Lock";
                case KeyCode.ScrollLock: return "Scr";
                case KeyCode.RightShift: return "Shift";
                case KeyCode.LeftShift: return "Shift";
                case KeyCode.RightControl: return "Control";
                case KeyCode.LeftControl: return "Control";
                case KeyCode.RightAlt: return "Alt";
                case KeyCode.LeftAlt: return "Alt";
                case KeyCode.AltGr: return "Alt";
                case KeyCode.Menu: return "Menu";
                case KeyCode.Mouse0: return "Mouse 0";
                case KeyCode.Mouse1: return "Mouse 1";
                case KeyCode.Mouse2: return "M2";
                case KeyCode.Mouse3: return "M3";
                case KeyCode.Mouse4: return "M4";
                case KeyCode.Mouse5: return "M5";
                case KeyCode.Mouse6: return "M6";
                case KeyCode.JoystickButton0: return "(A)";
                case KeyCode.JoystickButton1: return "(B)";
                case KeyCode.JoystickButton2: return "(X)";
                case KeyCode.JoystickButton3: return "(Y)";
                case KeyCode.JoystickButton4: return "(RB)";
                case KeyCode.JoystickButton5: return "(LB)";
                case KeyCode.JoystickButton6: return "(Back)";
                case KeyCode.JoystickButton7: return "(Start)";
                case KeyCode.JoystickButton8: return "(LS)";
                case KeyCode.JoystickButton9: return "(RS)";
                case KeyCode.JoystickButton10: return "J10";
                case KeyCode.JoystickButton11: return "J11";
                case KeyCode.JoystickButton12: return "J12";
                case KeyCode.JoystickButton13: return "J13";
                case KeyCode.JoystickButton14: return "J14";
                case KeyCode.JoystickButton15: return "J15";
                case KeyCode.JoystickButton16: return "J16";
                case KeyCode.JoystickButton17: return "J17";
                case KeyCode.JoystickButton18: return "J18";
                case KeyCode.JoystickButton19: return "J19";
            }
            return null;
        }

        private static void CheckIsEnum<T>(bool withFlags)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException(string.Format("Type '{0}' is not an enum", typeof(T).FullName));
            if (withFlags && !Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
                throw new ArgumentException(string.Format("Type '{0}' doesn't have the 'Flags' attribute", typeof(T).FullName));
        }

        public static bool HasFlag<T>(this T value, T flag) where T : struct
        {
            CheckIsEnum<T>(true);
            long lValue = Convert.ToInt64(value);
            long lFlag = Convert.ToInt64(flag);
            return (lValue & lFlag) != 0;
        }

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            return Handler.StartCoroutine(routine);
        }

        public static Coroutine StartCoroutine(string methodName, object value)
        {
            return Handler.StartCoroutine(methodName, value);
        }

        public static Coroutine StartCoroutine(string methodName)
        {
            return Handler.StartCoroutine(methodName);
        }

        public static void StopCoroutine(IEnumerator routine)
        {
            Handler.StopCoroutine(routine);
        }

        public static void StopCoroutine(string methodName)
        {
            Handler.StopCoroutine(methodName);
        }

        public static void StopCoroutine(Coroutine routine)
        {
            Handler.StopCoroutine(routine);
        }

        public static void StopAllCoroutines()
        {
            Handler.StopAllCoroutines();
        }
    }
}