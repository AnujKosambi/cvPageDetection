﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System.Drawing;
namespace LastCV
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
       

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
   
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.pictureBoxDebug = new System.Windows.Forms.PictureBox();
            this.threshold1 = new System.Windows.Forms.TrackBar();
            this.threshold2 = new System.Windows.Forms.TrackBar();
            this.cannyBar1 = new System.Windows.Forms.TrackBar();
            this.cannyBar2 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyBar2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(12, 58);
            this.pictureBox.MaximumSize = new System.Drawing.Size(1666, 2222);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(640, 480);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // pictureBoxDebug
            // 
            this.pictureBoxDebug.Location = new System.Drawing.Point(658, 58);
            this.pictureBoxDebug.Name = "pictureBoxDebug";
            this.pictureBoxDebug.Size = new System.Drawing.Size(640, 480);
            this.pictureBoxDebug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDebug.TabIndex = 1;
            this.pictureBoxDebug.TabStop = false;
            // 
            // threshold1
            // 
            this.threshold1.Location = new System.Drawing.Point(12, 7);
            this.threshold1.Name = "threshold1";
            this.threshold1.Size = new System.Drawing.Size(327, 45);
            this.threshold1.TabIndex = 1;
            this.threshold1.Scroll += new System.EventHandler(this.threshold1_Scroll);
            // 
            // threshold2
            // 
            this.threshold2.Location = new System.Drawing.Point(345, 7);
            this.threshold2.Name = "threshold2";
            this.threshold2.Size = new System.Drawing.Size(307, 45);
            this.threshold2.TabIndex = 1;
            this.threshold2.Scroll += new System.EventHandler(this.threshold2_Scroll);
            // 
            // cannyBar1
            // 
            this.cannyBar1.Location = new System.Drawing.Point(653, 7);
            this.cannyBar1.Name = "cannyBar1";
            this.cannyBar1.Size = new System.Drawing.Size(316, 45);
            this.cannyBar1.TabIndex = 2;
            this.cannyBar1.Scroll += new System.EventHandler(this.cannyBar1_Scroll);
            // 
            // cannyBar2
            // 
            this.cannyBar2.Location = new System.Drawing.Point(975, 7);
            this.cannyBar2.Name = "cannyBar2";
            this.cannyBar2.Size = new System.Drawing.Size(287, 45);
            this.cannyBar2.TabIndex = 3;
            this.cannyBar2.Scroll += new System.EventHandler(this.cannyBar2_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.cannyBar2);
            this.Controls.Add(this.cannyBar1);
            this.Controls.Add(this.threshold2);
            this.Controls.Add(this.threshold1);
            this.Controls.Add(this.pictureBoxDebug);
            this.Controls.Add(this.pictureBox);
            this.Name = "Form1";
            this.Text = "CV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDebug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threshold2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cannyBar2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.PictureBox pictureBoxDebug;
        private System.Windows.Forms.TrackBar threshold1;
        private System.Windows.Forms.TrackBar threshold2;
        private System.Windows.Forms.TrackBar cannyBar1;
        private System.Windows.Forms.TrackBar cannyBar2;

    }
}

