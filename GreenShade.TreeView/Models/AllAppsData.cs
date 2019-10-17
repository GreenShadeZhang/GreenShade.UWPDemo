using GreenShade.TreeView.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GreenShade.TreeView.Models
{
    public class AllAppsData
    {
        public string stat { get; set; }
        public string msg { get; set; }
        public SonData data { get; set; }


    }
    public class SonData
    {
        public Info info { get; set; }
        public ObservableCollection<Son> datas { get; set; }
        public object[] promotions { get; set; }
    }

    public class Info
    {
        public string update_date { get; set; }
    }

    public class Son : INotifyPropertyChanged
    {
        private ICommand _switchThemeCommand;

        public ICommand SwitchThemeCommand
        {
            get
            {
                if (_switchThemeCommand == null)
                {
                    _switchThemeCommand = new RelayCommand<Son>(
                        (param) =>
                        {
                            var par = param as Son;
                            IsAdded = !IsAdded;
                            if (isAdded == true)
                            {
                              
                            }

                            else
                            {
                              
                            }
                            BtnText = "已添加o";
                            // ElementTheme = param;
                            //  await ThemeSelectorService.SetThemeAsync(param);
                        });
                }

                return _switchThemeCommand;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string pk { get; set; }
        public string title { get; set; }
        public string stitle { get; set; }
        public string pics { get; set; }
        public string is_end { get; set; }
        public string list_icon { get; set; }
        public string list_stitle { get; set; }
        public string father_id { get; set; }
        public string p_pk { get; set; }
        public ObservableCollection<Son> sons { get; set; }
        public string skey { get; set; }
        public string token { get; set; }
        public string api_url { get; set; }
        public string block_bg_color { get; set; }
        public string data_type { get; set; }
        public string require_web { get; set; }
        public string pic { get; set; }

        private string _btnText;
        public string BtnText
        {
            get { return IsAdded == true ? "已订阅" : "订阅"; }
            set
            {
                if (_btnText != value)
                {
                    _btnText = value;
                    NotifyPropertyChanged("BtnText");
                }
            }
        }

        private bool isAdded = false;
        public bool IsAdded
        {
            get { return isAdded; }

            set
            {
                if (isAdded != value)
                {
                    isAdded = value;
                    NotifyPropertyChanged("IsAdded");
                }
            }

        }

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
