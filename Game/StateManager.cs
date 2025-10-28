using System.Collections.Generic;
using ShapezMono.Game.State;

namespace ShapezMono.Game
{
    /// <summary>
    /// 状態管理クラス
    /// </summary>
    public class StateManager
    {
        private readonly Dictionary<string, GameState> _states = new();
        private GameState? _current;

        /// <summary>
        /// 状態を登録する
        /// </summary>
        /// <param name="name">登録する状態名</param>
        /// <param name="state">登録する状態インスタンス</param>
        public void Register(string name, GameState state)
        {
            _states[name] = state;
        }

        /// <summary>
        /// 状態を移動する
        /// </summary>
        /// <param name="name">遷移先の状態名</param>
        public void MoveTo(string name)
        {
            if (_states.TryGetValue(name, out var state))
            {
                _current = state;
            }
        }

        public GameState? Current => _current;
    }
}
