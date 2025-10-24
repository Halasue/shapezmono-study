using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    public abstract class ReadWriteProxy
    {
        private Storage _storage;
        private string _fileName;
        private object _currentData;

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
                        Debug.Assert(result.IsGood(), $"Verify() が失敗しました。デフォルトデータ: {result.Reason}");
                    });
            }
        }

        /// <summary>
        /// データの検証を行う
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ExplainedResult Verify(object data)
        {
            return ExplainedResult.Bad(null);
        }

        /// <summary>
        /// デフォルトデータを取得する
        /// </summary>
        /// <returns></returns>
        protected virtual object GetDefaultData()
        {
            return new Dictionary<string, object>();
        }

        public void DoWriteAsync()
        {
        }
    }
}
