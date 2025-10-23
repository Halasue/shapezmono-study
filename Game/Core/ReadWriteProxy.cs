using System;
using System.Collections.Generic;
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


        }
    }
}
