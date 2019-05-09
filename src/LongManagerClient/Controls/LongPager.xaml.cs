using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LongManagerClient.Controls
{
    /// <summary>
    /// LongPagerxaml.xaml 的交互逻辑
    /// </summary>
    public partial class LongPager : UserControl
    {
        public delegate void PageIndexChangeEvent(object sender, EventArgs e);
        public event PageIndexChangeEvent PageIndexChange;
        public LongPage LongPage = new LongPage() { PageIndex = 1, PageSize = 20 };
        public LongPager()
        {
            InitializeComponent();
            ShowCount.DataContext = LongPage;
        }

        public void InitButton()
        {
            FirstBtn.IsEnabled = true;
            PreBtn.IsEnabled = true;
            NextBtn.IsEnabled = true;
            LastBtn.IsEnabled = true;
            FirstBtn.Background = Brushes.Black;
            PreBtn.Background = Brushes.Black;
            NextBtn.Background = Brushes.Black;
            LastBtn.Background = Brushes.Black;

            //总共一页
            if (LongPage.PageCount < 2)
            {
                FirstBtn.IsEnabled = false;
                PreBtn.IsEnabled = false;
                NextBtn.IsEnabled = false;
                LastBtn.IsEnabled = false;
                FirstBtn.Background = Brushes.Red;
                PreBtn.Background = Brushes.Red;
                NextBtn.Background = Brushes.Red;
                LastBtn.Background = Brushes.Red;
                return;
            }

            //第一页
            if (LongPage.PageIndex == 1)
            {
                FirstBtn.IsEnabled = false;
                PreBtn.IsEnabled = false;
                FirstBtn.Background = Brushes.Red;
                PreBtn.Background = Brushes.Red;
                return;
            }

            //最后一页
            if (LongPage.PageIndex == LongPage.PageCount)
            {
                NextBtn.IsEnabled = false;
                LastBtn.IsEnabled = false;
                NextBtn.Background = Brushes.Red;
                LastBtn.Background = Brushes.Red;
            }
        }

        private void FirstBtn_Click(object sender, RoutedEventArgs e)
        {
            LongPage.PageIndex = 1;
            InitButton();
            PageIndexChange(sender, e);
        }

        private void PreBtn_Click(object sender, RoutedEventArgs e)
        {
            LongPage.PageIndex--;
            InitButton();
            PageIndexChange(sender, e);
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            LongPage.PageIndex++;
            InitButton();
            PageIndexChange(sender, e);
        }

        private void LastBtn_Click(object sender, RoutedEventArgs e)
        {
            LongPage.PageIndex = LongPage.PageCount;
            InitButton();
            PageIndexChange(sender, e);
        }
    }

    public class LongPage : INotifyPropertyChanged
    {
        private int _pageIndex;
        private int _pageSize;
        private int _pageCount;
        private int _allCount;
        private string _search;

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
            set
            {
                if (value != _pageIndex)
                {
                    _pageIndex = value;
                    Notify("PageIndex");
                }
            }
        }

        /// <summary>
        /// 每页几条
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value != _pageSize)
                {
                    _pageSize = value;
                    Notify("PageSize");
                }
            }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                if (value != _pageCount)
                {
                    _pageCount = value;
                    Notify("PageCount");
                }
            }
        }

        /// <summary>
        /// 总条数
        /// </summary>
        public int AllCount
        {
            get
            {
                return _allCount;
            }
            set
            {
                if (value != _allCount)
                {
                    _allCount = value;
                    Notify("AllCount");

                    if (_allCount == 0)
                    {
                        PageCount = 0;
                    }
                    else
                    {
                        PageCount = _allCount % PageSize == 0 ? _allCount / PageSize : _allCount / PageSize + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 当前搜索条件
        /// </summary>
        public string Search
        {
            get
            {
                return _search;
            }
            set
            {
                if (value != _search)
                {
                    _search = value;
                    Notify("Search");
                    PageIndex = 1;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
