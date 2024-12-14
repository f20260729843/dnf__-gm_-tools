using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace AY.DNF.GMTool.Db.DbModels.taiwan_game_event
{
    [SugarTable(TableName = "event_1306_account_reward", TableDescription = "登录事件")]
    public class Event1306AccountReward
    {
        [SugarColumn(ColumnName = "m_id", ColumnDataType = "int", IsPrimaryKey = true)]
        public string MID { get; set; }

        [SugarColumn(ColumnName = "charac_no", ColumnDataType = "int")]
        public int CharacNo { get; set; }

        [SugarColumn(ColumnName = "occ_date")]
        public DateTime OccDate { get; set; }
    }
}
