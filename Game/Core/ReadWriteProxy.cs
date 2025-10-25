using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    public class Data
    {
        /// <summary>
        /// データのバージョン
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// データのフィールド
        /// </summary>
        public Dictionary<string, object> Fields { get; set; } = new();
    }

    public abstract class ReadWriteProxy
    {
        private Storage _storage;
        private string _fileName;
        private Data? _currentData;

        private Logger _logger = LoggerFactory.Create("ReadWriteProxy");

        public ReadWriteProxy(Storage storage, string fileName)
        {
            _storage = storage;
            _fileName = fileName;
            _currentData = null;

            // 開発モード時にデフォルトデータの検証を行う。とってもHACKY!
            if (BuildOptions.IsDev)
            {
                _ = Task.Run(async () =>
                    {
                        await Task.Yield();
                        var result = Verify(GetDefaultData());
                        Debug.Assert(result.Result, $"Verify() が失敗しました。デフォルトデータ: {result.Reason}");
                    });
            }
        }

        /// <summary>
        /// データの検証を行う
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ExplainedResult Verify(Data data)
        {
            return ExplainedResult.Bad(null);
        }

        protected virtual int GetCurrentVersion()
        {
            return 0;
        }

        /// <summary>
        /// デフォルトデータを取得する
        /// </summary>
        /// <returns></returns>
        protected virtual Data GetDefaultData()
        {
            return new Data();
        }

        /// <summary>
        /// データを書き込む内部処理
        /// </summary>
        /// <returns></returns>
        private async Task DoWriteAsync()
        {
            try
            {
                await _storage.WriteFileAsync(_fileName, _currentData!);
                _logger.Log($"📄 書き込み完了 {_fileName}");
            }
            catch (Exception)
            {
                _logger.Error($"{_fileName} の書き込みに失敗しました。");
                throw;
            }
        }

        /// <summary>
        /// データを書き込む（50ms デバウンス）
        /// </summary>
        /// <returns></returns>
        private void DebouncedWrite()
        {
            _ = new Debounce(() => DoWriteAsync(), 50);
        }

        /// <summary>
        /// データを書き込む
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task WriteAsync()
        {
            var verifyResult = InternalVerifyEntry(_currentData);

            if (verifyResult.IsBad())
            {
                _logger.Error($"無効なデータを {_fileName} に書き込もうとしました。: {verifyResult.Reason}");
                throw new InvalidOperationException(verifyResult.Reason);
            }

            DebouncedWrite();
            return Task.CompletedTask;
        }

        /// <summary>
        /// 基本的な構造の検証を行う
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ExplainedResult InternalVerifyBasicStructure(Data data)
        {
            if (data.Version < 0)
            {
                return ExplainedResult.Bad($"無効なバージョン: {data.Version}");
            }

            return ExplainedResult.Good();
        }

        /// <summary>
        /// エントリの検証を行う
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ExplainedResult InternalVerifyEntry(Data? data)
        {
            var currentVersion = GetCurrentVersion();

            if (data == null)
            {
                return ExplainedResult.Bad("Data が null です。");
            }

            if (data.Version != currentVersion)
            {
                return ExplainedResult.Bad($"バージョン不一致: 取得値 {data.Version} / 期待値 {currentVersion}");
            }
            
            var verifyStructureError = InternalVerifyBasicStructure(data);
            if (verifyStructureError.IsBad())
            {
                return verifyStructureError;
            }
            return Verify(data);
        }
    }
}
