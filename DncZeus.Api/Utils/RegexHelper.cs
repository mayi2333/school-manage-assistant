using System;
using System.Text.RegularExpressions;

namespace DncZeus.Api.Utils
{
    /// <summary>
    /// 正则表达式公共类
    /// </summary>
    public class RegexHelper
    {
        #region 常见类型正则验证
        public enum Type
        {
            整数 = 0,
            正整数,
            负整数,
            数字,
            电话,
            正数,
            负数,
            浮点数,
            正浮点数,
            负浮点数,
            浮点数2,
            非负浮点数,
            非正浮点数,
            邮件,
            颜色,
            url,
            中文,
            ACSII字符,
            邮编,
            手机,
            IP地址,
            非空,
            图片,
            压缩文件,
            日期,
            QQ号码,
            字母,
            大写字母,
            小写字母,
            身份证,
            用户名=1000
        }
        private static string _pattern;
        /// <summary>
        /// 常见类型正则验证，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="t">模式类型</param>
        /// <returns>匹配返回true</returns>
        public static bool Check(string input, Type t)
        {
            switch (Convert.ToInt32(t))
            {
                case 0://整数
                    _pattern = @"^-?[1-9]\d*$";
                    break;
                case 1://正整数
                    _pattern = "^[0-9]*[1-9][0-9]*$";
                    break;
                case 2://负整数
                    _pattern = "^-[0-9]*[1-9][0-9]*$";
                    break;
                case 3://数字
                    _pattern = "^([+-]?)\\d*\\.?\\d+$";
                    break;
                case 4://电话
                    //在做项目时常常用到判断电话号码的正则表达式，写了一个，可验证如下27种格式：
                    //110
                    //8888888
                    //88888888
                    //8888888-123
                    //88888888-23435
                    //0871-8888888-123
                    //023-88888888-23435
                    //86-0871-8888888-123
                    //8888888_123
                    //88888888_23435
                    //0871_8888888_123
                    //023_88888888_23435
                    //86_0871_8888888_123
                    //8888888－123
                    //88888888－23435
                    //0871－8888888－123
                    //023－88888888－23435
                    //86－0871－8888888－123
                    //8888888—123
                    //88888888—23435
                    //0871—8888888—123
                    //023—88888888—23435
                    //86—0871—8888888—123
                    //13588888888
                    //15988888888
                    //013588888888
                    //015988888888
                    //(0315)7663551
                    _pattern = @"((^(\d{2,4}[-_－—]?)?\d{3,8}([-_－—]?\d{3,8})?([-_－—]?\d{1,7})?$)|(^0?1[35]\d{9}$)z)|(^(\([0-9]+\))?[0-9]{7,8}$)";
                    break;
                case 5://正数（正整数+ 0）
                    _pattern = @"^\d+$";
                    break;
                case 6://负数（负整数+ 0）
                    _pattern = @"^((-\d+)|(0+))$";
                    break;
                case 7://浮点数
                    _pattern = @"^(-?\d+)(\.\d+)?$";
                    break;
                case 8://正浮点数
                    _pattern = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
                    break;
                case 9://负浮点数
                    _pattern = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
                    break;
                case 10://浮点数
                    _pattern = "^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$";
                    break;
                case 11://非负浮点数（正浮点数+ 0）
                    _pattern = @"^\d+(\.\d+)?$";
                    break;
                case 12://非正浮点数（负浮点数+ 0）
                    _pattern = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
                    break;
                case 13://邮件                   //正确
                    _pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
                    break;
                case 14://颜色
                    _pattern = "^[a-fA-F0-9]{6}$";
                    break;
                case 15://url(http格式的)
                    _pattern = @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
                    break;
                case 16://仅中文
                    _pattern = @"[\u4e00-\u9fa5]";
                    break;
                case 17://仅ACSII字符
                    _pattern = "^[\\x00-\\xFF]+$";
                    break;
                case 18://邮编
                    _pattern = "^\\d{6}$";
                    break;
                case 19://手机(13号段和号段)
                    _pattern = "^13[0-9]{9}$";
                    break;
                case 20://ip地址
                    _pattern = @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$";
                    break;
                case 21://非空
                    _pattern = "^\\S+$";
                    break;
                case 22://图片
                    _pattern = @"(.*)\.(jpg|gif|png|bmp)$";
                    break;
                case 23://压缩文件
                    _pattern = "(.*)\\.(rar|zip|7zip|tgz)$";
                    break;
                case 24://日期
                    //这个日期正则表达式支持
                    //YYYY-MM-DD
                    //YYYY/MM/DD
                    //YYYY_MM_DD
                    //YYYY.MM.DD的形式
                    _pattern = @"((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))";
                    break;
                case 25://QQ号码
                    _pattern = "[1-9][0-9]{4,}";
                    break;
                case 26://字母
                    _pattern = "^[A-Za-z]+$";
                    break;
                case 27://大写字母
                    _pattern = "^[A-Z]+$";
                    break;
                case 28://小写字母
                    _pattern = "^[a-z]+$";
                    break;
                case 29://身份证
                    _pattern = @"^[1-9]([0-9]{16}|[0-9]{13})[xX0-9]$";
                    break;
                case 1000://用来用户注册。匹配由数字、个英文字母或者下划线组成的字符串
                    _pattern = "^(?!\\d)[a-zA-Z0-9_\\u4e00-\\u9fa5]+$";
                    break;
                default:
                    _pattern = string.Empty;
                    break;
            }
            return IsMatch(input, _pattern);
        }
        #endregion
        #region 验证输入字符串是否与模式字符串匹配
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion
    }
}
