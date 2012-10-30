﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;  

namespace HeraDMS.Layouts.Helper
{
        /* 
 * Class:WaterImage 
 * Use for add a water Image to the picture both words and image 
 * 2007.07.23   create the file 
 * 
 *  
 * 使用说明： 
 * 　建议先定义一个WaterImage实例 
 * 　然后利用实例的属性，去匹配需要进行操作的参数 
 * 　然后定义一个WaterImageManage实例 
 * 　利用WaterImageManage实例进行DrawImage（），印图片水印 
 * 　DrawWords（）印文字水印 
 *  
-*/  
  

/// <summary>   
/// 图片位置   
/// </summary>   
public enum ImagePosition  
{  
    LeftTop,        //左上   
    LeftBottom,    //左下   
    RightTop,       //右上   
    RigthBottom,  //右下   
    TopMiddle,     //顶部居中   
    BottomMiddle, //底部居中   
    Center           //中心   
}  
  
/// <summary>   
/// 水印图片的操作管理 Design by Gary Gong From Demetersoft.com   
/// </summary>   
public class WaterImageManage  
{  
    /// <summary>   
    /// 生成一个新的水印图片制作实例   
    /// </summary>   
    public WaterImageManage ()  
    {  
        //   
        // TODO: Add constructor logic here   
        //   
    }  
  
    /// <summary>   
    /// 添加图片水印   
    /// </summary>   
    /// <param name="sourcePicture">源图片文件名</param>   
    /// <param name="waterImage">水印图片文件名</param>   
    /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>   
    /// <param name="position">位置</param>   
    /// <param name="PicturePath" >生产水印图片的路径</param>   
    /// <returns>返回生成于指定文件夹下的水印文件名</returns>   
    public bool DrawImage(Stream sourcePicture,
                                      string waterImage,  
                                      float alpha,
                                      float waterImageSize,
                                      ImagePosition position,  
                                      string PicturePath )  
    {
        bool IsSuccess = true;
        try
        {

            // 将需要加上水印的图片装载到Image对象中   
            //   
            Image imgPhoto = Image.FromStream(sourcePicture);
            //   
            // 确定其长宽   
            //   
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //   
            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。   
            //   
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //   
            // 设定分辨率   
            //    
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //   
            // 定义一个绘图画面用来装载位图   
            //   
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //   
            //同样，由于水印是图片，我们也需要定义一个Image来装载它   
            //   
            Image imgWatermark = new Bitmap(waterImage);

            //   
            // 获取水印图片的高度和宽度   
            //   
            int wmWidth = (int)(imgWatermark.Width * waterImageSize);
            int wmHeight = (int)(imgWatermark.Height * waterImageSize);

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。   
            // 成员名称   说明    
            // AntiAlias      指定消除锯齿的呈现。     
            // Default        指定不消除锯齿。     
            // HighQuality  指定高质量、低速度呈现。     
            // HighSpeed   指定高速度、低质量呈现。     
            // Invalid        指定一个无效模式。     
            // None          指定不消除锯齿。    
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //   
            // 第一次描绘，将我们的底图描绘在绘图画面上   
            //   
            grPhoto.DrawImage(imgPhoto,
                                        new Rectangle(0, 0, phWidth, phHeight),
                                        0,
                                        0,
                                        phWidth,
                                        phHeight,
                                        GraphicsUnit.Pixel);

            //   
            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率   
            //   
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //   
            // 继续，将水印图片装载到一个绘图画面grWatermark   
            //   
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //   
            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。   
            //          
            ImageAttributes imageAttributes = new ImageAttributes();

            //   
            //Colormap: 定义转换颜色的映射   
            //   
            ColorMap colorMap = new ColorMap();

            //   
            //我的水印图被定义成拥有绿色背景色的图片被替换成透明   
            //   
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {   
           new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色   
           new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色   
           new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色          
           new float[] {0.0f,  0.0f,  0.0f,  alpha, 0.0f}, //透明度        
           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//   

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。   
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。   
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);

            //   
            //上面设置完颜色，下面开始设置位置   
            //   
            int xPosOfWm;
            int yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    //yPosOfWm = phHeight - wmHeight - 10;
                    yPosOfWm = phHeight - wmHeight-2;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 2;
                    //yPosOfWm = phHeight - wmHeight - 10;
                    yPosOfWm = phHeight - wmHeight-2;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 2;
                    yPosOfWm = 2;
                    break;
                case ImagePosition.RightTop:
                    //xPosOfWm = phWidth - wmWidth - 10;
                    xPosOfWm = phWidth - wmWidth-2;
                    yPosOfWm = 2;
                    break;
                case ImagePosition.RigthBottom:
                    //xPosOfWm = phWidth - wmWidth - 10;
                    //yPosOfWm = phHeight - wmHeight - 10;
                    xPosOfWm = phWidth - wmWidth-2;
                    yPosOfWm = phHeight - wmHeight-2;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 2;
                    break;
                default:
                    xPosOfWm = 2;
                    //yPosOfWm = phHeight - wmHeight - 10;
                    yPosOfWm = phHeight - wmHeight-2;
                    break;
            }

            //   
            // 第二次绘图，把水印印上去   
            //   
            grWatermark.DrawImage(imgWatermark,
             new Rectangle(xPosOfWm,
                                 yPosOfWm,
                                 wmWidth,
                                 wmHeight),
                                 0,
                                 0,
                                 wmWidth,
                                 wmHeight,
                                 GraphicsUnit.Pixel,
                                 imageAttributes);


            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            //   
            // 保存文件到服务器的文件夹里面   
            //   
            ImageFormat imgFormat = null;
            string extension = Path.GetExtension(PicturePath).ToLower();
            if (extension.Equals(".jpg"))
            {
                imgFormat = ImageFormat.Jpeg;
            }
            else if (extension.Equals(".gif"))
            {
                imgFormat = ImageFormat.Gif;
            }
            else if (extension.Equals(".png"))
            {
                imgFormat = ImageFormat.Png;
            }
            imgPhoto.Save(PicturePath, imgFormat);
            imgPhoto.Dispose();
            imgWatermark.Dispose();
            
        }
        catch(Exception ex)
        {
            IsSuccess = false;
        }
        return IsSuccess;
    }  
  
    /// <summary>   
    /// 在图片上添加水印文字   
    /// </summary>   
    /// <param name="sourcePicture">源图片文件</param>   
    /// <param name="waterWords">需要添加到图片上的文字</param>   
    /// <param name="alpha">透明度</param>   
    /// <param name="position">位置</param>   
    /// <param name="PicturePath">文件路径</param>   
    /// <returns></returns>   
    public bool DrawWords(Stream sourcePicture,  
                                      string waterWords,  
                                      float alpha,  
                                      Font crFont,
                                      Color forColor,
                                      ImagePosition position,  
                                      string PicturePath)  
    {  
        bool IsSuccess = true;
        try
        {

            //创建一个图片对象用来装载要被添加水印的图片   
            Image imgPhoto = Image.FromStream(sourcePicture);

            //获取图片的宽和高   
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //   
            //建立一个bitmap，和我们需要加水印的图片一样大小   
            Bitmap bmPhoto = new Bitmap(sourcePicture);

            //SetResolution：设置此 Bitmap 的分辨率   
            //这里直接将我们需要添加水印的图片的分辨率赋给了bitmap   
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //Graphics：封装一个 GDI+ 绘图图面。   
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //设置图形的品质   
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //将我们要添加水印的图片按照原始大小描绘（复制）到图形中   
            grPhoto.DrawImage(
             imgPhoto,                                           //   要添加水印的图片   
             new Rectangle(0, 0, phWidth, phHeight), //  根据要添加的水印图片的宽和高   
             0,                                                     //  X方向从0点开始描绘   
             0,                                                     // Y方向    
             phWidth,                                            //  X方向描绘长度   
             phHeight,                                           //  Y方向描绘长度   
             GraphicsUnit.Pixel);                              // 描绘的单位，这里用的是像素   

            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空   
            SizeF crSize = new SizeF();


            //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。   
            crSize = grPhoto.MeasureString(waterWords, crFont);


            //截边5%的距离，定义文字显示(由于不同的图片显示的高和宽不同，所以按百分比截取)   
            int yPixlesFromBottom = (int)(phHeight * .05);

            //定义在图片上文字的位置   
            float wmHeight = crSize.Height;
            float wmWidth = crSize.Width;

            float xPosOfWm;
            float yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = phWidth/ 2;
                    yPosOfWm = phHeight - wmHeight;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = wmWidth/2;
                    yPosOfWm = phHeight - wmHeight;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = wmWidth / 2;
                    yPosOfWm = 0;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth/2;
                    yPosOfWm =0;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth/2;
                    yPosOfWm = phHeight - wmHeight;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = phWidth / 2;
                    yPosOfWm = 0;
                    break;
                default:
                    xPosOfWm = wmWidth;
                    yPosOfWm = phHeight - wmHeight - 2;
                    break;
            }  

            //封装文本布局信息（如对齐、文字方向和 Tab 停靠位），显示操作（如省略号插入和国家标准 (National) 数字替换）和 OpenType 功能。   
            StringFormat StrFormat = new StringFormat();

            //定义需要印的文字居中对齐   
            StrFormat.Alignment = StringAlignment.Center;

            //SolidBrush:定义单色画笔。画笔用于填充图形形状，如矩形、椭圆、扇形、多边形和封闭路径。   
            //这个画笔为描绘阴影的画笔，呈灰色   
            int m_alpha = Convert.ToInt32(255 * alpha);
            SolidBrush semiTransBrush2 = new SolidBrush(Color.FromArgb(m_alpha,forColor.R, forColor.G, forColor.B));

            //描绘文字信息，这个图层向右和向下偏移一个像素，表示阴影效果   
            //DrawString 在指定矩形并且用指定的 Brush 和 Font 对象绘制指定的文本字符串。   
            grPhoto.DrawString(waterWords,                                    //string of text   
                                       crFont,                                         //font   
                                       semiTransBrush2,                            //Brush   
                                       new PointF(xPosOfWm, yPosOfWm),  //Position   
                                       StrFormat);

            //从四个 ARGB 分量（alpha、红色、绿色和蓝色）值创建 Color 结构，这里设置透明度为153   
            //这个画笔为描绘正式文字的笔刷，呈白色   
            //SolidBrush semiTransBrush = new SolidBrush(forColor);
            
            //SolidBrush semiTransBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255)); 
            ////第二次绘制这个图形，建立在第一次描绘的基础上 
  
            ////update by wp
            //grPhoto.DrawString(waterWords,                 //string of text   
            //                           crFont,                                   //font   
            //                           semiTransBrush,                           //Brush   
            //                           new PointF(xPosOfWm, yPosOfWm),  //Position   
            //                           StrFormat);

            //imgPhoto是我们建立的用来装载最终图形的Image对象   
            //bmPhoto是我们用来制作图形的容器，为Bitmap对象   
            imgPhoto = bmPhoto;
            //释放资源，将定义的Graphics实例grPhoto释放，grPhoto功德圆满   
            grPhoto.Dispose();
            ImageFormat imgFormat = null;
            string extension = Path.GetExtension(PicturePath).ToLower();
            if (extension.Equals(".jpg"))
            {
                imgFormat = ImageFormat.Jpeg;
            }
            else if (extension.Equals(".gif"))
            {
                imgFormat = ImageFormat.Gif;
            }
            else if (extension.Equals(".png"))
            {
                imgFormat = ImageFormat.Png;
            }

            //将grPhoto保存   
            imgPhoto.Save(PicturePath, imgFormat);
            imgPhoto.Dispose();
        }
        catch (Exception ex)
        {
            IsSuccess = false;
        }
        return IsSuccess;
    }

    /// <summary>
    /// 获取图片位置
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public ImagePosition GetPosition(string position)
    {
        switch (position)
        {
            case "LeftTop":
                return ImagePosition.LeftTop;
            case "LeftBottom":
                return ImagePosition.LeftBottom;
            case "RightTop":
                return ImagePosition.RightTop;
            case "RigthBottom":
                return ImagePosition.RigthBottom;
            case "TopMiddle":
                return ImagePosition.TopMiddle;
            case "BottomMiddle":
                return ImagePosition.BottomMiddle;
            case "Center":
                return ImagePosition.Center;
            default:
                return ImagePosition.RigthBottom;
             

        }
    }
}  
  
/// <summary>   
/// 装载水印图片的相关信息   
/// </summary>   
public class WaterImage  
{  
    public WaterImage ()  
    {  
  
    }  
  
    private string m_sourcePicture;  
    /// <summary>   
    /// 源图片地址名字(带后缀)   
    /// </summary>   
    public string SourcePicture  
    {  
        get { return m_sourcePicture; }  
        set { m_sourcePicture = value; }  
    }  
  
    private string  m_waterImager;  
    /// <summary>   
    /// 水印图片名字(带后缀)   
    /// </summary>   
    public string  WaterPicture  
    {  
        get { return m_waterImager; }  
        set { m_waterImager = value; }  
    }  
  
    private float  m_alpha;  
    /// <summary>   
    /// 水印图片文字的透明度   
    /// </summary>   
    public float  Alpha  
    {  
        get { return m_alpha; }  
        set { m_alpha = value; }  
    }  
  
    private ImagePosition  m_postition;  
    /// <summary>   
    /// 水印图片或文字在图片中的位置   
    /// </summary>   
    public ImagePosition  Position  
    {  
        get { return m_postition; }  
        set { m_postition = value; }  
    }  
  
    private string  m_words;  
    /// <summary>   
    /// 水印文字的内容   
    /// </summary>   
    public string  Words  
    {  
        get { return m_words; }  
        set { m_words = value; }  
    }  
       
}  

}
