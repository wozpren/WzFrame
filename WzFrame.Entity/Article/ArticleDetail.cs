namespace WzFrame.Entity.Article
{
    public enum ArticleStatus
    {
        Normal = 0,
        Draft = 1,
        Auditing = 2,
        Ban = 3,
    }

    [SugarTable("ArticleDetail", TableDescription = "文章细节")]
    public class ArticleDetail : EntityUserTimeBase
    {
        [SugarColumn(ColumnDescription = "文章标题")]
        public string Title { get; set; } = string.Empty;

        [SugarColumn(ColumnDescription = "文章标签")]
        public string Tags { get; set; } = string.Empty;

        [SugarColumn(ColumnDescription = "文章分类")]
        public string Category { get; set; } = string.Empty;

        [SugarColumn(ColumnDescription = "文章状态")]
        public ArticleStatus Status { get; set; } = ArticleStatus.Draft;


        [SugarColumn(ColumnDescription = "文章阅读量")]
        public int ReadCount { get; set; } = 0;

        [SugarColumn(ColumnDescription = "文章点赞量")]
        public int LikeCount { get; set; } = 0;

        [SugarColumn(ColumnDescription = "文章点踩量")]
        public int DislikeCount { get; set; } = 0;

        [SugarColumn(ColumnDescription = "文章评论量")]
        public int CommentCount { get; set; } = 0;

        [SugarColumn(ColumnDescription = "文章收藏量")]
        public int CollectCount { get; set; } = 0;

        [SugarColumn(ColumnDescription = "文章转载量")]
        public int ReprintCount { get; set; } = 0;



        [SugarColumn(ColumnDescription = "文章置顶")]
        public bool IsTop { get; set; } = false;

        [SugarColumn(ColumnDescription = "文章推荐")]
        public bool IsRecommend { get; set; } = false;

        [SugarColumn(ColumnDescription = "文章热门")]
        public bool IsHot { get; set; } = false;


    }
}
