using System;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    /// <summary>
    /// 未処理例外ハンドラ
    /// </summary>
    public class ErrorHandler
    {
        //　再入防止用フラグ
        private bool _isActive = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ErrorHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        /// <summary>
        /// 未処理例外発生時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!_isActive) return;
            _isActive = false;
            var ex = e.ExceptionObject as Exception;
            ShowError(ex);
        }

        /// <summary>
        /// 未観測タスク例外発生時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            OnUnhandledException(sender, new UnhandledExceptionEventArgs(e.Exception, false));
        }

        /// <summary>
        /// コンソールにエラーを表示
        /// </summary>
        /// <param name="ex"></param>
        private void ShowError(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("✖ 未処理の例外が発生しました");
            Console.WriteLine(ex);
            Console.ResetColor();
        }
    }
}
