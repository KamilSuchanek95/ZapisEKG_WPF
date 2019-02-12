using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Microsoft.Win32;
using System.Windows.Threading;

namespace ZapisEKG
{
    public partial class MainWindow : Window
    {
        string HEROKU_CLI = "heroku pg:psql postgresql-clean-81393 --app ekg-app";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        //string record;
        string sciezka;
        int Id;

        private void Wybierz_folder(object sender, RoutedEventArgs e)
        {
            czekaj.Visibility = Visibility.Visible; UpdateLayout();
            if (int.TryParse(ID.Text, out Id))
            {
                try
                {
                    OpenFileDialog op = new OpenFileDialog();
                    op.InitialDirectory = "c:\\";
                    op.RestoreDirectory = true;

                    op.ShowDialog();
                    FileInfo info = new FileInfo(op.FileName);
                    sciezka = info.Directory.FullName;

                }
                catch
                {
                    ID.Text = "#1-!";
                }
            }
            else
            {
                ID.Text = "#2-!";
            }
            czekaj.Visibility = Visibility.Hidden; UpdateLayout();
        }

        private void Przeslij(object sender, RoutedEventArgs e)
        {

            czekaj.Visibility = Visibility.Visible; UpdateLayout();

            Dispatcher.Invoke(new Action(Function1), DispatcherPriority.ContextIdle, null);

            czekaj.Visibility = Visibility.Hidden; UpdateLayout();

        }

        public void Function1()
        {
            if (int.TryParse(ID.Text, out Id))
            {

                for (int i = 1; i <= 1440; i++)
                {
                    try
                    {
                        if (File.Exists(sciezka + "\\EKG" + i + ".txt"))
                        {
                            StreamWriter sw = new StreamWriter("logEKG.txt", true);
                            try { sw.WriteLine("Init EKG" + i); sw.WriteLine(DateTime.Now); }
                            catch { }
                            StreamReader sr = new StreamReader(sciezka + "\\EKG" + i + ".txt");
                            int miaona = sr.Peek();
                            if (sr.Peek() >= 0)
                            {
                                Process process = new Process();
                                process.StartInfo.FileName = "cmd.exe";
                                process.StartInfo.RedirectStandardInput = true;
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.UseShellExecute = false;
                                process.Start();
                                process.StandardInput.WriteLine("heroku pg:psql postgresql-clean-81393 --app ekg-app");

                                try
                                {
                                    sw.WriteLine("ansfer heroku...");
                                    sw.WriteLine(process.StandardOutput.ReadLine());
                                    sw.WriteLine(process.StandardOutput.ReadLine());
                                }
                                catch { }

                                process.StandardInput.Flush();
                                process.StandardInput.Write("insert into przebiegis (user_id, zapis) values (" + ID.Text + ", '");
                                process.StandardInput.Write("{");

                                while (sr.Peek() >= 0)
                                {
                                    process.StandardInput.Write((char)sr.Read());
                                }

                                sr.Close();

                                process.StandardInput.Write("0}');");
                                process.StandardInput.Flush();


                                process.StandardInput.Close();
                                try
                                {
                                    sw.WriteLine("ansfer query...");
                                    string spr = process.StandardOutput.ReadToEnd();
                                    sw.WriteLine(spr);
                                    if (!spr.Contains("INSERT 0 1")) { ID.Text = "#4-!"; };
                                }
                                catch { }

                                sw.Close();
                                process.WaitForExit();
                            }
                        }
                    }
                    catch
                    {
                        ID.Text = "#3-!";
                    }
                }



            }
            else
            {
                ID.Text = "#2-!";
            }
        }
    }
}
/*
                            if (sr.Peek() >=0)
                            {
                                record = "{";

                                while (sr.Peek() >=1)
                                {
                                    record += (char)sr.Read();
                                }

                                record += " }";
                                record = record.Replace("  ", " ").Replace(", ", "");
                                string query = "insert into przebiegis (user_id, zapis) values (" + ID.Text + ", '" + record + "');";

                                Process process = new Process();
                                process.StartInfo.FileName = "cmd.exe";
                                process.StartInfo.RedirectStandardInput = true;
                                process.StartInfo.RedirectStandardOutput = true;
                                process.StartInfo.UseShellExecute = false;
                                process.Start();

                                process.StandardInput.WriteLine("heroku pg:psql postgresql-angular-57385 --app ekg-app");
                                process.StandardInput.Flush();
                                process.StandardInput.WriteLine(query);
                                process.StandardInput.Flush();

                                process.StandardInput.Close();
                                process.WaitForExit();
                            }

*/



//    Thread.Sleep(1000);

//string output = process.StandardOutput.ReadToEnd();
//Console.WriteLine(output);


//process.StandardInput.WriteLine("psql -U jdbigwjpuutcuh --password 8e684ca67665828a0bb7567d03d1faa3bb0875749d88f65af3db0fcb0cfd924d" +
//" -p 5432 -h ec2-54-221-237-246.compute-1.amazonaws.com -d d6vbf3bajgqsh4;");
//process.StandardInput.Flush();
//Thread.Sleep(1000);

//process.StandardInput.Write("8e684ca67665828a0bb7567d03d1faa3bb0875749d88f65af3db0fcb0cfd924d;");
//process.StandardInput.Flush();
// Thread.Sleep(1000);


/*Thread.Sleep(1000);
    ProcessStartInfo miao = new ProcessStartInfo();
    miao.RedirectStandardOutput = true;
    miao.RedirectStandardInput = true;
    miao.FileName = "cmd.exe";
    miao.UseShellExecute = false;
    using (Process p = Process.Start(miao))
    {
        p.StandardInput.WriteLine("'/k psql -U jdbigwjpuutcuh --password 8e684ca67665828a0bb7567d03d1faa3bb0875749d88f65af3db0fcb0cfd924d" +
        " -p 5432 -h ec2-54-221-237-246.compute-1.amazonaws.com -d d6vbf3bajgqsh4'");
        p.StandardInput.Flush();
        Thread.Sleep(1000);

        p.StandardInput.WriteLine("8e684ca67665828a0bb7567d03d1faa3bb0875749d88f65af3db0fcb0cfd924d");
        p.StandardInput.Flush();
        Thread.Sleep(1000);

        p.StandardInput.WriteLine("update users set role=0 where id=1;");
        p.StandardInput.Flush();
        Thread.Sleep(1000);

        string output = p.StandardOutput.ReadToEnd();
        Console.WriteLine(output);
        p.StandardInput.Close();
    }/*



    /*string strCmdText;
    strCmdText = 
    "'/k psql -U jdbigwjpuutcuh --password 8e684ca67665828a0bb7567d03d1faa3bb0875749d88f65af3db0fcb0cfd924d"+
    " -p 5432 -h ec2-54-221-237-246.compute-1.amazonaws.com -d d6vbf3bajgqsh4'" +
    "; '8e684ca67665828a0bb7567d03d1faa3bb0875749d88f65af3db0fcb0cfd924d'";
    Process.Start("CMD.exe", strCmdText);*/



