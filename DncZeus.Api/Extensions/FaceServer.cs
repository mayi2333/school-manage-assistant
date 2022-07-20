using DlibDotNet;
using DlibDotNet.Extensions;
using FaceRecognitionDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace DncZeus.Api.Extensions
{
    public static class FaceServer
    {
        //
        private static FaceRecognition _FaceRecognition;
        //模型对应的标记名
        private static Dictionary<FaceEncoding, Guid> _faceDictionary;
        /// <summary>
        /// 当前已加载的人脸编码数量
        /// </summary>
        public static int FaceEncodingCount
        {
            get { return _faceDictionary.Count; }
        }
        /// <summary>
        /// 容忍差值
        /// </summary>
        private static double _tolerance;// = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["Tolerance"].ToString());
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init(double tolerance=0.6)
        {
            _tolerance = tolerance;
            _FaceRecognition = FaceRecognition.Create("facemodels");//当前目录
            _faceDictionary = new Dictionary<FaceEncoding, Guid>();//GetFaceEncodesList();
        }
        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static Dictionary<Guid, bool> FaceRecognitionForImage(string base64)
        {
            byte[] imageBytes = Convert.FromBase64String(base64);
            return FaceRecognitionForImage(imageBytes);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static Dictionary<Guid, bool> FaceRecognitionForImage(byte[] imageBytes)
        {
            if (imageBytes.Length > 0)
            {
                var bitmap = ToFormat24bpprgb(imageBytes);
                var array2d = bitmap.ToArray2D<RgbPixel>();
                bitmap.Dispose();
                var bytes = array2d.ToBytes();
                var image1 = FaceRecognition.LoadImage(bytes, array2d.Rows, array2d.Columns, array2d.Columns * 3, Mode.Rgb);
                array2d.Dispose();
                var endodings1 = _FaceRecognition.FaceEncodings(image1).ToArray();
                image1.Dispose();
                if (endodings1.Length > 0)
                {
                    var faceList = _faceDictionary.Keys.ToList();
                    Dictionary<Guid, bool> m_dic = new Dictionary<Guid, bool>();
                    //if (list.Count > 0)
                    //{
                    foreach (var endoding in endodings1)
                    {
                        var list = FaceRecognition.CompareFaces(faceList, endoding, _tolerance).ToList();
                        //Dictionary<string, double> dic = new Dictionary<string, double>();
                        //string res = string.Empty;
                        if (list.Count > 0)
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                //res += _faceDictionary[faceList[i]] + "|" + list[i] + ",";
                                if (list[i])
                                {
                                    m_dic.Add(_faceDictionary[faceList[i]], list[i]);
                                }
                            }
                        }
                        endoding.Dispose();
                        //res.TrimEnd(',');
                        //return m_dic;
                    }
                    return m_dic;
                    //}
                    //else
                    //{
                    //    throw new Exception("no suitable data");
                    //    //return "No Suitable Data";
                    //}
                }
                else
                {
                    throw new Exception("no recognition");
                    //return "No Recognition";
                }
            }
            else { throw new Exception("imageBytes is null"); }
        }
        /// <summary>
        /// 入录
        /// </summary>
        /// <param name="base64">图片base64</param>
        /// <param name="guid">该录入的人脸Guid</param>
        /// <returns></returns>
        public static string FaceEntry(string base64, Guid guid)
        {
            base64 = HttpUtility.HtmlDecode(base64);
            byte[] imageBytes = Convert.FromBase64String(base64);
            return FaceEntry(imageBytes, guid);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static string FaceEntry(byte[] imageBytes, Guid guid)
        {
            try
            {
                var bitmap = ToFormat24bpprgb(imageBytes);
                var array2d = bitmap.ToArray2D<RgbPixel>();
                var bytes = array2d.ToBytes();
                var res = string.Empty;
                using (var image1 = FaceRecognition.LoadImage(bytes, array2d.Rows, array2d.Columns, array2d.Columns * 3, Mode.Rgb))
                {
                    array2d.Dispose();
                    var encoding2 = _FaceRecognition.FaceEncodings(image1).ToArray()[0];
                    //FaceRecognition.LoadFaceEncoding(encoding2.GetRawEncoding());
                    if (encoding2 != null)
                    {

                        SaveFaceEncodes(imageBytes, guid.ToString());
                        //同步字典
                        _faceDictionary.Add(encoding2, guid);
                        //encoding2.Dispose();
                        res = string.Join(',', encoding2.GetRawEncoding());
                    }
                    else
                    {
                        res = "NO Face";
                    }
                }
                return res;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return "Error";
            }
        }

        /// <summary>
        /// 检测人脸
        /// </summary>
        /// <param name="base64"></param>
        /// <returns></returns>
        public static int FaceCheck(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                return 0;
            byte[] imageBytes = Convert.FromBase64String(base64);

            using (var faceDetector = DlibDotNet.Dlib.GetFrontalFaceDetector())
            //using (var ms = new MemoryStream(imageBytes))
            //using (var bitmap = (Bitmap)System.Drawing.Image.FromStream(ms))
            {
                var bitmap = ToFormat24bpprgb(imageBytes);
                using (var image = bitmap.ToArray2D<RgbPixel>())
                {
                    var dets = faceDetector.Operator(image);
                    //foreach (var r in dets)
                    //    DlibDotNet.Dlib.DrawRectangle(image, r, new RgbPixel { Green = 255 });
                    return dets.Length;
                }
            }
        }
        public static int LoadFaceEncodings(Dictionary<string, Guid> faceEncodes)
        {
            List<double> face;
            foreach (var item in faceEncodes)
            {
                face = new List<double>();
                item.Key.Split(',').ToList().ForEach(x => face.Add(Convert.ToDouble(x)));
                var key = FaceRecognition.LoadFaceEncoding(face.ToArray());
                if (!_faceDictionary.ContainsKey(key))
                { 
                    _faceDictionary.Add(key, item.Value);
                }
            }
            return _faceDictionary.Count;
        }
        /// <summary>
        /// 重新加载人脸模型
        /// </summary>
        /// <returns></returns>
        public static int ResetFaceEncodings(Dictionary<string, Guid> faceEncodes)
        {
            _faceDictionary.Clear();
            //_faceDictionary = GetFaceEncodesList();
            return LoadFaceEncodings(faceEncodes);
        }
        /// <summary>
        /// 释放
        /// </summary>
        public static void Dispose()
        {
            _FaceRecognition.Dispose();
        }
        /// <summary>
        /// 保存人脸模型
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="name"></param>
        //private static void SaveFaceEncodes(byte[] imageBytes, FaceEncoding encoding, string name)
        private static void SaveFaceEncodes(byte[] imageBytes, string name)
        {
            var dest = $"faces/{name}/";
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
                File.Create(Path.Combine(dest, name + ".txt"));
            }
            string[] files = Directory.GetFiles(dest);
            //imageBytes
            //string ts = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + encoding.Size;
            //格式 日期加上特征数
            //string datFilePath = $"{dest}/{ts}.dat";
            string jpgFilePath = $"{dest}/{name}.jpg";

            //var bf = new BinaryFormatter();
            //using (var fs = new FileStream(datFilePath, FileMode.OpenOrCreate))
            //{
            //    bf.Serialize(fs, encoding);
            //}
            File.WriteAllBytes(jpgFilePath, imageBytes);
        }
        ///// <summary>
        ///// 载入已入录人脸模型
        ///// </summary>
        ///// <returns></returns>
        //private static Dictionary<FaceEncoding, Guid> GetFaceEncodesList()
        //{
        //    Dictionary<FaceEncoding, string> dictionary = new Dictionary<FaceEncoding, string>();
        //    string path = @"faces";
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
        //    foreach (var file in files)
        //    {
        //        var name = Path.GetFileNameWithoutExtension(file);//取特征的人名
        //        var dirPath = Path.GetDirectoryName(file);
        //        string[] dats = Directory.GetFiles(dirPath, "*.dat", SearchOption.AllDirectories);
        //        foreach (var dat in dats)
        //        {
        //            var bf = new BinaryFormatter();
        //            using (var fs = new FileStream(dat, FileMode.Open))
        //            {
        //                FaceEncoding face = bf.Deserialize(fs) as FaceEncoding;
        //                dictionary.Add(face, name);
        //            }
        //        }
        //    }
        //    return dictionary;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        private static Bitmap ToFormat24bpprgb(byte[] bs)
        {
            MemoryStream ms = new MemoryStream(bs);
            System.Drawing.Image imgr = System.Drawing.Image.FromStream(ms);
            Bitmap bmp = new Bitmap(imgr.Width, imgr.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(imgr, new System.Drawing.Point(0, 0));
            g.Dispose();
            imgr.Dispose();
            ms.Dispose();
            return bmp;
        }
    }
}
