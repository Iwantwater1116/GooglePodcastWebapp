using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CefSharp;
using CefSharp.Wpf;
using CefSharp.DevTools.Network;
using System.Xml.Serialization;

namespace GooglePodcast
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {

        private delegate void ShowErrorCode(string ErrorCode);
        private delegate void ShowLoading();
        private delegate void LoadiFinish();

        int errorlod_flag = 2;

        private void ShowErrorMessage(string ErrorCode)
        {
            if(!this.Dispatcher.CheckAccess() )
            {
                ShowErrorCode showError = new ShowErrorCode(ShowErrorMessage);
                this.Dispatcher.Invoke(showError,ErrorCode);
            }
            else
            {
                Loading_Cricle.Visibility = Visibility.Hidden;
                Broser.Visibility = Visibility.Hidden;
                Lbl_ConnectError.Content = $"無法連線至Podcast，請檢查您的連線。 ({ErrorCode})";
                Lbl_ConnectError.Visibility = Visibility.Visible;
                Btn_Reloading.Visibility = Visibility.Visible;
            }
        }
        private void LoadingPage()
        {
            if (!this.Dispatcher.CheckAccess())
            {
                ShowLoading showLoad = new ShowLoading(LoadingPage);
                this.Dispatcher.Invoke(showLoad);
            }
            else
            {
                Broser.Visibility = Visibility.Hidden;
                Lbl_ConnectError.Visibility = Visibility.Hidden;
                Btn_Reloading.Visibility = Visibility.Hidden;
                Loading_Cricle.Visibility = Visibility.Visible;
   
            }
        }

        private void LoadPageFinish()
        {
            if (!this.Dispatcher.CheckAccess())
            {
                LoadiFinish showLoad = new LoadiFinish(LoadPageFinish);
                this.Dispatcher.Invoke(showLoad);
            }
            else
            {
                Lbl_ConnectError.Visibility = Visibility.Hidden;
                Btn_Reloading.Visibility = Visibility.Hidden;
                Loading_Cricle.Visibility = Visibility.Hidden;
                Broser.Visibility = Visibility.Visible ;

            }
        }

        public MainWindow()
        {
            CefSettings settings = new CefSettings();
            settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.110 Safari/537.36 /CefSharp Browser" + Cef.CefSharpVersion;
            var cache = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.IO.Path.Combine("Google Podcast", "Cache")); 
            if (!System.IO.Directory.Exists(cache)) 
                System.IO.Directory.CreateDirectory(cache); 
            var locate = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.IO.Path.Combine("Google Podcast", "locate")); 
            if (!System.IO.Directory.Exists(cache)) 
                System.IO.Directory.CreateDirectory(cache); 
            settings.CachePath = cache;
            settings.LocalesDirPath = locate;
            Cef.Initialize(settings);
            InitializeComponent();
            Broser.LoadError += OnLoadError;
            Broser.LoadingStateChanged += BroserLodingStateChanged;
            Broser.FrameLoadEnd += BroserLoadingFinish;
            
        }
        private void OnLoadError(object sendoer, LoadErrorEventArgs e)
        {
            ShowErrorMessage("404");
            errorlod_flag = 1;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Broser.LifeSpanHandler = new MyCustomLifeSpanHandler();
        }
        private void Btn_Reloading_Click(object sender, RoutedEventArgs e)
        {
            Broser.Reload(true);
        }

        private void BroserLodingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if(e.IsLoading == true)
            {
                if(errorlod_flag == 1 || errorlod_flag == 2)
                {
                    LoadingPage();
                }
            }
        }

        private void BroserLoadingFinish(object sender, FrameLoadEndEventArgs e)
        {
            if(errorlod_flag == 2 || errorlod_flag == 3) 
            {
                LoadPageFinish();
            }
            if(errorlod_flag == 1)
            {
                errorlod_flag = 2;
            }
            else
            {
                errorlod_flag = 3;
            }
        }
    }
    public class MyCustomLifeSpanHandler : ILifeSpanHandler
    {
        // Load new URL (when clicking a link with target=_blank) in the same frame
        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            if(targetUrl == "https://podcasts.google.com/")
            {
                browser.MainFrame.LoadUrl(targetUrl);
                newBrowser = null;
                return true;
            }
            else
            {
                MessageBox.Show("Sorry, this application is only for podcast, other functions please use your browser","Sorry",MessageBoxButton.OK,MessageBoxImage.Information);
                newBrowser = null;
                return true;
            }
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            return true;
        }
    }
}
