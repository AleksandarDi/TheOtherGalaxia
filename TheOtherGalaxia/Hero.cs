using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheOtherGalaxia.Properties;

namespace TheOtherGalaxia
{
    public class Hero
    {
        //Sets the X coordinate of the ship
        public int X { get; set; }
        
        //Sets the Y coordinate of the ship
        public int Y { get; set; }
        
        //Players health
        public int Health { get; set; }
        
        //The image of the ship
        public Image destroyer { get; set; }
        
        //The Y coordinate of the projectile the hero shoots
        public int ProjectileY { get; set; }
        
        //The X coordinate of the projectile the hero shoots
        public int ProjectileX { get; set; }
        
        //The image of the projectile
        public Image Projectile { get; set; }

        //Sets the initial values of the players health and position aswell as the images of the hero and projectile
        public Hero()
        {
            Projectile = Resources.HeroBeam;
            destroyer = Resources.Destroyer;
            Health = 3;
            X = 350;
            Y = 450;
            
        }

        //Moves the player to the left or right side
        public void Move(string side)
        {
                if (side == "Left" && X-15 > 0)
                {
                    X -=15;
            }
                else if (side == "Right" && X+15 <760)
                {
                    X += 15;
                }
        }

        // Draws and shoots the projectile
        // Returns true if the projectile hasn't left the screen
        // Otherwise returns false and is removed so that the player can shoot again
        public bool Shoot(Graphics g)
        {
            Brush brush = new SolidBrush(Color.White);           
            Rectangle rect = new Rectangle(ProjectileX, ProjectileY, 5, 20);

            g.DrawImageUnscaled(Projectile, ProjectileX, ProjectileY);
            ProjectileY -= 20;
            if(ProjectileY > 0)
            {
                return true;
            }
            else { return false; }
            
        }
    }
}
