namespace Ip2regionApi.Model;

/// <summary>
/// IP 地址区域详情
/// 每个 ip 数据段的 region 信息都固定了格式：国家|区域|省份|城市|ISP
/// 只有中国的数据绝大部分精确到了城市，其他国家部分数据只能定位到国家，后前的选项全部是0
/// </summary>
public class IpRegionModel
{
    public string Ip { get; set; }

    /// <summary>
    /// 国家
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// 区域
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// 省份
    /// </summary>
    public string Province { get; set; } = string.Empty;

    /// <summary>
    /// 城市
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// ISP
    /// </summary>
    public string ISP { get; set; } = string.Empty;

    public IpRegionModel(string ip)
    {
        Ip = ip;
    }

    public IpRegionModel(string ip, string country, string region, string province, string city, string isp)
    {
        Ip = ip;
        Country = country;
        Region = region;
        Province = province;
        City = city;
        ISP = isp;
    }
}