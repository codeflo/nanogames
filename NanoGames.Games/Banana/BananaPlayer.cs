﻿// Copyright (c) the authors of nanoGames. All rights reserved.
// Licensed under the MIT license. See LICENSE.txt in the project root.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoGames.Games.Banana
{
    class BananaPlayer : Player<BananaMatch>
    {
        public Vector Position = new Vector(0, 0);

        public bool HasFinished = false;

        public double Radius = 5;
        
        private Vector velocity = new Vector(0, 0);

        private double speedProjectile;

        private double lengthGun = 2;

        private double angle = 0;
        private double realAngle = 0;
        private double power = 0;
        private double powerMax = 100;

        private int wait = 20;
        private int countLeft = 0;
        private int countRight = 0;
        private int countUp = 0;
        private int countDown = 0;
        private int countFire = 0;
        private int waitFire = 0;
        private int idxWeapon = 0;
        private string[] weapons = new string[] { "Gun", "Grenade" };
        private bool looksRight;

        public void GetBorn()
        {
            int tmp1 = Convert.ToInt32(Match.Random.NextDouble() * (Match.Land.Border.Count - 1));
            int tmp2 = Convert.ToInt32(Match.Random.NextDouble() * (Match.Land.Border[tmp1].Count -1));
            
            Position = Match.Land.Border[tmp1][tmp2];
            
            power = 0;

            angle = Match.Random.NextDouble() * Math.PI - Math.PI / 2;

            if (Match.Random.Next(0, 1) == 0)
            {
                looksRight = false;
                realAngle = -angle + Math.PI;
            }
            else
            {
                looksRight = true;
                realAngle = angle;
            }
        }
        
        public void DrawScreen()
        {
            /* Draw each player. */
            foreach (var player in Match.Players)
            {
                /* Skip players that have already finished. */
                if (player.HasFinished)
                {
                    continue;
                }

                Color color;
                if (player == this)
                {
                    /* Always show the current player in white. */
                    color = new Color(1, 1, 1);
                }
                else
                {
                    color = player.Color;
                }

                /* Draw the body of the player. */
                Output.Graphics.Circle(color, new Vector(player.Position.X, player.Position.Y), Radius);
                if (player == Match.ActivePlayer)
                {
                    Output.Graphics.Circle(color, new Vector(player.Position.X, player.Position.Y), 0.9 * Radius);
                    Output.Graphics.Circle(color, new Vector(player.Position.X, player.Position.Y), 0.8 * Radius);
                }

                /* Draw the Gun of the player. */
                Output.Graphics.Line(color, new Vector(player.Position.X + Radius * Math.Cos(player.realAngle), player.Position.Y - Radius * Math.Sin(player.realAngle)),
                    new Vector(player.Position.X + lengthGun * Math.Cos(player.realAngle), player.Position.Y - lengthGun * Math.Sin(player.realAngle)));
            }

            /* Draw all the bullets. */
            foreach (SimpleBullet bullet in Match.ListBullets)
            {
                Output.Graphics.Line(new Color(1, 1, 1), bullet.Position, bullet.PositionBefore);
            }

            /* Draw all the grenades. */
            foreach (Grenade grenade in Match.ListGrenades)
            {
                Output.Graphics.Circle(new Color(1, 1, 1), grenade.Position, grenade.Radius);
            }

            /* Draw landscape */
            Match.Land.Draw(Output.Graphics);
            
            // Draw Information on Screen
            Output.Graphics.Print(new Color(1, 1, 1), 4, new Vector(10, 10), "ACTIVEPLAYER: " + Match.ActivePlayer.Name);
            Output.Graphics.Print(new Color(1, 1, 1), 4, new Vector(10, 20), "REALANGLE: " + (Convert.ToInt32(Match.ActivePlayer.realAngle * 180 / Math.PI)).ToString());
            Output.Graphics.Print(new Color(1, 1, 1), 4, new Vector(10, 40), "STATEOFGAME: " + Match.StateOfGame.ToUpper());
            Output.Graphics.Print(new Color(1, 1, 1), 4, new Vector(10, 50), "WEAPON: " + weapons[idxWeapon].ToUpper());
            Output.Graphics.Print(new Color(1, 1, 1), 10, new Vector(150, 20), Match.SecToGoInRound.ToString());

            if (Match.Wind.Speed >= 0)
            {
                Output.Graphics.Print(new Color(1, 1, 1), 4, new Vector(10, 60), "Wind: P " + (Convert.ToInt32(Match.Wind.Speed * 10)).ToString());
            }
            else
            {
                Output.Graphics.Print(new Color(1, 1, 1), 4, new Vector(10, 60), "Wind: M " + (Convert.ToInt32(Match.Wind.Speed * 10)).ToString());
            }
        }

        public void SetWeapon()
        {
            if (Input.AltFire.WasActivated)
            {
                idxWeapon++;
                idxWeapon = idxWeapon % weapons.Count();
            }
        }

        public void Move()
        {
            int travel = 0;
            int order = Math.Sign(Match.Land.Directions[IdxPosition].X);

            if (Input.Left.WasActivated)
            {
                if (countLeft < wait)
                {
                    travel = -2; 
                }

                else
                {
                    travel = -4;
                }
            }

            if (Input.Right.WasActivated)
            {
                if (countRight < wait)
                {
                    travel = 2;
                }

                else
                {
                    travel = 4;
                }
            }

            if (travel != 0)
            {

                if (travel < 0)
                {
                    looksRight = false;
                }
                else
                {
                    looksRight = true;
                }

                int orientation = Math.Sign(order * travel);
                int idx1 = IdxPosition;
                int idx2 = IdxPosition + orientation;
                int order1 = Math.Sign(Match.Land.Directions[idx1].X);
                int order2 = Math.Sign(Match.Land.Directions[idx2].X);

                double dist2pointRight = (Match.Land.Points[IdxPosition + 1] - Position).Length;
                double dist2pointLeft = (Match.Land.Points[IdxPosition] - Position).Length;

                if (orientation == 1)
                {
                    if (Math.Abs(travel) >= (Match.Land.Points[IdxPosition + 1] - Position).Length)
                    {
                        if (order1 == order2)
                        {
                            IdxPosition = idx2;
                        }

                        if ()
                    }

                    Position = Match.Land.Points[IdxPosition + 1];

                    if (Match.Land.Directions[IdxPosition].X > 0 && Match.Land.Directions[IdxPosition + 1].X < 0)
                    {
                        
                    }

                    if (Match.Land.Directions[IdxPosition].X < 0 && Match.Land.Directions[IdxPosition + 1].X > 0)
                    {
                        Match.StateOfGame = "ActivePLayerFalling";
                    }

                    if (Match.Land.Directions[IdxPosition].X < 0 && Match.Land.Directions[IdxPosition + 1].X < 0)
                    {
                        IdxPosition -= 1;
                    }
                }

                if (dist2travel >= dist2pointLeft && !looksRight)
                {
                    Position = Match.Land.Points[IdxPosition];

                    if (Match.Land.Directions[IdxPosition].X * Match.Land.Directions[IdxPosition - 1].X < 0)
                    {
                        IdxPosition -= 1;
                    }
                }

                else
                {
                    Position += travel * Match.Land.Directions[IdxPosition] * Math.Sign(Match.Land.Directions[IdxPosition].X);
                }

                Output.Audio.Play(Sounds.Walk);
            }
            
            if (Input.Left.IsPressed)
            {
                countLeft++;
            }
            else
            {
                countLeft = 0;
            }

            if (Input.Right.IsPressed)
            {
                countRight++;
            }
            else
            {
                countRight = 0;
            }
        }

        public void SetAngle()
        {
            if (Input.Up.WasActivated)
            {

                if (countUp < wait)
                {
                    angle += 1 * Math.PI / 180;
                }

                else
                {
                    angle += 5 * Math.PI / 180;
                }
             }

             if (Input.Down.WasActivated)
             {
                if (countDown < wait)
                {
                    angle -= 1 * Math.PI / 180;
                }

                else
                {
                    angle -= 5 * Math.PI / 180;
                }
             }

             if (Input.Up.IsPressed)
             {
                 countUp++;
             }
             else
             {
                 countUp = 0;
             }
              
             if (Input.Down.IsPressed)
             {
                 countDown++;
             }
             else
             {
                 countDown = 0;
             }

            if (angle > Math.PI / 2)
            {
                angle = Math.PI / 2;
            }

            if (angle < - Math.PI / 2)
            {
                angle = - Math.PI / 2;
            }

            if (looksRight)
            {
                realAngle = angle;
            }
            else
            {
                realAngle = -angle + Math.PI;
            }
        }

        public void Shoot1()
        {
            if (Input.Fire.WasActivated)
            {
                Match.StateOfGame = "AnimationBeforeShoot";
                countFire = 0;
            }
        }

        public void Shoot2()
        {
            speedProjectile = 2;// 1 + countFire * 9.0 / waitFire;
            
            if (!Input.Fire.IsPressed || countFire > waitFire)
            {
                Match.StateOfGame = "AnimationShoot";
            }

                countFire++;
        }

        public void Shoot3()
        {
            if (weapons[idxWeapon] == "Gun")
            {
                Vector velocity = speedProjectile * new Vector(Math.Cos(realAngle), -Math.Sin(realAngle));
                Vector position = Position + (Radius + speedProjectile) * new Vector(Math.Cos(realAngle), -Math.Sin(realAngle));

                Output.Audio.Play(Sounds.GunFire);

                Match.ListBullets.Add(new SimpleBullet(position, velocity));
            }

            if (weapons[idxWeapon] == "Grenade")
            {
                Vector velocity = speedProjectile * new Vector(Math.Cos(realAngle), -Math.Sin(realAngle));
                Vector position = Position + (Radius + speedProjectile +4) * new Vector(Math.Cos(realAngle), -Math.Sin(realAngle));

                Output.Audio.Play(Sounds.GrenadeFire);

                Match.ListGrenades.Add(new Grenade(position, velocity));

            }

            Match.StateOfGame = "AnimationProjectileFly";
        }

        public void Fall()
        {

        }

    }
}
