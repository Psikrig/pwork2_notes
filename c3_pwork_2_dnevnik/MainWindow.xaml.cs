using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace c3_pwork_2_dnevnik
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            data.Text = Convert.ToString(DateTime.Now);
            if (!Directory.Exists("D:\\daily planner"))
            {
                Directory.CreateDirectory("D:\\daily planner");
            }
            if (!Directory.Exists("D:\\daily planner\\" + data.Text))
            {
                Directory.CreateDirectory("D:\\daily planner\\" + data.Text);
            }
            String[] files = Directory.GetFiles("D:\\daily planner\\" + data.Text);
            List<String> zametky = new List<String>();
            foreach (String file in files)
            {
                string f = System.IO.Path.GetFileNameWithoutExtension(file);
                zametky.Add(f);
            }
            zamet.ItemsSource = zametky;
        }



        private void del_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(("D:\\daily planner\\" + data.Text + "\\" + act_name));
            String[] files = Directory.GetFiles("D:\\daily planner\\" + data.Text);
            List<String> zametky = new List<String>();
            foreach (String file in files)
            {
                string f = System.IO.Path.GetFileNameWithoutExtension(file);
                zametky.Add(f);
            }
            zamet.ItemsSource = zametky;
        }

        private void creat_Click(object sender, RoutedEventArgs e)
        {
            note new_note = new note();
            new_note.note_date = data.Text;
            new_note.note_name = tname.Text;
            new_note.note_dis = tdis.Text;
            string json = jsonwork.jsave(new_note);
            File.WriteAllText(("D:\\daily planner\\" + new_note.note_date + "\\" + new_note.note_name), json);
            String[] files = Directory.GetFiles("D:\\daily planner\\" + data.Text);
            List<String> zametky = new List<String>();
            foreach (String file in files)
            {
                string f = System.IO.Path.GetFileNameWithoutExtension(file);
                zametky.Add(f);
            }
            zamet.ItemsSource = zametky;
        }

        public string act_name;

        private void sav_Click(object sender, RoutedEventArgs e)
        {
            note new_note = new note();
            File.Delete(("D:\\daily planner\\" + data.Text + "\\" + act_name));
            new_note.note_name = tname.Text;
            new_note.note_dis = tdis.Text;
            string json = jsonwork.jsave(new_note);
            File.WriteAllText(("D:\\daily planner\\" + new_note.note_date + "\\" + new_note.note_name), json);
            String[] files = Directory.GetFiles("D:\\daily planner\\" + data.Text);
            List<String> zametky = new List<String>();
            foreach (String file in files)
            {
                string f = System.IO.Path.GetFileNameWithoutExtension(file);
                zametky.Add(f);
            }
            zamet.ItemsSource = zametky;

        }

        private void data_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Directory.Exists("D:\\daily planner\\" + data.Text))
            {
                Directory.CreateDirectory("D:\\daily planner\\" + data.Text);
            }
            zamet.ItemsSource = Directory.GetFiles("D:\\daily planner\\" + data.Text);
        }


        private void zamet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            act_name = zamet.SelectedItem.ToString();
            Dictionary<string, string> l_zamet = jsonwork.jload(act_name, data.Text);
            tname.Text = l_zamet["note_name"];
            tdis.Text = l_zamet["note_dis"];
        }        
    }
    public class note
    {
        public string note_date;
        public string note_name;
        public string note_dis;
    }

    public class jsonwork
    {
        public static string jsave(note new_note) 
        {
            string json = JsonConvert.SerializeObject(new_note);
            return json;
        }

        public static Dictionary<string, string> jload(string act_name, string Text)
        {
            string j_disc = File.ReadAllText("D:\\daily planner\\" + Text + "\\" + act_name);
            Dictionary<string, string> l_zamet = JsonConvert.DeserializeObject<Dictionary<string, string>>(j_disc);
            return l_zamet;
        }
    }
}
