using System;
using UnityEngine;

namespace Mystic
{
    class RectScope
    {
        public struct Scope : IDisposable
        {
            public Scope(RectScope scope)
            {
                _scope = scope;
                if (_scope != null)
                {
                    _scope.BeginScoped();
                }
            }
            public void Dispose()
            {
                if (_scope != null)
                {
                    _scope.EndScoped();
                }
            }
            public Rect GetRect()
            {
                return _scope?.GetRect() ?? default;
            }
            public bool TryGetRect(out Rect rect)
            {
                if (_scope is null)
                {
                    rect = default;
                    return false;
                }
                rect = _scope.GetRect();
                return rect.height > 0;
            }
            RectScope _scope;
        }

        public Scope Scan()
        {
            return new(this);
        }
        Rect GetRect()
        {
            Rect rect = _begin;
            rect.height = _end.y - _begin.y;
            return rect;
        }
        void BeginScoped()
        {
            GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _begin = GUILayoutUtility.GetLastRect();
            }
        }
        void EndScoped()
        {
            GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _end = GUILayoutUtility.GetLastRect();
            }
        }
        Rect _begin;
        Rect _end;
    }
}
