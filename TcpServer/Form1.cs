using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Data.SqlClient;

namespace TcpServer
{
    public partial class Form1 : Form
    {
        Server server;

        public Form1()
        {
            InitializeComponent();
            label3.Text = "Server is Stop";
        }

        private void startListnerButton_Click(object sender, EventArgs e)
        {


            server = new Server(ipAddressTextbox.Text.Trim(), int.Parse(portTextBox.Text.Trim()));
            server.startListener();
            server.OnclientConnected += new Server.PropertyChangeHandler(server_OnclientConnected);

            label3.Text = "Server is Up and Running";

            server.OnClientSent += new Server.PropertyChangeHandler(server_OnClientSent);
        }

        void server_OnclientConnected(object sender, EventArgs args)
        {
            // User usr = (User)sender;
            // show in win form
            string users = sender.ToString().Split(',')[0];
            string nUsers = sender.ToString().Split(',')[1];

            this.Invoke(new MethodInvoker(delegate()
                {
                    textBox1.Text = users;
                    label4.Text = "Conencted Clients: " + nUsers;
                }));

            // save in text file
            /*
            string path = @"D:\c#project\Tcp Apps\LiveDevice.txt";

            if (File.Exists(path))
            {
                File.WriteAllText(path, users);
            }
            else
            {
                File.Create(path).Close();
                File.WriteAllText(path, users);
            }
             */

            // save in database \r\n
            string[] uArray = users.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            // make all user off line
            SqlConnection con1;
            SqlCommand cmd1;
            string conStr = "Data Source=localhost;Initial Catalog=TwoStepDb;User ID=sa;Password=123";
            con1 = new SqlConnection(conStr);
            cmd1 = new SqlCommand();
            cmd1.Connection = con1;
            con1.Open();
            cmd1.CommandText = "UPDATE [dbo].[Tbl_TrackLast] SET Live = @Live";
            cmd1.Parameters.Add(new SqlParameter("Live", false));
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();
            con1.Close();

            // make Live user on line
            for (int i = 0; i < uArray.Length; i++)
            {
                SqlConnection con;
                SqlCommand cmd;
                con = new SqlConnection(conStr);
                cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE [dbo].[Tbl_TrackLast] SET Live = @Live WHERE Tid = @Tid";
                cmd.Parameters.Add(new SqlParameter("Live", true));
                cmd.Parameters.Add(new SqlParameter("Tid", uArray[i]));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();   
            }
        }

        void server_OnClientSent(object sender, EventArgs args)
        {
            string users = sender.ToString().Split(',')[0];
            string nUsers = sender.ToString().Split(',')[1];

            this.Invoke(new MethodInvoker(delegate()
            {
                textBox1.Text = users;
                label4.Text = "Conencted Clients: " + nUsers;
            }));

            // save in text file
            /*
            string path = @"D:\c#project\Tcp Apps\LiveDevice.txt";

            if (File.Exists(path))
            {
                File.WriteAllText(path, users);
            }
            else
            {
                File.Create(path).Close();
                File.WriteAllText(path, users);
            }
            */

            // save in database \r\n
            string[] uArray = users.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            // make all user off line
            SqlConnection con1;
            SqlCommand cmd1;
            string conStr = "Data Source=localhost;Initial Catalog=TwoStepDb;User ID=sa;Password=123";
            con1 = new SqlConnection(conStr);
            cmd1 = new SqlCommand();
            cmd1.Connection = con1;
            con1.Open();
            cmd1.CommandText = "UPDATE [dbo].[Tbl_TrackLast] SET Live = @Live";
            cmd1.Parameters.Add(new SqlParameter("Live", false));
            cmd1.ExecuteNonQuery();
            cmd1.Dispose();
            con1.Close();

            // make Live user on line
            for (int i = 0; i < uArray.Length; i++)
            {
                SqlConnection con;
                SqlCommand cmd;
                con = new SqlConnection(conStr);
                cmd = new SqlCommand();
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE [dbo].[Tbl_TrackLast] SET Live = @Live WHERE Tid = @Tid";
                cmd.Parameters.Add(new SqlParameter("Live", true));
                cmd.Parameters.Add(new SqlParameter("Tid", uArray[i]));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
            }
        }

        private void stopListnerButton_Click(object sender, EventArgs e)
        {
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
