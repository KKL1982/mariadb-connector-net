// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation; version 3 of the License.
//
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License
// for more details.
//
// You should have received a copy of the GNU Lesser General Public License along
// with this program; if not, write to the Free Software Foundation, Inc.,
// 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using MariaDB.Data.MySqlClient.Properties;

namespace MariaDB.Data.Common
{
<<<<<<< HEAD
	/// <summary>
	/// Summary description for StreamCreator.
	/// </summary>
	internal class StreamCreator
	{
		private string hostList;
		private uint port;
		private uint timeOut;
		private uint keepalive;

#if !DNX
		private string pipeName;

		public StreamCreator(string hosts, uint port, string pipeName, uint keepalive)
#else
		public StreamCreator(string hosts, uint port, uint keepalive)
#endif
		{
			hostList = hosts;
			if (hostList == null || hostList.Length == 0)
				hostList = "localhost";
			this.port = port;
#if !DNX
			this.pipeName = pipeName;
#endif
			this.keepalive = keepalive;
		}

#if !DNX
		private Stream GetStreamFromHost(string pipeName, string hostName, uint timeout)
		{
			if (pipeName != null && pipeName.Length != 0)
				return NamedPipeStream.Create(pipeName, hostName, timeout);
#else
		private Stream GetStreamFromHost(string hostName, uint timeout)
		{
#endif
			Stream stream = null;
			IPHostEntry ipHE = GetHostEntry(hostName);
			foreach (IPAddress address in ipHE.AddressList)
			{
<<<<<<< HEAD
<<<<<<< HEAD
				try
				{
					stream = CreateSocketStream(address, false);
					if (stream != null) break;
				}
				catch (Exception ex)
				{
					SocketException socketException = ex as SocketException;
					// if the exception is a ConnectionRefused then we eat it as we may have other address
					// to attempt
					if (socketException == null) throw;
					if (socketException.SocketErrorCode != SocketError.ConnectionRefused) throw;
=======
=======
    /// <summary>
    /// Summary description for StreamCreator.
    /// </summary>
    internal class StreamCreator
    {
        private string hostList;
        private uint port;
        private string pipeName;
        private uint timeOut;
        private uint keepalive;

        public StreamCreator(string hosts, uint port, string pipeName, uint keepalive)
        {
            hostList = hosts;
            if (hostList == null || hostList.Length == 0)
                hostList = "localhost";
            this.port = port;
            this.pipeName = pipeName;
            this.keepalive = keepalive;
        }

        private Stream GetStreamFromHost(string pipeName, string hostName, uint timeout)
        {
            Stream stream = null;
            if (pipeName != null && pipeName.Length != 0)
            {
>>>>>>> parent of f34caa9... c# code for a basic MariaDB and MySql connector for dotnet open source
#if !CF
                stream = NamedPipeStream.Create(pipeName, hostName, timeout);
#endif
            }
            else
            {
                IPHostEntry ipHE = GetHostEntry(hostName);
                foreach (IPAddress address in ipHE.AddressList)
                {
                    try
                    {
                        stream = CreateSocketStream(address, false);
                        if (stream != null) break;
                    }
                    catch (Exception ex)
                    {
                        SocketException socketException = ex as SocketException;
                        // if the exception is a ConnectionRefused then we eat it as we may have other address
                        // to attempt
                        if (socketException == null) throw;
#if !CF
                        if (socketException.SocketErrorCode != SocketError.ConnectionRefused) throw;
#endif
<<<<<<< HEAD
=======
#if !CF
				stream = NamedPipeStream.Create(pipeName, hostName, timeout);
#endif
			}
			else
			{
				IPHostEntry ipHE = GetHostEntry(hostName);
				foreach (IPAddress address in ipHE.AddressList)
				{
					try
					{
						stream = CreateSocketStream(address, false);
						if (stream != null) break;
					}
					catch (Exception ex)
					{
						SocketException socketException = ex as SocketException;
						// if the exception is a ConnectionRefused then we eat it as we may have other address
						// to attempt
						if (socketException == null) throw;
#if !CF
						if (socketException.SocketErrorCode != SocketError.ConnectionRefused) throw;
#endif
>>>>>>> parent of 529ee02... fix typo
					}
>>>>>>> parent of 529ee02... fix typo
				}
			}

			return stream;
		}
=======
                    }
                }
            }
            return stream;
        }
>>>>>>> parent of f34caa9... c# code for a basic MariaDB and MySql connector for dotnet open source

        public Stream GetStream(uint timeout)
        {
            timeOut = timeout;

            if (hostList.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                return CreateSocketStream(null, true);

            string[] dnsHosts = hostList.Split(',');

            Random random = new Random((int)DateTime.Now.Ticks);
            int index = random.Next(dnsHosts.Length);
            int pos = 0;
            Stream stream = null;

<<<<<<< HEAD
			while (stream == null && pos < dnsHosts.Length)
			{
#if !DNX
				stream = GetStreamFromHost(pipeName, dnsHosts[index++], timeout);
#else
				stream = GetStreamFromHost(dnsHosts[index++], timeout);
#endif
				if (index == dnsHosts.Length) index = 0;
				pos++;
			}
			return stream;
		}
=======
            while (stream == null && pos < dnsHosts.Length)
            {
                stream = GetStreamFromHost(pipeName, dnsHosts[index++], timeout);
                if (index == dnsHosts.Length) index = 0;
                pos++;
            }
            return stream;
        }
>>>>>>> parent of f34caa9... c# code for a basic MariaDB and MySql connector for dotnet open source

        private IPHostEntry ParseIPAddress(string hostname)
        {
            IPHostEntry ipHE = null;
#if !CF
            IPAddress addr;
            if (IPAddress.TryParse(hostname, out addr))
            {
                ipHE = new IPHostEntry();
                ipHE.AddressList = new IPAddress[1];
                ipHE.AddressList[0] = addr;
            }
#endif
            return ipHE;
        }

#if CF
		IPHostEntry GetDnsHostEntry(string hostname)
		{
<<<<<<< HEAD
			return Dns.GetHostEntry(hostname);
=======
			IPHostEntry ipHE = null;
#if !CF
			IPAddress addr;
			if (IPAddress.TryParse(hostname, out addr))
			{
				ipHE = new IPHostEntry();
				ipHE.AddressList = new IPAddress[1];
				ipHE.AddressList[0] = addr;
			}
#endif
			return ipHE;
>>>>>>> parent of 529ee02... fix typo
		}
#else

<<<<<<< HEAD
        private IPHostEntry GetDnsHostEntry(string hostname)
        {
            LowResolutionStopwatch stopwatch = new LowResolutionStopwatch();
=======
#if CF
		IPHostEntry GetDnsHostEntry(string hostname)
		{
			return Dns.GetHostEntry(hostname);
		}
#else

		private IPHostEntry GetDnsHostEntry(string hostname)
		{
			LowResolutionStopwatch stopwatch = new LowResolutionStopwatch();
>>>>>>> parent of 529ee02... fix typo

            try
            {
                stopwatch.Start();
                return Dns.GetHostEntry(hostname);
            }
            catch (SocketException ex)
            {
                string message = String.Format(Resources.GetHostEntryFailed,
                stopwatch.Elapsed, hostname, ex.SocketErrorCode,
                ex.ErrorCode, ex.NativeErrorCode);
                throw new Exception(message, ex);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

#endif

<<<<<<< HEAD
        private IPHostEntry GetHostEntry(string hostname)
        {
            IPHostEntry ipHE = ParseIPAddress(hostname);
            if (ipHE != null) return ipHE;
            return GetDnsHostEntry(hostname);
        }

#if !CF

<<<<<<< HEAD
=======
		private IPHostEntry GetHostEntry(string hostname)
		{
			IPHostEntry ipHE = ParseIPAddress(hostname);
			if (ipHE != null) return ipHE;
			return GetDnsHostEntry(hostname);
		}

#if !CF

>>>>>>> parent of 529ee02... fix typo
		private static EndPoint CreateUnixEndPoint(string host)
		{
#if !DNX
			// first we need to load the Mono.posix assembly
			Assembly a = Assembly.Load(@"Mono.Posix, Version=2.0.0.0,
				Culture=neutral, PublicKeyToken=0738eb9f132ed756");

			// then we need to construct a UnixEndPoint object
			EndPoint ep = (EndPoint)a.CreateInstance("Mono.Posix.UnixEndPoint",
				false, BindingFlags.CreateInstance, null,
				new object[1] { host }, null, null);
			return ep;
#else
			return null;
#endif
		}
=======
        private static EndPoint CreateUnixEndPoint(string host)
        {
            // first we need to load the Mono.posix assembly
            Assembly a = Assembly.Load(@"Mono.Posix, Version=2.0.0.0,
				Culture=neutral, PublicKeyToken=0738eb9f132ed756");

<<<<<<< HEAD
            // then we need to construct a UnixEndPoint object
            EndPoint ep = (EndPoint)a.CreateInstance("Mono.Posix.UnixEndPoint",
                false, BindingFlags.CreateInstance, null,
                new object[1] { host }, null, null);
            return ep;
        }
>>>>>>> parent of f34caa9... c# code for a basic MariaDB and MySql connector for dotnet open source

#endif

        private Stream CreateSocketStream(IPAddress ip, bool unix)
        {
            EndPoint endPoint;
#if !CF
            if (!Platform.IsWindows() && unix)
                endPoint = CreateUnixEndPoint(hostList);
            else
#endif
                endPoint = new IPEndPoint(ip, (int)port);

            Socket socket = unix ?
                new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP) :
                new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            if (keepalive > 0)
            {
                SetKeepAlive(socket, keepalive);
            }
            IAsyncResult ias = socket.BeginConnect(endPoint, null, null);
            if (!ias.AsyncWaitHandle.WaitOne((int)timeOut * 1000, false))
            {
                socket.Close();
                return null;
            }
            try
            {
                socket.EndConnect(ias);
            }
            catch (Exception)
            {
                socket.Close();
                throw;
            }
            MyNetworkStream stream = new MyNetworkStream(socket, true);
            GC.SuppressFinalize(socket);
            GC.SuppressFinalize(stream);
            return stream;
        }

        /// <summary>
        /// Set keepalive + timeout on socket.
        /// </summary>
        /// <param name="s">socket</param>
        /// <param name="time">keepalive timeout, in seconds</param>
        private static void SetKeepAlive(Socket s, uint time)
        {
#if !CF
            uint on = 1;
            uint interval = 1000; // default interval = 1 sec

            uint timeMilliseconds;
            if (time > UInt32.MaxValue / 1000)
                timeMilliseconds = UInt32.MaxValue;
            else
                timeMilliseconds = time * 1000;

<<<<<<< HEAD
<<<<<<< HEAD
=======
			// Use Socket.IOControl to implement equivalent of
			// WSAIoctl with  SOL_KEEPALIVE_VALS
=======
            // Use Socket.IOControl to implement equivalent of
            // WSAIoctl with  SOL_KEEPALIVE_VALS
>>>>>>> parent of f34caa9... c# code for a basic MariaDB and MySql connector for dotnet open source

            // the native structure passed to WSAIoctl is
            //struct tcp_keepalive {
            //    ULONG onoff;
            //    ULONG keepalivetime;
            //    ULONG keepaliveinterval;
            //};
            // marshal the equivalent of the native structure into a byte array

<<<<<<< HEAD
>>>>>>> parent of 529ee02... fix typo
=======
#endif

		private Stream CreateSocketStream(IPAddress ip, bool unix)
		{
			EndPoint endPoint;
#if !CF
			if (!Platform.IsWindows() && unix)
				endPoint = CreateUnixEndPoint(hostList);
			else
#endif
				endPoint = new IPEndPoint(ip, (int)port);

			Socket socket = unix ?
				new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.IP) :
				new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			if (keepalive > 0)
			{
				SetKeepAlive(socket, keepalive);
			}
			IAsyncResult ias = socket.BeginConnect(endPoint, null, null);
			if (!ias.AsyncWaitHandle.WaitOne((int)timeOut * 1000, false))
			{
				socket.Close();
				return null;
			}
			try
			{
				socket.EndConnect(ias);
			}
			catch (Exception)
			{
				socket.Close();
				throw;
			}
			MyNetworkStream stream = new MyNetworkStream(socket, true);
			GC.SuppressFinalize(socket);
			GC.SuppressFinalize(stream);
			return stream;
		}

		/// <summary>
		/// Set keepalive + timeout on socket.
		/// </summary>
		/// <param name="s">socket</param>
		/// <param name="time">keepalive timeout, in seconds</param>
		private static void SetKeepAlive(Socket s, uint time)
		{
#if !CF
			uint on = 1;
			uint interval = 1000; // default interval = 1 sec

			uint timeMilliseconds;
			if (time > UInt32.MaxValue / 1000)
				timeMilliseconds = UInt32.MaxValue;
			else
				timeMilliseconds = time * 1000;

			// Use Socket.IOControl to implement equivalent of
			// WSAIoctl with  SOL_KEEPALIVE_VALS

			// the native structure passed to WSAIoctl is
			//struct tcp_keepalive {
			//    ULONG onoff;
			//    ULONG keepalivetime;
			//    ULONG keepaliveinterval;
			//};
			// marshal the equivalent of the native structure into a byte array

>>>>>>> parent of 529ee02... fix typo
			byte[] inOptionValues = new byte[12];
			BitConverter.GetBytes(on).CopyTo(inOptionValues, 0);
			BitConverter.GetBytes(time).CopyTo(inOptionValues, 4);
			BitConverter.GetBytes(interval).CopyTo(inOptionValues, 8);
			try
			{
				// call WSAIoctl via IOControl
				s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
				return;
			}
			catch (NotImplementedException)
			{
				// Mono throws not implemented currently
			}
<<<<<<< HEAD
=======
            byte[] inOptionValues = new byte[12];
            BitConverter.GetBytes(on).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes(time).CopyTo(inOptionValues, 4);
            BitConverter.GetBytes(interval).CopyTo(inOptionValues, 8);
            try
            {
                // call WSAIoctl via IOControl
                s.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
                return;
            }
            catch (NotImplementedException)
            {
                // Mono throws not implemented currently
            }
>>>>>>> parent of f34caa9... c# code for a basic MariaDB and MySql connector for dotnet open source
#endif
            // Fallback if Socket.IOControl is not available ( Compact Framework )
            // or not implemented ( Mono ). Keepalive option will still be set, but
            // with timeout is kept default.
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
        }
    }
=======
#endif
			// Fallback if Socket.IOControl is not available ( Compact Framework )
			// or not implemented ( Mono ). Keepalive option will still be set, but
			// with timeout is kept default.
			s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
		}
	}
>>>>>>> parent of 529ee02... fix typo
}