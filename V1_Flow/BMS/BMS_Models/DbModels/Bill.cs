namespace BMS_Models.DbModels
{
    public class Bill
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// 上传用户
        /// </summary>
        public string UserCode { get; set; } = string.Empty;
        /// <summary>
        /// 上传用户
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 月份
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;
        /// <summary>
        /// 累心
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// 累计收入
        /// </summary>
        public decimal InMoney { get; set; }
        /// <summary>
        /// 支出
        /// </summary>
        public decimal OutMoney { get; set; }
    }
}

