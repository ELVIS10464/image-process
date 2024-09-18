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

namespace finial_project
{
    public partial class Form1 : Form
    {
        public IntPtr Image = iImage.CreateGrayiImage();
        public IntPtr hbitmap;
        E_iVision_ERRORS err = E_iVision_ERRORS.E_NULL;
        public Form1()
        {
            InitializeComponent();
        }  

        private void btn_loadimage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "BMP file |* .bmp";
            string filepath;                                       // Declare a string type of variable
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filepath = openFileDialog1.FileName;
                err = iImage.iReadImage(Image, filepath);
                if (err == E_iVision_ERRORS.E_OK)
                {
                    hbitmap = iImage.iGetBitmapAddress(Image); //Get GrayImage's hbitmap
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

        void print_error(E_iVision_ERRORS src)
        {
            if (src != E_iVision_ERRORS.E_OK)
            {
                MessageBox.Show(src.ToString(), "ERROR");
                return;
            }
        }

        void show_img(IntPtr img, PictureBox pic)
        {
            hbitmap = iImage.iGetBitmapAddress(img);
            if (pic.Image != null)
                pic.Image.Dispose();
            pic.Image = System.Drawing.Image.FromHbitmap(hbitmap);
            pic.Refresh();
            pic.SizeMode = PictureBoxSizeMode.Zoom;
        }

        void save_img(IntPtr img)
        {
            openFileDialog1.Filter = "BMP file |*.bmp";
            string path;
            E_iVision_ERRORS err = E_iVision_ERRORS.E_NULL;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                err = iImage.iSaveImage(img, path);
                if (err != E_iVision_ERRORS.E_OK)
                    MessageBox.Show(err.ToString(), "Error");
            }
        }       

        private void btn_contour_Click(object sender, EventArgs e)
        {
            int Width = iImage.GetWidth(Image);        // Get image width
            int Height = iImage.GetHeight(Image);      // Get image height        

            byte[,] img = new byte[Height, Width];
            byte[,] img_mean = new byte[Height, Width];
            byte[,] img_sob1 = new byte[Height, Width];
            byte[,] img_sob2 = new byte[Height, Width];
            byte[,] img_sob_add = new byte[Height, Width];
            byte[,] img_sob_add_thr = new byte[Height, Width];
            byte[,] img_sob_add_thr_mor = new byte[Height, Width];
            byte[,] img_open_ero = new byte[Height, Width];
            byte[,] img_open_dil = new byte[Height, Width];
            byte[,] img_clos_dil = new byte[Height, Width];
            byte[,] img_clos_ero = new byte[Height, Width];
            byte[,] img_prun = new byte[Height, Width];//暫存
            byte[,] img_in_prun = new byte[Height, Width];
            byte[,] img_prun_b1 = new byte[Height, Width];
            byte[,] img_prun_b2 = new byte[Height, Width];
            byte[,] img_prun_af = new byte[Height, Width];
            byte[,] img_prun_A = new byte[Height, Width];
            byte[,] img_prun_x1 = new byte[Height, Width];
            byte[,] img_prun_x2 = new byte[Height, Width];
            byte[,] img_prun_x3 = new byte[Height, Width];
            byte[,] img_prun_x4 = new byte[Height, Width];
            

            err = iImage.iPointerFromiImage(Image, ref img[0, 0], Width, Height);
            print_error(err);
            //x方向
            for (int i = 1; i < Height - 1; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 1; j < Width - 1; j++)            // j index for rows ( 0~Width-1)
                {
                    img_sob1[i, j] = (byte)(Math.Abs((img[i + 1, j - 1] + 2 * img[i + 1, j] + img[i + 1, j + 1]) - (img[i - 1, j - 1] + 2 * img[i - 1, j] + img[i - 1, j + 1])));
                }
            }

            //y方向
            for (int i = 1; i < Height - 1; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 1; j < Width - 1; j++)            // j index for rows ( 0~Width-1)
                {
                    img_sob2[i, j] = (byte)(Math.Abs((img[i - 1, j + 1] + 2 * img[i, j + 1] + img[i + 1, j + 1]) - (img[i - 1, j - 1] + 2 * img[i, j - 1] + img[i + 1, j - 1])));
                }
            }
            //x + y
            for (int i = 0; i < Height; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 0; j < Width; j++)            // j index for rows ( 0~Width-1)
                {
                    img_sob_add[i, j] = (byte)(img_sob1[i, j] + img_sob2[i, j]);
                }
            }

            byte max_num = 0;
            for (int i = 0; i < Height; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 0; j < Width; j++)            // j index for rows ( 0~Width-1)
                {
                    if (img_sob_add[i, j] > max_num)
                    {
                        max_num = img_sob_add[i, j];
                    }
                }
            }
            byte thr_num;
            thr_num = (byte)(max_num * 0.33);
            label1.Text = Convert.ToString(max_num);
            label2.Text = Convert.ToString(thr_num);
            for (int i = 0; i < Height; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 0; j < Width; j++)            // j index for rows ( 0~Width-1)
                {
                    if (img_sob_add[i, j] >= thr_num)
                    {
                        img_sob_add_thr[i, j] = 255;
                    }
                    else
                    {
                        img_sob_add_thr[i, j] = 0;
                    }
                }
            }
            
            int white_count_ne;
            for (int i = 1; i < Height - 1; i++)               // i index for cols ( 0~Hight-1)(Because that matrix index is start from 0)
            {
                for (int j = 1; j < Width - 1; j++)            // j index for rows ( 0~Width-1)
                {
                    white_count_ne = 0;
                    for (int s = -1; s < 2; s++)
                    {
                        for (int t = -1; t < 2; t++)
                        {
                            if (img_sob_add_thr[i + s, j + t] == 255)
                            {
                                white_count_ne++;
                            }
                        }
                    }
                    if (white_count_ne >= 5)
                    {
                        img_sob_add_thr_mor[i, j] = 255;
                    }
                    else
                    {
                        img_sob_add_thr_mor[i, j] = 0;
                    }
                }
            }
            
            byte[,] stru = new byte[,]{
                {255, 255, 255},
                {255, 255, 255},
                {255, 255, 255}};
            int flag;
            for (int i = 1; i < Height - 1; i++)//侵蝕               
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    flag = 1;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            //自身及領域中若有一個為0
                            //則將該點設為0 
                            if (img_sob_add_thr_mor[i + m, j + n] != stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                            {
                                flag = 0;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        img_open_ero[i, j] = (byte)0;
                    }
                    else
                    {
                        img_open_ero[i, j] = (byte)255;
                    }
                }
            }

            for (int i = 1; i < Height - 1; i++)//膨脹
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    flag = 1;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            //自身及領域中若有一個為0
                            //則將該點設為0 
                            if (img_open_ero[i + m, j + n] == stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                            {
                                flag = 0;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        img_open_dil[i, j] = 255;
                    }
                    else
                    {
                        img_open_dil[i, j] = 0;
                    }
                }
            }

            for (int i = 1; i < Height - 1; i++)//膨脹
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    flag = 1;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            //自身及領域中若有一個為0
                            //則將該點設為0 
                            if (img_open_dil[i + m, j + n] == stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                            {
                                flag = 0;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        img_clos_dil[i, j] = 255;
                    }
                    else
                    {
                        img_clos_dil[i, j] = 0;
                    }
                }
            }

            for (int i = 1; i < Height - 1; i++)//侵蝕               
            {
                for (int j = 1; j < Width - 1; j++)
                {
                    flag = 1;
                    for (int m = -1; m < 2; m++)
                    {
                        for (int n = -1; n < 2; n++)
                        {
                            //自身及領域中若有一個為0
                            //則將該點設為0 
                            if (img_clos_dil[i + m, j + n] != stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                            {
                                flag = 0;
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            break;
                        }
                    }
                    if (flag == 0)
                    {
                        img_clos_ero[i, j] = (byte)0;
                    }
                    else
                    {
                        img_clos_ero[i, j] = (byte)255;
                    }
                }
            }
             
            int ero_count = 0;
            bool con_next = true;
            byte[,] img_ske = new byte[Height, Width];
            byte[,] img_ske_ero = new byte[Height, Width];
            byte[,] img_ske_open_ero = new byte[Height, Width];
            byte[,] img_ske_open_dil = new byte[Height, Width];
            byte[,] img_ske_diff = new byte[Height, Width];
            byte[,] img_ske_af = new byte[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_ske[i, j] = img_clos_ero[i, j];
                }
            }
          
            while(true)
            {
                if(ero_count > 0)
                {
                    con_next = false;
                    for (int i = 1; i < Height - 1; i++)//侵蝕               
                    {
                        for (int j = 1; j < Width - 1; j++)
                        {
                            flag = 1;
                            for (int m = -1; m < 2; m++)
                            {
                                for (int n = -1; n < 2; n++)
                                {
                                    //自身及領域中若有一個為0
                                    //則將該點設為0 
                                    if (img_ske[i + m, j + n] != stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                                    {
                                        flag = 0;
                                        break;
                                    }
                                }
                                if (flag == 0)
                                {
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                img_ske_ero[i, j] = (byte)0;
                            }
                            else
                            {
                                img_ske_ero[i, j] = (byte)255;
                            }
                        }
                    }
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            if(img_ske_ero[i, j] == 255)
                            {
                                con_next = true;
                            }
                            img_ske[i, j] = img_ske_ero[i, j];
                        }
                    }
                }

                if(con_next == false)
                {
                    break;
                }
                else
                {
                    ero_count++;
                }

                for (int i = 1; i < Height - 1; i++)//侵蝕               
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        flag = 1;
                        for (int m = -1; m < 2; m++)
                        {
                            for (int n = -1; n < 2; n++)
                            {
                                //自身及領域中若有一個為0
                                //則將該點設為0 
                                if (img_ske[i + m, j + n] != stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                                {
                                    flag = 0;
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            img_ske_open_ero[i, j] = (byte)0;
                        }
                        else
                        {
                            img_ske_open_ero[i, j] = (byte)255;
                        }
                    }
                }

                for (int i = 1; i < Height - 1; i++)//膨脹
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        flag = 1;
                        for (int m = -1; m < 2; m++)
                        {
                            for (int n = -1; n < 2; n++)
                            {
                                //自身及領域中若有一個為0
                                //則將該點設為0 
                                if (img_ske_open_ero[i + m, j + n] == stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                                {
                                    flag = 0;
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            img_ske_open_dil[i, j] = 255;
                        }
                        else
                        {
                            img_ske_open_dil[i, j] = 0;
                        }
                    }
                }

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        img_ske_diff[i, j] = (byte)(img_ske[i, j] - img_ske_open_dil[i, j]);
                    }
                }

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if(img_ske_diff[i, j] == 255 || img_ske_af[i, j] == 255)
                        {
                            img_ske_af[i, j] = 255;
                        }
                        else
                        {
                            img_ske_af[i, j] = 0;
                        }
                    }
                }                                            
            }          
            

            //purning
            //x1
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    img_prun_A[i, j] = img_ske_af[i, j];
                }
            }
            byte[,,] b1_stru = new byte[,,]
            {
                { {2, 0, 0}, {255, 255, 0}, {2, 0, 0} }, //b1
                { {2, 255, 2}, {0, 255, 0}, {0, 0, 0} }, //b2
                { {0, 0, 2}, {0, 255, 255}, {0, 0, 2} }, //b3
                { {0, 0, 0}, {0, 255, 0}, {2, 255, 2} }, //b4

                { {255, 0, 0}, {0, 255, 0}, {0, 0, 0} }, //b5
                { {0, 0, 255}, {0, 255, 0}, {0, 0, 0} }, //b6
                { {0, 0, 0}, {0, 255, 0}, {0, 0, 255} }, //b7
                { {0, 0, 0}, {0, 255, 0}, {255, 0, 0} }  //b8
            };
            byte[,,] b2_stru = new byte[8, 3, 3];
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    for(int k = 0; k < 3; k++)
                    {
                        if(b1_stru[i, j, k] != 2)
                        {
                            b2_stru[i, j, k] = (byte)(255 - b1_stru[i, j, k]);
                        }
                        else
                        {
                            b2_stru[i, j, k] = 2;
                        }
                    }
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_prun_af[i, j] = img_ske_af[i, j];
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_in_prun[i, j] = (byte)(255 - img_prun_af[i, j]);
                }
            }

            for (int c = 0; c < 1; c++)
            {
                for (int b = 0; b < 8; b++)
                {
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            img_prun_b1[i, j] = 0;
                            img_prun_b2[i, j] = 0;
                        }
                    }
                    for (int i = 1; i < Height - 1; i++)//侵蝕b1               
                    {
                        for (int j = 1; j < Width - 1; j++)
                        {
                            flag = 1;
                            for (int m = -1; m < 2; m++)
                            {
                                for (int n = -1; n < 2; n++)
                                {
                                    //自身及領域中若有一個為0
                                    //則將該點設為0 
                                    
                                    if (b1_stru[b, m + 1, n + 1] != 2)
                                    {
                                        if (img_prun_af[i + m, j + n] != b1_stru[b, m + 1, n + 1] && b1_stru[b, m + 1, n + 1] == 255)
                                        {
                                            flag = 0;
                                            break;
                                        }
                                    }                                                                   
                                }
                                if (flag == 0)
                                {
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                img_prun_b1[i, j] = 0;
                            }
                            else
                            {
                                img_prun_b1[i, j] = 255;
                            }
                        }
                    }

                    for (int i = 1; i < Height - 1; i++)//侵蝕b2               
                    {
                        for (int j = 1; j < Width - 1; j++)
                        {
                            flag = 1;
                            for (int m = -1; m < 2; m++)
                            {
                                for (int n = -1; n < 2; n++)
                                {
                                    //自身及領域中若有一個為0
                                    //則將該點設為0 

                                    if (b2_stru[b, m + 1, n + 1] != 2)
                                    {
                                        if (img_in_prun[i + m, j + n] != b2_stru[b, m + 1, n + 1] && b2_stru[b, m + 1, n + 1] == 255)
                                        {
                                            flag = 0;
                                            break;
                                        }
                                    }
                                }
                                if (flag == 0)
                                {
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                img_prun_b2[i, j] = 0;
                            }
                            else
                            {
                                img_prun_b2[i, j] = 255;
                            }
                        }
                    }

                    for (int i = 0; i < Height; i++)  //取交集
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            if (img_prun_b1[i, j] == 255 && img_prun_b2[i, j] == 255)
                            {
                                img_prun[i, j] = 255;
                            }
                            else
                            {
                                img_prun[i, j] = 0;
                            }
                        }
                    }                   
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            img_prun_af[i, j] = (byte)(img_prun_af[i, j] - img_prun[i, j]);
                        }
                    }
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            img_in_prun[i, j] = (byte)(255 - img_prun_af[i, j]);
                        }
                    }
                }                              
            }
            
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    img_prun_x1[i, j] = img_prun_af[i, j];
                }
            }
            //x2
            byte[,] img_prun_x2_tm = new byte[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_prun_x2_tm[i, j] = img_prun_x1[i, j];
                    img_prun_b1[i, j] = 0;
                    img_prun_b2[i, j] = 0;
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_in_prun[i, j] = (byte)(255 - img_prun_x2_tm[i, j]);
                }
            }
            for (int b = 0; b < 8; b++)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        img_prun_b1[i, j] = 0;
                        img_prun_b2[i, j] = 0;
                    }
                }
                for (int i = 1; i < Height - 1; i++)//侵蝕b1               
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        flag = 1;
                        for (int m = -1; m < 2; m++)
                        {
                            for (int n = -1; n < 2; n++)
                            {
                                //自身及領域中若有一個為0
                                //則將該點設為0 

                                if (b1_stru[b, m + 1, n + 1] != 2)
                                {
                                    if (img_prun_x2_tm[i + m, j + n] != b1_stru[b, m + 1, n + 1] && b1_stru[b, m + 1, n + 1] == 255)
                                    {
                                        flag = 0;
                                        break;
                                    }
                                }
                            }
                            if (flag == 0)
                            {
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            img_prun_b1[i, j] = 0;
                        }
                        else
                        {
                            img_prun_b1[i, j] = 255;
                        }
                    }
                }

                for (int i = 1; i < Height - 1; i++)//侵蝕b2               
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        flag = 1;
                        for (int m = -1; m < 2; m++)
                        {
                            for (int n = -1; n < 2; n++)
                            {
                                //自身及領域中若有一個為0
                                //則將該點設為0 

                                if (b2_stru[b, m + 1, n + 1] != 2)
                                {
                                    if (img_in_prun[i + m, j + n] != b2_stru[b, m + 1, n + 1] && b2_stru[b, m + 1, n + 1] == 255)
                                    {
                                        flag = 0;
                                        break;
                                    }
                                }
                            }
                            if (flag == 0)
                            {
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            img_prun_b2[i, j] = 0;
                        }
                        else
                        {
                            img_prun_b2[i, j] = 255;
                        }
                    }
                }

                for (int i = 0; i < Height; i++)  //取交集
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (img_prun_b1[i, j] == 255 && img_prun_b2[i, j] == 255)
                        {
                            img_prun_x2_tm[i, j] = 255;
                        }
                        else
                        {
                            img_prun_x2_tm[i, j] = 0;
                        }
                    }
                }
                for (int i = 0; i < Height; i++)  
                {
                    for (int j = 0; j < Width; j++)
                    {
                        img_in_prun[i, j] = (byte)(255 - img_prun_x2_tm[i, j]);
                    }
                }
            }
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_prun_x2[i, j] = img_prun_x2_tm[i, j];
                }
            }
            //x3
            byte[,] img_prun_x3_tm = new byte[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    img_prun_x3_tm[i, j] = img_prun_x2[i, j];
                }
            }
            for (int c = 0; c < 1; c++)
            {
                for (int i = 1; i < Height - 1; i++)//膨脹
                {
                    for (int j = 1; j < Width - 1; j++)
                    {
                        flag = 1;
                        for (int m = -1; m < 2; m++)
                        {
                            for (int n = -1; n < 2; n++)
                            {
                                //自身及領域中若有一個為0
                                //則將該點設為0 
                                if (img_prun_x3_tm[i + m, j + n] == stru[m + 1, n + 1] && stru[m + 1, n + 1] == 255)
                                {
                                    flag = 0;
                                    break;
                                }
                            }
                            if (flag == 0)
                            {
                                break;
                            }
                        }
                        if (flag == 0)
                        {
                            img_prun_x3[i, j] = 255;
                        }
                        else
                        {
                            img_prun_x3[i, j] = 0;
                        }
                    }
                }

                for(int i = 0; i < Height; i++)
                {
                    for(int j = 0; j < Width; j++)
                    {   
                        if(img_prun_x3[i, j] == 255 && img_prun_A[i, j] == 255)
                        {
                            img_prun_x3_tm[i, j] = 255;
                        }
                        else
                        {
                            img_prun_x3_tm[i, j] = 0;
                        }                      
                    }
                }
            }

            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    img_prun_x3[i, j] = img_prun_x3_tm[i, j];
                }
            }
            //x4
            for(int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    if(img_prun_x1[i, j] == 255 || img_prun_x3[i, j] == 255)
                    {
                        img_prun_x4[i, j] = 255;
                    }
                    else
                    {
                        img_prun_x4[i, j] = 0;
                    }
                }
            }

            //邊界算法
            byte[,] bound = new byte[Height, Width];
            byte[,] bound_af = new byte[Height, Width];
            int[] point_x = new int[0];
            int[] point_y = new int[0];                                  

            for (int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    bound[i, j] = img_prun_x4[i, j];
                }
            }

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if(bound[i, j] == 255)
                    {
                        Array.Resize(ref point_x, point_x.Length + 1);
                        Array.Resize(ref point_y, point_y.Length + 1);
                        point_x[point_x.Length - 1] = j;
                        point_y[point_y.Length - 1] = i;
                    }
                }
            }           
            //排序
            double[] record_degree = new double[point_x.Length];
            int[] record_in = new int[point_x.Length];
            int tem_x, tem_y;
            int area = 0, cen_x_total = 0, cen_y_total = 0, cen_x, cen_y;
            for (int i = 0; i < Height; i++)
            {
                for(int j = 0; j < Width; j++)
                {
                    if(bound[i, j] == 255)
                    {
                        area++;
                        cen_y_total += i;
                        cen_x_total += j;
                    }
                }
            }
            cen_x = cen_x_total / area;
            cen_y = cen_y_total / area;         
            record_degree[0] = 0;
            int record_j = -1;
            double min_degree = 100000, com_degree, max_value, com_value, a_l = 0, b_l = 0, c_l = 0;
            int logic_x, logic_y;
            b_l = Math.Sqrt((point_x[0] - cen_x) * (point_x[0] - cen_x) + (point_y[0] - cen_y) * (point_y[0] - cen_y));           
            logic_x = point_x[0] - cen_x;
            logic_y = point_y[0] - cen_y;
            for (int i = 1; i < point_x.Length; i++)
            {
                record_j = i;
                c_l = Math.Sqrt((point_x[i] - cen_x) * (point_x[i] - cen_x) + (point_y[i] - cen_y) * (point_y[i] - cen_y));
                a_l = Math.Sqrt((point_x[i] - point_x[0]) * (point_x[i] - point_x[0]) + (point_y[i] - point_y[0]) * (point_y[i] - point_y[0]));
                max_value = ((b_l * b_l) + (c_l * c_l) - (a_l * a_l)) / (2 * b_l * c_l);
                min_degree = Math.Acos(max_value) * 180 / Math.PI;
                if ((logic_x * (point_y[i] - cen_y) - logic_y * (point_x[i] - cen_x)) < 0)
                {
                    min_degree = 360 - min_degree;
                }
                for (int j = i; j < point_x.Length; j++)
                {                    
                    c_l = Math.Sqrt((point_x[j] - cen_x) * (point_x[j] - cen_x) + (point_y[j] - cen_y) * (point_y[j] - cen_y));
                    a_l = Math.Sqrt((point_x[j] - point_x[0]) * (point_x[j] - point_x[0]) + (point_y[j] - point_y[0]) * (point_y[j] - point_y[0]));
                    com_value = ((b_l * b_l) + (c_l * c_l) - (a_l * a_l)) / (2 * b_l * c_l);
                    com_degree = Math.Acos(com_value) * 180 / Math.PI;
                    if ((logic_x * (point_y[j] - cen_y) - logic_y * (point_x[j] - cen_x)) < 0)
                    {
                        com_degree = 360 - com_degree;
                    }
                    if (min_degree > com_degree)
                    {
                        min_degree = com_degree;
                        record_j = j;
                    }
                }
                tem_x = point_x[i];
                tem_y = point_y[i];
                point_x[i] = point_x[record_j];
                point_y[i] = point_y[record_j];
                point_x[record_j] = tem_x;
                point_y[record_j] = tem_y;
                record_degree[i] = min_degree;
            }
            
            //畫線
            double a_con, b_con;
            int beg_x, beg_y, end_x, end_y, line_xy, x1 = -1, x2 = -1, y1 = -1, y2 = -1;           
            for(int i = 0; i < point_x.Length - 1; i++)
            {
                x1 = point_x[i];
                y1 = point_y[i];
                x2 = point_x[i + 1];
                y2 = point_y[i + 1];
                if(x1 == x2 && y1 == y2)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("error:{0},{1},{2},{3},{4}", x1, y1, x2, y2, i));
                }

                if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
                {
                    a_con = (double)(y2 - y1) / (double)(x2 - x1);
                    b_con = y1 - (double)(x1 * a_con);
                    if (x2 > x1)
                    {
                        beg_x = x1;
                        beg_y = y1;
                        end_x = x2;
                        end_y = y2;
                    }
                    else
                    {
                        beg_x = x2;
                        beg_y = y2;
                        end_x = x1;
                        end_y = y1;
                    }
                    for (int k = beg_x; k <= end_x; k++)
                    {
                        line_xy = (int)(k * a_con + b_con);
                        if (k >= 0 && k < Width && line_xy >= 0 && line_xy < Height)
                        {
                            bound[line_xy, k] = 255;
                        }
                    }
                }
                else if(Math.Abs(x2 - x1) < Math.Abs(y2 - y1))
                {
                    a_con = (double)(x2 - x1) / (double)(y2 - y1);
                    b_con = x1 - (double)(y1 * a_con);
                    if (y2 > y1)
                    {
                        beg_x = x1;
                        beg_y = y1;
                        end_x = x2;
                        end_y = y2;
                    }
                    else
                    {
                        beg_x = x2;
                        beg_y = y2;
                        end_x = x1;
                        end_y = y1;
                    }
                    for (int k = beg_y; k <= end_y; k++)
                    {
                        line_xy = (int)(k * a_con + b_con);
                        if (k >= 0 && k < Height && line_xy >= 0 && line_xy < Width)
                        {
                            bound[k, line_xy] = 255;
                        }
                    }
                }
            }

            x1 = point_x[point_x.Length - 1];
            y1 = point_y[point_y.Length - 1];
            x2 = point_x[0];
            y2 = point_y[0];
            if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
            {
                a_con = (double)(y2 - y1) / (double)(x2 - x1);
                b_con = y1 - (double)(x1 * a_con);
                if (x2 > x1)
                {
                    beg_x = x1;
                    beg_y = y1;
                    end_x = x2;
                    end_y = y2;
                }
                else
                {
                    beg_x = x2;
                    beg_y = y2;
                    end_x = x1;
                    end_y = y1;
                }
                for (int k = beg_x; k <= end_x; k++)
                {
                    line_xy = (int)(k * a_con + b_con);
                    if (k >= 0 && k < Width && line_xy >= 0 && line_xy < Height)
                    {
                        bound[line_xy, k] = 255;
                    }
                }
            }
            else if (Math.Abs(x2 - x1) < Math.Abs(y2 - y1))
            {
                a_con = (double)(x2 - x1) / (double)(y2 - y1);
                b_con = x1 - (double)(y1 * a_con);
                if (y2 > y1)
                {
                    beg_x = x1;
                    beg_y = y1;
                    end_x = x2;
                    end_y = y2;
                }
                else
                {
                    beg_x = x2;
                    beg_y = y2;
                    end_x = x1;
                    end_y = y1;
                }
                for (int k = beg_y; k <= end_y; k++)
                {
                    line_xy = (int)(k * a_con + b_con);
                    if (k >= 0 && k < Height && line_xy >= 0 && line_xy < Width)
                    {
                        bound[k, line_xy] = 255;
                    }
                }
            }

            //均值濾波
            byte[,] smooth = new byte[Height, Width];
            byte[,] smooth_af = new byte[Height, Width];
            int[] point_x_af = new int[point_x.Length - 31];
            int[] point_y_af = new int[point_y.Length - 31];
            int sum_x = 0, sum_y = 0;
            for(int i = 0; i < point_x_af.Length; i++)
            {
                sum_x = 0;
                sum_y = 0;
                for(int j = 0; j < 31; j++)
                {
                    sum_x += point_x[i + j];
                    sum_y += point_y[i + j];
                }
                point_x_af[i] = sum_x / 31;
                point_y_af[i] = sum_y / 31;
            }
           
            for (int i = 0; i < point_x_af.Length - 1; i++)
            {
                x1 = point_x_af[i];
                y1 = point_y_af[i];
                x2 = point_x_af[i + 1];
                y2 = point_y_af[i + 1];
                if (x1 == x2 && y1 == y2)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("error:{0},{1},{2},{3},{4}", x1, y1, x2, y2, i));
                }

                if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
                {
                    a_con = (double)(y2 - y1) / (double)(x2 - x1);
                    b_con = y1 - (double)(x1 * a_con);
                    if (x2 > x1)
                    {
                        beg_x = x1;
                        beg_y = y1;
                        end_x = x2;
                        end_y = y2;
                    }
                    else
                    {
                        beg_x = x2;
                        beg_y = y2;
                        end_x = x1;
                        end_y = y1;
                    }
                    for (int k = beg_x; k <= end_x; k++)
                    {
                        line_xy = (int)(k * a_con + b_con);
                        if (k >= 0 && k < Width && line_xy >= 0 && line_xy < Height)
                        {
                            smooth_af[line_xy, k] = 255;
                        }
                    }
                }
                else if (Math.Abs(x2 - x1) < Math.Abs(y2 - y1))
                {
                    a_con = (double)(x2 - x1) / (double)(y2 - y1);
                    b_con = x1 - (double)(y1 * a_con);
                    if (y2 > y1)
                    {
                        beg_x = x1;
                        beg_y = y1;
                        end_x = x2;
                        end_y = y2;
                    }
                    else
                    {
                        beg_x = x2;
                        beg_y = y2;
                        end_x = x1;
                        end_y = y1;
                    }
                    for (int k = beg_y; k <= end_y; k++)
                    {
                        line_xy = (int)(k * a_con + b_con);
                        if (k >= 0 && k < Height && line_xy >= 0 && line_xy < Width)
                        {
                            smooth_af[k, line_xy] = 255;
                        }
                    }
                }
            }

            x1 = point_x_af[point_x_af.Length - 1];
            y1 = point_y_af[point_y_af.Length - 1];
            x2 = point_x_af[0];
            y2 = point_y_af[0];
            if (Math.Abs(x2 - x1) > Math.Abs(y2 - y1))
            {
                a_con = (double)(y2 - y1) / (double)(x2 - x1);
                b_con = y1 - (double)(x1 * a_con);
                if (x2 > x1)
                {
                    beg_x = x1;
                    beg_y = y1;
                    end_x = x2;
                    end_y = y2;
                }
                else
                {
                    beg_x = x2;
                    beg_y = y2;
                    end_x = x1;
                    end_y = y1;
                }
                for (int k = beg_x; k <= end_x; k++)
                {
                    line_xy = (int)(k * a_con + b_con);
                    if (k >= 0 && k < Width && line_xy >= 0 && line_xy < Height)
                    {
                        smooth_af[line_xy, k] = 255;
                    }
                }
            }
            else if (Math.Abs(x2 - x1) < Math.Abs(y2 - y1))
            {
                a_con = (double)(x2 - x1) / (double)(y2 - y1);
                b_con = x1 - (double)(y1 * a_con);
                if (y2 > y1)
                {
                    beg_x = x1;
                    beg_y = y1;
                    end_x = x2;
                    end_y = y2;
                }
                else
                {
                    beg_x = x2;
                    beg_y = y2;
                    end_x = x1;
                    end_y = y1;
                }
                for (int k = beg_y; k <= end_y; k++)
                {
                    line_xy = (int)(k * a_con + b_con);
                    if (k >= 0 && k < Height && line_xy >= 0 && line_xy < Width)
                    {
                        smooth_af[k, line_xy] = 255;
                    }
                }
            }

            IntPtr imgPtr;           

            imgPtr = iImage.iVarPtr(ref img_sob_add_thr[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox2);

            imgPtr = iImage.iVarPtr(ref img_sob_add_thr_mor[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox3);           

            imgPtr = iImage.iVarPtr(ref img_clos_ero[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox7);           

            imgPtr = iImage.iVarPtr(ref img_ske_af[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox4);           

            imgPtr = iImage.iVarPtr(ref img_prun_x4[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox5);          
            
            imgPtr = iImage.iVarPtr(ref bound[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox6);           

            imgPtr = iImage.iVarPtr(ref smooth_af[0, 0]);
            err = iImage.iPointerToiImage(Image, imgPtr, Width, Height);
            print_error(err);
            //save_img(Image);
            show_img(Image, pictureBox8);
        }     

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            iImage.DestroyiImage(Image);  // Release the image struction before shut down the Application
        }      
    }
}
