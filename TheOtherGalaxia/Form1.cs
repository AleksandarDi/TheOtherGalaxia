using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheOtherGalaxia.Properties;

namespace TheOtherGalaxia
{
    public partial class Form1 : Form
    {
        bool showStartMenu = true;

        bool isFired, isLeftPressed, isRightPressed, restart, startGame, wonGame, showInstructions;

        bool gameWon = true;

        Timer gameRestartTimer;

        List<List<Alien>> aliens;

        Hero destroyer;

        Timer timer;

        List<Image> lives;

        Brush startBrush, instructionBrush;

        bool option = true;

        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbfont, uint cbfont, IntPtr pdv, [In] ref uint pcFonts);

        FontFamily ff;
        Font bitFont;

        public Form1()
        {
            startBrush = new SolidBrush(Color.White);

            instructionBrush = new SolidBrush(Color.Red);

            loadFont();

            newGame();

            gameRestartTimer = new Timer();
            gameRestartTimer.Interval = 100;
            gameRestartTimer.Tick += new EventHandler(gameRestartTimer_Tick);
            timer = new Timer();
            timer.Interval = 5;
            timer.Tick += new EventHandler(timer_Tick);

            this.DoubleBuffered = true;

            InitializeComponent();

            timer.Start();
        }

        private void loadFont()
        {
            byte[] fontArray = TheOtherGalaxia.Properties.Resources._8_BIT_WONDER;
            int dataLength = TheOtherGalaxia.Properties.Resources._8_BIT_WONDER.Length;

            IntPtr ptrData = Marshal.AllocCoTaskMem(dataLength);

            Marshal.Copy(fontArray, 0, ptrData, dataLength);

            uint cFonts = 0;

            AddFontMemResourceEx(ptrData, (uint)fontArray.Length, IntPtr.Zero, ref cFonts);

            PrivateFontCollection pfc = new PrivateFontCollection();

            pfc.AddMemoryFont(ptrData, dataLength);

            Marshal.FreeCoTaskMem(ptrData);

            ff = pfc.Families[0];
            bitFont = new Font(ff, 25f);
            

        }

        private void gameRestartTimer_Tick(object o, EventArgs e)
        {
            if (wonGame)
            {
                gameIsWon();
            }
            else if (restart)
            {
                restartGame();
            }
        }

        public void newGame()
        {
            destroyer = new Hero();
            lives = new List<Image>();
            lives.Add(Resources.smallDestroyer_Lives);
            lives.Add(Resources.smallDestroyer_Lives);
            lives.Add(Resources.smallDestroyer_Lives);
            aliens = new List<List<Alien>>();

            AddAliens();

            if (restart || wonGame)
            {
                gameRestartTimer.Stop();
                timer.Start();
                restart = false;
                wonGame = false;
            }
        }

        public void AddAliens()
        {
            List<Alien> firstRow = new List<Alien>();
            int X = 200;
            int Y = 50;
            int StopLeft = 0;
            int StopRight = 280;
            //First row Aliens
            for (int i = 0; i < 10; i++)
            {
                Alien al = new Alien();
                if (i == 1 || i == 4 || i == 7)
                {
                    al.alien = Resources.spaceship6;
                    al.Name = "spaceship6";
                    al.Health = 3;

                }
                else {
                    al.alien = Resources.spaceship1;
                    al.Name = "spaceship1";
                    al.Health = 1;
                }

                al.isAlive = true;
                al.StopLeft = StopLeft + 10;
                al.StopRight = StopRight + 10;
                al.X = X;
                al.Y = Y;
                X += 50;
                StopLeft += 50;
                StopRight += 50;
                firstRow.Add(al);
            }
            aliens.Add(firstRow);
            //Second row of Aliens
            List<Alien> secondRow = new List<Alien>();
            X = 250;
            Y += 50;
            StopLeft = 50;
            StopRight = 330;
            for (int i = 0; i < 8; i++)
            {
                Alien al = new Alien();
                if (i % 3 == 0)
                {
                    al.alien = Resources.spaceship3;
                    al.Name = "spaceship3";
                    al.Health = 2;
                }
                else
                {
                    al.alien = Resources.spaceship2;
                    al.Name = "spaceship2";
                    al.Health = 1;
                }
                al.isAlive = true;
                al.StopLeft = StopLeft + 10;
                al.StopRight = StopRight + 10;
                al.X = X;
                al.Y = Y;
                X += 50;
                StopLeft += 50;
                StopRight += 50;
                secondRow.Add(al);
            }
            aliens.Add(secondRow);
            //Third row of aliens
            List<Alien> thirdRow = new List<Alien>();
            X = 300;
            Y += 50;
            StopLeft = 100;
            StopRight = 380;
            for (int i = 0; i < 6; i++)
            {
                Alien al = new Alien();
                al.Health = 1;
                al.alien = Resources.spaceship7;
                al.Name = "spaceship7";
                al.isAlive = true;
                al.StopLeft = StopLeft + 10;
                al.StopRight = StopRight + 10;
                al.X = X;
                al.Y = Y;
                X += 50;
                StopLeft += 50;
                StopRight += 50;
                thirdRow.Add(al);
            }
            aliens.Add(thirdRow);

        }

        public void timer_Tick(object o, EventArgs e)
        {
            if (!showStartMenu)
            {
                moveHero();
                if (startGame)
                    alienShoot();
                isHeroHit();

                if (isFired)
                {
                    Graphics g = CreateGraphics();
                    isFired = destroyer.Shoot(g);
                    checkHit(destroyer.ProjectileX, destroyer.ProjectileY);
                }
            }
            Invalidate(true);
        }

        private void isHeroHit()
        {
            Rectangle rect = new Rectangle(destroyer.X, destroyer.Y + 10, 60, 82);
            for (int i = 0; i < aliens.Count(); i++)
            {
                for (int j = 0; j < aliens[i].Count(); j++)
                {
                    Rectangle alien = new Rectangle(aliens[i][j].ProjectileX, aliens[i][j].ProjectileY, 11, 20);
                    if (rect.IntersectsWith(alien))
                    {
                        aliens[i][j].isFired = false;
                        aliens[i][j].ProjectileX = 0;
                        aliens[i][j].ProjectileY = 0;
                        destroyer.Health -= 1;
                        lives.RemoveAt(0);
                        if (destroyer.Health == 0)
                        {


                            gameOver();
                        }
                    }
                }
            }
        }

        private void alienShoot()
        {

            Random r = new Random();
            for (int i = 0; i < aliens.Count(); i++)
            {
                for (int j = 0; j < aliens[i].Count(); j++)
                {
                    if (aliens[i][j].isAlive)
                    {
                        int broj = r.Next(0, 1000);
                        if (aliens[i][j].isFired == false && aliens[i][j].Name.Equals("spaceship6"))
                        {

                            if (broj < 50)
                            {
                                aliens[i][j].ProjectileX = aliens[i][j].X + 20;
                                aliens[i][j].ProjectileY = aliens[i][j].Y + 50;
                                aliens[i][j].ProjectileXSpeed = (destroyer.X + 25 - aliens[i][j].X) / 20;
                                aliens[i][j].ProjectileYSpeed = (destroyer.Y + 10 - aliens[i][j].Y) / 20;
                                aliens[i][j].isFired = true;
                            }
                        }
                        else if (aliens[i][j].isFired == false && broj < 14)
                        {
                            aliens[i][j].ProjectileX = aliens[i][j].X + 20;
                            aliens[i][j].ProjectileY = aliens[i][j].Y + 50;
                            aliens[i][j].ProjectileXSpeed = 0;
                            aliens[i][j].ProjectileYSpeed = 20;
                            aliens[i][j].isFired = true;
                        }
                    }

                    Graphics g = CreateGraphics();
                    aliens[i][j].Shoot(g);

                }
            }
        }

        private void moveHero()
        {
            if (isLeftPressed)
            {
                destroyer.Move("Left");
            }
            else if (isRightPressed)
            {
                destroyer.Move("Right");
            }
        }

        private void checkHit(int X, int Y)
        {
            Rectangle rect = new Rectangle(X, Y, 11, 20);
            for (int i = 0; i < aliens.Count(); i++)
            {
                for (int j = 0; j < aliens[i].Count(); j++)
                {
                    Rectangle alien = new Rectangle(aliens[i][j].X, aliens[i][j].Y, 40, 40);
                    if (rect.IntersectsWith(alien))
                    {
                        if (aliens[i][j].isAlive == true)
                        {
                            destroyer.ProjectileX = 800;
                            destroyer.ProjectileY = 900;
                            aliens[i][j].Health -= 1;
                            if (aliens[i][j].Health == 0)
                            {
                                aliens[i][j].isAlive = false;
                            }
                            isFired = false;
                        }
                    }

                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (showStartMenu)
            {
                if (showInstructions)
                {
                    Instructions(e.Graphics);
                }
                else
                startMenu(e.Graphics);
            }
            else
            {
                gameWon = true;
                for (int i = 0; i < aliens.Count(); i++)
                {
                    for (int j = 0; j < aliens[i].Count(); j++)
                    {
                        if (aliens[i][j].isAlive == true)
                        {
                            gameWon = false;
                            e.Graphics.DrawImageUnscaled(aliens[i][j].alien, aliens[i][j].X, aliens[i][j].Y);
                            aliens[i][j].Move();
                        }
                    }
                }
                e.Graphics.DrawImageUnscaled(destroyer.destroyer, destroyer.X, destroyer.Y);
                drawLives(e.Graphics);
                if (gameWon)
                    gameState();
            }

        }

        private void startMenu(Graphics g)
        {
            float x = 130;
            float y = 50;

            g.DrawString("The Other Galaxia", bitFont, new SolidBrush(Color.Red), x, y);
            g.DrawString("Start Game", bitFont, startBrush, x + 90, y + 150);
            g.DrawString("Instructions", bitFont, instructionBrush, x + 80, y + 200);

        }

        private void Instructions(Graphics g)
        {
            float x = 130;
            float y = 50;

            g.DrawString("The Other Galaxia", bitFont, new SolidBrush(Color.Red), x, y);

            g.DrawString("Are you ready to protect the humans from alien", new Font(ff,13f), new SolidBrush(Color.White), x-100, y+150);

            g.DrawString("invaders? Shoot the aliens with your laser while", new Font(ff, 13f), new SolidBrush(Color.White), x-100, y+170);

            g.DrawString("dodging their attacks.", new Font(ff, 13f), new SolidBrush(Color.White), x-100, y+190);

            g.DrawString("Use the arrow keys to move left and right.", new Font(ff, 15f), new SolidBrush(Color.White), x-100, y+310);

            g.DrawString("Press space to shoot.", new Font(ff, 14f), new SolidBrush(Color.White), x-100, y+350);

        }

        private void drawLives(Graphics g)
        {
            int X = 10;
            int Y = 500;
            for (int i = 0; i < lives.Count(); i++)
            {
                g.DrawImageUnscaled(lives[i], X, Y);
                X += 20;
            }
        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && isFired == false)
            {
                destroyer.ProjectileY = destroyer.Y - 20;
                destroyer.ProjectileX = destroyer.X + 25;
                isFired = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!showStartMenu)
            {

                if (e.KeyCode.ToString() == "Left")
                {
                    startGame = true;
                    isLeftPressed = true;
                }
                else if (e.KeyCode.ToString() == "Right")
                {

                    startGame = true;
                    isRightPressed = true;
                }
                if (e.KeyCode.ToString() == "Space" && isFired == false)
                {
                    startGame = true;
                    destroyer.ProjectileY = destroyer.Y - 20;
                    destroyer.ProjectileX = destroyer.X + 25;
                    isFired = true;
                }
            }
            if (e.KeyCode.ToString() == "Y" && (restart || wonGame))
            {
                newGame();
            }
            if (e.KeyCode.ToString() == "N" && (restart || wonGame))
            {
                Close();
            }

            if (showStartMenu)
            {
                if (e.KeyCode.ToString() == "Down")
                {
                    Brush temp = startBrush;
                    startBrush = instructionBrush;
                    instructionBrush = temp;
                    option = !option;
                } else if (e.KeyCode.ToString() == "Up")
                    {
                    Brush temp = startBrush;
                    startBrush = instructionBrush;
                    instructionBrush = temp;
                    option = !option;
                }
                if(e.KeyCode.ToString() == "Return")
                {
                    if (option)
                    {
                        showStartMenu = false;
                    }
                    else if (!option)
                    {
                        showInstructions = true;
                    }
                }
                if(e.KeyCode.ToString() == "Back")
                {
                    showInstructions = false;
                }
            }

        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.ToString() == "Left")
            {
                isLeftPressed = false;
            }
            if (e.KeyCode.ToString() == "Right")
            {
                isRightPressed = false;
            }
        }

        private void gameState()
        {
            wonGame = true;
            timer.Stop();

            gameRestartTimer.Start();
        }

        private void gameIsWon()
        {
            Graphics g = CreateGraphics();
            Brush brush = new SolidBrush(Color.White);
            float x = 130;
            float y = 50;
            g.DrawString("Congratulations you Won", bitFont, brush, x-90, y);
            g.DrawString("Play again", bitFont, brush, x + 90, y + 150);
            g.DrawString("(Y/N)", bitFont, brush, x + 110, y + 200);

        }
        private void gameOver()
        {
            destroyer.destroyer = Resources.Explosion;
            timer.Stop();
            startGame = false;
            gameRestartTimer.Start();
            restart = true;
        }

        private void restartGame()
        {
            Graphics g = CreateGraphics();
            Brush brush = new SolidBrush(Color.White);
            float x = 250;
            float y = 250;
            g.DrawString("Restart game?", bitFont, brush, x, y);
            g.DrawString("(Y/N)", bitFont, brush, x + 80, y + 50);
        }
    }
}
