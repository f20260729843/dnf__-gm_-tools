using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AY.DNF.GMTool.Db.DbModels.taiwan_login
{
    [SugarTable(TableName = "login_account_3", TableDescription = "账号登录表")]
    public class LoginAccount3
    {
        [SugarColumn(ColumnName = "m_id", ColumnDataType = "int", IsPrimaryKey = true)]
        public int MID { get; set; }

        [SugarColumn(ColumnName = "m_channe_no", ColumnDataType = "int")]
        public int MChannelNo { get; set; }

        [SugarColumn(ColumnName = "login_status", ColumnDataType = "tinyint")]
        public long LoginStatus { get; set; }

        [SugarColumn(ColumnName = "last_login_date")]
        public DateTime LastLoginDate { get; set; }

        [SugarColumn(ColumnName = "login_ip", ColumnDataType = "varchar(15)")]
        public string LoginIP { get; set; }
    }
}
