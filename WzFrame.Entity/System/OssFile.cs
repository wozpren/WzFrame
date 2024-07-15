﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.System
{
    [SugarTable("File", TableDescription = "文件表")]
    public class OssFile : EntityUserBase
    {
        /// <summary>
        /// 提供者
        /// </summary>
        [SugarColumn(ColumnDescription = "提供者", Length = 128)]
        [MaxLength(128)]
        public string? Provider { get; set; }

        /// <summary>
        /// 仓储名称
        /// </summary>
        [SugarColumn(ColumnDescription = "仓储名称", Length = 128)]
        [MaxLength(128)]
        public string? BucketName { get; set; }

        /// <summary>
        /// 文件名称（源文件名）
        /// </summary>
        [SugarColumn(ColumnDescription = "文件名称", Length = 128)]
        [MaxLength(128)]
        public string? FileName { get; set; }


        /// <summary>
        /// 文件后缀
        /// </summary>
        [SugarColumn(ColumnDescription = "文件后缀", Length = 16)]
        [MaxLength(16)]
        public string? Suffix { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        [SugarColumn(ColumnDescription = "存储路径", Length = 128)]
        [MaxLength(128)]
        public string? FilePath { get; set; }

        /// <summary>
        /// 文件大小KB
        /// </summary>
        [SugarColumn(ColumnDescription = "文件大小KB", Length = 16)]
        [MaxLength(16)]
        public string? SizeKb { get; set; }

        /// <summary>
        /// 外链地址-OSS上传后生成外链地址方便前端预览
        /// </summary>
        [SugarColumn(ColumnDescription = "外链地址", Length = 512)]
        [MaxLength(512)]
        public string? Url { get; set; }

        /// <summary>
        /// 文件MD5
        /// </summary>
        [SugarColumn(ColumnDescription = "文件MD5", Length = 128)]
        [MaxLength(128)]
        public string? FileMd5 { get; set; }
    }
}
