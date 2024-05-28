
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CommonModule.org.io;


namespace GameRemake
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
      

        private MediaPlayer mediaPlayer = null;

        //백그라운드와 관련된 영역 변수들
        private Bitmap background = null;
        System.Windows.Controls.Image backgroundImage;
        DispatcherTimer backgroundMoveTimer = null;
        double backgroundMovePosition = 0;

        private Bitmap character = null;
        System.Windows.Controls.Image characterImage;
        int characterFrameCount = 0;
        DispatcherTimer characterMoveTimer = null;
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 모든 리소스를 초기화하는 메소드
        /// </summary>
        private void initAll()
        {
            playBgmPlayer(Properties.Resources.login_bgm);
            initBackground(Properties.Resources.start_image1);
            initBackgroundMoveTimer();
            initCharacterMoveTimer();
        }



        private void initCharacter(Bitmap myCharacter)
        {
            character = myCharacter;
            characterImage = new System.Windows.Controls.Image();
            characterImage.Source = new BitmapToBitmapSource(character).getBitmapSource();
            gameCanvas.Children.Add(characterImage);
        }

        private void setCharacterFrameIndex(Bitmap myCharacter)
        {
            character = myCharacter;
            characterImage.Source = new BitmapToBitmapSource(character).getBitmapSource();

            //gameCanvas.Children.Add(characterImage);
        }

        private void initCharacterMoveTimer()
        {
            characterMoveTimer = new DispatcherTimer();
            characterMoveTimer.Interval = TimeSpan.FromMilliseconds(73);
            characterMoveTimer.Tick += CharacterMoveTimer_Tick;

        }

        private void CharacterMoveTimer_Tick(object sender, EventArgs e)
        {
           // gameCanvas.Children.Remove(characterImage);
            switch (characterFrameCount)
            {
                case 0:
                    setCharacterFrameIndex(Properties.Resources.walk_1);
                    break;

                case 1:
                    setCharacterFrameIndex(Properties.Resources.walk_2);
                    break;

                case 2:
                    setCharacterFrameIndex(Properties.Resources.walk_3);
                    break;

                case 3:
                    setCharacterFrameIndex(Properties.Resources.walk_4);
                    break;
                default:
                    characterFrameCount = 0;
                    break;
            }
            
            characterFrameCount++;
          
        }


        /// <summary>
        /// 백그라운드를 추가하는 메소드
        /// </summary>
        /// <param name="myBackground">백그라운드로 지정할 비트맵</param>
        private void initBackground(Bitmap myBackground)
        {
            background = myBackground;
            backgroundImage = new System.Windows.Controls.Image();
            backgroundImage.Source = new BitmapToBitmapSource(background).getBitmapSource();
            gameCanvas.Children.Add(backgroundImage);
        }
        /// <summary>
        /// 백그라운드를 움직이는 타이머를 초기화 시키는 메소드
        /// </summary>
        private void initBackgroundMoveTimer()
        {
            backgroundMoveTimer = new DispatcherTimer();
            backgroundMoveTimer.Interval = TimeSpan.FromMilliseconds(43);
            backgroundMoveTimer.Tick += BackgroundMoveTimer_Tick;

        }
        /// <summary>
        /// 백그라운드를 움직이는 타이머에서 동작할 영역 메소드
        /// </summary>
        /// <param name="sender">전달자</param>
        /// <param name="e">전달될 이벤트 인자</param>
        private void BackgroundMoveTimer_Tick(object sender, EventArgs e)
        {
            backgroundMovePosition -= 7;
            Canvas.SetLeft(backgroundImage, backgroundMovePosition);
        }
        /// <summary>
        /// Player에서 play할 bgm을 지정하는 메소드
        /// </summary>
        /// <param name="myStream">Properties에서 지정할 myStream</param>
        private void playBgmPlayer(UnmanagedMemoryStream myStream)
        {
            if (mediaPlayer == null)
            {
                mediaPlayer = new MediaPlayer();
            }
            mediaPlayer.MediaEnded += MediaPlayer_MediaEnded;
            mediaPlayer.Volume = 1.0f;

            // UnmanagedMemoryStream을 파일에 쓰고, 해당 파일의 경로를 Uri로 변환하여 재생
            string tempFilePath = WriteUnmanagedStreamToFile(myStream);
            if (!string.IsNullOrEmpty(tempFilePath))
            {
                mediaPlayer.Open(new Uri(tempFilePath));
                mediaPlayer.MediaFailed += (sender, args) =>
                {
                    MessageBox.Show("미디어 재생에 실패했습니다: " + args.ErrorException.Message);
                };
                // 자동 재생 설정 (선택 사항)
                mediaPlayer.Play();
            }
        }
        /// <summary>
        /// UnmanagementMemoryStream을 Temp폴더 측에 파일로 변환 시키는 함수
        /// </summary>
        /// <param name="unmanagedStream">안전하지 않은 메모리 스트림 인자 값</param>
        /// <returns></returns>
        private string WriteUnmanagedStreamToFile(UnmanagedMemoryStream unmanagedStream)
        {
            // UnmanagedMemoryStream을 임시 파일에 쓰기
            string tempFilePath = Path.GetTempFileName() + ".mp3";
            using (FileStream fs = File.OpenWrite(tempFilePath))
            {

                unmanagedStream.CopyTo(fs);

            }
            return tempFilePath;
        }


        private void MediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            // 배경음악 재생이 끝나면 다시 재생
            mediaPlayer.Position = TimeSpan.Zero; // 재생 위치를 처음으로 설정
            mediaPlayer.Play();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            initAll();
        }

        /// <summary>
        /// 키 입력시 타는 메소드 영역
        /// </summary>
        /// <param name="sender">전달자</param>
        /// <param name="e">이벤트 아규먼트</param>
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Enter:
                    playBgmPlayer(Properties.Resources.game_bgm);
                    initBackground(Properties.Resources.background);
                    backgroundMoveTimer.Start();
                    initCharacter(Properties.Resources.walk_1);

                    characterMoveTimer.Start();
                    break;

            }
        }
    }
}
