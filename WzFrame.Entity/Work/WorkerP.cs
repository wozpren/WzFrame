using BootstrapBlazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Work
{
    [SugarTable("WorkerP")]
    public class WorkerP : EntityUserTimeBase
    {
        [AutoGenerateColumn(Text = "工人名称")]
        [SugarColumn(ColumnDescription = "工人名称")]
        public string? Name { get; set; }

        [AutoGenerateColumn(Text = "工人身份证")]
        [SugarColumn(ColumnDescription = "工人身份证", Length = 18)]
        public string? IdCard { get; set; }

        [AutoGenerateColumn(Text = "工人照片")]
        [SugarColumn(ColumnDescription = "工人照片")]
        public string? Photo { get; set; }

        [AutoGenerateColumn(Text = "工人电话")]
        [SugarColumn(ColumnDescription = "工人电话", Length = 11)]
        public string? Phone { get; set; }

        [AutoGenerateColumn(Text = "培训公司")]
        [SugarColumn(ColumnDescription = "培训公司")]
        public string? TrainingCompany { get; set; }

        [AutoGenerateColumn(Text = "培训师")]
        [SugarColumn(ColumnDescription = "培训师")]
        public string? TrainingTeacher { get; set; }

        [AutoGenerateColumn(Text = "培训签名")]
        [SugarColumn(ColumnDescription = "培训签名")]
        public string? TrainingName { get; set; }

        [AutoGenerateColumn(Text = "培训内容")]
        [SugarColumn(ColumnDescription = "培训内容")]
        public string? TrainingContent { get; set; }


        [AutoGenerateColumn(Text = "证书编号")]
        [SugarColumn(ColumnDescription = "证书编号")]
        public string? CertificateNo { get; set; }


        [AutoGenerateColumn(Text = "颁证日期")]
        [SugarColumn(ColumnDescription = "颁证日期")]
        public DateTime CertificateDate { get; set; }

        [AutoGenerateColumn(Text = "有效时间")]
        [SugarColumn(ColumnDescription = "有效日期")]
        public DateTime ValidDate { get; set; }

        [AutoGenerateColumn(Text = "所属", Ignore = true)]
        public string? Belong { get; set; } = string.Empty;
    }
}
