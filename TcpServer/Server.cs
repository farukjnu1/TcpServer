using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace TcpServer
{
    public class Server
    {
        private IPAddress serverIp;
        private int serverPort;
        private TcpListener serverListener;
        private bool isServerRunning = false;

        private Thread backgroundListner;
        private Thread connectionThread;

        List<User> tcpClients = new List<User>();


        public delegate void PropertyChangeHandler(object sender, EventArgs args);
        
        public event PropertyChangeHandler OnclientConnected;

        public event PropertyChangeHandler OnClientSent;


      


        public Server(string serverIp, int serverPort)
        {
            this.serverIp = IPAddress.Parse(serverIp);
            this.serverPort = serverPort;
        }

        public List<User> TcpClients
        {
            get { return tcpClients; }
            set { tcpClients = value; }
        }



        public void startListener()
        {
            this.serverListener = new TcpListener(this.serverIp, this.serverPort);
            this.serverListener.Start();
            this.isServerRunning = true;

            backgroundListner = new Thread(()=>KeepListening());
            backgroundListner.IsBackground = true;
            backgroundListner.Start();

          
        }


        private void KeepListening()
        {
            // While the server is running
            while (this.isServerRunning == true)
            {

                TcpClient tcpClient = this.serverListener.AcceptTcpClient(); // Otherwise this will block the UI // Called when a Client Connect

                connectionThread = new Thread(() => connectionHandler(tcpClient));
                connectionThread.IsBackground = true;
                connectionThread.Start();

                
                // my code
                Thread.Sleep(50);

            }
        }

        private void connectionHandler(TcpClient client)
        {
            TcpClient this_client = client;

            Stream clientStream = this_client.GetStream();

            string rep = this_client.Client.RemoteEndPoint.ToString();
            StreamWriter strWritter = new StreamWriter(clientStream);
            StreamReader strReader = new StreamReader(clientStream);
            string clientMessage = string.Empty;

            // Keep waiting for a message from the user
            while (this.isServerRunning == true)
            {
                try
                {
                    Thread.Sleep(15);

                    if (this_client.Connected)
                    {
                        clientMessage = strReader.ReadLine();

                        // check the user message whether it is in a correct format or not
                        if (clientMessage != null && clientMessage.Contains(';') && clientMessage.Split(';').Length > 1)
                        {
                            string[] response = clientMessage.Split(';');
                            clientMessage = string.Empty;

                            this.TcpClients.RemoveAll(x => x.TcpClient.Connected == false);

                            if (response[0] == "ATx")
                            {
                                // before adding a user check whether it is already exist not not
                                User isUser = this.TcpClients.Where(x => x.UserName == response[1]).FirstOrDefault();
                                if (isUser == null)
                                {
                                    User usr = new User(client, response[1]);
                                    this.TcpClients.Add(usr);

                                    // refresh user list
                                    var uList = this.TcpClients.Where(x => x.TcpClient.Connected == true).ToList();
                                    string str = string.Empty;
                                    foreach (var u in uList)
                                    {
                                        str += u.TcpClient.Client.RemoteEndPoint + "\r\n";
                                    }
                                    OnclientConnected(str + "," + uList.Count, new EventArgs());
                                }
                                else
                                {
                                    // remove existing user
                                    this.TcpClients.RemoveAll(x => x.UserName == response[1]);

                                    // add new user
                                    User usr = new User(client, response[1]);
                                    this.TcpClients.Add(usr);

                                    // refresh user list
                                    var uList = this.TcpClients.Where(x => x.TcpClient.Connected == true).ToList();
                                    string str = string.Empty;
                                    foreach (var u in uList)
                                    {
                                        str += u.TcpClient.Client.RemoteEndPoint + "\r\n";
                                    }
                                    OnclientConnected(str + "," + uList.Count, new EventArgs());
                                }
                            }

                            if (response[0] == "SDx")
                            {
                                string to = response[1];
                                string msgBody = response[2];

                                // refresh user list
                                /*
                                var uList = this.TcpClients.Where(x => x.TcpClient.Connected == true).ToList();
                                string str = string.Empty;
                                foreach (var u in uList)
                                {
                                    str += u.UserName + "\r\n";

                                }
                                OnClientSent(str + "," + uList.Count, new EventArgs());
                                */

                                User source = this.TcpClients.Where(c => c.TcpClient.Equals(this_client)).Select(c => c).FirstOrDefault();
                                User destination = this.TcpClients.Where(c => c.UserName == to && c.TcpClient.Connected == true).Select(c => c).FirstOrDefault();
                                if (destination != null && source != null)
                                {
                                    string client_chat = "RDx;" + source.UserName + ";" + msgBody;


                                    Stream desStream = destination.TcpClient.GetStream();

                                    StreamWriter wr = new StreamWriter(desStream);
                                    wr.WriteLine(client_chat);
                                    wr.Flush();
                                }
                                else
                                {
                                    if (destination == null)
                                    {
                                        this.TcpClients.RemoveAll(x => x.UserName == to);

                                        // refresh user list
                                        var uList1 = this.TcpClients.Where(x => x.TcpClient.Connected == true).ToList();
                                        string str1 = string.Empty;
                                        foreach (var u1 in uList1)
                                        {
                                            str1 += u1.UserName + "\r\n";
                                        }
                                        OnclientConnected(str1 + "," + uList1.Count, new EventArgs());
                                    }
                                }
                            }
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    this.TcpClients.RemoveAll(x => x.TcpClient.Connected == false);

                    // refresh user list
                    var uList1 = this.TcpClients.Where(x => x.TcpClient.Connected == true).ToList();
                    string str1 = string.Empty;
                    foreach (var u1 in uList1)
                    {
                        str1 += u1.UserName + "\r\n";
                    }
                    OnclientConnected(str1 + "," + uList1.Count, new EventArgs());
                }



            }


        }

    }
}
