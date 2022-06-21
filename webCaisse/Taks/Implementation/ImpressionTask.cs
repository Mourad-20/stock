using Ghostscript.NET;
using Ghostscript.NET.Processor;
using Ghostscript.NET.Rasterizer;
using Ghostscript.NET.Viewer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using webCaisse.Taks.Contracts;

namespace webCaisse.Taks.Implementation
{
    public class ImpressionTask : IImpressionTask
    {
        public void imprimerPreparation(byte[] _content, string _printerName)
        {
            Boolean ACTIVATE_PRINT = ConfigInfrastructure.ACTIVATE_PRINT;
            
            String _pathPdfCrop = @"C:\CaisseFileRoot\Impression\PREPA_" + _printerName + "_" + DateTime.Now.ToFileTime() + ".pdf";

            //System.Drawing.Image _pdfAsImage = convertPDFToImg(_content);
            //_pdfAsImage = removeWhiteSpace(_pdfAsImage);

            //Double? _widthPaper = ConfigInfrastructure.WIDTH_PAPER;
            //Double? _heightPaper = (getMinHeightOfImage(_pdfAsImage) - 15) * 0.75;
            //Byte[] _contentPDFCrop = cropPdf(_content, (float)_heightPaper);
            //File.WriteAllBytes(_pathPdfCrop, _contentPDFCrop);
            File.WriteAllBytes(_pathPdfCrop, _content);
            //------------------------------------------------------------------            
            GhostscriptProcessor processor = new GhostscriptProcessor();
            List<string> switches = new List<string>();
            //switches.Add("-empty");
            //switches.Add("-dQUIET");
            //switches.Add("-dSAFER");
            switches.Add("-dBATCH");
            //switches.Add("-dNOPAUSE");
            //switches.Add("-dPrinted");
            switches.Add("-sDEVICE=mswinpr2");
            //switches.Add("-dDEVICEWIDTHPOINTS=" + _widthPaper * 2.83465); //unité point
            //switches.Add("-dDEVICEHEIGHTPOINTS=" + _heightPaper); // unité point
            //switches.Add("-dPDFFitPage");
            switches.Add("-sOutputFile=%printer%" + _printerName);
            //switches.Add("-q");
            switches.Add(_pathPdfCrop);
            
            if (ACTIVATE_PRINT == true)
            {
                processor.Process(switches.ToArray());
            }

            
        }

        private Byte[] cropPdf(byte[] _contentPdf,float _heightPaper) {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Byte[] _result = null;
            PdfReader reader = new PdfReader(_contentPdf);
            PdfDictionary dict = reader.GetPageN(1);
            PdfArray mediaBox = new PdfArray();
            iTextSharp.text.Rectangle cropBox = reader.GetCropBox(1);
            mediaBox.Add(new PdfNumber(cropBox.Left));
            mediaBox.Add(new PdfNumber(cropBox.Top));
            mediaBox.Add(new PdfNumber(cropBox.Right));
            mediaBox.Add(new PdfNumber(cropBox.Top - _heightPaper));
            dict.Put(PdfName.MEDIABOX, mediaBox);
            using (MemoryStream ms = new MemoryStream())
            {
                PdfStamper stamp = new PdfStamper(reader, ms);
                stamp.Dispose();
                _result = ms.ToArray();
            }

            watch.Stop();
            Debug.WriteLine($"cropPdf Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");

            return _result;
        }
        private double? getMinHeightOfImage(System.Drawing.Image _pdfAsImage) {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            double? _height = null;
            
            _height = _pdfAsImage.Height;
            watch.Stop();
            Debug.WriteLine($"getMinHeightOfImage Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");

            return _height;
        }
        private System.Drawing.Image convertPDFToImg(Byte[] _contentPDF)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Byte[] _contentImg = null;
            int xDpi = 96;
            System.Drawing.Image _image = null;
            using (GhostscriptViewer gsViewer = new GhostscriptViewer())
            {
                MemoryStream ms = new MemoryStream(_contentPDF);
                gsViewer.Open(ms);
                using (GhostscriptRasterizer gsRasterizer = new GhostscriptRasterizer(gsViewer))
                {
                    int pageCount = gsRasterizer.PageCount;

                    for (int i = 0; i < pageCount; i++)
                    {
                        _image = gsRasterizer.GetPage(xDpi, i + 1);
                    }
                }
            }
            
            watch.Stop();
            Debug.WriteLine($"convertPDFToImg Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");

            return _image;
        }

        private System.Drawing.Image removeWhiteSpace(System.Drawing.Image _img)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            System.Drawing.Image _imgResult = null;
            Bitmap imgBitmap = new Bitmap(_img);
            Color pixel = Color.White;
            Point _point = new Point(0, 0);
            for (int j = imgBitmap.Height - 10; j > 10; j--)
            {
                pixel = imgBitmap.GetPixel(100, j);
                if (pixel.R != 255 && pixel.G != 255 && pixel.B != 255)
                {
                    _point = new Point(100, j);
                    break;
                }
            }
            System.Drawing.Rectangle _rectangle = new System.Drawing.Rectangle(new Point(0, 0), new Size(imgBitmap.Width, _point.Y + 10));
            watch.Stop();
            Debug.WriteLine($"removeWhiteSpace Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");
            imgBitmap = cropAtRect(imgBitmap, _rectangle);
            _imgResult = (System.Drawing.Image)imgBitmap;

            

            return _imgResult;
        }
        
        private Bitmap cropAtRect(Bitmap b, System.Drawing.Rectangle cropRect) {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(b, new System.Drawing.Rectangle(0, 0, target.Width, target.Height),cropRect,GraphicsUnit.Pixel);
            }

            watch.Stop();
            Debug.WriteLine($"cropAtRect Execution Time: {watch.ElapsedMilliseconds} ms");
            Debug.WriteLine("===============================================================");

            return target;
        }

        public void imprimerPreparationWithGsView(byte[] _content, string _printerName)
        {
            /*
            String _pathPdfCrop = @"C:\CaisseFileRoot\Impression\CROPED" + _printerName + "_" + DateTime.Now.ToFileTime() + ".pdf";
            Double? _widthPaper = ConfigInfrastructure.WIDTH_PAPER;
            Double? _heightPaper = (getMinHeightOfImage(_content) - 10) * 0.75;
            Byte[] _contentPDFCrop = cropPdf(_content, (float)_heightPaper);
            File.WriteAllBytes(_pathPdfCrop, _contentPDFCrop);
            //-------------------------------------------------------------------
            string _gsLocation = ConfigurationManager.AppSettings["pathGsprint"];
            string _gsArguments;
            ProcessStartInfo _gsProcessInfo;
            Process _gsProcess;
            _gsArguments = string.Format("-ghostscript -noquery -portrait -printer \"{0}\" \"{1}\"", _printerName, _pathPdfCrop);
            _gsProcessInfo = new ProcessStartInfo();
            _gsProcessInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _gsProcessInfo.FileName = _gsLocation;
            _gsProcessInfo.Arguments = _gsArguments;
            _gsProcess = Process.Start(_gsProcessInfo);
            _gsProcess.WaitForExit();*/
        }

        public void imprimerNote(byte[] _content)
        {
            Boolean ACTIVATE_PRINT = ConfigInfrastructure.ACTIVATE_PRINT;
            String IMPRIMANTE_CENTRALE = ConfigInfrastructure.IMPRIMANTE_CENTRALE;

            String _pathPdfCrop = @"C:\CaisseFileRoot\Impression\NOTE_" + IMPRIMANTE_CENTRALE + "_" + DateTime.Now.ToFileTime() + ".pdf";

            //System.Drawing.Image _pdfAsImage = convertPDFToImg(_content);
            //_pdfAsImage = removeWhiteSpace(_pdfAsImage);

            //Double? _widthPaper = ConfigInfrastructure.WIDTH_PAPER;
            //Double? _heightPaper = (getMinHeightOfImage(_pdfAsImage) - 15) * 0.75;
            //Byte[] _contentPDFCrop = cropPdf(_content, (float)_heightPaper);
            //File.WriteAllBytes(_pathPdfCrop, _contentPDFCrop);
            File.WriteAllBytes(_pathPdfCrop, _content);
            //------------------------------------------------------------------            
            GhostscriptProcessor processor = new GhostscriptProcessor();
            List<string> switches = new List<string>();
            //switches.Add("-empty");
            //switches.Add("-dQUIET");
            //switches.Add("-dSAFER");
            switches.Add("-dBATCH");
            //switches.Add("-dNOPAUSE");
            //switches.Add("-dPrinted");
            switches.Add("-sDEVICE=mswinpr2");
            //switches.Add("-dDEVICEWIDTHPOINTS=" + _widthPaper * 2.83465); //unité point
            //switches.Add("-dDEVICEHEIGHTPOINTS=" + _heightPaper); // unité point
            //switches.Add("-dPDFFitPage");
            switches.Add("-sOutputFile=%printer%" + IMPRIMANTE_CENTRALE);
            //switches.Add("-q");
            switches.Add(_pathPdfCrop);

            if (ACTIVATE_PRINT == true)
            {
                processor.Process(switches.ToArray());
            }
        }

        public void imprimerPreparationWithGsView(byte[] _content)
        {
            throw new NotImplementedException();
        }
    }
}