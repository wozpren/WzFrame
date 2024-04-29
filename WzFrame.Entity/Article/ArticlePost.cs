using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Article
{
    [SugarTable("ArticleDetail", TableDescription = "文章帖子")]
    public class ArticlePost : EntityUserTimeBase
    {
        public long ArticleId { get; set; }

        public long ReplyId { get; set; }

        [SqlSugar.SugarColumn(IsIgnore = true)]
        public List<ArticlePost>? Replys { get; set; }

        [SugarColumn(ColumnDataType = "TEXT")]
        public string Content { get; set; } = string.Empty;

        [SugarColumn(ColumnDescription = "楼层")]
        public int Floor { get; set; } = 0;
    }
}
