using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WiFiDirect {
	public unsafe delegate void OpenSessionCallback(IntPtr SessionHandle, IntPtr Context, Guid GUIDSessionInterface, int Error, int ReasonCode);

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct MacAddress {
		public fixed byte Address[6];

		public MacAddress(byte[] Addr) {
			if (Addr.Length != 6)
				throw new Exception("Mac address has to be 6 bytes long");

			fixed (byte* AddressPtr = Address) {
				for (int i = 0; i < 6; i++)
					AddressPtr[i] = Addr[i];
			}
		}

		public MacAddress(byte A, byte B, byte C, byte D, byte E, byte F) : this(new byte[] { A, B, C, D, E, F }) {
		}

		public static readonly MacAddress Broadcast = new MacAddress(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF);
	}

	public static unsafe class DirectAPI {
		const string DllName = "Wlanapi";
		const CallingConvention CConv = CallingConvention.Winapi;
		const CharSet CSet = CharSet.Ansi;

		public const int WFD_API_VERSION = 0x1;

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		public static extern int WFDOpenHandle(int ClientVersion, out int NegotiatedVersion, out IntPtr ClientHandle);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		public static extern int WFDCloseHandle(IntPtr ClientHandle);

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		public static extern int WFDStartOpenSession(IntPtr ClientHandle, ref MacAddress MacAddress, IntPtr Context, OpenSessionCallback Callback, out IntPtr SessionHandle);

		public static int WFDStartOpenSession(IntPtr ClientHandle, MacAddress MacAddress, IntPtr Context, OpenSessionCallback Callback, out IntPtr SessionHandle) {
			return WFDStartOpenSession(ClientHandle, ref MacAddress, Context, Callback, out SessionHandle);
		}

		[DllImport(DllName, CallingConvention = CConv, CharSet = CSet)]
		public static extern int WFDCloseSession(IntPtr SessionHandle);
	}
}
