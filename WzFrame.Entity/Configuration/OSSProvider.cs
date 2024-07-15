using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Configuration
{
    public class OSSProviderOptions : OSSOptions
    {
        /// <summary>
        /// 自定义桶名称 不能直接使用Provider来替代桶名称
        /// 例：阿里云 1.只能包括小写字母，数字，短横线（-）2.必须以小写字母或者数字开头 3.长度必须在3-63字节之间
        /// </summary>
        public string Bucket { get; set; } = string.Empty;

        /// <summary>
        /// 外链域名
        /// </summary>
        public string? Url { get; set; }
    }
}
