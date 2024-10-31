using System;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        int concurrentRequests = 300; // 并发请求数量
        var tasks = new Task[concurrentRequests];
        var client = new HttpClient();

        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks[i] = MakeRequest(client, i);
        }

        await Task.WhenAll(tasks);
    }

    static async Task MakeRequest(HttpClient client, int requestId)
    {
        try
        {
            var response = await client.GetAsync("http://localhost:5000/DataManage/OperationLog/GetPageListJson?pageSize=10&pageIndex=1&sort=Id&sortType=desc&_=1728141778155");
            Console.WriteLine($"Request {requestId}: {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Request {requestId} failed: {ex.Message}");
        }
    }
}
