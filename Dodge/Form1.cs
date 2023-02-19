using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dodge
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Asteroid> astlAsteroids;       // store asteroids

        SolidBrush playerBrush = new SolidBrush(Color.Green);
        Rectangle recPlayer;
        
        int iCounter;
        int iCursorX;       // store cursor position relative to form window
        int iCursorY;

        const int iPlayerSize = 20;

        private void NewGame()
        {
            progressBar1.Value = 0;
            progressBar1.Maximum = 10;

            recPlayer = new Rectangle(ClientSize.Width / 2 - iPlayerSize / 2, ClientSize.Height / 2 - iPlayerSize / 2, iPlayerSize, iPlayerSize);

            iCounter = 0;

            astlAsteroids = new List<Asteroid>();

            for (int i = 0; i < 5; i++) // add 5 asteroids initially
            {
                astlAsteroids.Add(new Asteroid(ClientSize.Width, ClientSize.Height, recPlayer));
            }

            timer1.Interval = 1;        // refresh animation every 1 ms
            timer1.Enabled = true;

            timer2.Interval = 1000;     // refresh progress every 1 s
            timer2.Enabled = true;

            Cursor.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            NewGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            iCounter++;

            if (iCounter > 55)      // add an asteroid every 55 ms
            {
                iCounter = 0;
                astlAsteroids.Add(new Asteroid(ClientSize.Width, ClientSize.Height, recPlayer));
            }

            // Check if asteroid left the game domain. If yes, create a new asteroid.
            for (int i = 0; i < astlAsteroids.Count; i++)
            {
                if (astlAsteroids[i].ReachedWall(ClientSize.Width, ClientSize.Height, progressBar1.Height))
                {
                    astlAsteroids.RemoveAt(i);
                    astlAsteroids.Add(new Asteroid(ClientSize.Width, ClientSize.Height, recPlayer));
                }
            }

            // Check if player collided with any asteroid. If yes, stop the game.
            foreach (Asteroid currentAsteroid in astlAsteroids)
            {
                if (currentAsteroid.Intersects(recPlayer))
                {
                    Cursor.Show();
                    timer2.Enabled = false;
                    timer1.Enabled = false;
                    MessageBox.Show("You collided with an asteroid. Press [enter] to start a new game.", "Game over", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Move asteroids.
            foreach (Asteroid currentAsteroid in astlAsteroids)
            {
                currentAsteroid.Move();
            }

            // Set player coordinates to cursor coordinates. Also make sure that player can't escape the game domain.
            iCursorX = this.PointToClient(Cursor.Position).X;
            iCursorY = this.PointToClient(Cursor.Position).Y;

            // Set boundaries for player in x direction.
            if (iCursorX < iPlayerSize / 2)
            {
                recPlayer.X = 0;
            } 
            else if (iCursorX > ClientSize.Width - iPlayerSize / 2)
            {
                recPlayer.X = ClientSize.Width - iPlayerSize;
            }
            else
            {
                recPlayer.X = iCursorX;
            }

            // Set boundaries for player in y direction.
            if (iCursorY < iPlayerSize / 2 + progressBar1.Height)
            {
                recPlayer.Y = progressBar1.Height;
            }
            else if (iCursorY > ClientSize.Height - iPlayerSize / 2)
            {
                recPlayer.Y = ClientSize.Height - iPlayerSize;
            }
            else
            {
                recPlayer.Y = iCursorY;
            }

            // refresh
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillEllipse(playerBrush, recPlayer);

            foreach(Asteroid currentAsteroid in astlAsteroids)
            {
                currentAsteroid.Draw(e.Graphics);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                NewGame();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            progressBar1.Value += 1;        // increment progress on every second passed

            if (progressBar1.Value == progressBar1.Maximum)
            {
                Cursor.Show();
                timer1.Enabled = false;
                timer2.Enabled = false;
                MessageBox.Show("You survived, congratulations! Press [enter] to start a new game.", "Victory!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
    }
}
