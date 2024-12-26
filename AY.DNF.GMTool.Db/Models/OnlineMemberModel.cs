using System;
using System.Collections.Generic;
using System.Text;

namespace AY.DNF.GMTool.Db.Models
{
    public class OnlineMemberModel
    {
        /// <summary>
        /// UID
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// 账号名
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public int CharacNo { get; set; }

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string CharacName { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public long Lev { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public int Job { get; set; }

        public string JobName { get; set; }

        /// <summary>
        /// 职业
        /// </summary>
        public long GrowType { get; set; }

        public string GrowTypeNames { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public long DeleteFlag { get; set; }

        public string DeleteFlagStr => DeleteFlag == 0 ? "正常" : "已删除";
    }
}
