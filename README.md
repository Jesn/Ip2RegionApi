# Ip2RegionApi

## 介绍

* 数据来源于 https://github.com/lionsoul2014/ip2region
* 基于 `ip2region` 写了个简单的IP地址查询的接口，方便自己调用
* 提供单个接口查询和多个接口查询，多个接口查询是缓存了整个XDB到内存，然后在内存中查询，保证线程安全

![iShot_2023-05-08_12.01.39.png](docs%2Fimg%2FiShot_2023-05-08_12.01.39.png)

## docker
```
docker push richpeople/ip2regionapi:latest
docker run -it -d --name ip2regionapi -p 8080:80 ip2regionapi
```

