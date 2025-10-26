using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    /// <summary>
    /// バージョン付きデータ構造
    /// </summary>
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

    public abstract class ReadWriteProxy<TData> where TData : Data
    {
        private Storage _storage;
        private string _fileName;
        protected TData? CurrentData;

        private Logger _logger = LoggerFactory.Create("ReadWriteProxy");

        public ReadWriteProxy(Storage storage, string fileName)
        {
            _storage = storage;
            _fileName = fileName;
            CurrentData = null;

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
        protected virtual ExplainedResult Verify(TData data)
        {
            return ExplainedResult.Bad(null);
        }

        /// <summary>
        /// デフォルトデータを取得する
        /// </summary>
        /// <returns></returns>
        protected abstract TData GetDefaultData();

        /// <summary>
        /// バージョンを取得する
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCurrentVersion()
        {
            return 0;
        }

        /// <summary>
        /// データのマイグレーションを行う
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ExplainedResult Migrate(TData data)
        {
            return ExplainedResult.Bad(null);
        }

        // ------------------------------ 公開メソッド ------------------------------

        /// <summary>
        /// すべてのデータを初期化する
        /// </summary>
        /// <returns></returns>
        public Task ResetEverythingAsync()
        {
            _logger.Warn("⚠️ データを初期化します。");
            CurrentData = GetDefaultData();
            return WriteAsync();
        }

        /// <summary>
        /// オブジェクトをシリアライズする
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual object SerializeObject(object obj)
        {
            return obj;
        }

        /// <summary>
        /// オブジェクトをデシリアライズする
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual object DeserializeObject(object obj)
        {
            return obj;
        }

        public async Task<TData> ReadAsync()
        {
            try
            {
                var contents = await _storage.ReadFileAsync<TData>(_fileName);
                var result = InternalVerifyBasicStructure(contents);

                // 基本構造の検証
                if (result.IsBad())
                    throw new InvalidDataException($"Verify失敗: {result.Reason}");

                // バージョンの検証とマイグレーション
                if (contents!.Version > GetCurrentVersion())
                    throw new InvalidDataException("保存されたデータはサポートされていません。");

                else if (contents.Version < GetCurrentVersion())
                {
                    _logger.Log($"{_fileName} のマイグレーションを開始します: {contents.Version} -> {GetCurrentVersion()}");
                    var migrationResult = Migrate(contents);
                    if (migrationResult.IsBad())
                    {
                        throw new InvalidDataException($"マイグレーションに失敗しました: {migrationResult.Reason}");
                    }
                    _logger.Log($"{_fileName} のマイグレーションが完了しました。");
                }

                // エントリの検証
                var verifyResult = InternalVerifyEntry(contents);
                if (verifyResult.IsBad())
                    throw new InvalidDataException($"読み込み失敗: {_fileName}, 理由: {verifyResult.Reason}, コンテンツ: {contents}");

                CurrentData = contents;
                _logger.Log($"📄 読み込み完了 バージョン: {CurrentData.Version}");

                return contents;
            }
            catch (FileNotFoundException)
            {
                _logger.Log($"{_fileName} が見つかりません。デフォルトデータを使用します。");
                return GetDefaultData();
            }
            catch (Exception ex)
            {
                _logger.Error($"{_fileName} の読み込みに失敗しました: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// データを書き込む
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task WriteAsync()
        {
            var verifyResult = InternalVerifyEntry(CurrentData);

            if (verifyResult.IsBad())
            {
                _logger.Error($"無効なデータを {_fileName} に書き込もうとしました。: {verifyResult.Reason}");
                throw new InvalidOperationException(verifyResult.Reason);
            }

            DebouncedWrite();
            return Task.CompletedTask;
        }

        public async Task DeleteAsync()
        {
            await _storage.DeleteFileAsync(_fileName);
        }

        // ------------------------------ 内部メソッド ------------------------------

        /// <summary>
        /// データを書き込む内部処理
        /// </summary>
        /// <returns></returns>
        private async Task DoWriteAsync()
        {
            try
            {
                await _storage.WriteFileAsync(_fileName, CurrentData!);
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
        /// 基本的な構造の検証を行う
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ExplainedResult InternalVerifyBasicStructure(TData? data)
        {
            if (data?.Version < 0)
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
        private ExplainedResult InternalVerifyEntry(TData? data)
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
