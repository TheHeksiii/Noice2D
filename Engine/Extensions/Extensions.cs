using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Color = System.Drawing.Color;

public static class Extensions
{
    public static Vector2 MaxY(Vector2 vector1, Vector2 vector2)
    {
        if (vector1.Y > vector2.Y)
        {
            return vector1;
        }
        else
        {
            return vector2;
        }
    }
    public static float MaxVectorMember(this Vector2 vector)
    {
        if (vector.X > vector.Y)
        {
            return vector.X;
        }
        else
        {
            return vector.Y;
        }
    }
    public static Vector2 MinY(Vector2 vector1, Vector2 vector2)
    {
        if (vector1.Y < vector2.Y)
        {
            return vector1;
        }
        else
        {
            return vector2;
        }
    }
    public static Vector2 VectorX(this Vector2 vector)
    {
        return new Vector2(vector.X, 0);
    }
    public static Vector2 VectorY(this Vector2 vector)
    {
        return new Vector2(0, vector.Y);
    }
    public static System.Drawing.Point ToSystemPoint(this Microsoft.Xna.Framework.Point point)
    {
        return new System.Drawing.Point(point.X, point.Y);
    }
    public static Microsoft.Xna.Framework.Point ToGamePoint(this System.Drawing.Point point)
    {
        return new Microsoft.Xna.Framework.Point(point.X, point.Y);
    }
    public static Vector2 ToVector2(this MonoGame.Extended.Point2 point)
    {
        return new Vector2(point.X, point.Y);
    }
    public static Vector2 ToVector2(this MonoGame.Extended.Size2 point)
    {
        return new Vector2(point.Width, point.Height);
    }
    public static System.Drawing.Color ToOtherColor(this Microsoft.Xna.Framework.Color color)
    {
        return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
    public static Microsoft.Xna.Framework.Color ToOtherColor(this System.Drawing.Color color)
    {
        return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
    }
    public static Microsoft.Xna.Framework.Color ColorFromHSVToXna(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return new Microsoft.Xna.Framework.Color(v, t, p, 255);
        else if (hi == 1)
            return new Microsoft.Xna.Framework.Color(q, v, p, 255);
        else if (hi == 2)
            return new Microsoft.Xna.Framework.Color(p, v, t, 255);
        else if (hi == 3)
            return new Microsoft.Xna.Framework.Color(p, q, v, 255);
        else if (hi == 4)
            return new Microsoft.Xna.Framework.Color(t, p, v, 255);
        else
            return new Microsoft.Xna.Framework.Color(v, p, q, 255);
    }
    public static Color ColorFromHSV(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }
    public static int Clamp(int value, int min, int max)
    {
        if (value < min) { return min; }
        if (value > max) { return max; }
        return value;
    }
    public static Vector2 Clamp(Vector2 value, float minX, float maxX, float minY, float maxY)
    {
        if (value.X < minX) { value.X = minX; }
        if (value.X > maxX) { value.X = maxX; }
        if (value.Y < minY) { value.Y = minY; }
        if (value.Y > maxY) { value.Y = maxY; }
        return value;
    }
    public static float Clamp(float value, float min, float max)
    {
        if (value < min) { return min; }
        if (value > max) { return max; }
        return value;
    }
    public static float ClampMin(float value, float min)
    {
        if (value < min) { return min; }
        return value;
    }
    public static float Clamp01(float value)
    {
        if (value < 0) { return 0; }
        if (value > 1) { return 1; }
        return value;
    }
    public static Vector2 Round(this Vector2 vector)
    {
        return new Vector2((float)Math.Round((decimal)vector.X, 2), (float)Math.Round((decimal)vector.Y, 2));
    }
    public static System.Drawing.Point Round(this System.Drawing.Point point, int scale)
    {
        return new System.Drawing.Point((int)Math.Floor(point.X / (float)scale) * scale, (int)Math.Floor(point.Y / (float)scale) * scale);
    }
    public static Vector2 TranslateToGrid(this Vector2 vector)
    {
        return new Vector2((int)((decimal)vector.X / 10) * 10, (int)((decimal)vector.Y / 10) * 10);
    }
    public static float AngleBetween(Vector2 vector1, Vector2 vector2)
    {
        float returnAngle = (float)Math.Acos(Vector2.Dot(vector1, vector2) / (vector1.Length() * vector2.Length()));
        if (returnAngle == float.NaN)
        {
            returnAngle = 0;
        }
        return returnAngle;
    }
}

