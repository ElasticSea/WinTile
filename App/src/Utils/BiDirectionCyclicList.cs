using System.Collections.Generic;

namespace App
{
    public class BiDirectionCyclicList<T>
    {
        private readonly List<T> list;
        private int index;

        public BiDirectionCyclicList(List<T> list)
        {
            this.list = list;
        }

        public T Prev => list[updateIndex(-1)];
        public T Next => list[updateIndex(+1)];

        private int updateIndex(int dir)
        {
            index = (index + dir + list.Count) % list.Count;
            return index;
        }
    }
}