# The Other Galaxia

Играта претставува имплементација на веќе постоечката игра "Galaxian" (https://en.wikipedia.org/wiki/Galaxian). Главната цел на играта е убивање на вонземјаните за да ја одбраните вашата планета. Вие сте вселенски брод кој се појавува на средината на дното на екранот, додека вонземјаните се појавуваат во формација на горната средина.

## Упатство за користење
![alt-text](https://i.imgur.com/YW40q9p.png)
 - Почетно мени
 Играта започнува со почетно мени каде може да изберете нова игра или пак да ги видете инструкциите за играње.
 - Инструкции
 ![alt-text](https://i.imgur.com/2JzcsVG.png)
      -	Играчот ја започнува играта со 3 животи кои се покажуваат на левото ќоше на играта. Со секој ласер кој ќе го погоди тој губи по 1 живот доколку ги изгуби сите животи тој губи и добива порака дали сака да игра одново. Тој се движи со стрелките за лево и десно а пука од Space Bar копчето на тестатурата.Играчот може да пука само еднаш односно не може да пука повторно се додека проектилот не погодил вонземјанин или не излегол надвор од мапата.
![alt-text](https://i.imgur.com/3Q0w695.png)
     - Вонземјаните се појавуваат во формација на средината на екранот и се движат лево-десно и пукаат ласери со цел да те погодат. Има три различни видови вонземјани за кои треба одреден број пати да бидат погодени за да бидат уништени.

## Главен изглед на играта
![alt-text](https://i.imgur.com/j3hxtWq.png)

## Податочни структури

 - Во класата Hero се иницијализира почетната состојба на играчот како и функциите за негово движенје и пукање. Секоја променлива и функција содржи краток коментар.

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


 - Алгоритми и методи
   - newGame(): со повикување на овој метод се генерираат вонземјаните и се креира нова игра.
   - isHeroHit(): со оваа метода се прави проверка дали играчот е погоден од страна на вонземјаните.
   - gameIsWon(): се повикува кога играчот ќе победи за да се исцрта прозорот за победа.
