using Newtonsoft.Json;
using NHotkey;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinStroke
{
    class DoStroke
    {
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte vk, byte scan, int flags, ref int extrainfo);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        string path;
        double speed;

        public void set_pathspeed(string _path, double _speed)
        {
            this.path = _path;
            this.speed = _speed;
        }

        // 일단 한줄한줄 불러오면서 실행시키는걸로 해보자.

        public void Do(object sender, HotkeyEventArgs e)
        {

            e.Handled = true;

            /*
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = @"C:\";
            openFile.Title = "Browse Json Files";

            openFile.CheckFileExists = true;
            openFile.CheckPathExists = true;

            openFile.DefaultExt = "json";
            openFile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFile.FilterIndex = 2;
            openFile.RestoreDirectory = true;

            openFile.ReadOnlyChecked = true;
            openFile.ShowReadOnly = true;
            */
            

            //if (openFile.ShowDialog() == DialogResult.OK)
            //{
            //String strdata = JsonConvert.DeserializeObject<String>(File.ReadAllText(openFile.FileName));
            String strdata = JsonConvert.DeserializeObject<String>(File.ReadAllText(path));

            char sp1 = ';';
            char sp2 = '.';

            String[] substrings = strdata.Split(sp1);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (String str in substrings)
            {
                String[] _rowdata = str.Split(sp2);

                AwaitTime(sw, Int32.Parse(_rowdata[0]));

                Keyboard_Play(_rowdata);
                Mouse_Play(_rowdata);
            }
            //}

            


        }




        private void AwaitTime(Stopwatch sw, int _listTime)
        {
            for (;;)
            {

                if (Int32.Parse(sw.ElapsedMilliseconds.ToString())  *  speed >= _listTime)
                {
                    break;
                }
            }
        }

        private void Keyboard_Play(String[] _row)
        {
            int info = 0;
            if (_row[1].Substring(0, 3) == "키보드")
            {
                // 데이터를 받아서 실행한다.
                if (_row[1] == "키보드 누름")
                {
                    keybd_event((byte)Int32.Parse(_row[2]), 0, 0, ref info);
                    //keybd_event((byte)Int32.Parse(Macro_Macrodata.Items[_line].SubItems[2].Text), 0, 0, ref info);
                }

                if (_row[1] == "키보드 뗌")
                {
                    keybd_event((byte)Int32.Parse(_row[2]), 0, 2, ref info);
                }

            }
        }

        private void Mouse_Play(String[] _row)
        {
            double Absol = 65535; // 65535 권장
            double _Width = Screen.PrimaryScreen.Bounds.Width;
            double _Height = Screen.PrimaryScreen.Bounds.Height;

            if (_row[1].Substring(0, 3) == "마우스")
            {
                // 이동
                string coord = _row[2];
                string[] coordxy = coord.Split(' ');
                double coord_x = Absol / _Width * Double.Parse(coordxy[0]);
                double coord_y = Absol / _Height * Double.Parse(coordxy[1]);

                if (_row[1] == "마우스 이동")
                {
                    mouse_event(0x8000 | 0x0001, (int)coord_x, (int)coord_y, 0, IntPtr.Zero);
                }

                // 좌클릭
                else if (_row[1] == "마우스 왼쪽 버튼 누름")
                {
                    mouse_event(0x8000 | 0x0002, (int)coord_x, (int)coord_y, 0, IntPtr.Zero);
                }

                else if (_row[1] == "마우스 왼쪽 버튼 뗌")
                {
                    mouse_event(0x8000 | 0x0004, (int)coord_x, (int)coord_y, 0, IntPtr.Zero);
                }

                // 우클릭
                else if (_row[1] == "마우스 오른쪽 버튼 누름")
                {
                    mouse_event(0x8000 | 0x0008, (int)coord_x, (int)coord_y, 0, IntPtr.Zero);
                }

                else if (_row[1] == "마우스 오른쪽 버튼 뗌")
                {
                    mouse_event(0x8000 | 0x0010, (int)coord_x, (int)coord_y, 0, IntPtr.Zero);
                }
            }
        }



    }
}
