using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ChainingAssertion.WP7
{
    public partial class App : Application
    {
        /// <summary>
        /// Phone アプリケーションのルート フレームへの容易なアクセスを提供します。
        /// </summary>
        /// <returns>Phone アプリケーションのルート フレームです。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application オブジェクトのコンストラクターです。
        /// </summary>
        public App()
        {
            // キャッチできない例外のグローバル ハンドラーです。 
            UnhandledException += Application_UnhandledException;

            // Silverlight の標準初期化
            InitializeComponent();

            // Phone 固有の初期化
            InitializePhoneApplication();

            // デバッグ中にグラフィックスのプロファイル情報を表示します。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 現在のフレーム レート カウンターを表示します。
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 各フレームで再描画されているアプリケーションの領域を表示します。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // 試験的な分析視覚化モードを有効にします。 
                // これにより、色付きのオーバーレイを使用して、GPU に渡されるページの領域が表示されます。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // アプリケーションの PhoneApplicationService オブジェクトの UserIdleDetectionMode プロパティを Disabled に設定して、
                // アプリケーションのアイドル状態の検出を無効にします。
                // 注意: これはデバッグ モードのみで使用してください。ユーザーが電話を使用していないときに、ユーザーのアイドル状態の検出を無効にする
                // アプリケーションが引き続き実行され、バッテリ電源が消耗します。
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // (たとえば、[スタート] メニューから) アプリケーションが起動するときに実行されるコード
        // このコードは、アプリケーションが再アクティブ化済みの場合には実行されません
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            
        }

        // アプリケーションがアクティブになった (前面に表示された) ときに実行されるコード
        // このコードは、アプリケーションの初回起動時には実行されません
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // アプリケーションが非アクティブになった (バックグラウンドに送信された) ときに実行されるコード
        // このコードは、アプリケーションの終了時には実行されません
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // (たとえば、ユーザーが戻るボタンを押して) アプリケーションが終了するときに実行されるコード
        // このコードは、 アプリケーションが非アクティブになっているときには実行されません
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // ナビゲーションに失敗した場合に実行されるコード
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // ナビゲーションに失敗しました。デバッガーで中断します。
                System.Diagnostics.Debugger.Break();
            }
        }

        // ハンドルされない例外の発生時に実行されるコード
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // ハンドルされない例外が発生しました。デバッガーで中断します。
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone アプリケーションの初期化

        // 初期化の重複を回避します
        private bool phoneApplicationInitialized = false;

        // このメソッドに新たなコードを追加しないでください
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // フレームを作成しますが、まだ RootVisual に設定しないでください。これによって、アプリケーションがレンダリングできる状態になるまで、
            // スプラッシュ スクリーンをアクティブなままにすることができます。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // ナビゲーション エラーを処理します
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 再初期化しないようにします
            phoneApplicationInitialized = true;
        }

        // このメソッドに新たなコードを追加しないでください
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // ルート visual を設定してアプリケーションをレンダリングできるようにします
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // このハンドラーは必要なくなったため、削除します
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}