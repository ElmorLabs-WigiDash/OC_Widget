using WigiDashWidgetFramework;
using WigiDashWidgetFramework.WidgetUtility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Controls;

using System;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;
using System.Diagnostics;
using System.Security.Permissions;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Reflection.Emit;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Windows;
using Point = System.Drawing.Point;
using Font = System.Drawing.Font;
using System.IO;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Media;
using System.Windows.Documents;
using Brush = System.Drawing.Brush;
using Pen = System.Drawing.Pen;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using System.Windows.Media.Media3D;
using static System.Windows.Forms.LinkLabel;
using System.Collections;
using System.IO.Packaging;
using System.Diagnostics.Contracts;
using static OCWidget.intel_core_ui;

namespace OCWidget
{  
    //screensize 1000 * 592
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct My_Processor_Core
    {
        public int Efficiency_Class_size;
        public int GroupCount;
        public int Group_size;
        public fixed int Node_num[512];
        public unsafe fixed int Efficiency_Class[512];
        public unsafe fixed int Hyperthreaded[512];
        public unsafe fixed ulong Processor_Mask[512];
        public int num_smallcores;
        public int num_bigcores;
        public unsafe fixed ushort get_Group[512];
        public int threads;
        public unsafe fixed int slice_number[512];
        public unsafe fixed int cluster_number[512];
        public unsafe fixed byte thread_usage_vec[512];
    }
    public struct grid_point_struct
    {
        public Point[]grid_ptrs;
        public int x_offset;
        public int y_offset;
        public int start_x;
        public int end_x;
        public int start_y;
        public int end_y;
        public int x_range;
        public int y_range;
        public int label_x;
        public int label_y;
    }
    public struct grid_line_struct
    {
        public Point[] grid_ptrs;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct intel_vf_pt_struct
    {
        public int num_pts;
        public fixed int ratio[16];
        public fixed int volt[16];

    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct intel_bycore_usage_vec
    {
        public fixed int bycore_usage[8];
        public fixed int bycore_1ae[8];
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct intel_settings_struct
    {
        public float per_core_ratio;
        public fixed int bycore_usage[8];
        public fixed int bycore_1ae[8];
        public int vid;
        public int offset;
        public int manual;
        public fixed int vf_pts[16];
        public int pl1;
        public int pl2;
        public int iccmax;
        public int iccmax_dis;
        public int for_all;

        public int min_ring;
        public int max_ring;
        public int ring_vid;
        public int ring_offset;
        public int ring_manual;
        public int ring_ocmb_ratio;
        public int ringdownbin_dis;
    }
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct amd_settings_struct
    {
        public int freq;
        public int vid;
        public int ocmode;
        public int ppt;
        public int tdc;
        public int edc;
        public int thm;
        public int fmax;
        public int fit;
        public int co;
        public int forall;
    }

    public unsafe struct amd_readme_struct
    {
        public fixed int type[10];
        public fixed int pb[10];
        public int vid;
        public int freq;
        public int family;
        public fixed int co[128];
    }
    public class my_button
    {
        public System.Drawing.Color text_color = new System.Drawing.Color();
        public System.Drawing.Pen borderpen = new System.Drawing.Pen(System.Drawing.Color.LightGoldenrodYellow);
        public int x;
        public int y;
        public int width;
        public int height;
        public System.Drawing.Rectangle box_rect;
        public string text;
        public my_button(int x_in, int y_in, Bitmap bp, Font myfont, string str)
        { 
            text = str;
            text_color = Color.LightSkyBlue;
            using (Graphics g = Graphics.FromImage(bp))
            {
                SizeF char_size = g.MeasureString(text, myfont); 
                width = (int)char_size.Width + 10;
                height = (int)char_size.Height*2;
                x = x_in;
                y = y_in;
                box_rect = new System.Drawing.Rectangle(x, y, width, height); 
            }
        }
    }
    public class my_checkbox
    {
        public System.Drawing.Pen mypen;
        public System.Drawing.Pen borderpen = new System.Drawing.Pen(System.Drawing.Color.LightBlue);
        public int x;
        public int y;
        public int width;
        public int height;
        public System.Drawing.Rectangle box_rect;
        public System.Drawing.Rectangle inner_rect;
        public bool is_check = false;
        public int width_each_box;
        public my_checkbox(int x_in, int y_in,Bitmap bp, Font myfont, bool is_checked)
        {
            string textsize = "8";
            is_check = is_checked;
            using (Graphics g = Graphics.FromImage(bp))
            {
                SizeF char_size = g.MeasureString(textsize, myfont);
                width_each_box = (int)char_size.Width * 2;
                width = width_each_box;
                height = width_each_box;
                x = x_in;
                y = y_in;
                box_rect = new System.Drawing.Rectangle(x, y, width, height);
                inner_rect = new System.Drawing.Rectangle(x+5, y+5, width-10, height - 10);
            }
        }
        public void toggle_checkbox()
        {
            is_check = !is_check;
        }
    }
    public class user_input_numerical
    {
        public System.Drawing.Pen mypen;
        public System.Drawing.Pen borderpen = new System.Drawing.Pen(System.Drawing.Color.OrangeRed);
        public int x;
        public int y;
        public int width;
        public int height;
        public string text;
        public bool isnum = true;
        public int maxchars = 10;
        public float maxvalue = 9999999;
        public float value;
        public int num_rows=3;
        public int num_cols = 10;
        public List<System.Drawing.Rectangle> rect_vec;
        public int width_each_box;
        public int height_each_box;
        public bool is_ready = false;
        public string original_text;
        public float original_value;
        public user_input_numerical()
        {

        }
        public user_input_numerical(Bitmap bp, Font myfont,float current_value)
        {
            value = current_value;
            text = value.ToString();
            string textsize = "8";
            using (Graphics g = Graphics.FromImage(bp))
            {
                SizeF char_size = g.MeasureString(textsize, myfont);
                width_each_box = (int)char_size.Width *4;
                height_each_box = (int)char_size.Height * 4;
                width = width_each_box * num_cols;
                height = height_each_box * num_rows;
                x = (bp.Width - width) / 2;
                y= (bp.Height - height) / 2;
                System.Drawing.Rectangle display_rect=new System.Drawing.Rectangle(x, y, width, height);
                rect_vec = new List<System.Drawing.Rectangle>();
                rect_vec.Add(display_rect);
                int startx = x;
                int starty = y+ height_each_box;
                for(int i=0;i< num_cols; i++)
                {
                    System.Drawing.Rectangle number_rect = new System.Drawing.Rectangle(startx, starty, width_each_box, height_each_box);
                    startx += width_each_box;
                    rect_vec.Add(number_rect);
                }
                startx = x;
                starty+= height_each_box;
                //we have negative,increment,decrement,dot,clr,backspace,enter
                int width_each_but = width / 8;
                for (int i = 0; i < 8; i++)
                {
                    System.Drawing.Rectangle but_rect = new System.Drawing.Rectangle(startx, starty, width_each_but, height_each_box);
                    startx += width_each_but;
                    rect_vec.Add(but_rect);
                }
            }
            original_text=text;
            original_value=value;
        }
        public bool hitme(int x_in,int y_in)
        {
            is_ready = false;
            bool hit = false;
            int index = 0;
            foreach(System.Drawing.Rectangle rect in rect_vec)
            {
                if(index>0)
                {
                    if(index<=10)
                    {
                        if(x_in>=rect.X && x_in <= (rect.X+rect.Width) && y_in>=rect.Y && y_in<= (rect.Y + rect.Height))
                        {
                            hit = true;
                            int num = index - 1;
                            if (text.Length < maxchars)
                            {
                                text += num.ToString();
                                value = (float)Convert.ToDouble(text);
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (x_in >= rect.X && x_in <= (rect.X + rect.Width) && y_in >= rect.Y && y_in <= (rect.Y + rect.Height))
                        {
                            hit = true;
                            //we have negative,increment,decrement,dot,clr, backspace,enter starting 11 index
                            if (index == 11)
                            {
                                is_ready = true;
                                text = original_text;
                                value = original_value;
                                break;
                            }
                            if (index==12)
                            {
                                value *= -1;
                            }
                            else if (index == 13)
                            {
                                value++;
                            }
                            else if (index == 14)
                            {
                                value--;
                            }
                            if (text.Length < maxchars)
                            {
                                text = value.ToString();
                            }
                            else
                            {
                                text = text.Remove(text.Length - 1);
                            }
                            if (index == 15)
                            {
                                if (!text.Contains("."))
                                {
                                    text += ".";
                                }

                            }
                            else if (index == 16)
                            {
                                text = "";

                            }
                            else if (index == 17)
                            {
                                if (text.Length > 0)
                                {
                                    text = text.Remove(text.Length - 1);
                                }

                            }
                            else if (index == 18)
                            {
                                is_ready = true;
                            }
                            value = (float)Convert.ToDouble(text);
                            break;
                        }
                    }
                }
                index++;
            }
            return hit;
        }
        public void update_value(float value_in)
        {
            if (value_in > maxvalue)
                value_in = maxvalue;
            value = value_in;
            text = value.ToString();
        }
    }

    public class my_line_edit
    {
        public System.Drawing.Pen mypen;
        public System.Drawing.Pen borderpen=new System.Drawing.Pen(System.Drawing.Color.LightGreen);
        public int x;
        public int y;
        public int width;
        public int height;
        public string text;
        public bool isnum = true;
        public int maxchars = 6;
        public float maxvalue=9999999;
        public float minvalue = -999999;
        public float value;
        public my_line_edit()
        {

        }
        public my_line_edit(int x_in, int y_in, int width_in, int height_in, string text_in, int maxchars_in, float maxvalue_in, System.Drawing.Pen pen)        
        {
            x= x_in;
            y = y_in;
            width = width_in;
            height = height_in;
            text = text_in;
            maxchars = maxchars_in;
            maxvalue = maxvalue_in;
            mypen = new System.Drawing.Pen(pen.Color);
        }
        public my_line_edit(int x_in, int y_in, int maxchars, float value_in, System.Drawing.Pen pen, Bitmap bp, Font myfont)
        {
            x = x_in;
            y = y_in;
            mypen = new System.Drawing.Pen(pen.Color);
            value = value_in;
            text = value.ToString();
            string textsize = "8";
            using (Graphics g = Graphics.FromImage(bp))
            {
                SizeF char_size = g.MeasureString(textsize, myfont);
                width = (int)(char_size.Width * maxchars + char_size.Width);
                height = (int)(char_size.Height *2);
            }
        }
        public void update_value(float value_in)
        {
            if (value_in > maxvalue)
                value_in = maxvalue;
            if (value_in < minvalue)
                value_in = minvalue;
            value = value_in;
            text = value.ToString();
        }
    }
    public class intel_core_ui
    {
        public struct vfp_arr
        {
            public Point vfp_pos;
            public string vfp_str;
            public my_line_edit vfp_line_edit;
        }

        public Point core_ratio_text_pos;
        public string core_ratio_text = "PCore";
        public Point trl_text_pos;
        public string trl_text = "TRL";
        public Point vid_text_pos;
        public string vid_text = "VID";
        public Point offset_text_pos;
        public string offset_text = "Offset";
        public Point override_text_pos;
        public string override_text = "Override";
        public Point vfp_text_pos;
        public string vfp_text = "VF Pts";
        public List<vfp_arr> vfp_list;

        public Point pl1_text_pos;
        public string pl1_text = "PL1";
        public Point pl2_text_pos;
        public string pl2_text = "PL2";
        public Point iccmax_text_pos;
        public string iccmax_text = "ICCMax";
        public Point iccmaxdis_text_pos;
        public string iccmaxdis_text = "ICCMax Dis";

        public Point vrm_text_pos;
        public string vrm_text = "VRM";

        public Point apply_for_all_text_pos;
        public string apply_for_all_text = "For All PCores";

        public Point ring_text_pos;
        public string ring_text = "Ring";
        public Point ring_min_text_pos;
        public string ring_min_text = "Min";
        public Point ring_max_text_pos;
        public string ring_max_text = "Max";
        public Point ring_vid_text_pos;
        public string ring_vid_text = "VID";
        public Point ring_offset_text_pos;
        public string ring_offset_text = "Offset";
        public Point ring_manual_text_pos;
        public string ring_manual_text = "Override";

        public Point ring_downbin_text_pos;
        public string ring_downbin_text = "Downbin";

        public Point sp_text_pos;
        public string sp_text = "SP 0";


        public List<my_line_edit>line_edit_list;
        public int current_line_edit=0;
        public float current_line_value = 0;

        public List<my_checkbox> checkbox_list;
        public List<my_button> button_list;

        public int vrm_index = 0;
        public int trl_index = 0;
        public int trlact_index = 0;
        public int vid_index = 0;
        public int offset_index = 0;
        public int pl1_index = 0;
        public int pl2_index = 0;
        public int iccmax_index = 0;
        public bool line_edit_list_hit = false;
        public bool vfp_list_hit = false;

        public int flat_freq_index=0;
        public int apply_index = 0;

        public int ring_min_index = 0;
        public int ring_max_index = 0;
        public int ring_downbin_index = 0;
        public int ring_vid_index = 0;
        public int ring_offset_index = 0;
        public int ring_manual_index = 0;
        public unsafe intel_core_ui(OCWidgetInstance ocwidget)
        {
            int ind = 0;
            if (!ocwidget.intel_core_class.pcore)
            {
                core_ratio_text = "ECore";
                apply_for_all_text = "For All ECores";
            }                
            core_ratio_text += ocwidget.target_core.ToString();
            core_ratio_text += " Ratio "; 
            int startx = 5;
            int starty = 5;
            core_ratio_text_pos = new Point(startx, starty); 
            System.Drawing.Pen ppen2 = new System.Drawing.Pen(System.Drawing.Color.PaleVioletRed);
            line_edit_list = new List<my_line_edit>();
            checkbox_list = new List<my_checkbox>();
            my_line_edit core_ratio_lineedit;
            int fontwidth = 0;
            int fontheight = 0;
            int firstline_endx = 0;
            int secondline_endx = 0;
            int thirdline_endx = 0;
            using (Graphics g = Graphics.FromImage(ocwidget.BitmapCurrent))
            {
                SizeF str_size_ratio = g.MeasureString(core_ratio_text, ocwidget.DrawFontDate);
                fontwidth = (int)str_size_ratio.Width;
                fontheight = (int)str_size_ratio.Height;
                startx += fontwidth; 
               core_ratio_lineedit = new my_line_edit(startx + 4, starty, 6, (float)ocwidget.intel_core_class.ratio, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
               core_ratio_lineedit.maxvalue = 120;
                core_ratio_lineedit.minvalue = 4;
                line_edit_list.Add(core_ratio_lineedit);
                firstline_endx = core_ratio_lineedit.x + core_ratio_lineedit.width;
                if (ocwidget.intel_core_class.sp != 0)
                {
                    startx = firstline_endx + 10;
                    sp_text_pos= new Point(startx, starty);
                    sp_text = "SP " + ocwidget.intel_core_class.sp.ToString();
                }
               

                startx =5;
                starty += ((fontheight * 2) + 10);
                string test = "8";
                str_size_ratio = g.MeasureString(test, ocwidget.DrawFontDate);
                fontwidth = (int)str_size_ratio.Width;
                fontheight = (int)str_size_ratio.Height;

                trl_text_pos = new Point(startx, starty);
                startx += (fontwidth*3+4);
                int trl_startx = startx;
                trl_index = ind;
                for (int i=0;i<8;i++)
                {
                    my_line_edit trl_lineedit=new my_line_edit(startx, starty, 3, (float)ocwidget.intel_core_class.bycore_usage_inst.bycore_usage[i], ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);                    
                    ind++;
                    trl_lineedit.maxvalue = 255;
                    trl_lineedit.minvalue = 0;

                    startx = trl_lineedit.x + trl_lineedit.width;
                    line_edit_list.Add(trl_lineedit);
                    secondline_endx = trl_lineedit.x + trl_lineedit.width;
                }
                if (ocwidget.intel_core_class.isarl || ocwidget.intel_core_class.isspr || ocwidget.intel_core_class.isadl || ocwidget.intel_core_class.isrpl)
                {
                    startx = trl_startx;
                    starty += ((fontheight * 2));
                    trlact_index = ind;
                    for (int i = 0; i < 8; i++)
                    {
                        my_line_edit trlact_lineedit = new my_line_edit(startx, starty, 3, (float)ocwidget.intel_core_class.bycore_usage_inst.bycore_1ae[i], ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                        ind++;
                        trlact_lineedit.maxvalue = 255;
                        trlact_lineedit.minvalue = 0;
                        startx = trlact_lineedit.x + trlact_lineedit.width;
                        line_edit_list.Add(trlact_lineedit);
                        secondline_endx = trlact_lineedit.x + trlact_lineedit.width;
                    }
                }
                startx = 5;
                starty += ((fontheight * 2) + 10);
                vid_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                vid_index = ind;
                my_line_edit vid_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.intel_core_class.vid, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                vid_lineedit.maxvalue = 1720;
                vid_lineedit.minvalue = 0;

                startx = (vid_lineedit.x + vid_lineedit.width+ 10);
                line_edit_list.Add(vid_lineedit);
                offset_text_pos = new Point(startx, starty);
                str_size_ratio = g.MeasureString(offset_text, ocwidget.DrawFontDate);
                startx += ((int)str_size_ratio.Width + 10);
                offset_index = ind;
                my_line_edit offset_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.intel_core_class.offset, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                offset_lineedit.maxvalue = 1000;
                offset_lineedit.minvalue = -600;
                startx = (offset_lineedit.x + offset_lineedit.width + 10);
                line_edit_list.Add(offset_lineedit);
                override_text_pos = new Point(startx, starty);
                str_size_ratio = g.MeasureString(override_text, ocwidget.DrawFontDate);
                startx += ((int)str_size_ratio.Width + 10);
                bool isoverride = false;
                if(ocwidget.intel_core_class.vid_mode!=0)
                    isoverride = true; 
                my_checkbox overbox = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, isoverride);
                checkbox_list.Add(overbox);
                thirdline_endx = overbox.x + overbox.width;
                startx = 5;
                starty += ((fontheight * 2) + 20);
                vfp_list = new List<vfp_arr>();
                if (ocwidget.intel_core_class.my_vf_pts.num_pts>0)
                {
                    vfp_text_pos = new Point(startx, starty);
                    startx += (fontwidth * 5 + 4);                   
                    Point firstx= new Point(startx, starty);
                    //vfp_index = ind;
                    for (int i=0;i< ocwidget.intel_core_class.my_vf_pts.num_pts;i++)
                    {
                        Point ratio= new Point(startx, starty);
                        if (i == 0)
                            firstx.X = startx;  
                        vfp_arr arr = new vfp_arr();
                        arr.vfp_pos=ratio;
                        string str = ocwidget.intel_core_class.my_vf_pts.ratio[i].ToString();
                        arr.vfp_str = str+"x"; 
                        my_line_edit value=new my_line_edit(startx, starty+ (fontheight * 2), 4, (float)ocwidget.intel_core_class.my_vf_pts.volt[i], ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                        //vfp_last_index = ind;
                        //ind++;
                        value.maxvalue = 1000;
                        value.minvalue = -600;
                        arr.vfp_line_edit=(value);
                        startx = value.x + value.width;
                        vfp_list.Add(arr);
                        if(ocwidget.BitmapCurrent.Width<(startx + value.width+20))
                        {
                            startx = firstx.X;
                            starty += ((fontheight * 3) + 25);
                        }

                    }
                    starty += ((fontheight * 3) + 30);
                    startx = 5;
                }
                pl1_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                pl1_index = ind;
                my_line_edit pl1line = new my_line_edit(startx, starty, 6,(float)ocwidget.intel_core_class.pl1, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                pl1line.maxvalue = 0x7fff;
                pl1line.minvalue = 0;
                line_edit_list.Add(pl1line);
                startx = (pl1line.x + pl1line.width + 10);

                pl2_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                pl2_index = ind;
                my_line_edit pl2line = new my_line_edit(startx, starty, 6, (float)ocwidget.intel_core_class.pl2, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                pl2line.maxvalue = 0x7fff;
                pl2line.minvalue = 0;
                line_edit_list.Add(pl2line);
                startx = (pl2line.x + pl2line.width + 10);

                iccmax_text_pos = new Point(startx, starty);
                startx += (fontwidth * 5 + 4);
                iccmax_index = ind;
                my_line_edit iccmaxline = new my_line_edit(startx, starty, 4, (float)ocwidget.intel_core_class.iccmax, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                iccmaxline.maxvalue = 0x3ff;
                iccmaxline.minvalue = 0;
                line_edit_list.Add(iccmaxline);
                startx = (iccmaxline.x + iccmaxline.width + 10);

                iccmaxdis_text_pos = new Point(startx, starty);
                startx += (fontwidth * 7 + 4);
                bool isdis = false;
                if (ocwidget.intel_core_class.iccmax_unlimit != 0)
                    isdis = true;
                my_checkbox idisbox = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, isdis);
                checkbox_list.Add(idisbox);
                startx = 5;
                starty += ((fontheight * 3) + 10);
                button_list = new List<my_button>();
                int but_index = 0;
                if (ocwidget.vrmtype>0)
                {
                    vrm_text_pos = new Point(startx, starty);
                    startx += (fontwidth * 3 + 4);
                    vrm_index = ind;
                    my_line_edit vrmline = new my_line_edit(startx, starty, 4, (float)ocwidget.intel_core_class.vcore_rail, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                    ind++;
                    if (ocwidget.intel_core_class.isspr)
                        vrmline.maxvalue = 2700;
                    else
                        vrmline.maxvalue = 1900;
                    vrmline.minvalue = 0;
                    line_edit_list.Add(vrmline);
                    startx = (vrmline.x + vrmline.width + 10);
                    string applyv = "Apply VRM";
                    my_button vrmbut = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, applyv);
                    button_list.Add(vrmbut);
                    startx = (vrmbut.x + vrmbut.width + 10);
                    but_index++;
                }
                apply_for_all_text_pos = new Point(startx, starty);
                str_size_ratio = g.MeasureString(apply_for_all_text, ocwidget.DrawFontDate);
                startx += ((int)str_size_ratio.Width + 4);
                my_checkbox apply_for_all_chkbox = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, false);
                checkbox_list.Add(apply_for_all_chkbox);
                startx = apply_for_all_chkbox.x + apply_for_all_chkbox.width + 30;
                my_button flat_but = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, "Flat Freq");
                flat_freq_index = but_index;
                button_list.Add(flat_but);
                but_index++;
                startx = 5;
                starty+= ((fontheight * 3) + 10);

                //ring
                ring_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                ring_min_text_pos = new Point(startx, starty);
                int min_x = startx;
                starty += ((fontheight * 2) + 10);
                ring_min_index = ind;
                my_line_edit minring = new my_line_edit(startx, starty, 3, (float)ocwidget.intel_core_class.min_ring, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                line_edit_list.Add(minring);
                ind++;
                startx = (minring.x+minring.width);
                starty -= ((fontheight * 2) + 10);
                ring_max_text_pos = new Point(startx, starty);
                starty += ((fontheight * 2) + 10);
                ring_max_index = ind;
                int actual = ocwidget.intel_core_class.max_ring;
                if (ocwidget.intel_core_class.ring_ocmb_ratio< ocwidget.intel_core_class.max_ring && ocwidget.intel_core_class.ring_ocmb_ratio != 0)
                {
                    actual = ocwidget.intel_core_class.ring_ocmb_ratio;
                }
                my_line_edit maxring = new my_line_edit(startx, starty, 3, (float)actual, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                line_edit_list.Add(maxring);
                ind++;
                starty -= ((fontheight * 2) + 10);
                startx = (maxring.x + maxring.width+4);
                ring_vid_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 2);
                ring_vid_index = ind;
                my_line_edit ringvid = new my_line_edit(startx, starty, 3, (float)ocwidget.intel_core_class.ring_vid, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                line_edit_list.Add(ringvid);
                ind++;
                startx = (ringvid.x + ringvid.width + 4);

                ring_offset_text_pos = new Point(startx, starty);
                startx += (fontwidth *5 + 2);
                ring_offset_index = ind;
                my_line_edit ringoffset = new my_line_edit(startx, starty, 3, (float)ocwidget.intel_core_class.ring_offset, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                line_edit_list.Add(ringoffset);
                ind++;
                startx = (ringoffset.x + ringoffset.width + 4);

                ring_manual_text_pos = new Point(startx, starty);
                startx += (fontwidth * 6 + 4);
                bool ovvr = ocwidget.intel_core_class.ring_manual == 1 ? true : false;
                my_checkbox ring_override = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, ovvr);
                checkbox_list.Add(ring_override);
                startx = ring_override.x + ring_override.width + 3;


                ring_downbin_text_pos = new Point(startx, starty);
                startx += (fontwidth * 5 + 4);
                bool rd = ocwidget.intel_core_class.ringdownbin_dis==1 ? false: true;

                my_checkbox ring_down = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate,rd);
                checkbox_list.Add(ring_down);

                //buts
                startx = ocwidget.BitmapCurrent.Width - (fontwidth * 5 + 15);
                starty = ocwidget.BitmapCurrent.Height - (fontheight * 2+15);
                my_button apply_but = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, "Apply");
                apply_index = but_index;
                button_list.Add(apply_but);
                but_index++;
                startx = ocwidget.BitmapCurrent.Width - (fontwidth * 4 + 15);
                starty = 5;
                my_button back_but = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, "Back");
                button_list.Add(back_but);
            }


        }
        public string get_core_ratio_text
        {
            get => core_ratio_text;
            set
            {
                core_ratio_text = value; 
            }
        }
        public Point get_core_ratio_text_pos
        {
            get => core_ratio_text_pos;
            set
            {
                get_core_ratio_text_pos = value;
            }
        }
        public bool hit_line_edit(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (my_line_edit rect in line_edit_list)
            {
                if (x_in >= rect.x && x_in <= (rect.x + rect.width) && y_in >= rect.y && y_in <= (rect.y + rect.height))
                {
                    hit = true;
                    current_line_edit = index;
                    current_line_value = rect.value;
                    line_edit_list_hit = true;
                    vfp_list_hit = false;
                    break;
                }
                index++;
            }
            return hit;
        }
        public bool hit_vfp_edit(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (vfp_arr rect in vfp_list)
            {
                if (x_in >= rect.vfp_line_edit.x && x_in <= (rect.vfp_line_edit.x + rect.vfp_line_edit.width) && y_in >= rect.vfp_line_edit.y && y_in <= (rect.vfp_line_edit.y + rect.vfp_line_edit.height))
                {
                    hit = true;
                    current_line_edit = index;
                    current_line_value = rect.vfp_line_edit.value;
                    line_edit_list_hit = false;
                    vfp_list_hit = true;
                    break;
                }
                index++;
            }
            return hit;
        }

        public bool hit_checkbox(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (my_checkbox rect in checkbox_list)
            {
                if (x_in >= rect.x && x_in <= (rect.x + rect.width) && y_in >= rect.y && y_in <= (rect.y + rect.height))
                {
                    hit = true;
                    rect.toggle_checkbox();
                    break;
                }
                index++;
            }
            return hit;
        }
        public bool hit_button(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (my_button rect in button_list)
            {
                if (x_in >= rect.x && x_in <= (rect.x + rect.width) && y_in >= rect.y && y_in <= (rect.y + rect.height))
                {
                    hit = true;
                    current_line_edit = index;
                    break;
                }
                index++;
            }
            return hit;
        }

        public void update_line_edit(float value)
        {
            if(line_edit_list_hit)
                line_edit_list[current_line_edit].update_value(value);
            else if(vfp_list_hit)
                vfp_list[current_line_edit].vfp_line_edit.update_value(value);
            line_edit_list_hit = false;
            vfp_list_hit = false;
        }
    }
    public class amd_core_ui
    { 
        public Point core_ratio_text_pos;
        public string core_ratio_text = "Core"; 
        public Point vid_text_pos;
        public string vid_text = "VID";
        public Point sp_text_pos;
        public string sp_text = "SP 0";

        public List<my_checkbox> checkbox_list;
        public List<my_button> button_list;

        public Point ppt_text_pos;
        public string ppt_text = "PPT";
        public Point tdc_text_pos;
        public string tdc_text = "TDC";
        public Point edc_text_pos;
        public string edc_text = "EDC";
        public Point thm_text_pos;
        public string thm_text = "THM";

        public Point fmax_text_pos;
        public string fmax_text = "FMax";
        public Point fit_text_pos;
        public string fit_text = "FIT";
        public Point co_text_pos;
        public string co_text = "CO";

        public Point vrm_text_pos;
        public string vrm_text = "VRM";

        public Point apply_for_all_text_pos;
        public string apply_for_all_text = "For All Cores";

        public Point  ocmode_text_pos;
        public string ocmode_text = "OC Mode";

        public List<my_line_edit> line_edit_list;
        public int current_line_edit = 0;
        public float current_line_value = 0;

        public int apply_index = 0;

        public int vrm_index = 0;
        public int freq_index = 0;
        public int ppt_index = 0;
        public int vid_index = 0;
        public int tdc_index = 0;
        public int edc_index = 0;
        public int thm_index = 0;
        public int fit_index = 0;
        public int fmax_index = 0;
        public int co_index = 0;

        public bool line_edit_list_hit = false;

        public unsafe amd_core_ui(OCWidgetInstance ocwidget)
        {
            int ind = 0;
            core_ratio_text += ocwidget.target_core.ToString();
            core_ratio_text += " Freq ";
            int startx = 5;
            int starty = 5;
            core_ratio_text_pos = new Point(startx, starty);
            System.Drawing.Pen ppen2 = new System.Drawing.Pen(System.Drawing.Color.PaleVioletRed);
            line_edit_list = new List<my_line_edit>();
            checkbox_list = new List<my_checkbox>();
            my_line_edit core_ratio_lineedit;
            int fontwidth = 0;
            int fontheight = 0;

            using (Graphics g = Graphics.FromImage(ocwidget.BitmapCurrent))
            {
                SizeF str_size_ratio = g.MeasureString(core_ratio_text, ocwidget.DrawFontDate);
                fontwidth = (int)str_size_ratio.Width;
                fontheight = (int)str_size_ratio.Height;
                startx += fontwidth;
                core_ratio_lineedit = new my_line_edit(startx + 4, starty, 6, (float)ocwidget.amd_core_class.freq, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                
                core_ratio_lineedit.maxvalue = 9999;
                core_ratio_lineedit.minvalue = 400;
                line_edit_list.Add(core_ratio_lineedit);
                freq_index = ind;
                ind++;
                int firstline_endx = core_ratio_lineedit.x + core_ratio_lineedit.width;
                if (ocwidget.amd_core_class.sp != 0)
                {
                    startx = firstline_endx + 10;
                    sp_text_pos = new Point(startx, starty);
                    sp_text = "SP " + ocwidget.amd_core_class.sp.ToString();
                }
                startx = 5;
                starty += ((fontheight * 2) + 10);
                string test = "8";
                str_size_ratio = g.MeasureString(test, ocwidget.DrawFontDate);
                fontwidth = (int)str_size_ratio.Width;
                fontheight = (int)str_size_ratio.Height;

                vid_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                int vid_startx = startx;
                vid_index = ind;
                my_line_edit vid_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.amd_core_class.vid, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                vid_lineedit.maxvalue = 1520;
                vid_lineedit.minvalue = 0;
                line_edit_list.Add(vid_lineedit);
                startx = vid_lineedit.x + vid_lineedit.width + 4;
                ocmode_text_pos = new Point(startx, starty);
                startx += (fontwidth * 5 + 2);

                my_checkbox overbox = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, ocwidget.amd_core_class.isocmode);
                checkbox_list.Add(overbox);
                startx = 5;
                starty += ((fontheight * 2) + 20);

                ppt_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                ppt_index = ind;
                my_line_edit ppt_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.amd_core_class.ppt, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                ppt_lineedit.maxvalue = 9999;
                ppt_lineedit.minvalue = 0;
                line_edit_list.Add(ppt_lineedit);
                startx = ppt_lineedit.x + ppt_lineedit.width + 4;

                tdc_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                tdc_index = ind;
                my_line_edit tdc_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.amd_core_class.tdc, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                tdc_lineedit.maxvalue = 9999;
                tdc_lineedit.minvalue = 0;
                line_edit_list.Add(tdc_lineedit);
                startx = tdc_lineedit.x + tdc_lineedit.width + 4;

                edc_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                edc_index = ind;
                my_line_edit edc_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.amd_core_class.edc, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                edc_lineedit.maxvalue = 9999;
                edc_lineedit.minvalue = 0;
                line_edit_list.Add(edc_lineedit);
                startx = edc_lineedit.x + edc_lineedit.width + 4;

                thm_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                thm_index = ind;
                my_line_edit thm_lineedit = new my_line_edit(startx, starty, 3, (float)ocwidget.amd_core_class.thm, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                thm_lineedit.maxvalue = 200;
                thm_lineedit.minvalue = 0;
                line_edit_list.Add(thm_lineedit);
                startx = 5;
                starty += ((fontheight * 2) + 20);

                fmax_text_pos = new Point(startx, starty);
                startx += (fontwidth * 4 + 4);
                fmax_index = ind;
                my_line_edit fmax_lineedit = new my_line_edit(startx, starty, 4, (float)ocwidget.amd_core_class.fmax, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                fmax_lineedit.maxvalue = 9999;
                fmax_lineedit.minvalue = 0;
                line_edit_list.Add(fmax_lineedit);
                startx = fmax_lineedit.x + fmax_lineedit.width + 4;

                fit_text_pos = new Point(startx, starty);
                startx += (fontwidth * 3 + 4);
                fit_index = ind;
                my_line_edit fit_lineedit = new my_line_edit(startx, starty, 6, (float)ocwidget.amd_core_class.fit, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                fit_lineedit.maxvalue = 999999;
                fit_lineedit.minvalue = 0;
                line_edit_list.Add(fit_lineedit);
                startx = fit_lineedit.x + fit_lineedit.width + 4;

                co_text_pos = new Point(startx, starty);
                startx += (fontwidth * 2 + 4);
                co_index = ind;
                my_line_edit co_lineedit = new my_line_edit(startx, starty, 5, (float)ocwidget.amd_core_class.co, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                ind++;
                co_lineedit.maxvalue = 100;
                co_lineedit.minvalue = -100;
                line_edit_list.Add(co_lineedit);
                //startx = co_lineedit.x + co_lineedit.width + 4;

                startx = 5;
                starty += ((fontheight * 2) + 20);

                button_list = new List<my_button>();
                int but_index = 0;
                if (ocwidget.vrmtype > 0)
                {
                    vrm_text_pos = new Point(startx, starty);
                    startx += (fontwidth * 3 + 4);
                    vrm_index = ind;
                    my_line_edit vrmline = new my_line_edit(startx, starty, 4, (float)ocwidget.amd_core_class.vcore_rail, ppen2, ocwidget.BitmapCurrent, ocwidget.DrawFontDate);
                    ind++;
                    vrmline.maxvalue = 1800;
                    vrmline.minvalue = 0;
                    line_edit_list.Add(vrmline);
                    startx = (vrmline.x + vrmline.width + 10);
                    string applyv = "Apply VRM";
                    my_button vrmbut = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, applyv);
                    button_list.Add(vrmbut);
                    startx = (vrmbut.x + vrmbut.width + 10);
                    but_index++;
                }
                startx = 5;
                starty += ((fontheight * 2) + 20);

                apply_for_all_text_pos = new Point(startx, starty);
                str_size_ratio = g.MeasureString(apply_for_all_text, ocwidget.DrawFontDate);
                startx += ((int)str_size_ratio.Width + 4);
                my_checkbox apply_for_all_chkbox = new my_checkbox(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, false);
                checkbox_list.Add(apply_for_all_chkbox);

                startx = ocwidget.BitmapCurrent.Width - (fontwidth * 5 + 15);
                starty = ocwidget.BitmapCurrent.Height - (fontheight * 2 + 15);
                my_button apply_but = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, "Apply");


                apply_index = but_index;
                button_list.Add(apply_but);
                but_index++;
                startx = ocwidget.BitmapCurrent.Width - (fontwidth * 4 + 15);
                starty = 5;
                my_button back_but = new my_button(startx, starty, ocwidget.BitmapCurrent, ocwidget.DrawFontDate, "Back");
                button_list.Add(back_but);
            }

        } 
        public bool hit_line_edit(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (my_line_edit rect in line_edit_list)
            {
                if (x_in >= rect.x && x_in <= (rect.x + rect.width) && y_in >= rect.y && y_in <= (rect.y + rect.height))
                {
                    hit = true;
                    current_line_edit = index;
                    current_line_value = rect.value;
                    line_edit_list_hit = true; 
                    break;
                }
                index++;
            }
            return hit;
        } 
        public bool hit_checkbox(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (my_checkbox rect in checkbox_list)
            {
                if (x_in >= rect.x && x_in <= (rect.x + rect.width) && y_in >= rect.y && y_in <= (rect.y + rect.height))
                {
                    hit = true;
                    rect.toggle_checkbox();
                    break;
                }
                index++;
            }
            return hit;
        }

        public bool hit_button(int x_in, int y_in)
        {
            bool hit = false;
            int index = 0;
            foreach (my_button rect in button_list)
            {
                if (x_in >= rect.x && x_in <= (rect.x + rect.width) && y_in >= rect.y && y_in <= (rect.y + rect.height))
                {
                    hit = true;
                    current_line_edit = index;
                    break;
                }
                index++;
            }
            return hit;
        }

        public void update_line_edit(float value)
        {
            if (line_edit_list_hit)
                line_edit_list[current_line_edit].update_value(value); 
            line_edit_list_hit = false; 
        }
    }


    public class AMD_CORE_CLASS
    {
        public int vid = 1250;
        public byte vid_mode = 0;//adaptive
        public int vcore_rail = 1250;
        public float freq = 4000;
        public int family = 0;

        public bool isphx = false;
        public bool isrpl = false;
        public bool isgnr = false;
        public bool isrembrand = false;
        public bool istr5 = false;
        public bool iscezanne = false;
        public bool isrenoir = false;
        public bool ispinnacle = false;
        public bool isvermeer = false;

        public int sp = 0;
        public bool isocmode = false;

        public int ppt = 0;
        public int tdc = 0;
        public int thm = 0;
        public int edc = 0;
        public int fmax = 0;
        public int fit = 0;
        public int co = 0; 

        public unsafe AMD_CORE_CLASS(OCWidgetInstance ocwidget)
        {
            OCWidgetInstance.initialize_amd();
            amd_readme_struct rm=new amd_readme_struct();
            OCWidgetInstance.read_amd(ref rm, ocwidget.target_core);
            sp = OCWidgetInstance.get_amd_sp(ocwidget.target_core);
            isocmode = rm.co[1]==1 ?  true: false;
            family = rm.family;
            vid = rm.vid;
            freq = (float)rm.freq;
            if (ocwidget.vrmtype == -99)
            {
                ocwidget.vrmtype = OCWidgetInstance.checkvrm();
                if (ocwidget.vrmtype != 0)
                {
                    OCWidgetInstance.initialize_vrm_parent();
                }
            }
            if (ocwidget.vrmtype != 0)
            {
                vcore_rail = OCWidgetInstance.vrm_rd_vcore();
            }
            isphx = rm.type[0] == 1 ? true : false;
            isrpl = rm.type[1] == 1 ? true : false;
            isgnr = rm.type[2] == 1 ? true : false;
            isrembrand = rm.type[3] == 1 ? true : false;
            istr5 = rm.type[4] == 1 ? true : false;
            iscezanne = rm.type[5] == 1 ? true : false;
            isrenoir = rm.type[6] == 1 ? true : false;
            ispinnacle = rm.type[7] == 1 ? true : false;
            isvermeer = rm.type[8] == 1 ? true : false;

            ppt = rm.pb[0];
            tdc = rm.pb[1];
            thm = rm.pb[2];
            edc = rm.pb[3];
            fmax = rm.pb[4];
            fit = rm.pb[5];
            co = rm.co[0];

        }
    }

    public class INTEL_CORE_CLASS
    {
        public int vid=1250;
        public byte vid_mode = 0;//adaptive
        public int vcore_rail = 1250;
        public double ratio = 40;
        public intel_bycore_usage_vec bycore_usage_inst; 
        public bool synch_allcore = false;
        public bool pcore = true;
        public bool isarl = true;
        public bool isspr = false;
        public bool isadl = false;
        public bool isrpl = false;
        public intel_vf_pt_struct my_vf_pts;
        public int offset=0; 
        public int pl1=0;
        public int pl2 = 0;
        public int iccmax = 0;
        public int iccmax_unlimit = 0;
        public int min_ring = 0;
        public int max_ring = 0;
        public int ring_vid = 0;
        public int ring_offset = 0;
        public int ring_manual = 0;
        public int ring_ocmb_ratio = 0;
        public int ringdownbin_dis;
        public int sp = 0;
        public unsafe INTEL_CORE_CLASS(OCWidgetInstance ocwidget)
        {
            if(ocwidget.mecores.num_smallcores>0 && ocwidget.mecores.Efficiency_Class[ocwidget.target_core]==0)
            {
                pcore = false;
            }
            isarl = OCWidgetInstance.isit_arl();
            isspr = OCWidgetInstance.isit_spr();
            isadl = OCWidgetInstance.isit_adl();
            isrpl = OCWidgetInstance.isit_rpl();            
            bycore_usage_inst = new intel_bycore_usage_vec();            
            if (pcore)
            {
                fixed (int* ptr = bycore_usage_inst.bycore_usage)
                {
                    OCWidgetInstance.Rd1ADs(ptr);
                }
                if(isarl||isspr||isadl||isrpl)
                {
                    fixed (int* ptr = bycore_usage_inst.bycore_1ae)
                    {
                        OCWidgetInstance.Rd1AEs(ptr);
                    }                    
                }
            }
            else
            {
                fixed (int* ptr = bycore_usage_inst.bycore_usage)
                {
                    OCWidgetInstance.Rd650s(ptr);
                }
                fixed (int* ptr = bycore_usage_inst.bycore_1ae)
                {
                    OCWidgetInstance.Rd651s(ptr);
                }
            }
            int overrid = 0;  
            vid = OCWidgetInstance.Rd_PC_Voltage_whole_masked((ulong)ocwidget.mecores.Processor_Mask[ocwidget.target_core], ocwidget.mecores.get_Group[ocwidget.target_core], ref overrid, ref offset);
            
            if (overrid==1)
                vid_mode = 1;
            if (ocwidget.vrmtype == -99)
            {
                ocwidget.vrmtype = OCWidgetInstance.checkvrm();
                if (ocwidget.vrmtype != 0)
                {
                    OCWidgetInstance.initialize_vrm_parent();
                }
            }
            if(ocwidget.vrmtype != 0)
            {
                vcore_rail = OCWidgetInstance.vrm_rd_vcore();
            }            
            ratio= OCWidgetInstance.Rd_PC_Ratio_masked_ext((ulong)ocwidget.mecores.Processor_Mask[ocwidget.target_core], ocwidget.mecores.get_Group[ocwidget.target_core]);
            my_vf_pts = new intel_vf_pt_struct();
            OCWidgetInstance.get_vfpts(ocwidget.target_core, ref my_vf_pts); 
            OCWidgetInstance.get_pl_iccmax(ref pl1, ref pl2, ref iccmax,ref iccmax_unlimit); 
            OCWidgetInstance.Rd_Ring(ref min_ring, ref max_ring); 
            OCWidgetInstance.rd_ring_ocmb(ref ring_ocmb_ratio, ref ring_vid, ref ring_offset, ref ring_manual, ref ringdownbin_dis);
            sp=OCWidgetInstance.get_intel_sp(ocwidget.target_core);
        }
    }
    public partial class OCWidgetInstance : IWidgetInstance
    {
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool initialize_libs();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void get_my_processor(ref My_Processor_Core p);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort get_cpu_vendor_id();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Rd_intel_bclk();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]        
        public static extern unsafe void get_core_usage(byte* p);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool check_if_amd();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isit_arl();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isit_spr();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isit_adl();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool isit_rpl();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Rd1ADs(int* p);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Rd1AEs(int* p);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Rd650s(int* p);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Rd651s(int* p);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Rd_PC_Voltage_whole_masked(ulong threadAffinityMask, int group, ref int overrid, ref int offset);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int checkvrm();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vrm_rd_vcore();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool initialize_vrm_parent();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool initialize_vrm_parent_id(int id);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double Rd_PC_Ratio_masked_ext(ulong threadAffinityMask, int group);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong Rd1AD_ext();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void get_vfpts(int core_index, ref intel_vf_pt_struct vf_struct);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void get_pl_iccmax(ref int pl1, ref int pl2, ref int iccmax, ref int iccmax_unlimit);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void vrm_wr_vcore(int voltage);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void write_intel_settings(int targetcore, ref intel_settings_struct settings);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void Rd_Ring(ref int min, ref int max);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void rd_ring_ocmb(ref int ratio, ref int volt, ref int offset, ref int manual,ref int ringdownbin_dis);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_intel_sp(int target_core);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void initialize_amd();
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void read_amd(ref amd_readme_struct rm, int target_core);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_amd_sp(int target_core);
        [DllImport("dllforcsharp.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void write_amd_settings(int target_core, ref amd_settings_struct settings);



        public volatile bool is_paused = false;
        public int x_pos = 0;
        public int y_pos = 0;
        bool isamd = false;
        public int target_core = 0;
        public int vrmtype = -99;
        public user_input_numerical input_keypad;
        public void RequestUpdate()
        {
            /*if (drawing_mutex.WaitOne(1000))
            {
                DrawCoreUse();
                drawing_mutex.ReleaseMutex();
            }*/
        }
        public void print_debug(float value)
        {
            pause_task = true;
            while (!is_paused) ;
            using (Graphics g = Graphics.FromImage(BitmapCurrent))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;   
                Brush textBrush = new SolidBrush(DrawForeColor);
                string date = value.ToString(); 
                SizeF str_size_date = g.MeasureString(date, DrawFontDate);
                g.DrawString(date, DrawFontDate, textBrush, 50, 350);
            }
            UpdateWidget();
        }
        public void print_debug(string value)
        {
            pause_task = true;
            while (!is_paused) ;
            using (Graphics g = Graphics.FromImage(BitmapCurrent))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                Brush textBrush = new SolidBrush(DrawForeColor); 
                SizeF str_size_date = g.MeasureString(value, DrawFontDate);
                g.DrawString(value, DrawFontDate, textBrush, 50, 350);
            }
            UpdateWidget();
            
        }

        public unsafe void apply_amd_settings()
        {
            amd_settings_struct settings = new amd_settings_struct();
            settings.freq = (int)amd_core_ui_instance.line_edit_list[0].value;
            settings.vid = (int)amd_core_ui_instance.line_edit_list[1].value;
            settings.ppt = (int)amd_core_ui_instance.line_edit_list[2].value;
            settings.tdc = (int)amd_core_ui_instance.line_edit_list[3].value;
            settings.edc = (int)amd_core_ui_instance.line_edit_list[4].value;
            settings.thm = (int)amd_core_ui_instance.line_edit_list[5].value;
            settings.fmax = (int)amd_core_ui_instance.line_edit_list[6].value;
            settings.fit = (int)amd_core_ui_instance.line_edit_list[7].value;
            settings.co = (int)amd_core_ui_instance.line_edit_list[8].value;
            settings.ocmode = amd_core_ui_instance.checkbox_list[0].is_check ? 1 : 0;
            settings.forall = amd_core_ui_instance.checkbox_list[1].is_check ? 1 : 0;

            write_amd_settings(target_core, ref settings);
        }

        public unsafe void apply_intel_settings()
        {            
            intel_settings_struct settings = new intel_settings_struct();
            settings.per_core_ratio = intel_core_ui_instance.line_edit_list[0].value;
            int index = intel_core_ui_instance.trl_index;
            int i = 0;
            while(index< (intel_core_ui_instance.trl_index+8))
            {
                settings.bycore_usage[i]= (int)intel_core_ui_instance.line_edit_list[index].value;
                index++;
                i++;
            }
            //patch user errors
            for(int j=7;j>0;j--)
            {
                if (settings.bycore_usage[j] > settings.bycore_usage[j - 1])
                    settings.bycore_usage[j] = settings.bycore_usage[j - 1];
            }
            if(intel_core_ui_instance.trlact_index!=0)
            {
                index = intel_core_ui_instance.trlact_index;
                i = 0;
                while (index < (intel_core_ui_instance.trlact_index + 8))
                {
                    settings.bycore_1ae[i] = (int)intel_core_ui_instance.line_edit_list[index].value;
                    index++;
                    i++;
                }
                for (int j = 7; j > 0; j--)
                {
                    if (settings.bycore_1ae[j] < settings.bycore_1ae[j - 1] && settings.bycore_1ae[j]!=0)
                        settings.bycore_1ae[j] = settings.bycore_1ae[j - 1];
                }
                for (int j = 0; j <8; j++)
                {
                    if (settings.bycore_1ae[j] == 0)
                        settings.bycore_usage[j] = 0;
                }
            }
            else
            {
                settings.bycore_1ae[0] = -1;
            }            
            settings.vid= (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.vid_index].value;
            settings.offset = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.offset_index].value;
            settings.manual = intel_core_ui_instance.checkbox_list[0].is_check ? 1: 0 ;
            if(intel_core_ui_instance.vfp_list.Count!=0)
            {
                i = 0;
                foreach (intel_core_ui.vfp_arr line in intel_core_ui_instance.vfp_list)
                {
                    settings.vf_pts[i] = (int)line.vfp_line_edit.value;
                    i++;
                }
            }
            settings.pl1 = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.pl1_index].value;
            settings.pl2 = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.pl2_index].value;
            settings.iccmax = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.iccmax_index].value;
            settings.iccmax_dis = intel_core_ui_instance.checkbox_list[1].is_check ? 1 : 0;
            settings.for_all = intel_core_ui_instance.checkbox_list[2].is_check ? 1 : 0;

            settings.min_ring = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.ring_min_index].value;
            settings.max_ring = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.ring_max_index].value;
            if (settings.max_ring < settings.min_ring)
                settings.max_ring = settings.min_ring;
            settings.ring_vid = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.ring_vid_index].value;
            settings.ring_offset = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.ring_offset_index].value;
            settings.ring_manual = intel_core_ui_instance.checkbox_list[3].is_check ? 1 : 0;
            settings.ringdownbin_dis = intel_core_ui_instance.checkbox_list[4].is_check ? 0 : 1;
            settings.ring_ocmb_ratio = 0;
            if(settings.max_ring> intel_core_class.max_ring || settings.max_ring== settings.min_ring || settings.ringdownbin_dis==1 || settings.ring_vid!=0)
            {
                settings.ring_ocmb_ratio = settings.max_ring;
            }
            write_intel_settings(target_core, ref settings);            
        }
        public unsafe void ClickEvent(ClickType click_type, int x, int y)
        {
            x_pos = x;
            y_pos = y;
            if (click_type == ClickType.SwipeLeft)
            {
                pause_task = true;
                while (!is_paused) ;               
                if (target_core == (mecores.Efficiency_Class_size - 1))
                {
                    target_core = 0;

                }
                else
                    target_core++;
                if (current_page == PAGE_STATE.intel_core_page)
                {
                    intel_core_page_changed = true;
                    intel_core_class = new INTEL_CORE_CLASS(this);
                    intel_core_ui_instance = new intel_core_ui(this);
                }
                else if (current_page == PAGE_STATE.amd_core_page)
                {
                    amd_core_page_changed = true;
                    amd_core_class = new AMD_CORE_CLASS(this);
                    amd_core_ui_instance = new amd_core_ui(this);
                }
                pause_task = false;
            }
            else if (click_type == ClickType.SwipeRight)
            {
                pause_task = true;
                while (!is_paused) ;
                if (target_core == 0)
                {
                    target_core = mecores.Efficiency_Class_size - 1;

                }
                else
                    target_core--;
                if (current_page == PAGE_STATE.intel_core_page)
                {
                    intel_core_page_changed = true;
                    intel_core_class = new INTEL_CORE_CLASS(this);
                    intel_core_ui_instance = new intel_core_ui(this);
                }
                else if (current_page == PAGE_STATE.amd_core_page)
                {
                    amd_core_page_changed = true;
                    amd_core_class = new AMD_CORE_CLASS(this);
                    amd_core_ui_instance = new amd_core_ui(this);
                }
                pause_task = false;
            }
            else if (click_type== ClickType.Single || click_type == ClickType.Double)
            {
                bool found = false;
                if (current_page == PAGE_STATE.main_page)
                {
                    for (int i = 0; i < num_core_grids; i++)
                    {
                        if (y <= grid_struct_vec[i].end_y && y >= grid_struct_vec[i].start_y && x >= grid_struct_vec[i].start_x && x <= grid_struct_vec[i].end_x)
                        {
                            target_core = i;
                            found = true;
                            break;
                        }

                    }
                }
                else if (current_page == PAGE_STATE.intel_core_page)
                {
                    if (intel_core_ui_instance.hit_line_edit(x, y))
                    {
                        input_keypad = new user_input_numerical(BitmapCurrent, DrawFontDate, intel_core_ui_instance.current_line_value);
                        current_page = PAGE_STATE.numerical_input;
                        previous_page = PAGE_STATE.intel_core_page;
                    }
                    else if(intel_core_ui_instance.hit_checkbox(x, y))
                    {
                        intel_core_page_changed = true;
                    }
                    else if (intel_core_ui_instance.hit_vfp_edit(x, y))
                    {
                        input_keypad = new user_input_numerical(BitmapCurrent, DrawFontDate, intel_core_ui_instance.current_line_value);
                        current_page = PAGE_STATE.numerical_input;
                        previous_page = PAGE_STATE.intel_core_page;
                    }
                    else if (intel_core_ui_instance.hit_button(x, y))
                    {
                        pause_task = true;
                        while (!is_paused) ;
                        intel_core_page_changed = true;
                        bool refresh_page = false;
                        if(intel_core_ui_instance.current_line_edit==0 && vrmtype>0)//apply vrm
                        {
                            int volt = (int)intel_core_ui_instance.line_edit_list[intel_core_ui_instance.vrm_index].value;
                            vrm_wr_vcore(volt);                           
                            refresh_page = true;
                        }
                        else if (intel_core_ui_instance.current_line_edit == intel_core_ui_instance.flat_freq_index)//flat_freq
                        {
                            string txt = intel_core_ui_instance.line_edit_list[intel_core_ui_instance.trl_index].text;
                            int i = intel_core_ui_instance.trl_index + 1;
                            while(i<(intel_core_ui_instance.trl_index+8))
                            {
                                if (intel_core_ui_instance.line_edit_list[i].value != 0)
                                {
                                    intel_core_ui_instance.line_edit_list[i].text = txt;
                                    intel_core_ui_instance.line_edit_list[i].value = (float)Convert.ToDouble(txt);
                                }
                                i++;
                            }
                        }
                        else if(intel_core_ui_instance.current_line_edit == intel_core_ui_instance.apply_index)//apply
                        {
                            apply_intel_settings();
                            refresh_page = true;
                        }
                        else
                        {
                            //back
                            current_page = PAGE_STATE.main_page;
                            previous_page = PAGE_STATE.intel_core_page;
                        }
                        if(refresh_page)
                        {
                            intel_core_class = new INTEL_CORE_CLASS(this);
                            intel_core_ui_instance = new intel_core_ui(this);
                        }
                        pause_task = false;
                    }
                }
                else if (current_page == PAGE_STATE.amd_core_page)
                {
                    if (amd_core_ui_instance.hit_line_edit(x, y))
                    {
                        input_keypad = new user_input_numerical(BitmapCurrent, DrawFontDate, amd_core_ui_instance.current_line_value);
                        current_page = PAGE_STATE.numerical_input;
                        previous_page = PAGE_STATE.amd_core_page;
                    }
                    else if (amd_core_ui_instance.hit_checkbox(x, y))
                    {
                        amd_core_page_changed = true;
                    }
                    else if (amd_core_ui_instance.hit_button(x, y))
                    {
                        pause_task = true;
                        while (!is_paused) ;
                        amd_core_page_changed = true;
                        bool refresh_page = false;
                        if (amd_core_ui_instance.current_line_edit == 0 && vrmtype > 0)//apply vrm
                        {
                            int volt = (int)amd_core_ui_instance.line_edit_list[amd_core_ui_instance.vrm_index].value;
                            vrm_wr_vcore(volt);
                            refresh_page = true;                            
                        }
                        else if (amd_core_ui_instance.current_line_edit == amd_core_ui_instance.apply_index)//apply
                        {
                            apply_amd_settings();
                            refresh_page = true;
                        }
                        else
                        {
                            //back
                            current_page = PAGE_STATE.main_page;
                            previous_page = PAGE_STATE.amd_core_page;
                        }
                        if (refresh_page)
                        {
                            amd_core_class = new AMD_CORE_CLASS(this);
                            amd_core_ui_instance = new amd_core_ui(this);
                        }
                        pause_task = false;
                    }
                }

                else if (current_page == PAGE_STATE.numerical_input)
                {
                    if (input_keypad.hitme(x, y))
                    {
                        if (input_keypad.is_ready)
                        {
                            if (previous_page == PAGE_STATE.intel_core_page)
                            { 
                                intel_core_ui_instance.update_line_edit(input_keypad.value);  
                                intel_core_page_changed = true;
                            }
                            else if (previous_page == PAGE_STATE.amd_core_page)
                            {
                                amd_core_ui_instance.update_line_edit(input_keypad.value);
                                amd_core_page_changed = true;
                            }
                            current_page = previous_page;
                        }
                    }
                }
                if (found)
                {
                    if (current_page == PAGE_STATE.main_page)
                    {
                        if (!isamd)
                        {
                            intel_core_class = new INTEL_CORE_CLASS(this);
                            intel_core_ui_instance = new intel_core_ui(this);
                            intel_core_page_changed = true;
                            current_page = PAGE_STATE.intel_core_page;
                            previous_page = PAGE_STATE.main_page;
                        }
                        else
                        {
                            amd_core_class = new AMD_CORE_CLASS(this);
                            amd_core_ui_instance = new amd_core_ui(this);
                            amd_core_page_changed = true;
                            current_page = PAGE_STATE.amd_core_page;
                            previous_page = PAGE_STATE.main_page;
                        }

                    }
                }
            }
            parent.WidgetManager.OnTriggerOccurred(my_clicked_trigger_guid);
        }

        public System.Windows.Controls.UserControl GetSettingsControl() => new SettingsControl(this);

        public void Dispose()
        {
            run_task = false;
            pause_task = false;
        }

        public void EnterSleep()
        {
            pause_task = true;
        }

        public void ExitSleep()
        {
            pause_task = false;
            timestamp_last = DateTime.MinValue;
        }

        // Class specific
        private Thread task_thread;
        private volatile bool run_task;
        private volatile bool pause_task;

        public Font DrawFontDate;
        public Font UserFontDate;
        public Font DrawFontTime;
        public Font UserFontTime;
        public Bitmap BitmapCurrent;
        private DateTime timestamp_last;

        Mutex drawing_mutex = new Mutex();

        public bool time_24h = true;

        private Guid clicked_trigger_guid = new Guid("F6228B98-B94B-4088-8CA6-484A36436E2A");
        private Guid me_toggle_guid = new Guid("cbd0c64c-13ff-49b6-a2f3-9f7f2eb8141c");
        private Guid my_clicked_trigger_guid = new Guid("b5c97a2c-9a9c-4e5f-be31-575d1ffcfb07");

        public Color DrawBackColor;
        public Color UserBackColor;
        public Color DrawForeColor;
        public Color UserForeColor;
        public bool UseGlobal = false;
        public My_Processor_Core mecores = new My_Processor_Core();

        public bool timer_started = false;
        public Stopwatch stopwatch = new Stopwatch();
        public long elapsed_time=0;
        public int num_ticks_elapsed = 0;
        public int num_core_grids = 0;
        public Color LineColor;
        public Pen ppen;
        public grid_point_struct[] grid_struct_vec;
        public grid_line_struct grid_line_vec;
        public int num_gridlines = 0;
        public volatile bool intel_core_page_changed = true;
        public volatile bool amd_core_page_changed = true;
        public enum PAGE_STATE
        {
            main_page,
            intel_core_page,
            amd_core_page,
            numerical_input,

        }

        public volatile static PAGE_STATE current_page = PAGE_STATE.main_page;
        public volatile static PAGE_STATE previous_page = PAGE_STATE.main_page;
        public INTEL_CORE_CLASS intel_core_class;
        public intel_core_ui intel_core_ui_instance;
        public AMD_CORE_CLASS amd_core_class;
        public amd_core_ui amd_core_ui_instance;
        public unsafe void initialize_grid()
        {
            int num_horizontal_boxes = 1;
            int num_vertical_boxes = 1;
            switch (num_core_grids)
            {
                case 2:
                    num_horizontal_boxes = 2;
                    break;
                case 3:
                case 4:
                    num_horizontal_boxes = 2;
                    num_vertical_boxes = 2;
                    break;
                case 5:
                case 6:
                    num_horizontal_boxes = 3;
                    num_vertical_boxes = 2;
                    break;
                case 7:
                case 8:
                    num_horizontal_boxes = 4;
                    num_vertical_boxes = 2;
                    break;
                case 9:
                case 10:
                case 11:
                case 12:
                    num_horizontal_boxes = 4;
                    num_vertical_boxes = 3;
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                    num_horizontal_boxes = 4;
                    num_vertical_boxes = 4;
                    break;
                case 17:
                case 18:
                case 19:
                case 20:
                    num_horizontal_boxes = 5;
                    num_vertical_boxes = 4;
                    break;
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    num_horizontal_boxes = 5;
                    num_vertical_boxes = 5;
                    break;
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                    num_horizontal_boxes = 6;
                    num_vertical_boxes = 5;
                    break;
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                    num_horizontal_boxes = 6;
                    num_vertical_boxes = 6;
                    break;
                default:
                    break;
            }

            int spacer = 2;
            int width_for_each = (BitmapCurrent.Width / num_horizontal_boxes)- spacer;
            int height_for_each = (BitmapCurrent.Height / num_vertical_boxes) - spacer-1;

            int num_vertical_lines = num_vertical_boxes - 1;
            int num_horizontal_lines = num_horizontal_boxes - 1;
            
            int index = 0;
            int startx = width_for_each + spacer;
            grid_line_vec.grid_ptrs = new Point[512];
            int nlines = 0;
            while (index< num_horizontal_lines)
            {
                Point mept= new Point(startx, 0);
                Point mept2 = new Point(startx, BitmapCurrent.Height);
                grid_line_vec.grid_ptrs[nlines] = mept;//.Append(mept);
                grid_line_vec.grid_ptrs[nlines + 1] = mept2;//.Append(mept2);
                num_gridlines++;
                nlines += 2;
                index++;
                startx += (width_for_each + spacer);
            }
            index = 0;
            int starty = height_for_each + spacer;
            while (index < num_vertical_lines)
            {
                Point mept = new Point(0, starty);
                Point mept2 = new Point(BitmapCurrent.Width, starty);
                grid_line_vec.grid_ptrs[nlines] = mept;//.Append(mept);
                grid_line_vec.grid_ptrs[nlines + 1] = mept2;//.Append(mept2);
                num_gridlines++;
                nlines += 2;
                index++;
                starty += (height_for_each + spacer);
            }
            index = 0;
            startx = 0;
            starty = 0;           

            for (int row=0;row<= num_vertical_lines; row++)
            {
                bool done = false;
                for (int col = 0; col <= num_horizontal_lines; col++)
                {
                    grid_struct_vec[index].start_y = starty;
                    grid_struct_vec[index].end_y = starty+ height_for_each;
                    grid_struct_vec[index].start_x = startx;
                    grid_struct_vec[index].end_x = startx + width_for_each;
                    grid_struct_vec[index].y_range = height_for_each;
                    grid_struct_vec[index].x_range = width_for_each;
                    grid_struct_vec[index].grid_ptrs = new Point[width_for_each+20];
                    grid_struct_vec[index].grid_ptrs[0] = new Point(startx, grid_struct_vec[index].end_y);
                    grid_struct_vec[index].label_x = startx + 2;
                    grid_struct_vec[index].label_y = starty + 2;
                    startx += (width_for_each+3);
                    index++;
                    if(index>=num_core_grids)
                    {
                        done = true;
                        break;
                    }
                }
                if (done)
                    break;
                startx = 0;
                starty += (height_for_each+3);
            }


        }

        public unsafe OCWidgetInstance(OCWidgetServer parent, WidgetSize widget_size, Guid instance_guid)
        {
            /*string exePath = @"File.exe";

            // Create a ProcessStartInfo with the required properties
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = exePath,
                Verb = "runas" // This verb runs the process with elevated privileges
            };

            try
            {
                // Start the process
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // Handle any exceptions, such as if the user cancels the UAC prompt
                Console.WriteLine("Error: " + ex.Message);
            }
*/
            this.parent = parent;
            this.WidgetSize = widget_size;
            this.Guid = instance_guid;
            if (widget_size.Equals(2, 1))
            {
                DrawFontDate = new Font("Verdana", 18, System.Drawing.FontStyle.Bold);
                DrawFontTime = new Font("Basic Square 7", 56);
            }
            else
            {
                DrawFontDate = new Font("Verdana", 14);// System.Drawing.FontStyle.Bold);
                DrawFontTime = new Font("Basic Square 7", 30);
            }
            LineColor = Color.LightPink;
            
            BitmapCurrent = new Bitmap(widget_size.ToSize().Width, widget_size.ToSize().Height);
            bool init_lib_success=initialize_libs(); 
            get_my_processor(ref mecores);
            num_core_grids = mecores.Efficiency_Class_size;
            if (num_core_grids > 32)
                num_core_grids = 32;
            grid_struct_vec = new grid_point_struct[num_core_grids];
            grid_line_vec = new grid_line_struct();
            isamd= check_if_amd();
            initialize_grid();


            LoadSettings();

            UpdateSettings();

            // Register widget clicked
            parent.WidgetManager.RegisterTrigger(this, my_clicked_trigger_guid, "Clicked");

            // Register toggle time
            parent.WidgetManager.RegisterAction(this, me_toggle_guid, "Toggle me");

            // Register for action events
            parent.WidgetManager.ActionRequested += WidgetManager_ActionRequested;

            parent.WidgetManager.GlobalThemeUpdated += WidgetManager_GlobalThemeUpdated;

            // Start thread
            ThreadStart thread_start = new ThreadStart(UpdateTask);
            task_thread = new Thread(thread_start);
            task_thread.IsBackground = true;
            run_task = true;
            pause_task = false; 
            task_thread.Start();
        }

        private void WidgetManager_GlobalThemeUpdated()
        {
            if (UseGlobal)
            {
                UpdateSettings();
            }
        }

        private void WidgetManager_ActionRequested(Guid action_guid) {
#if true
            if (action_guid == me_toggle_guid)
            {
                if (drawing_mutex.WaitOne(1000))
                {
                    UpdateWidget();
                    drawing_mutex.ReleaseMutex();
                }
            }
#endif 
        }
        private void DrawNumericKeypad()
        {
            if (drawing_mutex.WaitOne(1000))
            {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                int index = 0;
                foreach(System.Drawing.Rectangle line in input_keypad.rect_vec)
                {
                    Brush rectbrush = new SolidBrush(DrawBackColor);
                    g.FillRectangle(rectbrush, line);
                    g.DrawRectangle(input_keypad.borderpen, line);                    
                    if (index==0)
                    { 
                        Font tfont = new Font(DrawFontDate.Name, DrawFontDate.Size*2, DrawFontDate.Style, DrawFontDate.Unit); 
                        SizeF str_size = g.MeasureString(input_keypad.text, tfont);
                        int startx = (int)(line.Width - str_size.Width - 3);
                        startx += line.X;
                        int starty = (int)(line.Height - str_size.Width - 3);
                        Brush textBrush = new SolidBrush(DrawForeColor);
                        g.DrawString(input_keypad.text, tfont, textBrush, startx, line.Y + 2);
                    }
                    else if(index<=10)
                    {
                        string str = (index - 1).ToString();
                        Font tfont = new Font(DrawFontDate.Name, DrawFontDate.Size * 1.5f, DrawFontDate.Style, DrawFontDate.Unit);
                        SizeF str_size = g.MeasureString(str, tfont);
                        int startx = (int)(line.Width - str_size.Width - 3);
                        startx += line.X;                        
                        Brush textBrush = new SolidBrush(DrawForeColor);
                        g.DrawString(str, tfont, textBrush, startx, line.Y + 2);
                    }
                    else
                    {
                        //we have negative,increment,decrement,dot,backspace,enter starting 11 index
                        string str = "esc";
                        Font tfont = new Font(DrawFontDate.Name, DrawFontDate.Size * 1.5f, DrawFontDate.Style, DrawFontDate.Unit);
                        switch (index)
                        {
                            case 11:
                                break;
                            case 12:
                                str = "neg";
                                break;
                            case 13:
                                str = "inc";
                                break;
                            case 14:
                                str = "dec";
                                break;
                            case 15:
                                str = ".";
                                break;
                            case 16:
                                str = "clr";
                                break;
                            case 17:
                                str = "bksp";
                                break;
                            case 18:
                                str = "ent";
                                break;
                            default:
                                break;
                        }
                        SizeF str_size = g.MeasureString(str, tfont);
                        int startx = (int)(line.Width - str_size.Width - 3);
                        startx += line.X;                        
                        Brush textBrush = new SolidBrush(DrawForeColor);
                        g.DrawString(str, tfont, textBrush, startx, line.Y + 2);
                    }
                    index++;
                }
            }
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }
        private void DrawButtons(List<my_button> le_list)
        {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    foreach (my_button line in le_list)
                    {
                        g.DrawRectangle(line.borderpen, line.x, line.y, line.width, line.height);
                        string txt = line.text;
                        SizeF str_size = g.MeasureString(txt, DrawFontDate);
                        Brush textBrush = new SolidBrush(line.text_color);
                        float startx = line.width - str_size.Width - 2;
                        startx += line.x;
                        g.DrawString(txt, DrawFontDate, textBrush, startx, line.y + 4);
                    }
                }
        }
        private void DrawLineEdits(List<my_line_edit> le_list)
        {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    foreach (my_line_edit line in le_list)
                    {
                        g.DrawRectangle(line.borderpen, line.x, line.y, line.width, line.height);
                        string txt = line.text;
                        SizeF str_size = g.MeasureString(txt, DrawFontDate);
                        Brush textBrush = new SolidBrush(line.mypen.Color);
                        float startx = line.width - str_size.Width - 2;
                        startx += line.x;
                        g.DrawString(txt, DrawFontDate, textBrush, startx, line.y + 2);
                    }
                }
        }
        private void DrawCheckBox(List<my_checkbox> box_list)
        {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    foreach (my_checkbox line in box_list)
                    {
                        g.DrawRectangle(line.borderpen, line.box_rect);                        
                        if(line.is_check)
                        {
                            Brush rectbrush = new SolidBrush(DrawForeColor);
                            g.DrawRectangle(line.borderpen, line.inner_rect);
                            g.FillRectangle(rectbrush, line.inner_rect); 
                        }
                        else
                        {
                            Brush rectbrush = new SolidBrush(DrawBackColor);
                            g.DrawRectangle(line.borderpen, line.inner_rect);
                            g.FillRectangle(rectbrush, line.inner_rect);
                        }
                    }
                }
        }
        private unsafe void DrawCoreUse()
        {
            if (drawing_mutex.WaitOne(1000))
            {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit; 
                    g.Clear(DrawBackColor);
                    System.Drawing.Pen ppen = new System.Drawing.Pen(LineColor);
                    int index = 0;
                    for (int i=0;i<num_gridlines;i++)
                    {
                        g.DrawLine(ppen, grid_line_vec.grid_ptrs[index].X, grid_line_vec.grid_ptrs[index].Y, grid_line_vec.grid_ptrs[index+1].X, grid_line_vec.grid_ptrs[index + 1].Y);
                       
                        index += 2;
                    }
                        System.Drawing.Pen ppen2 = new System.Drawing.Pen(DrawForeColor);

                        System.Drawing.Brush textBrush = new SolidBrush(System.Drawing.Color.PaleVioletRed);
                   
                    if (num_ticks_elapsed > 4)
                    {
                        for (int i = 0; i < num_core_grids; i++)
                        {
                            g.DrawCurve(ppen2, grid_struct_vec[i].grid_ptrs, 0, num_ticks_elapsed, 0.0f);
                            string labe = "c"+i.ToString();
                            g.DrawString(labe, DrawFontDate, textBrush, grid_struct_vec[i].label_x, grid_struct_vec[i].label_y);
                        }
                    }
                } 
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }
        private unsafe void DrawIntelCore()
        {
            if (drawing_mutex.WaitOne(1000))
            {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.Clear(DrawBackColor);
                    Pen ppen2 = new Pen(DrawForeColor);
                    Brush textBrush = new SolidBrush(DrawForeColor);
                    g.DrawString(intel_core_ui_instance.core_ratio_text, DrawFontDate, textBrush, intel_core_ui_instance.core_ratio_text_pos.X, intel_core_ui_instance.core_ratio_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.trl_text, DrawFontDate, textBrush, intel_core_ui_instance.trl_text_pos.X, intel_core_ui_instance.trl_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.vid_text, DrawFontDate, textBrush, intel_core_ui_instance.vid_text_pos.X, intel_core_ui_instance.vid_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.offset_text, DrawFontDate, textBrush, intel_core_ui_instance.offset_text_pos.X, intel_core_ui_instance.offset_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.override_text, DrawFontDate, textBrush, intel_core_ui_instance.override_text_pos.X, intel_core_ui_instance.override_text_pos.Y);

                    DrawLineEdits(intel_core_ui_instance.line_edit_list);
                    DrawCheckBox(intel_core_ui_instance.checkbox_list);

                    if(intel_core_class.my_vf_pts.num_pts>0)
                    {
                        g.DrawString(intel_core_ui_instance.vfp_text, DrawFontDate, textBrush, intel_core_ui_instance.vfp_text_pos.X, intel_core_ui_instance.vfp_text_pos.Y); 
                        foreach (intel_core_ui.vfp_arr arr in intel_core_ui_instance.vfp_list)
                        {
                            g.DrawString(arr.vfp_str, DrawFontDate, textBrush, arr.vfp_pos.X, arr.vfp_pos.Y);
                            g.DrawRectangle(arr.vfp_line_edit.borderpen, arr.vfp_line_edit.x, arr.vfp_line_edit.y, arr.vfp_line_edit.width, arr.vfp_line_edit.height);
                            string txt = arr.vfp_line_edit.text;
                            SizeF str_size = g.MeasureString(txt, DrawFontDate);
                            Brush textBrush2 = new SolidBrush(arr.vfp_line_edit.mypen.Color);
                            float startx = arr.vfp_line_edit.width - str_size.Width - 2;
                            startx += arr.vfp_line_edit.x;
                            g.DrawString(txt, DrawFontDate, textBrush2, startx, arr.vfp_line_edit.y + 2);
                        }
                    }
                    g.DrawString(intel_core_ui_instance.pl1_text, DrawFontDate, textBrush, intel_core_ui_instance.pl1_text_pos.X, intel_core_ui_instance.pl1_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.pl2_text, DrawFontDate, textBrush, intel_core_ui_instance.pl2_text_pos.X, intel_core_ui_instance.pl2_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.iccmax_text, DrawFontDate, textBrush, intel_core_ui_instance.iccmax_text_pos.X, intel_core_ui_instance.iccmax_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.iccmaxdis_text, DrawFontDate, textBrush, intel_core_ui_instance.iccmaxdis_text_pos.X, intel_core_ui_instance.iccmaxdis_text_pos.Y);

                    if(vrmtype>0)
                    {
                        g.DrawString(intel_core_ui_instance.vrm_text, DrawFontDate, textBrush, intel_core_ui_instance.vrm_text_pos.X, intel_core_ui_instance.vrm_text_pos.Y);
                    }
                    g.DrawString(intel_core_ui_instance.apply_for_all_text, DrawFontDate, textBrush, intel_core_ui_instance.apply_for_all_text_pos.X, intel_core_ui_instance.apply_for_all_text_pos.Y);

                    //ring
                    g.DrawString(intel_core_ui_instance.ring_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_text_pos.X, intel_core_ui_instance.ring_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.ring_min_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_min_text_pos.X, intel_core_ui_instance.ring_min_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.ring_max_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_max_text_pos.X, intel_core_ui_instance.ring_max_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.ring_vid_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_vid_text_pos.X, intel_core_ui_instance.ring_vid_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.ring_offset_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_offset_text_pos.X, intel_core_ui_instance.ring_offset_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.ring_downbin_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_downbin_text_pos.X, intel_core_ui_instance.ring_downbin_text_pos.Y);
                    g.DrawString(intel_core_ui_instance.ring_manual_text, DrawFontDate, textBrush, intel_core_ui_instance.ring_manual_text_pos.X, intel_core_ui_instance.ring_manual_text_pos.Y);

                    if (intel_core_class.sp!=0)
                    {
                        g.DrawString(intel_core_ui_instance.sp_text, DrawFontDate, textBrush, intel_core_ui_instance.sp_text_pos.X, intel_core_ui_instance.sp_text_pos.Y);
                    }
                    DrawButtons(intel_core_ui_instance.button_list);
                }
                intel_core_page_changed = false;
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }

        private unsafe void DrawAmdCore()
        {
            if (drawing_mutex.WaitOne(1000))
            {
                using (Graphics g = Graphics.FromImage(BitmapCurrent))
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    g.Clear(DrawBackColor);
                    Pen ppen2 = new Pen(DrawForeColor);
                    Brush textBrush = new SolidBrush(DrawForeColor);
                    g.DrawString(amd_core_ui_instance.core_ratio_text, DrawFontDate, textBrush, amd_core_ui_instance.core_ratio_text_pos.X, amd_core_ui_instance.core_ratio_text_pos.Y); 
                    g.DrawString(amd_core_ui_instance.vid_text, DrawFontDate, textBrush, amd_core_ui_instance.vid_text_pos.X, amd_core_ui_instance.vid_text_pos.Y);

                    g.DrawString(amd_core_ui_instance.ppt_text, DrawFontDate, textBrush, amd_core_ui_instance.ppt_text_pos.X, amd_core_ui_instance.ppt_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.tdc_text, DrawFontDate, textBrush, amd_core_ui_instance.tdc_text_pos.X, amd_core_ui_instance.tdc_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.edc_text, DrawFontDate, textBrush, amd_core_ui_instance.edc_text_pos.X, amd_core_ui_instance.edc_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.thm_text, DrawFontDate, textBrush, amd_core_ui_instance.thm_text_pos.X, amd_core_ui_instance.thm_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.fmax_text, DrawFontDate, textBrush, amd_core_ui_instance.fmax_text_pos.X, amd_core_ui_instance.fmax_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.fit_text, DrawFontDate, textBrush, amd_core_ui_instance.fit_text_pos.X, amd_core_ui_instance.fit_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.co_text, DrawFontDate, textBrush, amd_core_ui_instance.co_text_pos.X, amd_core_ui_instance.co_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.apply_for_all_text, DrawFontDate, textBrush, amd_core_ui_instance.apply_for_all_text_pos.X, amd_core_ui_instance.apply_for_all_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.ocmode_text, DrawFontDate, textBrush, amd_core_ui_instance.ocmode_text_pos.X, amd_core_ui_instance.ocmode_text_pos.Y);
                    g.DrawString(amd_core_ui_instance.vid_text, DrawFontDate, textBrush, amd_core_ui_instance.vid_text_pos.X, amd_core_ui_instance.vid_text_pos.Y);


                    DrawLineEdits(amd_core_ui_instance.line_edit_list);
                    DrawCheckBox(amd_core_ui_instance.checkbox_list); 
                    if (vrmtype > 0)
                    {
                        g.DrawString(amd_core_ui_instance.vrm_text, DrawFontDate, textBrush, amd_core_ui_instance.vrm_text_pos.X, amd_core_ui_instance.vrm_text_pos.Y);
                    }
                    g.DrawString(amd_core_ui_instance.apply_for_all_text, DrawFontDate, textBrush, amd_core_ui_instance.apply_for_all_text_pos.X, amd_core_ui_instance.apply_for_all_text_pos.Y);
                     
                    if (amd_core_class.sp != 0)
                    {
                        g.DrawString(amd_core_ui_instance.sp_text, DrawFontDate, textBrush, amd_core_ui_instance.sp_text_pos.X, amd_core_ui_instance.sp_text_pos.Y);
                    }
                    DrawButtons(amd_core_ui_instance.button_list);
                }
                amd_core_page_changed = false;
                UpdateWidget();
                drawing_mutex.ReleaseMutex();
            }
        }

        private void UpdateWidget()
        {
            WidgetUpdatedEventArgs e = new WidgetUpdatedEventArgs();
            e.WidgetBitmap = BitmapCurrent;
            e.WaitMax = 1000;
            WidgetUpdated?.Invoke(this, e);
        }
        private unsafe void grid_struct_add_pts()
        {
            int index = 0;
            while(index<num_core_grids)
            {
                double load = mecores.thread_usage_vec[index];
                load = (load / 100.0) * (double)grid_struct_vec[index].y_range;
                int y= grid_struct_vec[index].end_y-(int)load;
                grid_struct_vec[index].grid_ptrs[num_ticks_elapsed] = new Point(num_ticks_elapsed+ grid_struct_vec[index].start_x, y);
                index++;
            }
        }
        private unsafe void grid_struct_move_forward()
        {
            int index = 0;
            while (index < num_core_grids)
            {
                for(int i=1;i<= num_ticks_elapsed;i++)
                {
                    grid_struct_vec[index].grid_ptrs[i - 1].Y = grid_struct_vec[index].grid_ptrs[i].Y; 
                }
                double load = mecores.thread_usage_vec[index];
                load = (load / 100.0) * (double)grid_struct_vec[index].y_range;
                int y = grid_struct_vec[index].end_y - (int)load;
                grid_struct_vec[index].grid_ptrs[num_ticks_elapsed].X = num_ticks_elapsed + grid_struct_vec[index].start_x;
                grid_struct_vec[index].grid_ptrs[num_ticks_elapsed].Y = y;
                index++;
            }
        }
        private unsafe void Update_intel_core_page()
        {
            if (intel_core_page_changed)
                DrawIntelCore();
        }
        private unsafe void Update_amd_core_page()
        {
            if (amd_core_page_changed)
                DrawAmdCore();
        }
        private unsafe void Update_core_usage()
        {
            if (!timer_started)
            {
                timer_started = true;
            }
            fixed (byte* p = mecores.thread_usage_vec)
            {
                get_core_usage(p);
            }
            if (num_ticks_elapsed < grid_struct_vec[0].x_range)
            {
                num_ticks_elapsed++;
                grid_struct_add_pts();                
            }
            else
            {
                grid_struct_move_forward();
            }
            DrawCoreUse();

        }
        private void UpdateTask()
        {             
            while (run_task)
            {
                if(pause_task)
                {
                    is_paused = true;
                    Thread.Sleep(100);
                    while(true)
                    {
                        if (!pause_task)
                        {
                            is_paused = false;
                            break;

                        }
                        Thread.Sleep(100);
                    }
                }
                if (current_page == PAGE_STATE.main_page)
                    Update_core_usage();
                else if (current_page == PAGE_STATE.intel_core_page)
                    Update_intel_core_page();
                else if (current_page == PAGE_STATE.amd_core_page)
                    Update_amd_core_page();
                else if (current_page == PAGE_STATE.numerical_input)
                    DrawNumericKeypad();
                if (!run_task) return;
               Thread.Sleep(100);
            }
        }

        public void SaveSettings()
        {
            // Save setting
            parent.WidgetManager.StoreSetting(this, nameof(UseGlobal), UseGlobal.ToString()); 
            parent.WidgetManager.StoreSetting(this, nameof(UserBackColor), ColorTranslator.ToHtml(UserBackColor));
            parent.WidgetManager.StoreSetting(this, nameof(UserForeColor), ColorTranslator.ToHtml(UserForeColor));
            parent.WidgetManager.StoreSetting(this, nameof(UserFontDate), new FontConverter().ConvertToInvariantString(UserFontDate)); 
        }

        public void LoadSettings()
        {
            if (parent.WidgetManager.LoadSetting(this, nameof (UseGlobal), out string useGlobalStr))
            {
                UseGlobal = bool.Parse(useGlobalStr);
            } else
            {
                UseGlobal = parent.WidgetManager.PreferGlobalTheme;
            }

            if (parent.WidgetManager.LoadSetting(this, nameof(UserBackColor), out string bgTintStr))
            {
                UserBackColor = ColorTranslator.FromHtml(bgTintStr);
            } else
            {
                Random rnd = new Random();
                UserBackColor = Color.FromArgb(rnd.Next(0, 150), rnd.Next(0, 150), rnd.Next(0, 150));
            }

            if (parent.WidgetManager.LoadSetting(this, nameof(UserForeColor), out string fgColorStr))
            {
                UserForeColor = ColorTranslator.FromHtml(fgColorStr);
            }
            else
            {
                UserForeColor = Color.FromArgb(255 - UserBackColor.R, 255 - UserBackColor.G, 255 - UserBackColor.B);
            }

            if (parent.WidgetManager.LoadSetting(this, nameof(UserFontDate), out var strDateFont))
            {
                UserFontDate = new FontConverter().ConvertFromInvariantString(strDateFont) as Font;
            } else
            {
                UserFontDate = DrawFontDate;
            }

            if (parent.WidgetManager.LoadSetting(this, nameof(UserFontTime), out var strTimeFont))
            {
                UserFontTime = new FontConverter().ConvertFromInvariantString(strTimeFont) as Font;
            } else
            {
                UserFontTime = DrawFontTime;
            }
        }

        public void UpdateSettings()
        {
            if (UseGlobal)
            {
                DrawBackColor = parent.WidgetManager.GlobalWidgetTheme.PrimaryBgColor;
                DrawForeColor = parent.WidgetManager.GlobalWidgetTheme.PrimaryFgColor;
                DrawFontDate = new Font(parent.WidgetManager.GlobalWidgetTheme.SecondaryFont.FontFamily, DrawFontDate.Size, DrawFontDate.Style);
                DrawFontTime = new Font(parent.WidgetManager.GlobalWidgetTheme.PrimaryFont.FontFamily, DrawFontTime.Size, DrawFontTime.Style);
            }
            else
            {
                DrawBackColor = UserBackColor;
                DrawForeColor = UserForeColor;
                DrawFontDate = UserFontDate;
                DrawFontTime = UserFontTime;
            }

            RequestUpdate();
        }
    }
}

