using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STGBUNPACK_SHARP
{
    public partial class Form1 : Form
    {
        string sbdData;
        Encoding eucJp = Encoding.GetEncoding("shift_jis");
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread1;
            thread1 = new Thread(new ParameterizedThreadStart(unpack));
            thread1.Start(textBox1.Text);
        }

        public void unpack(object objpath)
        {
            string file_path = (string)objpath;
            SetText("未完成");
            ControlProgressZero(0);
            FileStream file = new FileStream(file_path, FileMode.Open);
            file.Seek(8, SeekOrigin.Begin);
            byte[] the_Byte_Buffer = new byte[4];
            byte[] the_Byte_Name_Buffer = new byte[0x10];
            file.Read(the_Byte_Buffer, 0, 4);
            int data_Count = BitConverter.ToInt32(the_Byte_Buffer, 0);
            Console.WriteLine(data_Count);
            SetControlProgressMax(data_Count);
            file.Seek(0x10, SeekOrigin.Begin);
            byte[] index_Data = new byte[0x40];
            sbdData = get_sbd_file_data(file);     //sbd文件的数据
            int sbd_file_data_ptr = 0;              //sbd文件的索引
            // 编码：将字符串转换为字节序列
            for (int i = 0; i < data_Count; i++)
            {
                int ptr = 0x10 + i * 0x40;
                file.Seek(ptr + 4, SeekOrigin.Begin);
                file.Read(the_Byte_Buffer, 0, 4);
                int file_Length = BitConverter.ToInt32(the_Byte_Buffer, 0);
                file.Seek(ptr + 12, SeekOrigin.Begin);
                file.Read(the_Byte_Buffer, 0, 4);
                int file_addr = BitConverter.ToInt32(the_Byte_Buffer, 0);
                file.Seek(ptr + 0x30, SeekOrigin.Begin);
                file.Read(the_Byte_Name_Buffer, 0, 0x10);
                string file_Name = eucJp.GetString(the_Byte_Name_Buffer);
                file_Name = search_in_sbdfile(file_Name, ref sbd_file_data_ptr);
                file.Seek(file_addr, SeekOrigin.Begin);
                byte[] file_Data = new byte[file_Length];
                file.Read(file_Data, 0, file_Length);
                save_file(file_Name, file_Data);
                ControlProgressAdd(0);
            }
            file.Close();
            SetText("完成");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.FileName;
            }
        }
        private void save_file(string file_path, byte[] file_Data)
        {
            if (file_path.EndsWith("\\") || file_path.EndsWith("/"))
            {
                file_path = file_path.Substring(0, file_path.Length - 1);
            }
            file_path = file_path.Replace("\0", "");
            //file_path = file_path.Replace(".bmp", ".bm");
            //file_path = file_path.Replace(".bm", ".bmp");
            //file_path = file_path.Replace(".png", ".pn");
            //file_path = file_path.Replace(".pn", ".png");
            //file_path = file_path.Replace(" ", "");
            SetText(file_path);
            file_path = Path.Combine(Path.GetDirectoryName(textBox1.Text),"UnpackData\\", file_path);
            string directory = Path.GetDirectoryName(file_path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            //Console.WriteLine("file_path:"+file_path);
            try
            {

                File.WriteAllBytes(file_path, file_Data);
            }
            catch
            {
                File.WriteAllBytes(file_path+"\\ErrorData.dat", file_Data);
            }
        }

        private string get_sbd_file_data(FileStream file)
        {
            int ptr = 0x10;
            byte[] the_Byte_Buffer = new byte[4];
            byte[] the_Byte_Name_Buffer = new byte[0x10];
            file.Seek(ptr + 4, SeekOrigin.Begin);
            file.Read(the_Byte_Buffer, 0, 4);
            int file_Length = BitConverter.ToInt32(the_Byte_Buffer, 0);
            file.Seek(ptr + 12, SeekOrigin.Begin);
            file.Read(the_Byte_Buffer, 0, 4);
            int file_addr = BitConverter.ToInt32(the_Byte_Buffer, 0);
            file.Seek(ptr + 0x30, SeekOrigin.Begin);
            file.Read(the_Byte_Name_Buffer, 0, 0x10);
            string file_Name = eucJp.GetString(the_Byte_Name_Buffer);
            file.Seek(file_addr, SeekOrigin.Begin);
            byte[] file_Data = new byte[file_Length];
            file.Read(file_Data, 0, file_Length);
            return eucJp.GetString(file_Data);
        }

        private string search_in_sbdfile(string head_data,ref int sbd_file_ptr)
        {
            string oriname = head_data;
            string file_full_name="";
            int index = head_data.IndexOf('.');
            bool pointDelete = false;
            if (index >= 0)
            {
                head_data = head_data.Substring(0, index);
                //Console.WriteLine(head_data); // 输出 "Hello"
            }
            head_data = head_data.Replace("\\", "\\\\");
            head_data = head_data.Replace("^", "\\^");
            head_data = head_data.Replace("@", "\\@");
            head_data = head_data.Replace("$", "\\$");
            head_data = head_data.Replace("(", "\\(");
            head_data = head_data.Replace(")", "\\)");
            head_data = head_data.Replace("*", "\\*");
            head_data = head_data.Replace("+", "\\+");
            head_data = head_data.Replace(".", "\\.");
            head_data = head_data.Replace("[", "\\[");
            head_data = head_data.Replace("]", "\\]");
            head_data = head_data.Replace("{", "\\{");
            head_data = head_data.Replace("}", "\\}");
            head_data = head_data.Replace("?", "");
            int point = head_data.IndexOf("・");
            if (head_data[head_data.Count()-1] == '・')
                if (point>0)
                {
                    pointDelete = true;
                    head_data = head_data.Remove(point, 1);
                }
            string pattern = (head_data+ @"+(.*?\.(bmp|png|mp3|ogg|wav|jpg|midi|sbd|gif|raw|aac|wma))");
            MatchCollection matches = Regex.Matches(sbdData, pattern, RegexOptions.IgnoreCase);
            if (matches.Count == 0)
            {
                Console.WriteLine("nomatch"+ oriname + pattern);
                return oriname;
            }

            foreach (Match match in matches)
            {

                if (match.Value.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                    continue;
                if (match.Index > sbd_file_ptr)
                {
                    file_full_name = match.Value;
                    sbd_file_ptr = match.Index;
                    break;
                }
                try
                {
                    if (match == matches[matches.Count - 1])
                        return oriname;
                }
                catch
                {
                    return oriname;
                }
            }

            if (pointDelete)
                if (point > 0)
                    file_full_name.Insert(point, "・");
            return file_full_name;
        }

        public void SetControlProgressMax(int value)
        {
            new Thread(() =>
            {
                Action<int> action = (data) =>
                {
                    progressBar1.Maximum = data;
                };
                Invoke(action, value);
            }).Start();
        }

        public void ControlProgressAdd(int dat)
        {
            new Thread(() =>
            {
                Action<int> action = (data) =>
                {
                    progressBar1.Value++;
                };
                Invoke(action,dat);
            }).Start();
        }

        public void ControlProgressZero(int dat)
        {
            new Thread(() =>
            {
                Action<int> action = (data) =>
                {
                    progressBar1.Value=0;
                };
                Invoke(action,dat);
            }).Start();
        }

        public void SetText(string str)
        {

            new Thread(() =>
            {
                Action<string> action = (data) =>
                {
                    label1.Text=data;
                };
                Invoke(action, str);
            }).Start();

        }
    }
}
