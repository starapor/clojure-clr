﻿/**
 *   Copyright (c) David Miller. All rights reserved.
 *   The use and distribution terms for this software are covered by the
 *   Eclipse Public License 1.0 (http://opensource.org/licenses/eclipse-1.0.php)
 *   which can be found in the file epl-v10.html at the root of this distribution.
 *   By using this software in any fashion, you are agreeing to be bound by
 * 	 the terms of this license.
 *   You must not remove this notice, or any other, from this software.
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace clojure.lang
{
    public sealed class ArrayChunk : IChunk
    {
        #region Data

        readonly object[] _array;
        readonly int _off;
        readonly int _end;

        #endregion

        #region C-tors

        public ArrayChunk(object[] array, int off)
            : this(array,0,array.Length)
        {
        }

        public ArrayChunk(object[] array, int off, int end)
        {
            _array = array;
            _off = off;
            _end = end;
        }

        #endregion

        #region Indexed Members

        public object nth(int i)
        {
            return _array[_off + i];
        }

        #endregion

        #region Counted Members

        public int count()
        {
            return _end - _off;
        }

        #endregion

        #region IChunk Members

        public IChunk dropFirst()
        {
            if (_off == _end)
                throw new InvalidOperationException("dropFirst of empty chunk");
            return new ArrayChunk(_array, _off + 1, _end);
        }

        public object reduce(IFn f, object start)
        {
            object ret = f.invoke(start, _array[_off]);
            for (int x = _off + 1; x < _end; x++)
                ret = f.invoke(ret, _array[x]);
            return ret;
        }

        #endregion
    }
}
