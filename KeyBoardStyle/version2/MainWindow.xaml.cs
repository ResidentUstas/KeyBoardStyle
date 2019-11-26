using System;
using System.Collections.Generic;
using System.Windows;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;

namespace KeyBoardStyle
{
    /// <summary> 
    /// Логика взаимодействия для MainWindow.xaml 
    /// </summary> 
    /// 
    [Serializable]
    public class User
    {
        public string fio;
        public int id;
        public string password;
        public List<List<long>> keyinf = new List<List<long>>();
    }

    public class tabl
    {
        public char key { get; set; }
        public double time { get; set; }
        public double interval { get; set; }
    }
    public partial class MainWindow : Window
    {
        HashSet<int> checkID = new HashSet<int>();
        List<User> lst = new List<User>();
        Stopwatch timer = new Stopwatch();
        Queue<long> pressup = new Queue<long>();
        Queue<long> pressb = new Queue<long>();
        List<long> updown = new List<long>();
        List<long> between = new List<long>();
        List<double> checklist = new List<double>();
        bool fl = true;
        bool fbk = true;
        long millisec = 0;
        int ch = 1;
        int psw1 = 0;
        bool cont = true;
        public delegate void Entcl(object sender, RoutedEventArgs e);
        public event Entcl Added;
        public MainWindow()
        {
            InitializeComponent();
            timer.Start();
            deserealize();
        }

        public void deserealize()
        {
            var formatter = new XmlSerializer(typeof(List<User>));
            try
            {
                using (FileStream fs = new FileStream(@"E:\KeyBoardStyle\bin\Debug\text.xml", FileMode.Open))
                {
                    lst = (List<User>)formatter.Deserialize(fs);
                }
            }
            catch (Exception exs)
            {
                MessageBox.Show(exs.Message);
            }
            var form = new XmlSerializer(typeof(HashSet<int>));
            try
            {
                using (FileStream fs1 = new FileStream(@"E:\KeyBoardStyle\bin\Debug\text1.xml", FileMode.Open))
                {
                    checkID = (HashSet<int>)form.Deserialize(fs1);
                }
            }
            catch (Exception exs)
            {
                MessageBox.Show(exs.Message);
            }
        }
        private void Entered_Click(object sender, RoutedEventArgs e)//вход 
        {
            Fild.KeyDown += getst;
            Fild.KeyUp += getstat;
            Added += getres_Click;
            if (lst.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы одного пользователя");
            }
            else
            {
                updown.Clear();
                between.Clear();
                headpiece.Visibility = Visibility.Hidden;
                Entered.Visibility = Visibility.Hidden;
                NewUser.Visibility = Visibility.Hidden;
                keyboard.Visibility = Visibility.Hidden;
                delus.Visibility = Visibility.Hidden;
                xxx.Visibility = Visibility.Hidden;
                lbenter.Visibility = Visibility.Visible;
                password1.Visibility = Visibility.Visible;
                gkey.Visibility = Visibility.Visible;
                getres.Visibility = Visibility.Visible;
                bk1.Visibility = Visibility.Visible;
            }
        }

        private void NewUser_Click(object sender, RoutedEventArgs e)// добавить пользователя 
        {
            Added += next1_Click;
            headpiece.Visibility = Visibility.Hidden;
            Entered.Visibility = Visibility.Hidden;
            NewUser.Visibility = Visibility.Hidden;
            keyboard.Visibility = Visibility.Hidden;
            xxx.Visibility = Visibility.Hidden;
            delus.Visibility = Visibility.Hidden;
            lbadd.Visibility = Visibility.Visible;
            fio.Visibility = Visibility.Visible;
            tfio.Visibility = Visibility.Visible;
            lid.Visibility = Visibility.Visible;
            tid.Visibility = Visibility.Visible;
            pass.Visibility = Visibility.Visible;
            tpassword.Visibility = Visibility.Visible;
            next1.Visibility = Visibility.Visible;
            img.Visibility = Visibility.Visible;
            bk.Visibility = Visibility.Visible;
        }

        private void next1_Click(object sender, RoutedEventArgs e) //продолжить 
        {
            if (tfio.Text == "" || tid.Text == "" || tpassword.Text == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                if (tpassword.Text.Length < 6)
                {
                    MessageBox.Show("Пароль должен содержать не менее 6 символов.");
                    tpassword.Text = "";
                }
                else
                {
                    try
                    {
                        if (checkID.Contains(int.Parse(tid.Text)))
                        {
                            MessageBox.Show("Этот id уже занят");
                            tid.Text = "";
                        }
                        else
                        {
                            checkID.Add(int.Parse(tid.Text));
                            if (fbk)
                            {
                                makeuser();
                            }
                            else
                            {
                                fbk = true;
                            }
                            Added -= next1_Click;
                            password1.Visibility = Visibility.Visible;
                            newuser1.Visibility = Visibility.Visible;
                            Key2.Visibility = Visibility.Visible;
                            Next2.Visibility = Visibility.Visible;
                            bk3.Visibility = Visibility.Visible;
                            lbadd.Visibility = Visibility.Hidden;
                            fio.Visibility = Visibility.Hidden;
                            tfio.Visibility = Visibility.Hidden;
                            lid.Visibility = Visibility.Hidden;
                            tid.Visibility = Visibility.Hidden;
                            bk.Visibility = Visibility.Hidden;
                            pass.Visibility = Visibility.Hidden;
                            tpassword.Visibility = Visibility.Hidden;
                            next1.Visibility = Visibility.Hidden;
                            img.Visibility = Visibility.Hidden;
                            Added += Next2_Click;
                            Fild.KeyDown += getst;
                            Fild.KeyUp += getstat;
                            pressup.Clear();
                            pressb.Clear();
                            updown.Clear();
                            between.Clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        tid.Text = ex.Message;
                    }
                }
            }
        }

        public void getst(object sender, KeyEventArgs e)
        {
            millisec = timer.ElapsedMilliseconds;
            try
            {
                if (e.Key != Key.Enter && password1.Text != "")
                {
                    long x = pressb.Peek();
                    long q = millisec - x;
                    between.Add(q);
                }
            }
            catch (Exception ex)
            {

            }
            if (e.Key != Key.Enter)
            {
                pressup.Enqueue(millisec);
            }
        }
        public void getstat(object sender, System.Windows.Input.KeyEventArgs e)
        {
            millisec = timer.ElapsedMilliseconds;
            if (e.Key != Key.Enter)
            {
                pressb.Enqueue(millisec);
            }
            try
            {
                if (e.Key != Key.Enter && password1.Text != "")
                {
                    long x = millisec - pressup.Peek();
                    updown.Add(x);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void makeuser()
        {
            User man = new User();
            man.fio = tfio.Text;
            man.id = int.Parse(tid.Text);
            man.password = tpassword.Text;
            lst.Add(man);
        }

        private void Next2_Click(object sender, RoutedEventArgs e)
        {
            if (password1.Text != lst[lst.Count - 1].password)
            {
                psw1 = 0;
                password1.Clear();
                MessageBox.Show("Пароли должны совпадать!");
                pressup.Clear();
                pressb.Clear();
                updown.Clear();
                between.Clear();
            }
            else
            {
                pressup.Clear();
                pressb.Clear();
                List<long> lx = new List<long>();
                for (int i = 0; i < updown.Count; i++)
                {
                    lx.Add(updown[i]);
                }
                updown.Clear();
                lst[lst.Count - 1].keyinf.Add(lx);
                List<long> lx1 = new List<long>();
                for (int i = 0; i < between.Count; i++)
                {
                    lx1.Add(between[i]);
                }
                between.Clear();
                lst[lst.Count - 1].keyinf.Add(lx1);
                psw1 = 0;
                password1.Clear();
                newuser1.Content = " Добавление пользователя" + "\r\n" + "*Введите пароль ещё раз," + "\r\n" + "старайтесь вводить в том же темпе, что и в прошлый раз " + ch;
                ch++;
                if (ch == 6)
                {
                    ch = 1;
                    MessageBox.Show("Пользователь успешно добавлен");
                    Added -= Next2_Click;
                    Fild.KeyDown -= getst;
                    Fild.KeyUp -= getstat;
                    tid.Text = "";
                    tfio.Text = "";
                    tpassword.Text = "";
                    updown.Clear();
                    between.Clear();
                    psw1 = 0;
                    cont = false;
                    headpiece.Visibility = Visibility.Visible;
                    Entered.Visibility = Visibility.Visible;
                    NewUser.Visibility = Visibility.Visible;
                    delus.Visibility = Visibility.Visible;
                    xxx.Visibility = Visibility.Visible;
                    keyboard.Visibility = Visibility.Visible;
                    password1.Visibility = Visibility.Hidden;
                    newuser1.Visibility = Visibility.Hidden;
                    Key2.Visibility = Visibility.Hidden;
                    bk3.Visibility = Visibility.Hidden;
                    Next2.Visibility = Visibility.Hidden;
                    Serialize();
                }
            }
        }


        public void Serialize()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<User>));
            FileStream f = new FileStream("text.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            xml.Serialize(f, lst);
            f.Close();
            XmlSerializer xml1 = new XmlSerializer(typeof(HashSet<int>));
            FileStream f1 = new FileStream("text1.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            xml1.Serialize(f1, checkID);
            f1.Close();
        }
        private void getres_Click(object sender, RoutedEventArgs e)
        {
            if (password1.Text == "")
            {
                MessageBox.Show("Сначала введите пароль");
            }
            else
            {
                int us = getpass(password1.Text);
                if (us == -1)
                {
                    gend.Visibility = Visibility.Visible;
                    MessageBox.Show("Пароль неверный!");
                    pressup.Clear();
                    pressb.Clear();
                    between.Clear();
                    updown.Clear();
                    gend.Visibility = Visibility.Hidden;
                    psw1 = 0;
                    password1.Clear();
                }
                else
                {
                    analize(us);
                }
            }
        }

        private int getpass(string st)
        {
            int q = -1;
            for (int i = 0; i < lst.Count; i++)
            {
                if (lst[i].password == st)
                {
                    q = i;
                    return q;
                }
            }
            return q;
        }
        public void analize(int us)
        {
            double check1 = 0;
            checklist.Clear();
            for (int j = 0; j < 10; j++)
            {
                check1 = 0;
                if (j % 2 != 0)
                {
                    for (int i = 0; i < between.Count; i++)
                    {
                        check1 += Math.Sqrt((lst[us].keyinf[j][i] - between[i]) * (lst[us].keyinf[j][i] - between[i]));
                    }
                    checklist.Add(check1);
                }
                else
                {
                    for (int i = 0; i < updown.Count; i++)
                    {
                        check1 += Math.Sqrt((lst[us].keyinf[j][i] - updown[i]) * (lst[us].keyinf[j][i] - updown[i]));
                    }
                    checklist.Add(check1);
                }
            }
            pressup.Clear();
            pressb.Clear();
            result(us);
        }
        public void result(int us)
        {
          double locker = 900;
            if (lst[us].password.Length > 14)
            {
                locker = 1200;
            }        
            for (int i = 0; i < 9; i++)
            {
                if (checklist[i] < locker && checklist[i + 1] < locker)
                {
                    locker = 0;
                    break;
                }
                i++;
            }
            if (locker != 0)
            {
                gend.Visibility = Visibility.Visible;
                MessageBox.Show("Пользователь не определён ;-)");
                gend.Visibility = Visibility.Hidden;
                psw1 = 0;
                password1.Clear();
                between.Clear();
                updown.Clear();
                pressup.Clear();
                pressb.Clear();
            }
            else
            {
                psw1 = 0;
                Fild.KeyDown -= getst;
                Fild.KeyUp -= getstat;
                pressup.Clear();
                pressb.Clear();
                Added -= getres_Click;
                tab.ColumnDefinitions.Clear();
                tab.RowDefinitions.Clear();
                fl = true;
                maketab(us);
                lbenter.Visibility = Visibility.Hidden;
                password1.Visibility = Visibility.Hidden;
                gkey.Visibility = Visibility.Hidden;
                getres.Visibility = Visibility.Hidden;
                bk1.Visibility = Visibility.Hidden;
                double srt = avengertime(updown);
                double srupd = avengertime(between);
                resultlb.Content = "Вы вошли как: " + lst[us].fio + "\r\n" + "ID:" + lst[us].id + "\r\n" + "Среднее время между нажатиями клавиш: " + srt + "\r\n" + "Среднее время удержания клавиши: " + srupd;
                resultlb.Visibility = Visibility.Visible;
                menu.Visibility = Visibility.Visible;
                lock1.Visibility = Visibility.Visible;
                tabb.Visibility = Visibility.Visible;
                password1.Text = "";
                gridi(us);
            }
        }
        public double avengertime(List<long> st)
        {
            double x = 0;
            for (int i = 0; i < st.Count; i++)
            {
                x += st[i];
            }
            x = x / st.Count;
            return x;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Fild.KeyDown -= getst;
            Fild.KeyUp -= getstat;
            between.Clear();
            updown.Clear();
            resultlb.Visibility = Visibility.Hidden;
            menu.Visibility = Visibility.Hidden;
            lock1.Visibility = Visibility.Hidden;
            bk1.Visibility = Visibility.Hidden;
            bk.Visibility = Visibility.Hidden;
            tabb.Visibility = Visibility.Hidden;
            headpiece.Visibility = Visibility.Visible;
            Entered.Visibility = Visibility.Visible;
            NewUser.Visibility = Visibility.Visible;
            keyboard.Visibility = Visibility.Visible;
            delus.Visibility = Visibility.Visible;
            xxx.Visibility = Visibility.Visible;
        }
        public void gridi(int us)
        {
            int iq = 0;
            tab.Children.RemoveRange(3, tab.Children.Count);
            for (int j = 0; j < lst[us].password.Length; j++)
            {
                TextBlock x = new TextBlock();
                x.Text = "   ";
                x.Text += Convert.ToString(lst[us].password[j]);
                Grid.SetRow(x, j + 1);
                Grid.SetColumn(x, iq);
                tab.Children.Add(x);
            }
            iq = 2;
            for (int j = 0; j < between.Count; j++)
            {
                TextBlock x = new TextBlock();
                x.Text = "   ";
                x.Text += Convert.ToString(between[j]);
                Grid.SetRow(x, j + 2);
                Grid.SetColumn(x, iq);
                tab.Children.Add(x);
            }
            iq = 1;
            for (int j = 0; j < updown.Count; j++)
            {
                TextBlock x = new TextBlock();
                x.Text = "   ";
                x.Text += Convert.ToString(updown[j]);
                Grid.SetRow(x, j + 1);
                Grid.SetColumn(x, iq);
                tab.Children.Add(x);
            }
        }
        private void bk_Click(object sender, RoutedEventArgs e)
        {
            headpiece.Visibility = Visibility.Visible;
            Entered.Visibility = Visibility.Visible;
            NewUser.Visibility = Visibility.Visible;
            keyboard.Visibility = Visibility.Visible;
            delus.Visibility = Visibility.Visible;
            xxx.Visibility = Visibility.Visible;
            lbadd.Visibility = Visibility.Hidden;
            fio.Visibility = Visibility.Hidden;
            tfio.Visibility = Visibility.Hidden;
            tfio.Text = "";
            lid.Visibility = Visibility.Hidden;
            tid.Visibility = Visibility.Hidden;
            tid.Text = "";
            pass.Visibility = Visibility.Hidden;
            tpassword.Visibility = Visibility.Hidden;
            tpassword.Text = "";
            next1.Visibility = Visibility.Hidden;
            img.Visibility = Visibility.Hidden;
            bk.Visibility = Visibility.Hidden;
            Added -= next1_Click;
        }

        private void bk1_Click(object sender, RoutedEventArgs e)
        {
            headpiece.Visibility = Visibility.Visible;
            Entered.Visibility = Visibility.Visible;
            NewUser.Visibility = Visibility.Visible;
            keyboard.Visibility = Visibility.Visible;
            delus.Visibility = Visibility.Visible;
            xxx.Visibility = Visibility.Visible;
            lbenter.Visibility = Visibility.Hidden;
            password1.Visibility = Visibility.Hidden;
            password1.Text = "";
            Added -= getres_Click;
            Fild.KeyDown -= getst;
            Fild.KeyUp -= getstat;
            gkey.Visibility = Visibility.Hidden;
            bk1.Visibility = Visibility.Hidden;
            getres.Visibility = Visibility.Hidden;
        }

        private void dr_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            headpiece.Visibility = Visibility.Visible;
            Entered.Visibility = Visibility.Visible;
            NewUser.Visibility = Visibility.Visible;
            keyboard.Visibility = Visibility.Visible;
            delus.Visibility = Visibility.Visible;
            dr.Visibility = Visibility.Hidden;
            tab.Visibility = Visibility.Hidden;
            xxx.Visibility = Visibility.Visible;
            between.Clear();
            updown.Clear();
            Fild.KeyDown -= getst;
            Fild.KeyUp -= getstat;
        }

        private void tabb_Click(object sender, RoutedEventArgs e)
        {
            tab.Visibility = Visibility.Visible;
            dr.Visibility = Visibility.Visible;
            resultlb.Visibility = Visibility.Hidden;
            menu.Visibility = Visibility.Hidden;
            lock1.Visibility = Visibility.Hidden;
            delus.Visibility = Visibility.Hidden;
            tabb.Visibility = Visibility.Hidden;
        }

        public void delall()
        {
            XmlSerializer xml = new XmlSerializer(typeof(List<User>));
            FileStream f = new FileStream("text.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            List<User> ss = new List<User>();
            xml.Serialize(f, ss);
            lst.Clear();
            checkID.Clear();
            MessageBox.Show("Пользователи удалены");
            f.Close();
            XmlSerializer xml1 = new XmlSerializer(typeof(HashSet<int>));
            FileStream f1 = new FileStream("text1.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            HashSet<int> ss1 = new HashSet<int>();
            xml1.Serialize(f1, ss1);
            f1.Close();
        }
        public void delus_Click(object sender, RoutedEventArgs e)
        {
            delall();
        }

        public void maketab(int us)
        {
            tab.Children.RemoveRange(0, tab.Children.Count);
            if (fl)
            {
                ColumnDefinition colDef1 = new ColumnDefinition();
                ColumnDefinition colDef2 = new ColumnDefinition();
                ColumnDefinition colDef3 = new ColumnDefinition();
                tab.ColumnDefinitions.Add(colDef1);
                tab.ColumnDefinitions.Add(colDef2);
                tab.ColumnDefinitions.Add(colDef3);
                for (int i = 0; i < lst[us].password.Length + 1; i++)
                {
                    RowDefinition rowDef3 = new RowDefinition();
                    tab.RowDefinitions.Add(rowDef3);
                }
                TextBlock txt2 = new TextBlock();
                txt2.Text = "  Клавиша";
                txt2.FontSize = 12;
                txt2.FontWeight = FontWeights.Black;
                Grid.SetRow(txt2, 0);
                Grid.SetColumn(txt2, 0);
                tab.Children.Add(txt2);

                TextBlock txt1 = new TextBlock();
                txt1.Text = "  длительность (мс)";
                txt1.FontSize = 12;
                txt1.FontWeight = FontWeights.Black;
                Grid.SetRow(txt1, 0);
                Grid.SetColumn(txt1, 1);
                tab.Children.Add(txt1);

                TextBlock txt3 = new TextBlock();
                txt3.Text = "  Интервал (мс)";
                txt3.FontSize = 12;
                txt3.FontWeight = FontWeights.Black;
                Grid.SetRow(txt3, 0);
                Grid.SetColumn(txt3, 2);
                tab.Children.Add(txt3);
                fl = false;
            }
        }

        private void bk3_Click(object sender, RoutedEventArgs e)
        {
            password1.Visibility = Visibility.Hidden;
            newuser1.Visibility = Visibility.Hidden;
            Key2.Visibility = Visibility.Hidden;
            Next2.Visibility = Visibility.Hidden;
            bk3.Visibility = Visibility.Hidden;
            lbadd.Visibility = Visibility.Visible;
            fio.Visibility = Visibility.Visible;
            tfio.Visibility = Visibility.Visible;
            lid.Visibility = Visibility.Visible;
            tid.Visibility = Visibility.Visible;
            bk.Visibility = Visibility.Visible;
            pass.Visibility = Visibility.Visible;
            tpassword.Visibility = Visibility.Visible;
            next1.Visibility = Visibility.Visible;
            img.Visibility = Visibility.Visible;
            fbk = false;
            Added -= Next2_Click;
            Fild.KeyDown -= getst;
            Fild.KeyUp -= getstat;
            checkID.Remove(lst[lst.Count - 1].id);
        }
        private void password1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (password1.Text.Length >= psw1 || password1.Text.Length == 0)
            {
                psw1++;
            }
            else
            {
                MessageBox.Show("Вы сбились, попробуйте ещё раз.");
                psw1 = 0;
                password1.Text = "";
                pressup.Clear();
                pressb.Clear();
                updown.Clear();
                between.Clear();
            }
        }

        private void Fild_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Added(sender, e);
                if (!cont)
                {
                    cont = true;
                    Entered_Click(sender, e);
                }
            }
        }
        private void xxx_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Fild.Close();
        }
    }
}

