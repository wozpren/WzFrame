using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Shared.Core
{
    public class RuntimeExtension
    {
        public static IList<Assembly> GetAllAssemblies()
        {
            var list = new List<Assembly>();
            var deps = DependencyContext.Default;
            //只加载项目中的程序集
            var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type == "project"); //排除所有的系统程序集、Nuget下载包
            foreach (var lib in libs)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(assembly);
                }
                catch (Exception e)
                {
                    //Log.Debug(e, "GetAllAssemblies Exception:{ex}", e.Message);
                }
            }

            return list;
        }


    }
}
