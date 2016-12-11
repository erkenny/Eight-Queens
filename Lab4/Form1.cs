/*
 * 
 * Elizabeth Kenny
 * EC447 Lab 4 - Eight Queens
 * Due 10/19/2016
 * 
 ***************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;               // For ArrayList

namespace Lab4
{
    public partial class Form1 : Form
    {
        public int[,] isOccupied = new int[8, 8];       // array where 0 if safe, incremented if unsafe or occupied @ left click, decremented with right click
        public int QCount = 0;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Eight Queens by Elizabeth Kenny";
            this.Size = new System.Drawing.Size(620, 620);
        }
        // display hints
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {  
            this.Invalidate();         // update
        }
        // reset board
        private void button1_Click(object sender, EventArgs e)
        {
            for (int m = 0; m < 8; m++)  // clear boolean grid of blocked squares on board
            {
                for (int n = 0; n < 8; n++)
                {
                    isOccupied[m, n] = 0;
                }
            }
            this.Invalidate();
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

            decimal dtempX, dtempY;     // decimal temp vars for conversion to pixels
            int itempX, itempY;         // temp whole int indices to be scaled for pixel values on board

            if (e.Button == MouseButtons.Left)
            {
                if ((e.X >= 100) && (e.X <= 500) && (e.Y >= 100) && (e.Y <= 500))   // if click on on board
                {
                    // convert coords of click to indices
                    Point p = new Point(e.X, e.Y);
                    dtempX = p.X / 50;                        // convert pixel coords to decimal for math
                    itempX = (int)Math.Floor(dtempX)-2;         // round to whole number int corresponding to check index(+2)
                    dtempY = p.Y / 50;
                    itempY = (int)Math.Floor(dtempY)-2;
                    if (isOccupied[itempX, itempY] == 0)
                    {
                        // mark cells as blocked (occupied)
                        for (int k = 0; k < 8; k++)
                        {
                            isOccupied[itempX, k]++;
                            isOccupied[k, itempY]++;
                            if ((itempX + k) < 8 && (itempY + k) < 8)
                                isOccupied[itempX + k, itempY + k]++;
                            if ((itempX - k) > -1 && (itempY - k) > -1)
                                isOccupied[itempX - k, itempY - k]++;
                            if ((itempX + k) < 8 && (itempY - k) > -1)
                                isOccupied[itempX + k, itempY - k]++;
                            if ((itempX - k) > -1 && (itempY + k) < 8)
                                isOccupied[itempX - k, itempY + k]++;
                        }
                        isOccupied[itempX, itempY] = 100;         // 100 corresponds to queen
                        QCount++;
                    }
                    else
                        System.Media.SystemSounds.Beep.Play();
                    this.Invalidate();

                    if (QCount == 8)
                    {
                        MessageBox.Show("You did it!");
                    }
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if ((e.X > 100) && (e.X < 500) && (e.Y > 100) && (e.Y < 500))   // if click on on board
                {
                    Point d = new Point(e.X, e.Y);
                    dtempX = d.X / 50;
                    itempX = (int)Math.Floor(dtempX)-2;
                    dtempY = d.Y / 50;
                    itempY = (int)Math.Floor(dtempY)-2;
                    // see if there's a queen there
                    if (isOccupied[itempX, itempY] > 50)
                    {
                        for (int z = 0; z < 8; z++)
                        {   // mark all squares not safe from this queen as safe
                            isOccupied[itempX, z]--;
                            isOccupied[z, itempY]--;
                            if ((itempX + z) < 8 && (itempY + z) < 8)
                                isOccupied[itempX + z, itempY + z]--;
                            if ((itempX - z) > -1 && (itempY - z) > -1)
                                isOccupied[itempX - z, itempY - z]--;
                            if ((itempX + z) < 8 && (itempY - z) > -1)
                                isOccupied[itempX + z, itempY - z]--;
                            if ((itempX - z) > -1 && (itempY + z) < 8)
                                isOccupied[itempX - z, itempY + z]--;
                        }
                        isOccupied[itempX, itempY] = 0;
                        QCount--;
                    }
                    this.Invalidate();          // update
                }
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (!checkBox1.Checked)                  // standard black and white board when hints are off
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((i + j) % 2 == 0)// white squares
                        {
                            g.FillRectangle(Brushes.White, 100 + i * 50, 100 + j * 50, 50, 50);
                            g.DrawRectangle(Pens.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                        }
                        else if ((i + j) % 2 == 1) // black squares, black border
                        {
                            g.FillRectangle(Brushes.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                            g.DrawRectangle(Pens.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                        }
                    }
                }
            }
            else if (checkBox1.Checked)                 // hints on, unsafe squares red
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (isOccupied[i, j] > 0)               // red squares, black border
                        {
                            g.FillRectangle(Brushes.Red, 100 + i * 50, 100 + j * 50, 50, 50);
                            g.DrawRectangle(Pens.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                        }
                        else if ((isOccupied[i, j] == 0) && ((i + j) % 2 == 0)) // white squares, black border
                        {
                            g.FillRectangle(Brushes.White, 100 + i * 50, 100 + j * 50, 50, 50);
                            g.DrawRectangle(Pens.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                        }
                        else if ((isOccupied[i, j] == 0) && ((i + j) % 2 == 1)) // black squares, black border
                        {
                            g.FillRectangle(Brushes.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                            g.DrawRectangle(Pens.Black, 100 + i * 50, 100 + j * 50, 50, 50);
                        }
                    }
                }
            }
            // draw Q's
            Font myFont = new Font("Arial", 30, FontStyle.Bold);
            for (int a = 0; a < 8; a++)
            {
                for (int b = 0; b < 8; b++)
                {
                    if (!checkBox1.Checked)                     // hints off
                    {
                        if ((isOccupied[a, b] > 50) && ((a + b) % 2 == 1))      // if black square draw white Q
                        {
                            g.DrawString("Q", myFont, Brushes.White, 100 + a * 50, 100 + b * 50);
                        }
                        else if ((isOccupied[a, b] > 50) && ((a + b) % 2 == 0))
                        {
                            g.DrawString("Q", myFont, Brushes.Black, 100 + a * 50, 100 + b * 50);
                        }
                    }                                           // hints on ==> all Q's black
                    else if (checkBox1.Checked)
                        if (isOccupied[a, b] > 50)
                            g.DrawString("Q", myFont, Brushes.Black, 100 + a * 50, 100 + b * 50);
                }
            }
            g.DrawString("You have " + QCount + " Queens on the board.", Font, Brushes.Black, 200, 20);
        }
    }
}