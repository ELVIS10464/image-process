using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MiM_iVision;

namespace hw1
{
    public partial class Form1 : Form
    {
        public IntPtr GrayImage = iImage.CreateGrayiImage();
        public IntPtr GrayImage2 = iImage.CreateGrayiImage();
        public IntPtr hbitmap, hbitmap2;
        E_iVision_ERRORS err = E_iVision_ERRORS.E_NULL;
        E_iVision_ERRORS err2 = E_iVision_ERRORS.E_NULL; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            iImage.DestroyiImage(GrayImage);
        }

        private void btn_loadimage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "BMP file |* .bmp";
            string filepath;                                       // Declare a string type of variable
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
                err = iImage.iReadImage(GrayImage, filepath);
                err2 = iImage.iReadImage(GrayImage2, filepath);
                if (err == E_iVision_ERRORS.E_OK)
                {
                    hbitmap = iImage.iGetBitmapAddress(GrayImage); //Get GrayImage's hbitmap  
                    hbitmap2 = iImage.iGetBitmapAddress(GrayImage2);
                    if (pictureBox1.Image != null)                 //If there is an image on the Picturebox   
                        pictureBox1.Image.Dispose();               //clear Picturebox image                  
                    pictureBox1.Image = System.Drawing.Image.FromHbitmap(hbitmap);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;  //Set PictureBox size mode
                    pictureBox1.Refresh();                         // refresh to update the Picturebox
                }
                else
                    MessageBox.Show(err.ToString(), "Error");
            }
        }

        private void btn_intensity_level_Click(object sender, EventArgs e)
        {
            int Width = iImage.GetWidth(GrayImage);        // Get image width
            int Height = iImage.GetHeight(GrayImage);      // Get image height
            int level = Convert.ToInt32(level_num.Text);
            byte[,] Graymatrix = new byte[Height, Width];
            byte[,] Graymatrix_af = new byte[Height, Width];
            err = iImage.iPointerFromiImage(GrayImage, ref Graymatrix[0, 0], Width, Height);
            if (err != E_iVision_ERRORS.E_OK)              // Check the status from functions
            {
                MessageBox.Show(err.ToString(), "ERROR");  // This will open a MessagBox for warning.
                return;                                    // End "Binary Threshold Event Function" 
            }

            int range = (int)Math.Pow(2, level);
            int[] range_num = new int[range];
            int x = 256 / (range - 1);
            for (int i = 0; i < range ; i++)
            {              
                int number = x * i;
                if(number == 256)
                {
                    number = 255;
                }
                range_num[i] = number;
            }
            for (int i = 0; i < Height; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 0; j < Width; j++)            // j index for rows ( 0~Width-1)
                {
                    for(int k = 0; k < range; k++)
                    {   
                        if(k < (range_num.Length - 1))
                        {
                            if (Graymatrix[i, j] != range_num[k] && Graymatrix[i, j] > range_num[k] && Graymatrix[i, j] < range_num[k + 1])
                            {
                                int a = Graymatrix[i, j] - range_num[k];
                                int b = range_num[k + 1] - Graymatrix[i, j];
                                if (a > b)
                                {
                                    Graymatrix_af[i, j] = (Byte)range_num[k + 1];
                                }
                                else
                                {
                                    Graymatrix_af[i, j] = (Byte)range_num[k];
                                }
                                break;
                            }
                        }
                        if (Graymatrix[i, j] == range_num[k])
                        {
                            Graymatrix_af[i, j] = (Byte)range_num[k];
                            break;
                        }                       
                    }
                }
            }

            IntPtr imgPtr = iImage.iVarPtr(ref Graymatrix_af[0, 0]);
            err2 = iImage.iPointerToiImage(GrayImage2, imgPtr, Width, Height);
            if (err2 != E_iVision_ERRORS.E_OK)              // Check the status from functions
            {
                MessageBox.Show(err2.ToString(), "Error");
                return;
            }
            hbitmap2 = iImage.iGetBitmapAddress(GrayImage2); // transform to hbitmap for PictureBox
            if (pictureBox2.Image != null)                 //If there is an image on the Picturebox
                pictureBox2.Image.Dispose();               //clear Picturebox image 
            pictureBox2.Image = System.Drawing.Image.FromHbitmap(hbitmap2); // shows image
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;        //Set PictureBox size mode
            pictureBox2.Refresh();                         // refresh to update the Picturebox
            
        }

        private void btn_shrink_Click(object sender, EventArgs e)
        {              
            int Width = iImage.GetWidth(GrayImage);        // Get image width
            int Height = iImage.GetHeight(GrayImage);      // Get image height
            int rate = Convert.ToInt32(shrink_num.Text);
            int shirk_Width = Width / rate;
            int shirk_Height = Height / rate;

            byte[,] Graymatrix = new byte[Height, Width];
            byte[,] shirk_Graymatrix = new byte[shirk_Height, shirk_Width];

            err = iImage.iPointerFromiImage(GrayImage, ref Graymatrix[0, 0], Width, Height);
            if (err != E_iVision_ERRORS.E_OK)              // Check the status from functions
            {
                MessageBox.Show(err.ToString(), "ERROR");  // This will open a MessagBox for warning.
                return;                                    // End "Binary Threshold Event Function" 
            }
            // start sliding the matrix
            if (method.Text == "Nearest")
            {
                int scrx, scry;
                for (int i = 0; i < shirk_Height; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
                {
                    for (int j = 0; j < shirk_Width; j++)            // j index for rows ( 0~Width-1)
                    {
                        scrx = i * rate;
                        scry = j * rate;
                        shirk_Graymatrix[i, j] = Graymatrix[scrx, scry];
                    }
                }
            }
            if (method.Text == "Bilinear")
            {
                double fy1, fy2, a, b;
                int a_1, a_2, b_1, b_2, x, y;
                for (int i = 0; i < shirk_Height; i++)
                {
                    for (int j = 0; j < shirk_Width; j++)
                    {
                        x = (i * rate);
                        y = (j * rate);
                        a_1 = x - 1;
                        a_2 = x + 1;
                        b_1 = y - 1;
                        b_2 = y + 1;
                        if (x > 0 && x < (Height - 1) && y > 0 && y < (Width- 1))
                        {
                            a = x - a_1;
                            b = y - b_1;
                            //x軸方向
                            fy1 = a * Graymatrix[a_1, b_1] + (1 - a) * Graymatrix[a_2, b_1];
                            fy2 = a * Graymatrix[a_1, b_2] + (1 - a) * Graymatrix[a_2, b_2];
                            //y軸方向
                            shirk_Graymatrix[i, j] = (Byte)(b * fy1 + (1 - b) * fy2);
                        }
                    }
                }
            }
            if (method.Text == "Bicubic")
            {
                float[] stemp_x = new float[4];
                float[] stemp_y = new float[4];
                float[] w_x = new float[4];
                float[] w_y = new float[4];
                double weight = 0.5;
                for (int i = 0; i < shirk_Height; i++)
                {
                    for (int j = 0; j < shirk_Width; j++)
                    {
                        float x = (i * rate);
                        float y = (j * rate);
                        if ((int)x > 0 && (int)x < (Height - 2) && (int)y > 0 && (int)y < (Width - 2))
                        {
                            stemp_x[0] = 1 + (x - (int)x);
                            stemp_x[1] = (x - (int)x);
                            stemp_x[2] = 1 - (x - (int)x);
                            stemp_x[3] = 2 - (x - (int)x);

                            w_x[0] = (float)(weight * Math.Abs(Math.Pow(stemp_x[0], 3)) - 5 * weight * (Math.Pow(stemp_x[0], 2)) + 8 * weight * Math.Abs(stemp_x[0]) - 4 * weight);
                            w_x[1] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_x[1], 3)) - (weight + 3) * (Math.Pow(stemp_x[1], 2)) + 1);
                            w_x[2] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_x[2], 3)) - (weight + 3) * (Math.Pow(stemp_x[2], 2)) + 1);
                            w_x[3] = (float)(weight * Math.Abs(Math.Pow(stemp_x[3], 3)) - 5 * weight * (Math.Pow(stemp_x[3], 2)) + 8 * weight * Math.Abs(stemp_x[3]) - 4 * weight);

                            stemp_y[0] = 1 + (y - (int)y);
                            stemp_y[1] = (y - (int)y);
                            stemp_y[2] = 1 - (y - (int)y);
                            stemp_y[3] = 2 - (y - (int)y);

                            w_y[0] = (float)(weight * Math.Abs(Math.Pow(stemp_y[0], 3)) - 5 * weight * (Math.Pow(stemp_y[0], 2)) + 8 * weight * Math.Abs(stemp_y[0]) - 4 * weight);
                            w_y[1] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_y[1], 3)) - (weight + 3) * (Math.Pow(stemp_y[1], 2)) + 1);
                            w_y[2] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_y[2], 3)) - (weight + 3) * (Math.Pow(stemp_y[2], 2)) + 1);
                            w_y[3] = (float)(weight * Math.Abs(Math.Pow(stemp_y[3], 3)) - 5 * weight * (Math.Pow(stemp_y[3], 2)) + 8 * weight * Math.Abs(stemp_y[3]) - 4 * weight);

                            Byte number = 0;
                            for (int s = 0; s <= 3; s++)
                            {
                                for (int t = 0; t <= 3; t++)
                                {
                                    number += (Byte)(w_x[s] * w_y[t] * Graymatrix[(int)x + s - 1, (int)y + t - 1]);
                                }
                            }

                            shirk_Graymatrix[i, j] = number;
                        }
                    }
                }

            }          
            IntPtr imgPtr = iImage.iVarPtr(ref shirk_Graymatrix[0, 0]);
            iImage.iImageResize(GrayImage, shirk_Width, shirk_Height);
            err = iImage.iPointerToiImage(GrayImage, imgPtr, shirk_Width, shirk_Height);
                           
            if (err != E_iVision_ERRORS.E_OK)              // Check the status from functions
            {
                MessageBox.Show(err.ToString(), "Error");
                return;
            }

            hbitmap = iImage.iGetBitmapAddress(GrayImage); // transform to hbitmap for PictureBox
            if (pictureBox2.Image != null)                 //If there is an image on the Picturebox
                pictureBox2.Image.Dispose();               //clear Picturebox image
            pictureBox2.Width = pictureBox1.Width;
            pictureBox2.Height = pictureBox1.Height;
            pictureBox2.Image = System.Drawing.Image.FromHbitmap(hbitmap); // shows image
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;        //Set PictureBox size mode       
            pictureBox2.Refresh();                         // refresh to update the Picturebox
        }

        private void btn_zoom_Click(object sender, EventArgs e)
        {
            int Width = iImage.GetWidth(GrayImage);        // Get image width
            int Height = iImage.GetHeight(GrayImage);      // Get image height
            int rate = Convert.ToInt32(zoom_num.Text);
            int zoom_Width = Width * rate;
            int zoom_Height = Height * rate;

            byte[,] Graymatrix = new byte[Height, Width];
            byte[,] zoom_Graymatrix = new byte[zoom_Height, zoom_Width];

            err = iImage.iPointerFromiImage(GrayImage, ref Graymatrix[0, 0], Width, Height);
            if (err != E_iVision_ERRORS.E_OK)              // Check the status from functions
            {
                MessageBox.Show(err.ToString(), "ERROR");  // This will open a MessagBox for warning.
                return;                                    // End "Binary Threshold Event Function" 
            }
            // start sliding the matrix
            
            if (method.Text == "Nearest")
            {               
                for (int i = 0; i < zoom_Height; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
                {
                    for (int j = 0; j < zoom_Width; j++)            // j index for rows ( 0~Width-1)
                    {                    
                        int x = (int)(Math.Round((double)(i / rate)));
                        int y = (int)(Math.Round((double)(j / rate)));
                        zoom_Graymatrix[i, j] = Graymatrix[x, y];
                    }
                }
            }
            if (method.Text == "Bilinear")
            {
                double u, v, x, y;
                int a_1, a_2, b_1, b_2;
                for (int i = 0; i < zoom_Height; i++)
                {
                    for (int j = 0; j < zoom_Width; j++)
                    {
                        x = (i / rate);
                        y = (j / rate);
                        a_1 = (int)Math.Floor(x);
                        a_2 = (int)Math.Ceiling(x);
                        b_1 = (int)Math.Floor(y);
                        b_2 = (int)Math.Ceiling(y);
                        u = x - a_1;
                        v = y - b_1;
                        if (x >= 0 && x <= (Height - 1) && y >= 0 && y <= (Width - 1))
                        {
                            zoom_Graymatrix[i, j] = (Byte)((1 - u) * (1 - v) * Graymatrix[a_1, b_1] +
                            (1 - u) * v * Graymatrix[a_1, b_2] + u * (1 - v) * Graymatrix[a_2, b_1] +
                            u * v * Graymatrix[a_2, b_2]);
                        }
                    }
                }
            }
            if (method.Text == "Bicubic")
            {
                float[] stemp_x = new float[4];
                float[] stemp_y = new float[4];
                float[] w_x = new float[4];
                float[] w_y = new float[4];
                double weight = 0.5;
                for (int i = 0; i < zoom_Height; i++)
                {
                    for (int j = 0; j < zoom_Width; j++)
                    {
                        float x = (i / rate);
                        float y = (j / rate);
                        if ((int)x > 0 && (int)x < (Height - 2) && (int)y > 0 && (int)y < (Width - 2))
                        {
                            stemp_x[0] = 1 + (x - (int)x);
                            stemp_x[1] = (x - (int)x);
                            stemp_x[2] = 1 - (x - (int)x);
                            stemp_x[3] = 2 - (x - (int)x);

                            w_x[0] = (float)(weight * Math.Abs(Math.Pow(stemp_x[0], 3)) - 5 * weight * (Math.Pow(stemp_x[0], 2)) + 8 * weight * Math.Abs(stemp_x[0]) - 4 * weight);
                            w_x[1] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_x[1], 3)) - (weight + 3) * (Math.Pow(stemp_x[1], 2)) + 1);
                            w_x[2] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_x[2], 3)) - (weight + 3) * (Math.Pow(stemp_x[2], 2)) + 1);
                            w_x[3] = (float)(weight * Math.Abs(Math.Pow(stemp_x[3], 3)) - 5 * weight * (Math.Pow(stemp_x[3], 2)) + 8 * weight * Math.Abs(stemp_x[3]) - 4 * weight);

                            stemp_y[0] = 1 + (y - (int)y);
                            stemp_y[1] = (y - (int)y);
                            stemp_y[2] = 1 - (y - (int)y);
                            stemp_y[3] = 2 - (y - (int)y);

                            w_y[0] = (float)(weight * Math.Abs(Math.Pow(stemp_y[0], 3)) - 5 * weight * (Math.Pow(stemp_y[0], 2)) + 8 * weight * Math.Abs(stemp_y[0]) - 4 * weight);
                            w_y[1] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_y[1], 3)) - (weight + 3) * (Math.Pow(stemp_y[1], 2)) + 1);
                            w_y[2] = (float)((weight + 2) * Math.Abs(Math.Pow(stemp_y[2], 3)) - (weight + 3) * (Math.Pow(stemp_y[2], 2)) + 1);
                            w_y[3] = (float)(weight * Math.Abs(Math.Pow(stemp_y[3], 3)) - 5 * weight * (Math.Pow(stemp_y[3], 2)) + 8 * weight * Math.Abs(stemp_y[3]) - 4 * weight);

                            Byte number = 0;
                            for (int s = 0; s <= 3; s++)
                            {
                                for (int t = 0; t <= 3; t++)
                                {
                                    number += (Byte)(w_x[s] * w_y[t] * Graymatrix[(int)x + s - 1, (int)y + t - 1]);
                                }
                            }

                            zoom_Graymatrix[i, j] = number;
                        }                      
                    }
                }

            }

            IntPtr imgPtr = iImage.iVarPtr(ref zoom_Graymatrix[0, 0]);
            iImage.iImageResize(GrayImage, zoom_Width, zoom_Height);
            err = iImage.iPointerToiImage(GrayImage, imgPtr, zoom_Width, zoom_Height);

            if (err != E_iVision_ERRORS.E_OK)              // Check the status from functions
            {
                MessageBox.Show(err.ToString(), "Error");
                return;
            }

            hbitmap = iImage.iGetBitmapAddress(GrayImage); // transform to hbitmap for PictureBox
            if (pictureBox2.Image != null)                 //If there is an image on the Picturebox
                pictureBox2.Image.Dispose();               //clear Picturebox image

            pictureBox2.Image = System.Drawing.Image.FromHbitmap(hbitmap); // shows image
            pictureBox2.Width = pictureBox1.Width;
            pictureBox2.Height = pictureBox1.Height;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;        //Set PictureBox size mode       
            pictureBox2.Refresh();                         // refresh to update the Picturebox
        }
    }
}