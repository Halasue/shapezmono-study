using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    /// <summary>
    /// デバウンスユーティリティ
    /// </summary>
    public class Debounce
    {
        private CancellationTokenSource? _cts;

        public Debounce(Func<Task> action, int delayMs)
        {
            // 既存のキャンセレーションをキャンセル
            _cts?.Cancel();
            _cts?.Dispose();

            // 新しいキャンセレーションソースを作成
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            // 非同期でデバウンス処理を実行
            Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(delayMs, token);
                    token.ThrowIfCancellationRequested();
                    await action();
                }
                catch (TaskCanceledException)
                {
                    // タスクがキャンセルされた場合は何もしない
                }
            }, token);
        }
    }
}
