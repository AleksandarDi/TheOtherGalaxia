using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOtherGalaxia.Properties;

namespace TheOtherGalaxia
{
    public class Alien
    {
        public int X { get; set; }

        public int Y { get; set; }

        public string Name { get; set; }

        public int Health { get; set; }

        public Image alien { get; set; }

        public bool isAlive { get; set; }

        public int speed = -10;

        public int StopLeft { get; set; }

        public int StopRight { get; set; }

        public Image Projectile { get; set; }

        public int ProjectileX { get; set; }

        public int ProjectileY { get; set; }

        public int ProjectileXSpeed { get; set; }

        public int ProjectileYSpeed { get; set; }

        public bool isFired { get; set; }

        public Alien()
        {
            StopLeft = 0;
            StopRight = 750;
            Projectile = Resources.AlienBeam;
            isAlive = true;
        }

        public void Move()
        {
            if (X-10 < StopLeft || X + 10 > StopRight)
            {
                speed = -speed;
            }
            X += speed;

        }

        public void Shoot(Graphics g)
        {
            if (isFired == true)
            {

                g.DrawImageUnscaled(Projectile, ProjectileX, ProjectileY);
                ProjectileX += ProjectileXSpeed;
                ProjectileY += ProjectileYSpeed;
                if (ProjectileY > 500)
                {
                    isFired = false;
                    ProjectileX = 0;
                    ProjectileY = 0;
                }
            }
        }

    }
}
