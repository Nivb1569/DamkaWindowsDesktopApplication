﻿using System;

namespace DamkaForm
{
    public class Point
    {
        private int m_X;
        private int m_Y;
        public Point(int i_X, int i_Y)
        {
            m_X = i_X;
            m_Y = i_Y;
        }
        public int X
        {
            get { return m_X; }
            set { m_X = value; }
        }
        public int Y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }
        public bool IsEqualPoints(Point i_Point)
        {
            return m_X == i_Point.X && m_Y == i_Point.Y;
        }
    }
}
