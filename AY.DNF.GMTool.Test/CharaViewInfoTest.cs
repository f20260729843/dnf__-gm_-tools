using AY.DNF.GMTool.Common;
using AY.DNF.GMTool.Db;
using AY.DNF.GMTool.Db.DbModels.taiwan_cain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AY.DNF.GMTool.Test
{
    internal class CharaViewInfoTest
    {
        public static void CharacView()
        {
            //DbFrameworkScope.Init("192.168.200.131","game", "uu5!^%jg",3306);
            var view = DbFrameworkScope.TaiwanCain.Queryable<CharacView>().Where(t => t.MId == 18000001).First();

            // info流
            var blob = view.Info.ToList();
            blob = blob.GetRange(4, blob.Count - 4);
            blob = SharpZipHelper.SharpZipLibDecompress(blob.ToArray()).ToList();

            // 148位 为一个角色
            // 4位 id
            //
            var charaBytes = new List<byte[]>();
            var count = blob.Count / 148;
            for (int i = 0; i < count; i++)
            {
                charaBytes.Add(blob.Skip(i * 148).Take(148).ToArray());                
            }

            charaBytes.Where(t =>t.Take(4).Any(x=>!x.Equals(0))).ToList().ForEach(t =>
            {
                var bytes = t;
                var noBytes = bytes.Take(4).ToList();
                var no = int.Parse(noBytes[0].ToString());
                bytes = bytes.Skip(4).ToArray();

                var nameBytes = bytes.Take(20).Where(t=>t!=0).ToArray();
                var characName=  Encoding.UTF8.GetString(nameBytes);
                bytes = bytes.Skip(20).ToArray();

                var job = bytes.Take(1);

                bytes= bytes.Skip(1).ToArray();
                var growType = bytes.Take(1);

                bytes = bytes.Skip(1).ToArray();
                var level = bytes.Take(1);

                bytes = bytes.Skip(1).ToArray();

                var inentory_capacity = bytes.Take(2);

                bytes = bytes.Skip(2).ToArray();

                var unknow = bytes.Take(11).ToArray();
                bytes=bytes.Skip(11).ToArray();

                // 时装等
                var count = bytes.Count() / 8;
                var sjList = new List<long>();
                for (var i = 0; i < 11; i++)
                {
                    var tmp = bytes.Skip(i * 8).Take(8).ToArray();
                    tmp.Reverse();
                    sjList.Add(BitConverter.ToInt64(tmp));
                }

                bytes=bytes.Skip(8*count).ToArray();
            });
        }
    }
}
