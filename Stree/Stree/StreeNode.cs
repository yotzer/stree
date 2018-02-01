using System;
using System.Collections.Generic;

namespace Stree
{
    public class StreeNode<T>
    {
        private readonly string _key;
        private readonly int _level;
        private int _maxLevel;
        private Dictionary<string, StreeNode<T>> _nextNodes;
        private List<Tuple<string, T>> _values;

        public StreeNode(int level, int maxLevel, string key)
        {
            _level = level;
            _maxLevel = maxLevel;
            _key = key;
        }

        public void Add(string exp, T t)
        {
            if (string.IsNullOrWhiteSpace(exp))
                return;

            if (_level == _maxLevel || exp.Length <= _level)
            {
                if (_values == null)
                    _values = new List<Tuple<string, T>>();
                _values.Add(new Tuple<string, T>(exp, t));
                return;
            }

            if (_nextNodes == null)
                _nextNodes = new Dictionary<string, StreeNode<T>>();

            var hNextKey = exp.Substring(0, _level + 1);
            if (_nextNodes.ContainsKey(hNextKey))
            {
                _nextNodes[hNextKey].Add(exp, t);
            }
            else
            {
                var hNode = new StreeNode<T>(_level + 1, _maxLevel, hNextKey);
                _nextNodes.Add(hNextKey, hNode);
                hNode.Add(exp, t);
            }
        }

        public List<Tuple<string, T>> Find(string exp, int max = int.MaxValue)
        {
            var hList = new List<Tuple<string, T>>();
            if ((_values != null && exp.Length <= _level) || _level == _maxLevel)
                hList.AddRange(_values);
            if (hList.Count >= max)
                return hList;
            if (_nextNodes != null)
                if (_level < exp.Length)
                {
                    var hNextKey = exp.Substring(0, _level + 1);
                    if (_nextNodes.ContainsKey(hNextKey))
                    {
                        var hNode = _nextNodes[hNextKey];
                        hList.AddRange(hNode.Find(exp));
                        if (hList.Count >= max)
                            return hList;
                    }
                }
                else
                {
                    //only from next Level
                    hList.AddRange(GetAllValues(_level + 1));
                    if (hList.Count >= max)
                        return hList;
                }
            return hList;
        }

        private List<Tuple<string, T>> GetAllValues(int fromLevel = 0)
        {
            var hList = new List<Tuple<string, T>>();
            if (_values != null && _level >= fromLevel)
                hList.AddRange(_values);
            if (_nextNodes != null)
                foreach (var hNextNodesValue in _nextNodes.Values)
                    hList.AddRange(hNextNodesValue.GetAllValues());
            return hList;
        }

        public void Clear()
        {
            if (_nextNodes != null)
            {
                foreach (var hNode in _nextNodes)
                    hNode.Value.Clear();
                _nextNodes = null;
            }
            if (_values != null)
                _values = null;
        }

        public void SetMaxLevel(int maxlevel)
        {
            _maxLevel = maxlevel;
            if (_nextNodes != null)
                foreach (var hNode in _nextNodes)
                    hNode.Value.SetMaxLevel(maxlevel);
        }

        public void Remove(string exp, T t)
        {
            if ((_key.Equals(exp) || _level >= _maxLevel) && _values != null)
            {
                _values.Remove(new Tuple<string, T>(exp, t));
                return;
            }

            if (_nextNodes != null)
            {
                foreach (var hNextNode in _nextNodes)
                {
                    hNextNode.Value.Remove(exp, t);
                }
            }
        }
    }
}
