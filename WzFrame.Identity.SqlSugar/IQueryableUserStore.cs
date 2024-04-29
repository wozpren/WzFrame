using Microsoft.AspNetCore.Identity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Identity.SqlSugar
{
    public interface ISugarQueryableUserStore<TUser> : IUserStore<TUser>, IDisposable where TUser : class
    {
        ISugarQueryable<TUser> Users { get; }
    }

    public interface ISugarQueryableRoleStore<TRole> : IRoleStore<TRole>, IDisposable where TRole : class
    {
        ISugarQueryable<TRole> Users { get; }
    }
}


