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
using System.Diagnostics;
using NHotkey;
using NHotkey.WindowsForms;
using Newtonsoft.Json;
using System.IO;

namespace WinStroke
{
    public partial class MainForm : Form
    {

        DataGridView dgv;
        List<DoStroke> strokeList = new List<DoStroke>();

        public MainForm()
        {
            InitializeComponent();

            registerStroke();

            this.menuMacro.Click += menuMacro_Click;
            this.menuStroke.Click += menuStroke_Click;
            this.menuExit.Click += menuExit_Click;

        }

        private void registerStroke()
        {
            String strdata = JsonConvert.DeserializeObject<String>(File.ReadAllText(@"c:\Users\dusta\Desktop\asdf.json"));
            List<Stroke.Stroke_Dataclass> ClassData = new List<Stroke.Stroke_Dataclass>();
            String[] substrings;

            char sp1 = ';';
            char sp2 = ',';

            if (strdata != null)
            {
                substrings = strdata.Split(sp1);
            }
            else { return; }

            foreach (String str in substrings)
            {
                DoStroke onerowstroke = new DoStroke();
                String[] _rowdata = str.Split(sp2);

                onerowstroke.set_pathspeed(_rowdata[1], Double.Parse(_rowdata[3]));
                HotkeyManager.Current.AddOrReplace(_rowdata[0], calculateShortcut(_rowdata[2]), onerowstroke.Do);
                strokeList.Add(onerowstroke);
            }
        }

        // ex. Keys.Shift | Keys.Control | Keys.Alt | Keys.Add
        Keys calculateShortcut(String strshortcut)
        {
            Keys key = 0;
            String[] splitshortcut = strshortcut.Split(' ');
            foreach(String str in splitshortcut)
            {
                key |= (Keys)Enum.Parse(typeof(Keys), str);
            }

            return key;
        }
        

        void menuMacro_Click(object sender, EventArgs e)
        {
            MacroForm macForm = new MacroForm();
            macForm.Show();
        }

        void menuStroke_Click(object sender, EventArgs e)
        {
            Stroke stroke = new Stroke();
            stroke.Show();
        }

        void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


    }
    
}
