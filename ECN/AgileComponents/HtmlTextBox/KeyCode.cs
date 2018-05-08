using System;

namespace ActiveUp.WebControls
{
	/// <summary>
	/// Key code enumeration.
	/// </summary>
	public enum KeyCode
	{
		/// <summary>
		/// Key code.
		/// </summary>
		KeyCode = 65535,

		/// <summary>
		/// Modifier key code.
		/// </summary>
		Modifiers = -65536,

		/// <summary>
		/// None key code.
		/// </summary>
		None = 0,

		/// <summary>
		/// Left button key code.
		/// </summary>
		LButton = 1,

		/// <summary>
		/// Right button key code.
		/// </summary>
		RButton = 2,

		/// <summary>
		/// Cancel key code.
		/// </summary>
		Cancel = 3,

		/// <summary>
		/// Medium button key code.
		/// </summary>
		MButton = 4,

		/// <summary>
		/// Button 1 key code.
		/// </summary>
		XButton1 = 5,

		/// <summary>
		/// Button 2 key code.
		/// </summary>
		XButton2 = 6,

		/// <summary>
		/// Back key code.
		/// </summary>
		Back = 8,

		/// <summary>
		/// Tab key code.
		/// </summary>
		Tab = 9,

		/// <summary>
		/// Line feed key code.
		/// </summary>
		LineFeed = 10,

		/// <summary>
		/// Clear key code.
		/// </summary>
		Clear = 12,

		/// <summary>
		/// Returns key code.
		/// </summary>
		Return = 13,

		/// <summary>
		/// Enter key code.
		/// </summary>
		Enter = 13,

		/// <summary>
		/// Shift key code.
		/// </summary>
		ShiftKey = 16,

		/// <summary>
		/// Control key code.
		/// </summary>
		ControlKey = 17,

		/// <summary>
		/// Menu key code.
		/// </summary>
		Menu = 18,

		/// <summary>
		/// Pause key code.
		/// </summary>
		Pause = 19,

		/// <summary>
		/// Capital key code.
		/// </summary>
		Capital = 20,

		/// <summary>
		/// Caps lock key code.
		/// </summary>
		CapsLock = 20,

		/// <summary>
		/// Escape key code.
		/// </summary>
		Escape = 27,

		/// <summary>
		/// Space key code.
		/// </summary>
		Space = 32,

		/// <summary>
		/// Prior key code.
		/// </summary>
		Prior = 33,

		/// <summary>
		/// Page up key code.
		/// </summary>
		PageUp = 33,

		/// <summary>
		/// Next key code.
		/// </summary>
		Next = 34,

		/// <summary>
		/// Page down key code.
		/// </summary>
		PageDown = 34,

		/// <summary>
		/// End key code.
		/// </summary>
		End = 35,

		/// <summary>
		/// Home key code.
		/// </summary>
		Home = 36,

		/// <summary>
		/// Left key code.
		/// </summary>
		Left = 37,

		/// <summary>
		/// Up key code.
		/// </summary>
		Up = 38,

		/// <summary>
		/// Right key code.
		/// </summary>
		Right = 39,

		/// <summary>
		/// Down key code.
		/// </summary>
		Down = 40,

		/// <summary>
		/// Select key code.
		/// </summary>
		Select = 41,

		/// <summary>
		/// Print key code.
		/// </summary>
		Print = 42,

		/// <summary>
		/// Execute key code.
		/// </summary>
		Execute = 43,

		/// <summary>
		/// Snapshot key code.
		/// </summary>
		Snapshot = 44,

		/// <summary>
		/// Print screen key code.
		/// </summary>
		PrintScreen = 44,

		/// <summary>
		/// Insert key code.
		/// </summary>
		Insert = 45,

		/// <summary>
		/// Delete key code.
		/// </summary>
		Delete = 46,

		/// <summary>
		/// Help key code.
		/// </summary>
		Help = 47,

		/// <summary>
		/// D0 key code.
		/// </summary>
		D0 = 48,

		/// <summary>
		/// D1 key code.
		/// </summary>
		D1 = 49,

		/// <summary>
		/// D2 key code.
		/// </summary>
		D2 = 50,

		/// <summary>
		/// D3 key code.
		/// </summary>
		D3 = 51,

		/// <summary>
		/// D4 key code.
		/// </summary>
		D4 = 52,

		/// <summary>
		/// D5 key code.
		/// </summary>
		D5 = 53,

		/// <summary>
		/// D6 key code.
		/// </summary>
		D6 = 54,

		/// <summary>
		/// D7 key code.
		/// </summary>
		D7 = 55,

		/// <summary>
		/// D8 key code.
		/// </summary>
		D8 = 56,

		/// <summary>
		/// D9 key code.
		/// </summary>
		D9 = 57,

		/// <summary>
		/// A key code.
		/// </summary>
		A = 65,

		/// <summary>
		/// B key code.
		/// </summary>
		B = 66,

		/// <summary>
		/// C key code.
		/// </summary>
		C = 67,

		/// <summary>
		/// D key code.
		/// </summary>
		D = 68,

		/// <summary>
		/// E key code.
		/// </summary>
		E = 69,

		/// <summary>
		/// F key code.
		/// </summary>
		F = 70,

		/// <summary>
		/// G key code.
		/// </summary>
		G = 71,

		/// <summary>
		/// H key code.
		/// </summary>
		H = 72,

		/// <summary>
		/// I key code.
		/// </summary>
		I = 73,

		/// <summary>
		/// J key code.
		/// </summary>
		J = 74,

		/// <summary>
		/// K key code.
		/// </summary>
		K = 75,

		/// <summary>
		/// L key code.
		/// </summary>
		L = 76,

		/// <summary>
		/// M key code.
		/// </summary>
		M = 77,

		/// <summary>
		/// N key code.
		/// </summary>
		N = 78,

		/// <summary>
		/// O key code.
		/// </summary>
		O = 79,

		/// <summary>
		/// P key code.
		/// </summary>
		P = 80,

		/// <summary>
		/// Q key code.
		/// </summary>
		Q = 81,

		/// <summary>
		/// R key code.
		/// </summary>
		R = 82,

		/// <summary>
		/// S key code.
		/// </summary>
		S = 83,

		/// <summary>
		/// T key code.
		/// </summary>
		T = 84,

		/// <summary>
		/// U key code.
		/// </summary>
		U = 85,

		/// <summary>
		/// V key code.
		/// </summary>
		V = 86,

		/// <summary>
		/// W key code.
		/// </summary>
		W = 87,

		/// <summary>
		/// X key code.
		/// </summary>
		X = 88,

		/// <summary>
		/// Y key code.
		/// </summary>
		Y = 89,

		/// <summary>
		/// Z key code.
		/// </summary>
		Z = 90,

		/// <summary>
		/// Left window key code.
		/// </summary>
		LWin = 91,

		/// <summary>
		/// Right window key code.
		/// </summary>
		RWin = 92,

		/// <summary>
		/// Application key code.
		/// </summary>
		Apps = 93,

		/// <summary>
		/// Numeric pad 0 key code.
		/// </summary>
		NumPad0 = 96,

		/// <summary>
		/// Numeric pad 1 key code.
		/// </summary>
		NumPad1 = 97,

		/// <summary>
		/// Numeric pad 2 key code.
		/// </summary>
		NumPad2 = 98,

		/// <summary>
		/// Numeric pad 3 key code.
		/// </summary>
		NumPad3 = 99,

		/// <summary>
		/// Numeric pad 4 key code.
		/// </summary>
		NumPad4 = 100,

		/// <summary>
		/// Numeric pad 5 key code.
		/// </summary>
		NumPad5 = 101,

		/// <summary>
		/// Numeric pad 6 key code.
		/// </summary>
		NumPad6 = 102,

		/// <summary>
		/// Numeric pad 7 key code.
		/// </summary>
		NumPad7 = 103,

		/// <summary>
		/// Numeric pad 8 key code.
		/// </summary>
		NumPad8 = 104,

		/// <summary>
		/// Numeric pad 9 key code.
		/// </summary>
		NumPad9 = 105,

		/// <summary>
		/// Multiply key code.
		/// </summary>
		Multiply = 106,

		/// <summary>
		/// Add key code.
		/// </summary>
		Add = 107,

		/// <summary>
		/// Separator key code.
		/// </summary>
		Separator = 108,

		/// <summary>
		/// Substract key code.
		/// </summary>
		Subtract = 109,

		/// <summary>
		/// Decimal key code.
		/// </summary>
		Decimal = 110,

		/// <summary>
		/// Divide key code.
		/// </summary>
		Divide = 111,

		/// <summary>
		/// F1 key code.
		/// </summary>
		F1 = 112,

		/// <summary>
		/// F2 key code.
		/// </summary>
		F2 = 113,

		/// <summary>
		/// F3 key code.
		/// </summary>
		F3 = 114,

		/// <summary>
		/// F4 key code.
		/// </summary>
		F4 = 115,

		/// <summary>
		/// F5 key code.
		/// </summary>
		F5 = 116,

		/// <summary>
		/// F6 key code.
		/// </summary>
		F6 = 117,

		/// <summary>
		/// F7 key code.
		/// </summary>
		F7 = 118,

		/// <summary>
		/// F8 key code.
		/// </summary>
		F8 = 119,

		/// <summary>
		/// F9 key code.
		/// </summary>
		F9 = 120,

		/// <summary>
		/// F10 key code.
		/// </summary>
		F10 = 121,

		/// <summary>
		/// F11 key code.
		/// </summary>
		F11 = 122,

		/// <summary>
		/// F12 key code.
		/// </summary>
		F12 = 123,

		/// <summary>
		/// F13 key code.
		/// </summary>
		F13 = 124,

		/// <summary>
		/// F14 key code.
		/// </summary>
		F14 = 125,

		/// <summary>
		/// F15 key code.
		/// </summary>
		F15 = 126,

		/// <summary>
		/// F16 key code.
		/// </summary>
		F16 = 127,

		/// <summary>
		/// F17 key code.
		/// </summary>
		F17 = 128,

		/// <summary>
		/// F18 key code.
		/// </summary>
		F18 = 129,

		/// <summary>
		/// F19 key code.
		/// </summary>
		F19 = 130,

		/// <summary>
		/// F20 key code.
		/// </summary>
		F20 = 131,

		/// <summary>
		/// F21 key code.
		/// </summary>
		F21 = 132,

		/// <summary>
		/// F22 key code.
		/// </summary>
		F22 = 133,

		/// <summary>
		/// F23 key code.
		/// </summary>
		F23 = 134,

		/// <summary>
		/// F24 key code.
		/// </summary>
		F24 = 135,

		/// <summary>
		/// Numeric lock key code.
		/// </summary>
		NumLock = 144,

		/// <summary>
		/// Scroll key code.
		/// </summary>
		Scroll = 145,

		/// <summary>
		/// Left shift key code.
		/// </summary>
		LShiftKey = 160,

		/// <summary>
		/// Righ shift key code.
		/// </summary>
		RShiftKey = 161,

		/// <summary>
		/// Left control key code.
		/// </summary>
		LControlKey = 162,

		/// <summary>
		/// Right control key code.
		/// </summary>
		RControlKey = 163,

		/// <summary>
		/// Left menu key code.
		/// </summary>
		LMenu = 164,

		/// <summary>
		/// Right menu key code.
		/// </summary>
		RMenu = 165,

		/// <summary>
		/// Process key code.
		/// </summary>
		ProcessKey = 229,

		/// <summary>
		/// Attn key code.
		/// </summary>
		Attn = 246,

		/// <summary>
		/// Cursor selection key code.
		/// </summary>
		Crsel = 247,

		/// <summary>
		/// External selection key code.
		/// </summary>
		Exsel = 248,

		/// <summary>
		/// Erase end of file key code.
		/// </summary>
		EraseEof = 249,

		/// <summary>
		/// Play key code.
		/// </summary>
		Play = 250,

		/// <summary>
		/// Zoom key code.
		/// </summary>
		Zoom = 251,

		/// <summary>
		/// No name key code.
		/// </summary>
		NoName = 252,

		/// <summary>
		/// Pa 1 key code.
		/// </summary>
		Pa1 = 253,

		/// <summary>
		/// Oem clear key code.
		/// </summary>
		OemClear = 254,

		/// <summary>
		/// Kana mode key code.
		/// </summary>
		KanaMode = 21,

		/// <summary>
		/// Hanguel mode key code.
		/// </summary>
		HanguelMode = 21,

		/// <summary>
		/// Hangul mode key code.
		/// </summary>
		HangulMode = 21,

		/// <summary>
		/// Janja mode key code.
		/// </summary>
		JunjaMode = 23,

		/// <summary>
		/// Final mode key code.
		/// </summary>
		FinalMode = 24,

		/// <summary>
		/// Handja mode key code.
		/// </summary>
		HanjaMode = 25,

		/// <summary>
		/// Kanji mode key code.
		/// </summary>
		KanjiMode = 25,

		/// <summary>
		/// IME convert key code.
		/// </summary>
		IMEConvert = 28,

		/// <summary>
		/// IME non convert key code.
		/// </summary>
		IMENonconvert = 29,

		/// <summary>
		/// IME accept key code.
		/// </summary>
		IMEAceept = 30,

		/// <summary>
		/// IME mode change key code.
		/// </summary>
		IMEModeChange = 31,

		/// <summary>
		/// Browser back key code.
		/// </summary>
		BrowserBack = 166,

		/// <summary>
		/// Browser forward key code.
		/// </summary>
		BrowserForward = 167,

		/// <summary>
		/// Browser refresh key code.
		/// </summary>
		BrowserRefresh = 168,

		/// <summary>
		/// Browser stop key code.
		/// </summary>
		BrowserStop = 169,

		/// <summary>
		/// Browser search key code.
		/// </summary>
		BrowserSearch = 170,

		/// <summary>
		/// Browser favorite key code.
		/// </summary>
		BrowserFavorites = 171,

		/// <summary>
		/// Browser home key code.
		/// </summary>
		BrowserHome = 172,

		/// <summary>
		/// Volume mute key code.
		/// </summary>
		VolumeMute = 173,

		/// <summary>
		/// Volume down key code.
		/// </summary>
		VolumeDown = 174,

		/// <summary>
		/// Volume up key code.
		/// </summary>
		VolumeUp = 175,

		/// <summary>
		/// Media next track key code.
		/// </summary>
		MediaNextTrack = 176,

		/// <summary>
		/// Media previous track key code.
		/// </summary>
		MediaPreviousTrack = 177,

		/// <summary>
		/// Media stop key code.
		/// </summary>
		MediaStop = 178,

		/// <summary>
		/// Media play pause key code.
		/// </summary>
		MediaPlayPause = 179,

		/// <summary>
		/// Launch mail key code.
		/// </summary>
		LaunchMail = 180,

		/// <summary>
		/// Select media key code.
		/// </summary>
		SelectMedia = 181,

		/// <summary>
		/// Launch application 1 key code.
		/// </summary>
		LaunchApplication1 = 182,

		/// <summary>
		/// Launch application 2 key code.
		/// </summary>
		LaunchApplication2 = 183,

		/// <summary>
		/// Oem semi colon key code.
		/// </summary>
		OemSemicolon = 186,

		/// <summary>
		/// Oem plus key code.
		/// </summary>
		Oemplus = 187,

		/// <summary>
		/// Oem comma key code.
		/// </summary>
		Oemcomma = 188,

		/// <summary>
		/// Oem minus key code.
		/// </summary>
		OemMinus = 189,

		/// <summary>
		/// Oem period key code.
		/// </summary>
		OemPeriod = 190,

		/// <summary>
		/// Oem question key code.
		/// </summary>
		OemQuestion = 191,

		/// <summary>
		/// Oem tilde key code.
		/// </summary>
		Oemtilde = 192,

		/// <summary>
		/// Oem open brackets key code.
		/// </summary>
		OemOpenBrackets = 219,

		/// <summary>
		/// Oem pipe key code.
		/// </summary>
		OemPipe = 220,

		/// <summary>
		/// Oem close brackets key code.
		/// </summary>
		OemCloseBrackets = 221,

		/// <summary>
		/// Oem quotes key code.
		/// </summary>
		OemQuotes = 222,

		/// <summary>
		/// Oem 8 key code.
		/// </summary>
		Oem8 = 223,

		/// <summary>
		/// Oem back slash key code.
		/// </summary>
		OemBackslash = 226,

		/// <summary>
		/// Shift key code.
		/// </summary>
		Shift = 65536,

		/// <summary>
		/// Control key code.
		/// </summary>
		Control = 131072,

		/// <summary>
		/// Alt key code.
		/// </summary>
		Alt = 262144,

	}
}
