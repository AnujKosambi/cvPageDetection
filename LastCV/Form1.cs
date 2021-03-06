﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

using System.Diagnostics;
using System.Windows.Forms;

using System.IO;


namespace LastCV
{

    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            
            DirectionText = new string[10];
            DirectionText[0] = "Nice";
            DirectionText[1] = "Up";
            DirectionText[2] = "Down";
            DirectionText[3] = "Left";
            DirectionText[4] = "Top";
            DirectionText[5] = "Bottom";
            DirectionText[6] = "Right";
    
            InitializeComponent();
            CaptureCamera();
         
        }
     
        int deviceNumber = 0;
        private Capture cam;
        private Thread _cameraThread;
        public static int thre1,thre2;
        public static int cannyThre1, cannyThre2;
        public static  double MARGINW = 5, MARGINH = 5;
        private IntPtr m_ip = IntPtr.Zero;
        private static int[,] Status=new int[3,3];
        private string[] DirectionText;
        private CvCapture cap;
        private void CaptureCamera()
        {

            
            
            _cameraThread = new Thread(new ThreadStart(CaptureCameraCallback));

            _cameraThread.Start();
        }

        private void setMargin(double i, double j)
        {
            MARGINW = i;
            MARGINH = j;
        }

        private void CaptureCameraCallback()

        {
            
          
         //   using ( cap = CvCapture.FromCamera(CaptureDevice.Any,-1))
          //  {
             
                
                while (CvWindow.WaitKey(10) < 0)
                {
                    int[] HORI = new int[2];
                    int[] VERTI = new int[2];

                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            Status[i, j] = 0;
              
                    
    

                    IplImage display = cap.QueryFrame();
                    IplImage gray = new IplImage(display.Size, BitDepth.U8, 1);
                    IplImage mainImage = display.Clone();


                    int WIDTH = (mainImage.Width);

                    int HEIGHT = (mainImage.Height);
          
 
                    
                    HORI[0] =(int) (WIDTH/MARGINW);
                    HORI[1] = (int)((MARGINW - 1) * HORI[0]);
                    VERTI[0] = (int)(HEIGHT / MARGINH);
                    VERTI[1] = (int)((MARGINH - 1) * VERTI[0]);
                    
                    try
                    {  
                        #region BLOCK_DETECTION

                        mainImage.Smooth(mainImage, SmoothType.Blur, 15, 15);
                        mainImage.CvtColor(gray, ColorConversion.BgrToGray);
                       
                        
                        System.Diagnostics.Debug.WriteLine("" + cannyThre1);
                        Cv.Canny(gray, gray, 35, 35, ApertureSize.Size3);
                        gray.Smooth(gray, SmoothType.Blur, 3,3);
                        CvSeq<CvPoint> contours;
                        CvMemStorage _storage = new CvMemStorage();
                        Cv.FindContours(gray, _storage, out contours, CvContour.SizeOf, ContourRetrieval.Tree, ContourChain.ApproxSimple);
                       // Cv.DrawContours(mainImage, contours, CvColor.Blue, CvColor.Green, 1,1, LineType.Link8);
                        //CvLineSegmentPolar[] lines= gray.HoughLinesStandard(1, Cv.PI / 180, 50, 0, 0);
                        CvMemStorage storage = new CvMemStorage();
                        storage.Clear();

#if DEBUG
                        setupDebug(mainImage,HORI,VERTI);
#endif
                        int minL=0;
                        int minT=0;
                        
                        // gray.HoughLines2(storage, HoughLinesMethod.Standard, Cv.PI / 180, 0, 0);
                        CvSeq lines = gray.HoughLines2(storage, HoughLinesMethod.Probabilistic, 1, Math.PI / 180, 50,50,1);
                        for (int i = 0; i < lines.Total; i++)
                        {
                          

                            CvLineSegmentPoint elem = lines.GetSeqElem<CvLineSegmentPoint>(i).Value;
                            mainImage.Line(elem.P1, elem.P2, CvColor.Navy, 2);

                            if (elem.P1.X < HORI[0])
                            { minL = Math.Max(elem.P1.X, minL); }
                            if (elem.P2.X < HORI[0])
                            { minL = Math.Max(elem.P2.X, minL); }

                            if (elem.P1.Y < VERTI[0])
                            { minT = Math.Max(elem.P1.Y, minT); }
                            if (elem.P2.Y < VERTI[0])
                            { minT = Math.Max(elem.P2.Y, minT); }

                            

                   

                            try
                            {
                                if (elem.P1.Y <= HEIGHT-10  && elem.P2.Y <= HEIGHT-10)
                                {
                                    int i1 = (elem.P1.X < HORI[0]) ? 0 : (elem.P1.X / HORI[1])+1;
                                    int j1 = (elem.P1.Y < VERTI[0]) ? 0 : (elem.P1.Y / VERTI[1])+1;
                                    Status[i1, j1]++;
                                    int i2 = (elem.P2.X < HORI[0]) ? 0 : (elem.P2.X / HORI[1])+1;
                                    int j2 = (elem.P2.Y < VERTI[0]) ? 0 : (elem.P2.Y / VERTI[1])+1;
                                    Status[i2, j2]++;
                                    double slope=0,c=0;
                                    List<CvPoint> points = new List<CvPoint>(Math.Abs(i1 - i2) + Math.Abs(j1 - j2) + 2);
                                    if (elem.P1.X != elem.P2.X)
                                    {
                                        slope = 1.0d * (elem.P2.Y - elem.P1.Y) / (elem.P2.X - elem.P1.X);
                                        c = elem.P1.Y - slope * elem.P1.X;
                                        for (int p = Math.Min(i1, i2); p != Math.Max(i1, i2); p++)
                                            points.Add(new CvPoint(HORI[p], (int)(slope * HORI[p] + c)));
                                        for (int p = Math.Min(j1, j2); p != Math.Max(j1, j2); p++)
                                            points.Add(new CvPoint((int)((VERTI[p] - c) / slope), VERTI[p]));
                                    }
                                    else
                                    {

                                        for (int p = Math.Min(j1, j2); p != Math.Max(j1, j2); p++)
                                        {
                                            points.Add(new CvPoint(elem.P1.X, VERTI[p]));
                                         
                                        }
                                    }
#if DEBUG
                                        foreach (var point in points)
                                            mainImage.Line(point, point, CvColor.Yellow, 10);
#endif


                                        points.Add(elem.P1);
                                        points.Add(elem.P2);
                                      
                                        points.Sort((a, b) =>
                                        {
                                            int result = a.X.CompareTo(b.X);
                                            if (result == 0) result = a.Y.CompareTo(b.Y);
                                            return result;
                                        });

                                        for (int p = 0; p < points.Capacity - 1; p++)
                                        {
                                            CvPoint mid = new CvPoint((points[p].X + points[p + 1].X) / 2,
                                                                     (points[p].Y + points[p + 1].Y) / 2);

                                            int x = (mid.X < HORI[0]) ? 0 : (mid.X/ HORI[1]) + 1;
                                            int y = (mid.Y < VERTI[0]) ? 0 : (mid.Y / VERTI[1]) + 1;
                                            Status[x,y]++;
#if DEBUG
                                            mainImage.Line(mid, mid, CvColor.Green, 10);
#endif




                                        }
                                        
                                     
                                    
                                   
                                  
                                }
                            }
                            catch (IndexOutOfRangeException e)
                            {
                            }

                        }
                    #endregion

                        int DIRECTION = getClipDirection(display, HORI, VERTI, minL, minT);
                        display.PutText(getDirection(DIRECTION), new CvPoint(1 * WIDTH / 3 + (WIDTH / 6), 1 * HEIGHT / 3 + HEIGHT / 6), new CvFont(FontFace.HersheyTriplex,1,1), CvColor.Navy);
                        
                    }
                    catch (OpenCvSharp.OpenCvSharpException e)
                    {

                    }
                    catch (OpenCVException e) { }

                    Bitmap bm = BitmapConverter.ToBitmap(display);

                    bm.SetResolution(300,300);
             
                       pictureBox.Image = bm;
#if DEBUG
             
#endif 
                }
                
        //   }


        }
        private void newThreadCallBack()
        
        {
            
            cam = new Capture(0, 640, 480, 24, pictureBox);
            while (true) ;
        }

        protected  void dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);

            if (m_ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(m_ip);
                m_ip = IntPtr.Zero;
            }
        }
        private int getClipDirection(IplImage mainImage,int[] HORI,int[] VERTI,int minL,int minT){

            int[] _HORI = new int[4];
            int[] _VERTI = new int[4];
            int WIDTH=mainImage.Width;
            int HEIGHT=mainImage.Height;
            _HORI[0] = 0;
            _VERTI[0] = 0;
            _HORI[1] = HORI[0]; _HORI[2] = HORI[1];
            _VERTI[1] = VERTI[0]; _VERTI[2] = VERTI[1];
            _HORI[3] = mainImage.Width;
            _VERTI[3] = mainImage.Height;

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if (Status[i, j] > 0)
                        Cv.Rectangle(mainImage, new CvPoint(_HORI[i], _VERTI[j]), new CvPoint(_HORI[i + 1], _VERTI[j + 1]), new CvScalar(255, 255, 0, 0), 10);


                }

            int leftD = 0, topD = 0, bottomD = 0, rightD = 0;
            if (Status[0, 1] > 0) leftD = 3; if (Status[1, 0] > 0) topD = 4; if (Status[1, 2] > 1) bottomD = 5; if (Status[2, 1] > 0) rightD = 6;
            int sum = leftD + topD + bottomD + rightD;
            int DIRECTION = 1;
            if (sum == 18)
            {
                clickPhoto();
                DIRECTION = 0;
            }
            else if (sum > 11)
            {
                DIRECTION = sum - 9;
            }
            else if (sum == 9)
            {
                if (Status[1, 1] == 0)
                {
                    if (leftD == 3)
                    {

                        setMargin(WIDTH / (minL + 20), MARGINH);
                    }
                    if (topD == 4)
                    {

                        setMargin(MARGINW, HEIGHT / (minT + 20));
                    }
                    DIRECTION = 1;
                }
                else
                {
                    DIRECTION = 3 + (int)((leftD > 0) ? 0 : 1);

                    for (int i = 0; i < 3; i += 2)
                        for (int j = 0; j < 3; j += 2)
                            if (Status[i, j] > 0)
                                if (leftD > 0)
                                    DIRECTION = 5 - (int)(j * 0.5);
                                else if (topD > 0)
                                    DIRECTION = 6 - (int)(i * 1.5);


                }

            }
            else if (sum > 6)
            {
                if (sum > 9)
                {
                    if (Status[1, 1] == 0)
                        DIRECTION = 6;
                    else
                    {
                        DIRECTION = 3;
                    }

                }
                else
                {
                    if (Status[1, 1] == 0)
                        DIRECTION = 3;
                    else
                    {
                        DIRECTION = 6;
                    }

                }
            }
            else if (sum > 0)
            {
                DIRECTION = sum;
            }
            else if (Status[1, 1] > 0)
                DIRECTION = 2;
            return DIRECTION;
        }
        private void clickPhoto()
        {
         
           
            cap.Dispose();
           
            Thread newThread = new Thread(new ThreadStart(newThreadCallBack));

            newThread.Start();
            _cameraThread.Abort();

      
 
        }
        private void setupDebug(IplImage mainImage,int[] HORI,int[] VERTI)
        {
#if DEBUG
            CvPoint p01 = new CvPoint(HORI[0], 0);
            CvPoint p31 = new CvPoint(HORI[0], mainImage.Height);
            CvPoint p02 = new CvPoint(HORI[1], 0);
            CvPoint p32 = new CvPoint(HORI[1], mainImage.Height);
            //Horizatial
            CvPoint p10 = new CvPoint(0, VERTI[0]);
            CvPoint p13 = new CvPoint(mainImage.Width, VERTI[0]);
            CvPoint p20 = new CvPoint(0, VERTI[1]);
            CvPoint p23 = new CvPoint(mainImage.Width, VERTI[1]);

            mainImage.Line(p01, p31, CvColor.LightGreen, 1, LineType.AntiAlias, 0);
            mainImage.Line(p02, p32, CvColor.LightGreen, 1, LineType.AntiAlias, 0);
            mainImage.Line(p10, p13, CvColor.LightGreen, 1, LineType.AntiAlias, 0);
            mainImage.Line(p20, p23, CvColor.LightGreen, 1, LineType.AntiAlias, 0);
            int WIDTH = mainImage.Width;
            int HEIGHT = mainImage.Height;
         
#endif
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            
            if (_cameraThread != null && _cameraThread.ThreadState==System.Threading.ThreadState.Suspended)
            {
                _cameraThread.Resume();
            }
            if (_cameraThread != null && _cameraThread.IsAlive)
            {
                if(cam!=null)cam.Dispose();
                _cameraThread.Abort();
            }
          
        }

        private String getDirection(int i)
        {

            return DirectionText[i];
        }


  
    }
}
