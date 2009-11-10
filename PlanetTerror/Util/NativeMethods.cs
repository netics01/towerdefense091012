using System;
using System.Runtime.InteropServices;


namespace PlanetTerror.Util
{
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	NativeStructs
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// 	public static class NativeStructs
// 	{
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/commctls/tooltip/structures/toolinfo.asp
// 		[StructLayout(LayoutKind.Sequential)]
// 		public struct TOOLINFO
// 		{
// 			public int cbSize;
// 			public int uFlags;
// 			public IntPtr hwnd;
// 			public IntPtr uId;
// 			public RECT rect;
// 			public IntPtr hinst;
// 			[MarshalAs(UnmanagedType.LPTStr)]
// 			public string lpszText;
// 			public uint lParam;
// 		}
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_6cqa.asp
// 		[StructLayout(LayoutKind.Sequential)]
// 		public struct RECT
// 		{
// 			public int left;
// 			public int top;
// 			public int right;
// 			public int bottom;
// 		}
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/power/base/system_power_status_str.asp
// 		//[StructLayout(LayoutKind.Sequential)]
// 		public struct SYSTEM_POWER_STATUS
// 		{
// 			public byte ACLineStatus;  // One of the ACLineStatus values
// 			public byte BatteryFlag;  // One of the BatteryFlag values
// 			public byte BatteryLifePercent; // 0-100, or 255 if the status is unknown
// 			public byte Reserved1;            // must be 0
// 			public int BatteryLifeTime; // seconds of battery life remaining, or ? if unknown
// 			public int BatteryFullLifeTime; // seconds of battery life when at full charge, or ? if unknown
// 		}
// 
// 		// Encapsulate the magic numbers for the return value in an enumeration
// 		public enum ReturnCodes : int
// 		{
// 			DISP_CHANGE_SUCCESSFUL=0,
// 			DISP_CHANGE_BADDUALVIEW=-6,
// 			DISP_CHANGE_BADFLAGS=-4,
// 			DISP_CHANGE_BADMODE=-2,
// 			DISP_CHANGE_BADPARAM=-5,
// 			DISP_CHANGE_FAILED=-1,
// 			DISP_CHANGE_NOTUPDATED=-3,
// 			DISP_CHANGE_RESTART=1
// 		}
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/prntspol_8nle.asp
// 		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi)]
// 		public struct DevMode
// 		{
// 			// The MarshallAs attribute is covered in the Background section of the article
// 			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
// 			public string dmDeviceName;
// 
// 			public short dmSpecVersion;
// 			public short dmDriverVersion;
// 			public short dmSize;
// 			public short dmDriverExtra;
// 			public int dmFields;
// 			public int dmPositionX;
// 			public int dmPositionY;
// 			public int dmDisplayOrientation;
// 			public int dmDisplayFixedOutput;
// 			public short dmColor;
// 			public short dmDuplex;
// 			public short dmYResolution;
// 			public short dmTTOption;
// 			public short dmCollate;
// 
// 			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=32)]
// 			public string dmFormName;
// 
// 			public short dmLogPixels;
// 			public short dmBitsPerPel;
// 			public int dmPelsWidth;
// 			public int dmPelsHeight;
// 			public int dmDisplayFlags;
// 			public int dmDisplayFrequency;
// 			public int dmICMMethod;
// 			public int dmICMIntent;
// 			public int dmMediaType;
// 			public int dmDitherType;
// 			public int dmReserved1;
// 			public int dmReserved2;
// 			public int dmPanningWidth;
// 			public int dmPanningHeight;
// 
// 			public override string ToString()
// 			{
// 				return dmPelsWidth.ToString() + " x " + dmPelsHeight.ToString();
// 			}
// 
// 			public string[] GetInfoArray()
// 			{
// 				string[] items = new string[5];
// 
// 				items[0] = dmDeviceName;
// 				items[1] = dmPelsWidth.ToString();
// 				items[2] = dmPelsHeight.ToString();
// 				items[3] = dmDisplayFrequency.ToString();
// 				items[4] = dmBitsPerPel.ToString();
// 
// 				return items;
// 			}
// 		}
// 	}

	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	//	NativeMethods
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	public class NativeMethods
	{
// 		//===============================================================================================================================================
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/messagesandmessagequeues/messagesandmessagequeuesreference/messagesandmessagequeuesfunctions/sendmessage.asp
// 		[DllImport("User32", SetLastError=true)]
// 		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowfunctions/getclientrect.asp
// 		[DllImport("User32", SetLastError=true)]
// 		public static extern int GetClientRect(IntPtr hWnd, ref NativeStructs.RECT lpRect);
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/cordspac_2tny.asp
// 		[DllImport("User32", SetLastError=true)]
// 		public static extern int ClientToScreen(IntPtr hWnd, ref NativeStructs.RECT lpRect);

		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/windowing/windows/windowreference/windowfunctions/setwindowpos.asp
		[DllImport("User32", SetLastError=true)]
		public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, int uFlags);

		[DllImport("User32", SetLastError=true)]
		public static extern int SetActiveWindow(IntPtr hWnd);

// 		public static int MAKELONG(int loWord, int hiWord)
// 		{
// 			return (hiWord << 16) | (loWord & 0xffff);
// 		}

		public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);


// 		public const int WM_USER = 0x0400;
// 		public const int TTM_TRACKACTIVATE = WM_USER + 17;
// 		public const int TTM_TRACKPOSITION = WM_USER + 18;
// 		public const int TTM_SETMAXTIPWIDTH = WM_USER + 24;
// 		public const int TTM_SETTITLE = WM_USER + 33;
// 		public const int TTM_ADDTOOL = WM_USER + 50;
// 
// 		public const int WS_POPUP = unchecked((int)0x80000000);
// 
// 		public const string TOOLTIPS_CLASS = "tooltips_class32";
// 
// 		public const int TTS_BALLOON = 0x40;
// 		public const int TTS_NOPREFIX = 0x02;
// 		public const int TTS_ALWAYSTIP = 0x01;
// 		public const int TTS_CLOSE = 0x80;
// 
// 		public const int TTF_IDISHWND = 0x0001;
// 		public const int TTF_SUBCLASS = 0x0010;
// 		public const int TTF_TRACK = 0x0020;
// 		public const int TTF_ABSOLUTE = 0x0080;
// 		public const int TTF_TRANSPARENT = 0x0100;
// 		public const int TTF_CENTERTIP = 0x0002;
// 		public const int TTF_PARSELINKS = 0x1000;

		public const int SWP_NOSIZE = 0x0001;
		public const int SWP_NOMOVE = 0x0002;
		public const int SWP_NOACTIVATE = 0x0010;
		public const int SWP_NOZORDER = 0x0004;

// 		//===============================================================================================================================================
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/sysinfo/base/systemparametersinfo.asp
// 		[DllImport("user32.dll")]
// 		public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
// 
// 		public const int SPI_SETDESKWALLPAPER = 20;
// 		public const int SPIF_UPDATEINIFILE = 0x01;
// 		public const int SPIF_SENDWININICHANGE = 0x02;
// 
// 
// 		//===============================================================================================================================================
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/winui/winui/windowsuserinterface/resources/icons/iconreference/iconfunctions/extracticonex.asp
// 		[DllImport("Shell32.dll", EntryPoint="ExtractIconExW", CharSet=CharSet.Unicode, ExactSpelling=true, CallingConvention=CallingConvention.StdCall)]
// 		public static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion,
// 			out IntPtr piSmallVersion, int amountIcons);
// 
// 
// 		//===============================================================================================================================================
// 		// BOOL GetSystemPowerStatus(LPSYSTEM_POWER_STATUS lpSystemPowerStatus);
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/power/base/getsystempowerstatus.asp
// 		[DllImport("kernel32.dll")]
// 		public static extern bool GetSystemPowerStatus(ref NativeStructs.SYSTEM_POWER_STATUS lpSystemPowerStatus);

		[System.Flags]
		public enum PlaySoundFlags : int
		{
			SND_SYNC=0x0000,
			SND_ASYNC=0x0001,
			SND_NODEFAULT=0x0002,
			SND_LOOP=0x0008,
			SND_NOSTOP=0x0010,
			SND_NOWAIT=0x00002000,
			SND_FILENAME=0x00020000,
			SND_RESOURCE=0x00040004
		}

		[System.Runtime.InteropServices.DllImport("winmm.DLL", EntryPoint="PlaySound", SetLastError=true)]
		public static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

// 		//===============================================================================================================================================
// 		public const int MIN_VOLUME = 0x0000;
// 		public const int MAX_VOLUME = 0xFFFF;
// 
// 		// Error constants
// 		public const int MMSYSERR_NOERROR = 0;
// 		public const int MMSYSERR_BADDEVICEID = 2;
// 		public const int MMSYSERR_INVALHANDLE = 5;
// 		public const int MMSYSERR_NODRIVER = 6;
// 		public const int MMSYSERR_NOMEM = 7;
// 		public const int MMSYSERR_NOTSUPPORTED = 8;
// 		public const int MMSYSERR_INVALPARAM = 11;
// 
// 		// See the article Task 1 Declartion section for how to convert the unmanged types to the managed types for each function
// 
// 
// 		// Wave files
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_waveoutgetvolume.asp
// 		[DllImport("winmm.dll")]
// 		public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_waveoutsetvolume.asp
// 		[DllImport("winmm.dll")]
// 		public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
// 
// 
// 		// aux devices
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_auxgetvolume.asp
// 		[DllImport("winmm.dll")]
// 		public static extern int auxGetVolume(int uDeviceID, out uint dwVolume);
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_auxsetvolume.asp
// 		[DllImport("winmm.dll")]
// 		public static extern int auxSetVolume(int uDeviceID, uint dwVolume);
// 
// 
// 		// midi devices
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_midioutgetvolume.asp
// 		[DllImport("winmm.dll")]
// 		public static extern int midiOutGetVolume(IntPtr hmo, out uint dwVolume);
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/multimed/htm/_win32_midioutsetvolume.asp
// 		[DllImport("winmm.dll")]
// 		public static extern int midiOutSetVolume(IntPtr hmo, uint dwVolume);
// 
// 		//===============================================================================================================================================
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/devcons_84oj.asp
// 		[DllImport("user32.dll", CharSet=CharSet.Ansi)]
// 		public static extern int EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref NativeStructs.DevMode lpDevMode);
// 
// 		// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/devcons_7gz7.asp
// 		[DllImport("user32.dll", CharSet=CharSet.Ansi)]
// 		public static extern NativeStructs.ReturnCodes ChangeDisplaySettings(ref NativeStructs.DevMode lpDevMode, int dwFlags);
// 
// 
// 		public const int ENUM_CURRENT_SETTINGS = -1;
	}
}