using System.Diagnostics;
using IP2Region.Net.XDB;
using Ip2regionApi.Model;
using Ip2regionApi.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Ip2regionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IpController : ControllerBase
{
    private const string Ip2RegionXDBPath = "./data/ip2region.xdb";
    private readonly ISearcher _searcher;
    private readonly IHostEnvironment _hostEnvironment;


    public IpController(ISearcher searcher, IHostEnvironment hostEnvironment)
    {
        _searcher = searcher;
        _hostEnvironment = hostEnvironment;
    }

    /// <summary>
    /// 单个IP查询
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    [HttpGet("get", Name = "get")]
    public string? Get(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return string.Empty;
        }

        var result = _searcher.Search(ip);
        return result;
    }

    /// <summary>
    /// 单个IP查询
    /// </summary>
    /// <param name="ip"></param>
    /// <returns></returns>
    [HttpGet("GetModel", Name = "GetModel")]
    public IpRegionModel GetModel(string ip)
    {
        if (string.IsNullOrWhiteSpace(ip))
        {
            return new IpRegionModel(ip);
        }

        var result = _searcher.Search(ip);
        if (string.IsNullOrWhiteSpace(result) || result.Equals("0|0|0|0|0"))
        {
            return new IpRegionModel(ip);
        }
        else
        {
            var split = result.Split('|');
            return new IpRegionModel(ip, split[0], split[1], split[2], split[3], split[4]);
        }
    }


    /// <summary>
    /// IP list 集合查询
    /// </summary>
    /// <param name="ips"></param>
    /// <returns></returns>
    [HttpPost("list", Name = "list")]
    public Dictionary<string, string?> GetList(List<string>? ips)
    {
        var dic = new Dictionary<string, string?>();
        if (ips == null)
        {
            return dic;
        }

        /*
        *自定义缓存策略和xdb文件路径
        * CachePolicy.Content默认缓存策略，缓存整个xdb文件，线程安全
        * CachePolicy.VectorIndex 缓存向量索引，减少IO操作的次数，不是线程安全的
        * CachePolicy.File 没有缓存，不影响线程安全
        */
        var searcher = new Searcher(CachePolicy.File, Ip2RegionXDBPath);

        foreach (var ip in ips)
        {
            var result = searcher.Search(ip);
            dic.Add(ip, result);
        }

        return dic;
    }

    [HttpPost("ListModel", Name = "GetListModel")]
    public List<IpRegionModel> GetListModel(List<string>? ips)
    {
        var list = new List<IpRegionModel>();
        if (ips == null)
        {
            return list;
        }

        /*
        *自定义缓存策略和xdb文件路径
        * CachePolicy.Content默认缓存策略，缓存整个xdb文件，线程安全
        * CachePolicy.VectorIndex 缓存向量索引，减少IO操作的次数，不是线程安全的
        * CachePolicy.File 没有缓存，不影响线程安全
        */
        var searcher = new Searcher(CachePolicy.File, Ip2RegionXDBPath);

        foreach (var ip in ips)
        {
            var result = searcher.Search(ip);
            if (string.IsNullOrWhiteSpace(result) || result.Equals("0|0|0|0|0"))
            {
                list.Add(new IpRegionModel(ip));
            }
            else
            {
                var split = result.Split('|');
                list.Add(new IpRegionModel(ip, split[0], split[1], split[2], split[3], split[4]));
            }
        }

        return list;
    }


    /// <summary>
    /// 获取中国段IP
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    [HttpGet("/china")]
    public List<string> GetChinaCICD()
    {
        var shellPath = Path.Combine(_hostEnvironment.ContentRootPath, "data/chinaRule/apnic-chinaip.sh");
        var chinaRulePath = Path.Combine(_hostEnvironment.ContentRootPath, "cn_rules.conf");

        if (!System.IO.File.Exists(chinaRulePath) || System.IO.File.ReadAllText(chinaRulePath).Length == 0)
        {
            // 执行Shell脚本命令
            ShellCommandHelper.ExecuteShellCommand($"sh {shellPath}");
        }

        var chinaCiCd = System.IO.File.ReadAllLines(chinaRulePath);
        return chinaCiCd.ToList();
    }
}