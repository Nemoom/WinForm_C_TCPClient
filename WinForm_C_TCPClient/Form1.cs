using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }
        private Socket client;
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                System.Media.SystemSounds.Beep.Play();
                System.Threading.Thread.Sleep(100);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                System.Media.SystemSounds.Asterisk.Play();
                System.Threading.Thread.Sleep(300);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                System.Media.SystemSounds.Exclamation.Play();
                System.Threading.Thread.Sleep(300);
                
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                System.Media.SystemSounds.Hand.Play();
                System.Threading.Thread.Sleep(300);
            }
        }
        string str_IP;
        private void Form1_Load(object sender, EventArgs e)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            using (StreamReader sr = new StreamReader("ip.txt",Encoding.Default))
            {
                str_IP = sr.ReadLine();
            }
            System.Threading.Thread T_tryConnect = new System.Threading.Thread(tryConnect);
            T_tryConnect.IsBackground = true;
            T_tryConnect.Start();
        }

        private void tryConnect()
        {
            while (!client.Connected)
            {
                try
                {
                    client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    client.Connect(new IPEndPoint(IPAddress.Parse(str_IP), 60000));
                    //client.Connect(new IPEndPoint(IPAddress.Parse("172.16.141.58"), 60000));
                }
                catch (Exception)
                {

                }
                
            }
            b_response = false;
            toolStripStatusLabel1.Text = "已连接";
            System.Threading.Thread T_ReceiveMsg = new System.Threading.Thread(ReceiveMsg);
            T_ReceiveMsg.IsBackground = true;
            T_ReceiveMsg.Start();
        }

        private void ReceiveMsg()
        {
            while (true)
            {
                try
                {
                    byte[] b_msg = new byte[1024];
                    int length = client.Receive(b_msg);
                    if (length ==3)
                    {
                        toolStripStatusLabel1.Text = "有人找";
                        b_response = false;
                        while (!b_response)                        
                        {
                            System.Media.SystemSounds.Asterisk.Play();
                            System.Threading.Thread.Sleep(300);
                            //System.Media.SystemSounds.Beep.Play();
                            //System.Threading.Thread.Sleep(100);
                        }
                        b_response = false;
                    }

                }
                catch (Exception)
                {
                    client.Close();
                    toolStripStatusLabel1.Text = "连接中断";
                    break;
                }
            }
            b_response = true;
            System.Threading.Thread T_tryConnect = new System.Threading.Thread(tryConnect);
            T_tryConnect.IsBackground = true;
            T_tryConnect.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }
        bool b_response = false;
        private void button3_Click(object sender, EventArgs e)
        {
            b_response = true;
            client.Send(Encoding.UTF8.GetBytes("OK"));
            toolStripStatusLabel1.Text = "已处理";
        }
    }
}
