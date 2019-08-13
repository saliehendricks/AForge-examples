using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using AForge.Video;
using ImageProcessor;

namespace AForge.Wpf.IpCamera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Public properties

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; this.OnPropertyChanged("ConnectionString"); }
        }

        public bool UseMjpegStream
        {
            get { return _useMJPEGStream; }
            set { _useMJPEGStream = value; this.OnPropertyChanged("UseMjpegStream");}
        }

        public bool UseJpegStream
        {
            get { return _useJPEGStream; }
            set { _useJPEGStream = value; this.OnPropertyChanged("UseJpegStream");}
        }
        public string Prediction
        {
            get { return _prediction; }
            set { _prediction = value; this.OnPropertyChanged("Prediction"); }
        }
        
        #endregion

        #region Private fields

        private string _connectionString;
        private bool _useMJPEGStream;
        private bool _useJPEGStream;
        private IVideoSource _videoSource;
        private string _prediction;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ConnectionString = "http://127.0.0.1:9911";
            UseJpegStream = true;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {

            // create JPEG video source
            if (UseJpegStream)
            {
                _videoSource = new JPEGStream(ConnectionString);
            }
            else // UseMJpegStream
            {
                _videoSource = new MJPEGStream(ConnectionString);
            }
            _videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            _videoSource.Start();
        }


        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bi;
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    bi = bitmap.ToBitmapImage();
                }
                bi.Freeze(); // avoid cross thread operations and prevents leaks
                Dispatcher.BeginInvoke(new ThreadStart(delegate { videoPlayer.Source = bi; }));
            }
            catch (Exception exc)
            {
                _ = MessageBox.Show("Error on _videoSource_NewFrame:\n" + exc.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                StopCamera();
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            _videoSource.SignalToStop();
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
            }
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        #endregion

        private async void btnTest_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bi = videoPlayer.Source.Clone() as BitmapImage;
            string folderPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            string fileName = $"{folderPath}\\lulu.png";
            //save the image
            bi.Save(fileName);

            //test if its lulu

            Gateway g = new Gateway();
            var r = await g.MakePredictionRequest(fullfilepath: fileName);

            var pr = PredictionResult.FromJson(r);
            if (pr.Predictions.Any()) {
                Prediction = (pr.Predictions.First().Probability * 100).ToString("n");
            }
            

        }
    }
}
