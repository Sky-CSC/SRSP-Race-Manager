﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.SignalR;

namespace RaceManager.Communication
{
    // https://docs.microsoft.com/en-us/dotnet/framework/network-programming/asynchronous-server-socket-example?redirectedfrom=MSDN
    // State object for reading client data asynchronously
    //public class StateObject
    //{
    //    // Client  socket.
    //    public Socket workSocket = null;
    //    // Size of receive buffer.
    //    public const int BufferSize = 1024;
    //    // Receive buffer.
    //    public byte[] buffer = new byte[BufferSize];
    //    // Received data string. 
    //    public StringBuilder sb = new StringBuilder();
    //}

    public partial class AsyncServer
    {
        private static List<Client> clients = new List<Client>();
        private static RMLogger _logger = new RMLogger(LoggingLevel.DEBUG, "AsyncServer");
        public static Thread thread = new Thread(new ThreadStart(StartListening));
        public static int Port { get; set; } = 45879;

        // Semaphore
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsyncServer()
        {

        }

        public static void Run()
        {
            try
            {
                thread.Start();
                _logger.log(LoggingLevel.INFO, "Run()", "Starting server");
            }
            catch (Exception e)
            {
                _logger.log(LoggingLevel.ERROR, "Run()", "Error while starting server: " + e.Message);
            }
        }

        public static void Stop()
        {
            try
            {
                thread.Interrupt();
                _logger.log(LoggingLevel.INFO, "Stop()", "Stopping server");
            }

            catch (Exception e)
            {
                _logger.log(LoggingLevel.ERROR, "Stop()", "Error stopping server: " + e.Message);
            }
        }

        public static void StartListening()
        {
            byte[] bytes = new byte[1024];

            // Réservation du port d'écoute selon l'ip du serveur.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            _logger.log(LoggingLevel.INFO,"StartListening()", $"Host address : {ipAddress}");
            
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);
            Socket listener = new Socket(AddressFamily.InterNetworkV6,
                SocketType.Stream, ProtocolType.Tcp);
            listener.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {

                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    _logger.log(LoggingLevel.DEBUG, "StartListening()", $"Waiting for a connection... {Port}");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                _logger.log(LoggingLevel.ERROR, "StartListening()", "Error while listening: " + e.Message);
            }


            _logger.log(LoggingLevel.DEBUG, "AsyncServer.StartListening()", $"Listen is over.");

        }

        /// <summary>
        /// Here we treat if the new client already exists or not, 
        /// then create a new one or update the existing one
        /// </summary>
        /// <param name="ar"></param>
        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            Client client = new Client();
            client.Handler = handler;

            handler.BeginReceive(client.buffer, 0, Client.BufferSize, 0,
                new AsyncCallback(ReadCallback), client);
        }
    }
}
