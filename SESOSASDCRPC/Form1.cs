using DiscordRPC;
using DiscordRPC.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SESOSASDCRPC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        public DiscordRpcClient client;

        public bool started = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if (started)
            {
                Update();
            }
            else
            {
                Initialize();
                textBox1.Enabled = false;
                textBox5.Enabled = false;
                textBox7.Enabled = false;
                button2.Enabled = true;
                button1.Text = "Update";
            }
            SaveAndLoad(true);
        }

        //Called when your application first starts.
        //For example, just before your main loop, on OnEnable for unity.
        void Initialize()
        {
            /*
            Create a Discord client
            NOTE: 	If you are using Unity3D, you must use the full constructor and define
                     the pipe connection.
            */
            client = new DiscordRpcClient(textBox1.Text);

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            /*
            client.OnReady += (sender, e) =>
            {
                listBox1.Items.Add("Received Ready from user " + e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                listBox1.Items.Add("Received Update! " + e.Presence);
            };
            */
            //Connect to the RPC
            client.Initialize();

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            client.SetPresence(new RichPresence()
            {
                Details = textBox2.Text,
                State = textBox3.Text,
                Assets = new Assets()
                {
                    LargeImageKey = textBox4.Text,
                    LargeImageText = textBox5.Text,
                    SmallImageKey = textBox6.Text,
                    SmallImageText = textBox7.Text
                },
                Timestamps = Timestamps.Now
            });

            started = !started;
        }

        void Update()
        {
            client.UpdateDetails(textBox2.Text);
            client.UpdateState(textBox3.Text);
            client.UpdateLargeAsset(textBox4.Text);
            client.UpdateSmallAsset(textBox6.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SaveAndLoad(false);
            button2.Enabled = false;
            button1.Text = "Start";
        }

        void SaveAndLoad(bool save)
        {
            string path = Directory.GetCurrentDirectory() + "/config.cfg";
            try
            {
                if (save)
                {
                    File.WriteAllText(path, $"{textBox1.Text}" + "\n" +
                        $"{textBox2.Text}" + "\n" +
                        $"{textBox3.Text}" + "\n" +
                        $"{textBox6.Text}" + "\n" +
                        $"{textBox5.Text}" + "\n" +
                        $"{textBox4.Text}" + "\n" +
                        $"{textBox7.Text}");
                }
                else
                {
                    if (File.Exists(path))
                    {
                        if (File.ReadAllText(path) != null)
                        {
                            string[] cfg = File.ReadAllLines(path);
                            textBox1.Text = cfg[0];
                            textBox2.Text = cfg[1];
                            textBox3.Text = cfg[2];
                            textBox6.Text = cfg[3];
                            textBox5.Text = cfg[4];
                            textBox4.Text = cfg[5];
                            textBox7.Text = cfg[6];
                        }
                    }
                    else
                    {
                        File.WriteAllText(Directory.GetCurrentDirectory() + "/config.cfg", "");
                    }
                }
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client.ClearPresence();
            button2.Enabled = false;
            button1.Text = "Start";
            started = !started;
        }
    }
}
