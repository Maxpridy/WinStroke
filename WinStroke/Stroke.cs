using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;




namespace WinStroke
{
    

    public partial class Stroke : Form
    {
        /*
        //핫키등록
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(int hwnd, int id, int fsModifiers, int vk);

        //핫키제거
        [DllImport("user32.dll")]
        private static extern int UnregisterHotKey(int hwnd, int id);
        */

        public Stroke()
        {

            InitializeComponent();
            Stroke_autoLoad();
            
            /*
            RegisterHotKey((int)this.Handle, 0, 0x0, (int)Keys.F5); //(여기로가져와, 니 ID는 0이야, 조합키안써, F5눌러지면)
                                                                    // 0x1은 알트, 0x2는 콘트롤, 0x3은 쉬프트
            RegisterHotKey((int)this.Handle, 1, 0x0, (int)Keys.F6);
            */

        }

        private void Stroke_autoSave()
        {
            //File.WriteAllText(@"c:\movie1.json", JsonConvert.SerializeObject(Macro_Macrodata));

            using (StreamWriter file = File.CreateText(@"c:\Users\dusta\Desktop\asdf.json"))
            {

                JsonSerializer serializer = new JsonSerializer();
                String strarr = "";
                //Stroke_Data

                foreach (DataGridViewRow row in Stroke_Data.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (null == cell.Value) { }
                        else {
                            strarr += cell.Value.ToString();
                            if (cell.ColumnIndex != 3)
                                strarr += ",";
                        }

                    }
                    strarr += ";";
                }
                strarr = strarr.Substring(0, strarr.Length - 1);
                serializer.Serialize(file, strarr);
            }
        }




        public class Stroke_Dataclass
        {
            private String callName;
            private String filePath;
            private String shortcut;
            private double speed;

            public String CallName { get { return callName; } set { callName = value; } }
            public String FilePath { get { return filePath; } set { filePath = value; } }
            public String Shortcut { get { return shortcut; } set { shortcut = value; } }
            public double Speed { get { return speed; } set { speed = value; } }

            public Stroke_Dataclass(String _callName, String _FilePath, String _path, double _speed)
            {
                callName = _callName;
                FilePath = _FilePath;
                shortcut = _path;
                speed = _speed;
            }
        }
       
        private void Stroke_autoLoad()
        {
            String strdata = JsonConvert.DeserializeObject<String>(File.ReadAllText(@"c:\Users\dusta\Desktop\asdf.json"));
            List<Stroke_Dataclass> ClassData = new List<Stroke_Dataclass>();

            char sp1 = ';';
            char sp2 = ',';

            String[] substrings;
            if (strdata != null)
            {
                substrings = strdata.Split(sp1);
            }
            else { return; }

            foreach (String str in substrings)
            {
                String[] _rowdata = str.Split(sp2);
                try
                {
                    ClassData.Add(new Stroke_Dataclass(_rowdata[0], _rowdata[1], _rowdata[2], Double.Parse(_rowdata[3])));
                }
                catch (Exception) {  }
            }

            Stroke_Dataclass[] finalData = ClassData.ToArray();
            Stroke_Data.DataSource = finalData;
        }
        

        /*
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey((int)this.Handle, 0); //이 폼에 ID가 0인 핫키 해제
            UnregisterHotKey((int)this.Handle, 1);
        }
        */

        /*
        protected override void WndProc(ref Message m) //윈도우프로시저 콜백함수
        {
            base.WndProc(ref m);

            if (m.Msg == (int)0x312) //핫키가 눌러지면 312 정수 메세지를 받게됨
            {
                int id = m.WParam.ToInt32();
                if (id == 0x0) // 그 키의 ID가 0이면
                {
                    //시작버튼.PerformClick();
                    MessageBox.Show("asdf");
                }
                if (id == 0x1) // 그 키의 ID가 1이면
                {
                    MessageBox.Show("asdfasdf");
                }
            }
        }
        */



        /*
        public abstract class HotkeyManagerBase
        {
            private IntPtr _hwnd;
            internal static readonly IntPtr HwndMessage = (IntPtr)(-3);

            internal HotkeyManagerBase()
            {
            }

            internal void SetHwnd(IntPtr hwnd)
            {
                _hwnd = hwnd;
            }

            private const int WmHotkey = 0x0312;
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design",
        "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "This is a singleton; disposing it would break it")]
        public class HotkeyManager : HotkeyManagerBase
        {
            #region Singleton implementation

            public static HotkeyManager Current { get { return LazyInitializer.Instance; } }

            private static class LazyInitializer
            {
                static LazyInitializer() { }
                public static readonly HotkeyManager Instance = new HotkeyManager();
            }



            #endregion

            // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
            private readonly MessageWindow _messageWindow;

            public void Run()
            {
                RegisterHotKey((int)_messageWindow.Handle, 0, 0x0, (int)Keys.F5); //(여기로가져와, 니 ID는 0이야, 조합키안써, F5눌러지면)
                                                                                  // 0x1은 알트, 0x2는 콘트롤, 0x3은 쉬프트
                RegisterHotKey((int)_messageWindow.Handle, 1, 0x0, (int)Keys.F6);
            }

            private HotkeyManager()
            {
                _messageWindow = new MessageWindow(this);
                SetHwnd(_messageWindow.Handle);
            }

            class MessageWindow : ContainerControl
            {
                private readonly HotkeyManager _hotkeyManager;

                public MessageWindow(HotkeyManager hotkeyManager)
                {
                    _hotkeyManager = hotkeyManager;
                }

                protected override CreateParams CreateParams
                {
                    get
                    {
                        var parameters = base.CreateParams;
                        parameters.Parent = HwndMessage;
                        return parameters;
                    }
                }

                protected override void WndProc(ref Message m)
                {
                    base.WndProc(ref m);

                    if (m.Msg == (int)0x312) //핫키가 눌러지면 312 정수 메세지를 받게됨
                    {
                        int id = m.WParam.ToInt32();
                        if (id == 0x0) // 그 키의 ID가 0이면
                        {
                            //시작버튼.PerformClick();
                            MessageBox.Show("asdf");
                        }
                        if (id == 0x1) // 그 키의 ID가 1이면
                        {
                            MessageBox.Show("asdfasdf");
                        }
                    }
                }
            }
        }
        */

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Stroke_Load(object sender, EventArgs e)
        {

        }

        private void Stroke_Close(object sender, FormClosingEventArgs e)
        {
            try
            {
                Stroke_autoSave();
            }
            catch(Exception)
            {
                
            }
            Dispose();
        }
    }
}
