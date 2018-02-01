using System.Collections.Generic;
using System.Linq;

namespace Stree
{
    public class Stree<T>
    {
        private readonly bool _maxLevelControled;
        private readonly StreeNode<T> _root;
        private int _maxLevel;

        public Stree()
        {
            _root = new StreeNode<T>(0, 0, "");
            _maxLevel = 0;
            _maxLevelControled = false;
        }

        public Stree(int maxLevel)
        {
            _root = new StreeNode<T>(0, maxLevel, "");
            _maxLevel = maxLevel;
            _maxLevelControled = true;
        }

        public void Add(string exp, T t)
        {
            if (!_maxLevelControled)
                if (exp.Length > _maxLevel)
                    SetMaxLevel(exp.Length);
            _root.Add(exp.ToLower(), t);
        }

        public List<T> Find(string exp, int max = int.MaxValue)
        {
            var hList = new List<T>();
            if (string.IsNullOrWhiteSpace(exp))
                return hList;
            exp = exp.ToLower();

            var hTuples = _root.Find(exp, max);

            if (hTuples.Count > max)
                hTuples = hTuples.Take(max).ToList();

            if (_maxLevelControled && exp.Length > _maxLevel)
                hTuples.RemoveAll(x => !x.Item1.StartsWith(exp));

            foreach (var hTuple in hTuples)
                hList.Add(hTuple.Item2);
            return hList;
        }

        public void SetMaxLevel(int maxlevel)
        {
            _maxLevel = maxlevel;
            _root.SetMaxLevel(maxlevel);
        }

        public void Clear()
        {
            _root.Clear();
        }

        public void Remove(string exp, T t)
        {
            _root.Remove(exp.ToLower(), t);
        }
    }
}
