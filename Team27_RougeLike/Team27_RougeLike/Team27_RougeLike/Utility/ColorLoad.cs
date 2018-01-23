﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Team27_RougeLike.Utility
{
    public class ColorLoad
    {
        public static Color GetColor (string colorname)
        {
            switch (colorname)
            {
                case "White":
                    return Color.White;
                case "Navy":
                    return Color.Navy;
                case "Blue":
                    return Color.Blue;
                case "Gray":
                    return Color.Gray;
                case "Cyan":
                    return Color.Cyan;
                case "Gold":
                    return Color.Gold;
                case "Black":
                    return Color.Black;
                case "Brown":
                    return Color.Brown;
                case "Green":
                    return Color.Green;
                case "Lime":
                    return Color.Lime;
                case "Magenta":
                    return Color.Magenta;
                case "Orange":
                    return Color.Orange;
                case "Red":
                    return Color.Red;
                case "Pink":
                    return Color.Pink;
                case "Silver":
                    return Color.Silver;
                case "Yellow":
                    return Color.Yellow;
                case "Purple":
                    return Color.Purple;
                case "DarkBlue":
                    return new Color(40, 80, 220);
                case "DarkGray":
                    return new Color(75, 75, 75);
                case "LightBlue":
                    return new Color(120, 180, 255);
                case "LightGreen":
                    return new Color(80, 255, 200);
                default:
                    return Color.White;
            }
        }
    }
}
