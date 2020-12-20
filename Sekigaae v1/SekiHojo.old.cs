/*
CopyRight Kitachisuku
*/

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace Sekigaae
{
    class SEntry
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new SGui());
        }
    }
    class SGui : Form
    {
        public static SB[] sekis = new SB[36];
        private MenuStrip ms;
        public static bool cm = false;
        private HistoryDialog hd;
        public static ChangeDialog cd;
        private SaveAndDialog sd;
        private PrintViewDialog pd;
        public SGui()
        {
            InitComponents();
        }
        private void InitComponents()
        {
            sd = new SaveAndDialog();
            cd = new ChangeDialog(false);
            hd = new HistoryDialog();
            pd = new PrintViewDialog();
            this.Text = "席替え補助ソフト";
            this.Size = new Size(1400,800);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.MaximizeBox = false;
            SetMenu();
            Size SSIZE = new Size(90,90);
            //  false = man,true = woman
            bool seibetu = false;
            for(int i = 29;i < 36;i++)
            {
                if(i == 35)
                {
                    sekis[i] = new SB(36,10,1000,true);
                    this.Controls.Add(sekis[i]);
                    break;
                }
                if((i-29)%2 != 0) seibetu = false;
                else seibetu = true;
                sekis[i] = new SB(30 + (i - 29),10,10 + (i - 29) * 100,seibetu);
                this.Controls.Add(sekis[i]);
            }

            for(int i = 23;i < 29;i++)
            {
                if((i-29)%2 != 0) seibetu = false;
                else seibetu = true;
                sekis[i] = new SB(24 + (i - 23),110,10 + (i - 23) * 100,seibetu);
                this.Controls.Add(sekis[i]);
            }

            for(int i = 17;i < 23;i++)
            {
                if((i-29)%2 != 0) seibetu = false;
                else seibetu = true;
                sekis[i] = new SB(18 + (i - 17),250,10 + (i - 17) * 100,seibetu);
                this.Controls.Add(sekis[i]);
            }

            for(int i = 11;i < 17;i++)
            {
                if((i-29)%2 != 0) seibetu = false;
                else seibetu = true;
                sekis[i] = new SB(12 + (i - 11),350,10 + (i - 11) * 100,seibetu);
                this.Controls.Add(sekis[i]);
            }

            for(int i = 5;i < 11;i++)
            {
                if((i-29)%2 != 0) seibetu = false;
                else seibetu = true;
                sekis[i] = new SB(6 + (i - 5),490,10 + (i - 5) * 100,seibetu);
                this.Controls.Add(sekis[i]);
            }

            for(int i = 0;i < 5;i++)
            {
                if((i-29)%2 != 0) seibetu = false;
                else seibetu = true;
                sekis[i] = new SB(1 + i,590,10 + i * 100,seibetu);
                this.Controls.Add(sekis[i]);
            }
        }
        private void SetMenu()
        {
            ms = new MenuStrip();
            ToolStripMenuItem  files = new ToolStripMenuItem()
            {
                Text = "ファイル(&F)"
            };
            ToolStripMenuItem open = new ToolStripMenuItem()
            {
                Text = "開く(&O)..."
            };
            ToolStripMenuItem export = new ToolStripMenuItem()
            {
                Text = "設定をファイルに保存(&E)..."
            };
            ToolStripMenuItem historys = new ToolStripMenuItem()
            {
                Text = "履歴(&H)"
            };
            ToolStripMenuItem hopen = new ToolStripMenuItem()
            {
                Text = "履歴を開く(&O)..."
            };
            ToolStripMenuItem hsaveandname = new ToolStripMenuItem()
            {
                Text = "名前を付けて席の履歴を保存(&N)..."
            };
            ToolStripMenuItem hsave = new ToolStripMenuItem()
            {
                Text = "席の履歴を保存(&S)"
            };
            ToolStripMenuItem exit = new ToolStripMenuItem()
            {
                Text = "終了(&E)"
            };
            ToolStripMenuItem modes = new ToolStripMenuItem()
            {
                Text = "モード(&M)"
            };
            ToolStripMenuItem change = new ToolStripMenuItem()
            {
                Text = "席入れ替えモード(&C)"
            };
            ToolStripMenuItem printing = new ToolStripMenuItem()
            {
                Text = "印刷(&P)..."
            };
            open.Click += OpenFiles;
            export.Click += ExportDataInFile;
            hsave.Click += SaveHistory;
            hsaveandname.Click += SaveHistoryAndName;
            hopen.Click += OpenHistory;
            change.Click += ChangeMode;
            printing.Click += Printing;
            modes.DropDownItems.Add(change);
            historys.DropDownItems.Add(hopen);
            historys.DropDownItems.Add(hsaveandname);
            historys.DropDownItems.Add(hsave);
            exit.Click += (object sender,EventArgs e) => {Application.Exit();};
            files.DropDownItems.Add(open);
            files.DropDownItems.Add(export);
            files.DropDownItems.Add(historys);
            files.DropDownItems.Add(printing);
            files.DropDownItems.Add(exit);
            ms.Items.Add(files);
            ms.Items.Add(modes);
            ms.BackColor = Color.White;
            this.Controls.Add(ms);
            this.MainMenuStrip = ms;
        }
        private void Printing(object sender,EventArgs e)
        {
            pd.VisibleDialog();
        }
        private void OpenHistory(object sender,EventArgs e)
        {
            //  履歴を開く処理
            hd.VisibleDialog();
        }
        private void SaveHistoryAndName(object sender,EventArgs e)
        {
            sd.ShowDialog();
        }
        private void ChangeMode(object sender,EventArgs e)
        {
            var ts = (ToolStripMenuItem)sender;
            Selections.num1 = -1;
            Selections.num2 = -1;
            if(ts.Checked)
            {
                ts.Checked = false;
                this.BackColor = Color.White;
                cm = false;
            }else
            {
                ts.Checked = true;
                this.BackColor = Color.FromArgb(235,235,235);
                cm = true;
            }
        }
        private void ExportDataInFile(object sender,EventArgs e)
        {
            using(SaveFileDialog fd = new SaveFileDialog())
            {
                fd.Title = "設定をファイルに保存";
                fd.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
                fd.RestoreDirectory = true;
                if(fd.ShowDialog() == DialogResult.OK)
                {
                    string str = "";
                    for (int i = 0; i < 35; i++)
                    {
                        str += sekis[i].Text;
                        if(i != 34) 
                        {
                            if(sekis[i].seibetu)
                            {
                                str += "@1\n";
                            }else
                            {
                                str += "@0\n";
                            }
                        }
                    }
                    
                    File.WriteAllText(fd.FileName,str);
                }
            }
        }
        private void OpenFiles(object sender,EventArgs e)
        {
            using(OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Title = "開く";
                fd.Filter = "テキストファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
                fd.RestoreDirectory = true;
                if(fd.ShowDialog() == DialogResult.OK)
                {
                    OpenFile(fd.FileName);
                }
            }
        }
        public static void OpenFile(string filename)
        {
            try{
                string[] str = File.ReadAllLines(filename);
                for (int i = 0; i < 35; i++)
                {
                    if(str[i].Contains("@0"))
                    {
                        sekis[i].seibetu = false;
                        str[i] = str[i].Replace("@0","");
                    }
                    else
                    {
                        sekis[i].seibetu = true;
                        str[i] = str[i].Replace("@1","");
                    }
                    sekis[i].Text = str[i];
                }
            }catch(Exception)
            {
                return;
            }
        }
        private void SaveHistory(object sender,EventArgs e)
        {
            try
            {
                int historys = Directory.GetFiles(@"data\historys","*",SearchOption.AllDirectories).Length + 1;
                string str = "";
                for (int i = 0; i < 35; i++)
                {
                    str += sekis[i].Text;
                    if(i != 34) 
                    {
                        if(sekis[i].seibetu)
                        {
                            str += "@1\n";
                        }else
                        {
                            str += "@0\n";
                        }
                    }
                }
                File.WriteAllText(@"data\historys\history" + historys.ToString() + ".txt",str);
            }catch(System.Exception)
            {
                MessageBox.Show("保存に失敗しました。");
                return;
            }
            MessageBox.Show("保存しました。");
        }
    }
    class SB : Button
    {
        private int id = 0;
        public bool seibetu;
        public SB(int def,int xdf,int ldf,bool getseibetu)
        {
            seibetu = getseibetu;
            id = def;
            this.Size = new Size(90,90);
            this.Text = def.ToString() + "番";
            this.Location = new Point(xdf,ldf + 20);
            this.Click += ChangeSekiName;
            this.FlatStyle = FlatStyle.System;
            if(seibetu) this.ForeColor = Color.Red;
            else this.ForeColor = Color.Blue;
        }
        private void ChangeSekiName(object sender,EventArgs e)
        {
            if(SGui.cm)
            {
                if(Selections.num1 == -1)
                {
                    Selections.num1 = this.id - 1;
                }else if(Selections.num2 == -1)
                {
                    Selections.num2 = this.id;
                    string text = this.Text;
                    bool seib = this.seibetu;
                    this.Text = SGui.sekis[Selections.num1].Text;
                    this.seibetu = SGui.sekis[Selections.num1].seibetu;
                    SGui.sekis[Selections.num1].Text = text;
                    SGui.sekis[Selections.num1].seibetu = seib;
                    Selections.num1 = -1;
                    Selections.num2 = -1;
                }
                return;
            }
            if(SGui.sekis[id] != null)
            {
                SGui.cd.VisibleDialog(id,seibetu);
            }else
            {
                MessageBox.Show("予期しないエラーが発生しました。\n再度お試しください。");
            }
        }
    }
    class ChangeDialog : Form
    {
        private int id;
        private TextBox tx;
        private Button b,b2;
        private bool seibetu;
        private CheckBox c,c2;
        public ChangeDialog(bool getseibetu)
        {
            seibetu = getseibetu;
            this.Text = "席にいる人の名前を変更";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Size = new Size(280,180);
            this.StartPosition = FormStartPosition.CenterParent;
            b = new Button();
            b.Text = "変更";
            b.Location = new Point(30,50);
            b.Click += ChangeSeki;
            b.AutoSize = true;
            b2 = new Button();
            b2.Text = "キャンセル";
            b2.Location = new Point(130,50);
            b2.Click += (object sender,EventArgs e) =>
            {
                this.Close();
            };
            b.AutoSize = true;
            tx = new TextBox();
            tx.Size = new Size(160,12);
            tx.Location = new Point(30,10);
            tx.KeyDown += (object sender,KeyEventArgs e) =>
            {
                if(e.KeyData == Keys.Enter)
                {
                    b.PerformClick();
                }
            };
            tx.MouseDown += (object sender,MouseEventArgs e) =>
            {
                tx.SelectAll();
            };
            c = new CheckBox();
            c2 = new CheckBox();
            c.Location = new Point(220,10);
            c2.Location = new Point(220,100);
            c.Text = "男";
            c2.Text = "女";
            c.Click += (object sender,EventArgs e) =>
            {
                if(c2.Checked)
                {
                    c.Checked = true;
                    c2.Checked = false;
                    this.seibetu = false;
                }else
                {
                    c.Checked = false;
                    c2.Checked = true;
                    this.seibetu = true;
                }
            };
            c2.Click += (object sender,EventArgs e) =>
            {
                if(!c2.Checked)
                {
                    c.Checked = true;
                    c2.Checked = false;
                    this.seibetu = false;
                }else
                {
                    c.Checked = false;
                    c2.Checked = true;
                    this.seibetu = true;
                }
            };
            c.AutoSize = c2.AutoSize = true;
            this.Controls.Add(tx);
            this.Controls.Add(c);
            this.Controls.Add(c2);
            this.Controls.Add(b);
            this.Controls.Add(b2);
        }
        private void ChangeSeki(object sender,EventArgs e)
        {
            if(tx.Text != "")
            {
                SGui.sekis[id - 1].Text = tx.Text;
                SGui.sekis[id - 1].seibetu = this.seibetu;
            }
            this.Close();
        }
        public void VisibleDialog(int getid,bool getseibetu)
        {
            this.id = getid;
            tx.Text = SGui.sekis[id - 1].Text;
            seibetu = getseibetu;
            if(seibetu)
            {
                c.Checked = false;
                c2.Checked = true;
            }else
            {
                c.Checked = true;
                c2.Checked = false;
            }
            this.ShowDialog();
        }
    }
    public class Selections
    {
        public static int num1 = -1; //１つ目のボタンのID
        public static int num2 = -1; //2つ目のボタンのID
    }
    class HistoryDialog : Form
    {
        private Button b,b2;
        private ComboBox cb;
        public HistoryDialog()
        {
            this.Text = "履歴を開く";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(450,230);
            this.StartPosition = FormStartPosition.CenterParent;
            b = new Button();
            b.Text = "開く";
            b.AutoSize = true;
            b.Location = new Point(50,50);
            b2 = new Button();
            b2.Text = "キャンセル";
            b2.AutoSize = true;
            b2.Location = new Point(50,120);
            cb = new ComboBox();
            cb.Location = new Point(200,70);
            cb.Size = new Size(150,25);
            cb.AutoSize = true;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            AddComboFiles();
            b.Click += (object sender,EventArgs e) =>
            {
                SGui.OpenFile(@"data\historys\" + cb.SelectedItem.ToString());
                this.Close();
            };
            b2.Click += (object sender,EventArgs e) =>
            {
                this.Close();
            };
            this.Controls.Add(b);
            this.Controls.Add(b2);
            this.Controls.Add(cb);
        }
        private void AddComboFiles()
        {
            cb.Items.Clear();
            DirectoryInfo di = new DirectoryInfo(@"data\historys");
            cb.Items.AddRange(di.GetFiles("*.txt"));
            int l = di.GetFiles("*.txt").Length;
            if(l != 0) cb.SelectedIndex = 0;
            else
            {
                cb.Items.Add("履歴がありません");
                cb.SelectedIndex = 0;
            }
            di = null;
        }
        public void VisibleDialog()
        {
            this.ShowDialog();
            AddComboFiles();
        }
    }
    class SaveAndDialog : Form
    {
        private TextBox tx;
        private Button bt,bt2;
        public SaveAndDialog()
        {
            this.Text = "名前を付けて席の履歴を保存";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Size = new Size(450,230);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Paint += (object sender,PaintEventArgs e) =>
            {
                e.Graphics.DrawString("名前",new Font("Consoles",12),Brushes.Black,65,30);
            };
            tx = new TextBox();
            tx.Location = new Point(120,30);
            tx.Size = new Size(160,25);
            tx.WordWrap = false;
            bt = new Button();
            bt2 = new Button();
            bt.Location = new Point(50,100);
            bt2.Location = new Point(270,100);
            bt.Click += (object sender,EventArgs e) =>
            {
                try
                {
                    if(tx.Text == "")
                    {
                        MessageBox.Show("ファイル名を入力してください。");
                        return;
                    }
                    if(File.Exists(@"data\historys\" + tx.Text + ".txt")) throw new Exception("既に同名のファイルが存在しています。");
                    string str = "";
                    for (int i = 0; i < 35; i++)
                    {
                        str += SGui.sekis[i].Text;
                        if(i != 34) 
                        {
                            if(SGui.sekis[i].seibetu)
                            {
                                str += "@1\n";
                            }else
                            {
                                str += "@0\n";
                            }
                        }
                    }
                    File.WriteAllText(@"data\historys\" + tx.Text + ".txt",str);   
                }
                catch (Exception exc)
                {
                    if(exc.ToString().Contains("既に同名のファイルが存在しています。"))
                    {
                        MessageBox.Show("履歴" + tx.Text + "の保存に失敗しました。\n同名のファイルが既に存在しています。");
                    this.Close();
                        return;
                    }
                    MessageBox.Show("履歴" + tx.Text + "の保存に失敗しました。");
                    this.Close();
                    return;
                }
                MessageBox.Show("履歴" + tx.Text + "を保存しました");
                this.Close();
            };
            bt2.Click += (object sender,EventArgs e) =>
            {
                this.Close();
            };
            bt.Text = "保存";
            bt2.Text = "キャンセル";
            bt.AutoSize = bt2.AutoSize = true;
            this.Controls.Add(tx);
            this.Controls.Add(bt);
            this.Controls.Add(bt2);
        }
    }
    class PrintViewDialog : Form
    {
        private Button b,b2;
        private Panel p;
        public PrintViewDialog()
        {
            this.Text = "印刷";
            this.Size = new Size(850,790);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            p = new Panel();
            p.Location = new Point(10,10);
            p.Size = new Size(800,700);
            p.BackColor = Color.White;
            p.AutoScroll = true;
            p.Paint += (object sender,PaintEventArgs e) =>
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString("座席表",new Font("Consoles",32),Brushes.Black,320,10);
                for(int i = 29;i < 35;i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black,60,100 + (i - 29) * 100,90,90);
                    if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(65,135 + (i - 29) * 100,85,90),sf);
                    else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(65,135 + (i - 29) * 100,85,90),sf);
                }
                for(int i = 23;i < 29;i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black,160,100 + (i - 23) * 100,90,90);
                    if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(165,135 + (i - 23) * 100,85,90),sf);
                    else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(165,135 + (i - 23) * 100,85,90),sf);
                }

                for(int i = 17;i < 23;i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black,310,100 + (i - 17) * 100,90,90);
                    if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(315,135 + (i - 17) * 100,85,90),sf);
                    else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(315,135 + (i - 17) * 100,85,90),sf);
                }

                for(int i = 11;i < 17;i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black,410,100 + (i - 11) * 100,90,90);
                    if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(410,135 + (i - 11) * 100,85,90),sf);
                    else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(410,135 + (i - 11) * 100,85,90),sf);
                }

                for(int i = 5;i < 11;i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black,550,100 + (i - 5) * 100,90,90);
                    if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(555,135 + (i - 5) * 100,85,90),sf);
                    else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(555,135 + (i - 5) * 100,85,90),sf);
                }

                for(int i = 0;i < 5;i++)
                {
                    e.Graphics.DrawRectangle(Pens.Black,650,100 + i * 100,90,90);
                    if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(655,135 + i * 100,85,90),sf);
                    else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(655,135 + i * 100,85,90),sf);
                }
                sf = null;
            };
            b2 = new Button();
            b = new Button();
            b.Location = new Point(50,720);
            b.AutoSize = true;
            b.Text = "印刷";
            b.Click += (object sender2,EventArgs e2) =>
            {
                PrintDocument pdd = new PrintDocument();
                pdd.PrintPage += (object sender,PrintPageEventArgs e) =>
                {
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    e.Graphics.DrawString("座席表",new Font("Consoles",32),Brushes.Black,320,10);
                    for(int i = 29;i < 35;i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black,60,100 + (i - 29) * 100,90,90);
                        if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(65,135 + (i - 29) * 100,85,90),sf);
                        else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(65,135 + (i - 29) * 100,85,90),sf);
                    }
                    for(int i = 23;i < 29;i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black,160,100 + (i - 23) * 100,90,90);
                        if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(165,135 + (i - 23) * 100,85,90),sf);
                        else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(165,135 + (i - 23) * 100,85,90),sf);
                    }

                    for(int i = 17;i < 23;i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black,310,100 + (i - 17) * 100,90,90);
                        if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(315,135 + (i - 17) * 100,85,90),sf);
                        else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(315,135 + (i - 17) * 100,85,90),sf);
                    }

                    for(int i = 11;i < 17;i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black,410,100 + (i - 11) * 100,90,90);
                        if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(410,135 + (i - 11) * 100,85,90),sf);
                        else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(410,135 + (i - 11) * 100,85,90),sf);
                    }

                    for(int i = 5;i < 11;i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black,550,100 + (i - 5) * 100,90,90);
                        if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(555,135 + (i - 5) * 100,85,90),sf);
                        else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(555,135 + (i - 5) * 100,85,90),sf);
                    }

                    for(int i = 0;i < 5;i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black,650,100 + i * 100,90,90);
                        if(SGui.sekis[i].seibetu) e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Red,new RectangleF(655,135 + i * 100,85,90),sf);
                        else e.Graphics.DrawString(SGui.sekis[i].Text,new Font("Consoles",10),Brushes.Blue,new RectangleF(655,135 + i * 100,85,90),sf);
                    }
                    sf = null;
                };
                PrintDialog pdlg = new PrintDialog();
                pdlg.Document = pdd;
                if(pdlg.ShowDialog() == DialogResult.OK)
                {
                    pdd.Print();
                    this.Close();
                }
            };
            b2 = new Button();
            b2.Location = new Point(630,720);
            b2.AutoSize = true;
            b2.Text = "キャンセル";
            b2.Click += (object sender,EventArgs e) =>
            {
                this.Close();
            };
            this.Controls.Add(b);
            this.Controls.Add(b2);
            this.Controls.Add(p);
        }
        public void VisibleDialog()
        {
            this.ShowDialog();
        }
    }
}