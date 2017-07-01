using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WiFiDirect {
	class Program {
		static void Main(string[] args) {
			Console.Title = "Wi-Fi Direct";

			int APIVer;
			IntPtr ClientHandle;
			int Ret = DirectAPI.WFDOpenHandle(DirectAPI.WFD_API_VERSION, out APIVer, out ClientHandle);

			IntPtr SessionHandle;
			DirectAPI.WFDStartOpenSession(ClientHandle, MacAddress.Broadcast, IntPtr.Zero, SessionCallback, out SessionHandle);

			Guid GUID = Guid.NewGuid();
			int Len = Marshal.SizeOf(typeof(Guid));

			Console.WriteLine("Done!");
			Console.ReadLine();
		}

		static void SessionCallback(IntPtr SessionHandle, IntPtr Context, Guid GUIDSessionInterface, int Error, int ReasonCode) {
			Console.WriteLine("Error: {1}", Error, new Win32Exception(Error).Message);

		}
	}
}
