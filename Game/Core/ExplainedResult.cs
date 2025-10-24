#nullable enable
using System;
using System.Collections.Generic;

namespace ShapezMono.Game.Core
{
    /// <summary>
    /// 説明付きの結果を表すクラス
    /// </summary>
    public class ExplainedResult
    {
        public string? Reason { get; set; }

        private bool _result;
        private Dictionary<string, object>? _additionalProps;

        public ExplainedResult(bool result = true, string? reason = null, Dictionary<string, object>? additionalProps = null)
        {
            _result = result;
            Reason = reason;
            _additionalProps = additionalProps;
        }

        /// <summary>
        /// 結果が良好かどうかを示す
        /// </summary>
        /// <returns></returns>
        public bool IsGood()
        {
            return _result;
        }

        /// <summary>
        /// 結果が不良かどうかを示す
        /// </summary>
        /// <returns></returns>
        public bool IsBad()
        {
            return !_result;
        }

        /// <summary>
        /// 良好な結果を生成する
        /// </summary>
        /// <returns></returns>
        public static ExplainedResult Good()
        {
            return new ExplainedResult(true);
        }

        /// <summary>
        /// 不良な結果を生成する
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="additionalProps"></param>
        /// <returns></returns>
        public static ExplainedResult Bad(string? reason, Dictionary<string, object>? additionalProps = null)
        {
            return new ExplainedResult(false, reason, additionalProps);
        }

        /// <summary>
        /// すべてのバリデータが良好な結果を返すことを要求する
        /// </summary>
        /// <param name="validators"></param>
        /// <returns></returns>
        public static ExplainedResult RequireAll(params Func<ExplainedResult>[] validators)
        {
            foreach (var validator in validators)
            {
                var subResult = validator();
                if (subResult.IsBad())
                {
                    return subResult;
                }
            }
            return Good();
        }
    }
}
