using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Detour;

namespace mClient.Maps
{
    public static class Extensions
    {
        /// <summary>
        /// Add or remove items to list until it is a particular size
        /// </summary>
        /// <param name="list"></param>
        /// <param name="size"></param>
        public static void resize(this List<Vector3> list, int size)
        {
            if (list.Count < size)
            {
                for (int i = list.Count; i < size; i++)
                    list.Add(new Vector3());
            }
            else if (list.Count > size)
            {
                while (list.Count != size)
                    list.RemoveAt(list.Count - 1);
            }
        }

        public static dtNavMeshParams ReadDetourNavMeshParams(this BinaryReader reader)
        {
            var p = new dtNavMeshParams();
            p.orig[0] = reader.ReadSingle();
            p.orig[1] = reader.ReadSingle();
            p.orig[2] = reader.ReadSingle();

            p.tileWidth = reader.ReadSingle();
            p.tileHeight = reader.ReadSingle();

            p.maxTiles = reader.ReadInt32();
            p.maxPolys = reader.ReadInt32();
            return p;
        }

        public static MMapTileHeader ReadMMapTileHeader(this BinaryReader reader)
        {
            var header = new MMapTileHeader();
            header.mmapMagic = reader.ReadUInt32();
            header.dtVersion = reader.ReadUInt32();
            header.mmapVersion = reader.ReadUInt32();
            header.size = reader.ReadUInt32();
            header.useLiquids = (reader.ReadByte() == 1 ? true : false);
            return header;
        }
    }
}
