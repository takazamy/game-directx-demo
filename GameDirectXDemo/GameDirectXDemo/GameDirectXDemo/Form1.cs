using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GameDirectXDemo.Core;
namespace GameDirectXDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Show();
            // Hide the cursor
            this.Cursor.Dispose();
            GameLogic game = new GameLogic(this);
           // game.Initialize();
           // game.GameLoop();
        }
    }
}
